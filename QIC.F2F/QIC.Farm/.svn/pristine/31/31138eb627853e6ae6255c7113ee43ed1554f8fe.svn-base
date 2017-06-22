using QIC.Farm.Service.Dto;
using QIC.Farm.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using QIC.Farm.Service.Enums;

namespace QIC.Farm.Service.Dal
{
    public class ProductDal
    {
        public ProductResult Add(ProductDto product)
        {
            var result = new ProductResult();
            using (var context = new Entities())
            {
                if(context.Product.Any(x=>x.Name == product.Name))
                {
                    result.ProductError = ProductError.ProductExist;
                    return result;
                }

                var productReturn = context.Product.Add(new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    Cooking = product.Cooking,
                    IsClosed = product.IsClosed,
                    Unit = product.Unit
                });

                var count = context.SaveChanges();
                if (count > 0)
                {
                    result.ID = productReturn.ID;
                    result.ProductError = ProductError.Success;
                }
            }
            return result;
        }

        //修改isClosed状态
        public bool Delete(int id)
        {
            using (var context = new Entities())
            {
                var result = context.Product.Where(x => x.ID == id).Update(x => new Product { IsClosed = true });
                return result > 0;
            }
        }

        public ProductResult Update(ProductDto product)
        {
            var result = new ProductResult();
            using (var context = new Entities())
            {
                if (context.Product.Any(x => x.ID != product.ID && x.Name == product.Name))
                {
                    result.ProductError = ProductError.ProductExist;
                    return result;
                }

                var count = context.Product.Where(x => x.ID == product.ID).Update(x => new Product
                {
                    Name = product.Name,
                    Cooking = product.Cooking,
                    Description = product.Description,
                    Unit = product.Unit,
                    IsClosed = product.IsClosed
                });

                if (count > 0) result.ProductError = ProductError.Success;
            }
            return result;
        }

        public ProductDto GetProductByID(int id)
        {
            using (var context = new Entities())
            {
                return context.Product.Where(x => x.ID == id).Select(x => new ProductDto
                {
                    ID = x.ID,
                    Name = x.Name,
                    IsClosed = x.IsClosed,
                    Description = x.Description,
                    Cooking = x.Cooking,
                    Unit = x.Unit
                }).FirstOrDefault();
            }
        }

        public List<ProductDto> GetAllProduct()
        {
            using (var context = new Entities())
            {
                return context.Product.Where(x => !x.IsClosed).Select(x => new ProductDto {
                    ID = x.ID,
                    Name = x.Name,
                    IsClosed = x.IsClosed,
                    Description = x.Description,
                    Cooking = x.Cooking,
                    Unit = x.Unit
                }).ToList();
            }
        }

        public CommonSearchResult<ProductDto> GetProductList(ProductSearchDto searchDto)
        {
            var name = (searchDto.Name??"").Trim();

            using (var context = new Entities())
            {
                var query = context.Product.AsQueryable();

                if (searchDto.IsClosed.HasValue)
                {
                    query = query.Where(x => x.IsClosed == searchDto.IsClosed.Value);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.Name.Contains(name));
                }               

                var fianlQuery = query.Select(x => new ProductDto
                {
                    ID = x.ID,
                    Name = x.Name,
                    IsClosed = x.IsClosed,
                    Description = x.Description,
                    Cooking = x.Cooking,
                    Unit = x.Unit
                }).OrderBy(x => x.ID);

                var countQuery = fianlQuery.FutureCount();

                var listQuery = fianlQuery.Skip((searchDto.PageIndex - 1) * searchDto.PageSize).Take(searchDto.PageSize).Future();

                var products = listQuery.ToList();

                return new CommonSearchResult<ProductDto> { Result = products, TotalCount = countQuery.Value };
            }
        }
    }
}
