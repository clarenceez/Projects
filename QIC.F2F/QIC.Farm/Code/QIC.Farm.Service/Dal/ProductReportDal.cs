using QIC.Farm.Service.Dto;
using QIC.Farm.Service.Enums;
using QIC.Farm.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace QIC.Farm.Service.Dal
{
    public class ProductReportDal
    {
        public ProductReportResult Add(ProductReportDto productReport)
        {
            var result = new ProductReportResult();
            using (var context = new Entities())
            {
                if (context.ProductReport.Any(x => x.Name == productReport.Name))
                {
                    result.ProductReportError = ProductReportError.ReportExist;
                    return result;
                }

                var productReportReturn = context.ProductReport.Add(new ProductReport
                {
                     Name = productReport.Name,
                     Writer = productReport.Writer,
                     Description = productReport.Description,
                     IsClosed = productReport.IsClosed,
                     IsExpired = productReport.IsExpired
                });

                var count = context.SaveChanges();
                if (count > 0)
                {
                    result.ID = productReportReturn.ID;
                    result.ProductReportError = ProductReportError.Success;
                }
            }
            return result;
        }

        public bool Delete(int id)
        {
            using (var context = new Entities())
            {
                var result = context.ProductReport.Where(x => x.ID == id).Update(x => new ProductReport { IsClosed = true });
                return result > 0;
            }
        }

        public ProductReportResult Update(ProductReportDto productReport)
        {
            var result = new ProductReportResult();
            using (var context = new Entities())
            {
                if (context.ProductReport.Any(x => x.ID != productReport.ID && x.Name == productReport.Name))
                {
                    result.ProductReportError = ProductReportError.ReportExist;
                    return result;
                }

                var count = context.ProductReport.Where(x => x.ID == productReport.ID).Update(x => new ProductReport
                {
                    Name = productReport.Name,
                    Writer = productReport.Writer,
                    Description = productReport.Description,
                    IsClosed = productReport.IsClosed,
                    IsExpired = productReport.IsExpired
                });

                if (count > 0) result.ProductReportError = ProductReportError.Success;
            }
            return result;
        }

        public ProductReportDto GetProductReportByID(int id)
        {
            using (var context = new Entities())
            {
                return context.ProductReport.Where(x => x.ID == id).Select(x => new ProductReportDto
                {
                    ID = x.ID,
                    Name = x.Name,
                    Writer = x.Writer,
                    Description = x.Description,
                    IsClosed = x.IsClosed,
                    IsExpired = x.IsExpired,
                    CreateTime = x.CreateTime
                }).FirstOrDefault();
            }
        }

        public List<ProductReportDto> GetAllProductReport()
        {
            using (var context = new Entities())
            {
                return context.ProductReport.Where(x => !x.IsClosed).Select(x => new ProductReportDto
                {
                    ID = x.ID,
                    Name = x.Name,
                    Writer = x.Writer,
                    Description = x.Description,
                    IsClosed = x.IsClosed,
                    IsExpired = x.IsExpired,
                    CreateTime = x.CreateTime
                }).ToList();
            }
        }

        public CommonSearchResult<ProductReportDto> GetProductReportList(ProductReportSearchDto searchDto)
        {
            var name = (searchDto.Name??"").Trim();
            var writer = (searchDto.Writer??"").Trim();

            using (var context = new Entities())
            {
                var query = context.ProductReport.AsQueryable();

                if (searchDto.IsClosed.HasValue)
                {
                    query = query.Where(x => x.IsClosed == searchDto.IsClosed.Value);
                }

                if (searchDto.IsExpired.HasValue)
                {
                    query = query.Where(x => x.IsExpired == searchDto.IsExpired.Value);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.Name.Contains(name));
                }

                if (!string.IsNullOrEmpty(writer))
                {
                    query = query.Where(x => x.Writer.Contains(writer));
                }

                if (searchDto.StartTime.HasValue)
                {
                    query = query.Where(x => DateTime.Compare(x.CreateTime, searchDto.StartTime.Value) >= 0); 
                }

                if (searchDto.EndTime.HasValue)
                {
                    query = query.Where(x => DateTime.Compare(x.CreateTime, searchDto.EndTime.Value) <= 0);
                }

                var fianlQuery = query.Select(x => new ProductReportDto
                {
                    ID = x.ID,
                    Name = x.Name,
                    Writer = x.Writer,
                    Description = x.Description,
                    IsClosed = x.IsClosed,
                    IsExpired = x.IsExpired,
                    CreateTime = x.CreateTime
                }).OrderBy(x => x.ID);

                var countQuery = fianlQuery.FutureCount();

                var listQuery = fianlQuery.Skip((searchDto.PageIndex - 1) * searchDto.PageSize).Take(searchDto.PageSize).Future();

                var productReprots = listQuery.ToList();

                return new CommonSearchResult<ProductReportDto> { Result = productReprots, TotalCount = countQuery.Value };
            }
        }
    }
}
