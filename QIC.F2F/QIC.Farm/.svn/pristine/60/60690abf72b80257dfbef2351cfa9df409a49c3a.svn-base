using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Farm.Service.Dto;
using QIC.Farm.Service.Model;
using EntityFramework.Extensions;
using System.Data.Entity.Validation;

namespace QIC.Farm.Service.Dal
{
    public class DistributionDal
    {
        public DistributionResult Add(DistributionDto distribution)
        {
            var result = new DistributionResult();
            using (var context = new Entities())
            {
                var distri = context.Distribution.Add(new Distribution
                {
                    ID = distribution.ID,
                    FamilyID = distribution.FamilyID,
                    Contacter = distribution.Contacter,
                    Phone = distribution.Phone,
                    Address = distribution.Address,
                    DistributedDate = distribution.DistributedDate
                });

                var count = context.SaveChanges();
                if (count > 0)
                {
                    result.DistributionError = Enums.DistributionError.Success;
                    result.DistributionID = distri.ID;
                }
                else result.DistributionError = Enums.DistributionError.SystemError;
                return result;
            }
        }

        public bool Delete(string id)
        {
            using (var context = new Entities())
            {
                var result = context.Distribution.Where(x => x.ID == id).Delete();
                return result > 0;
            }
        }

        public bool Update(DistributionDto distribution)
        {
            using (var context = new Entities())
            {
                var count = context.Distribution.Where(x => x.ID == distribution.ID).Update(x => new Distribution
                {
                    FamilyID = distribution.FamilyID,
                    Contacter = distribution.Contacter,
                    Phone = distribution.Phone,
                    Address = distribution.Address,
                    DistributedDate = distribution.DistributedDate
                });

                return count > 0;
            }
        }

        public CommonSearchResult<DistributionDto> GetDistributionList(DistributionSearchDto searchDto)
        {
            var contracter = (searchDto.Contracter ?? "").Trim();
            var phone = (searchDto.Phone ?? "").Trim();

            using (var context = new Entities())
            {
                var query = from d in context.Distribution
                            join f in context.FamilyInfo on d.FamilyID equals f.ID
                            select new {
                                ID = d.ID,
                                FamilyID = d.FamilyID,  
                                FamilyName = f.Name,
                                Contacter = d.Contacter,
                                Phone = d.Phone,
                                Address = d.Address,
                                DistributedDate = d.DistributedDate,
                                CreateTime = d.CreateTime
                            };

                if (!string.IsNullOrEmpty(contracter))
                {
                    query = query.Where(x => x.Contacter.Contains(contracter));
                }

                if (!string.IsNullOrEmpty(phone))
                {
                    query = query.Where(x => x.Phone.Contains(phone));
                }

                var fianlQuery = query.Select(x => new DistributionDto
                {
                    ID = x.ID,
                    FamilyID = x.FamilyID,   
                    FamilyName = x.FamilyName,
                    Contacter = x.Contacter,
                    Phone = x.Phone,
                    Address = x.Address,
                    DistributedDate = x.DistributedDate,
                    CreateTime = x.CreateTime
                }).OrderByDescending(x => x.ID);

                var countQuery = fianlQuery.FutureCount();

                var listQuery = fianlQuery.Skip((searchDto.PageIndex - 1) * searchDto.PageSize).Take(searchDto.PageSize).Future();

                var distributions = listQuery.ToList();

                return new CommonSearchResult<DistributionDto> { Result = distributions, TotalCount = countQuery.Value };
            }
        }


        public DistributionDto GetDistributionByID(string id)
        {
            using (var context = new Entities())
            {
                var query = from d in context.Distribution
                            where d.ID == id
                            join f in context.FamilyInfo on d.FamilyID equals f.ID
                            select new { d, f } into temp
                            select new
                            {
                                ID = temp.d.ID,
                                FamilyID = temp.d.FamilyID,
                                Contacter = temp.d.Contacter,
                                Phone = temp.d.Phone,
                                Address = temp.d.Address,
                                FamilyInfo = temp.f,
                                CreateTime = temp.d.CreateTime,
                                DistributedDate = temp.d.DistributedDate,
                                DistributeDetail = (from d in context.Distribution
                                                    where d.ID == temp.d.ID
                                                    join dd in context.DistributionDetail on d.ID equals dd.DistributionID
                                                    join p in context.Product on dd.ProductID equals p.ID
                                                    select new
                                                    {
                                                        ID = dd.ID,
                                                        DistributionID = dd.DistributionID,
                                                        FamilyCardID = dd.FamilyCardID,
                                                        FamilyCardName = (from fc in context.FamilyCard 
                                                                          where fc.ID == dd.FamilyCardID
                                                                          join ck in context.CardKind on fc.CardKindID equals ck.ID select ck.Name),
                                                        Amount = dd.Amount,
                                                        ProductID = p.ID,
                                                        ProductName = p.Name,
                                                        ReportID = dd.ReportID
                                                    })
                            };

                var fianlQuery = query.Select(x => new DistributionDto
                {
                    ID = x.ID,
                    FamilyID = x.FamilyID,
                    Contacter = x.Contacter,
                    Phone = x.Phone,
                    Address = x.Address,
                    CreateTime = x.CreateTime,
                    DistributedDate = x.DistributedDate,
                    FamilyInfo = new FamilyInfoDto 
                    {
                         ID = x.FamilyInfo.ID,
                         NickName = x.FamilyInfo.NickName,
                         OpenID = x.FamilyInfo.OpenID,
                         IsClosed = x.FamilyInfo.IsClosed,
                         Name = x.FamilyInfo.Name,
                         Mobile = x.FamilyInfo.Mobile,
                         Description = x.FamilyInfo.Description,
                         FamilyNumber = x.FamilyInfo.FamilyNumber,
                         Region = x.FamilyInfo.Region
                    },
                    DistributionDetails = x.DistributeDetail.Select(n => new DistributionDetailDto
                    {
                        ID = n.ID,
                        DistributionID = n.DistributionID,
                        FamilyCardID = n.FamilyCardID,  
                        FamilyCardName = n.FamilyCardName.FirstOrDefault(),
                        Amount = n.Amount,
                        ProductID = n.ProductID,
                        ProductName = n.ProductName,
                        ReportID = n.ReportID
                    })
                }).FirstOrDefault();

                var distribution = fianlQuery;

                return distribution;
            }
        }

        //根据家庭 查询对应的配送信息（包括配送的产品详情列表）
        public FrontWebSearchResult<DistributionDto> GetDistributionListByOpenID(string openID, int pageIndex, int pageSize)
        {
            string memberName = string.Empty;
            using (var context = new Entities())
            {
                var member = context.FamilyInfo.FirstOrDefault(x => x.OpenID == openID);
                if (member != null) memberName = member.Name;

                var query = from f in context.FamilyInfo
                            where f.OpenID == openID
                            join d in context.Distribution on f.ID equals d.FamilyID
                            select new { f, d } into temp
                            select new
                            {
                                ID = temp.d.ID,
                                FamilyID = temp.d.FamilyID,
                                FamilyName = temp.f.Name,
                                DistributedDate = temp.d.DistributedDate,
                                FamilyCardInfo = (from fc in context.FamilyCard
                                                  where fc.FamilyID == temp.f.ID
                                                  join ck in context.CardKind on fc.CardKindID equals ck.ID
                                                  select new { Name = ck.Name, TotalCount = fc.TotalCount, DistributedCount = fc.DistributedCount }
                                ),
                                DistributeDetail = (from d in context.Distribution
                                                    where d.ID == temp.d.ID
                                                    join dd in context.DistributionDetail on d.ID equals dd.DistributionID
                                                    join p in context.Product on dd.ProductID equals p.ID
                                                    select new
                                                    {
                                                        DistributionID = dd.DistributionID,
                                                        FamilyCardID = dd.FamilyCardID,
                                                        Amount = dd.Amount,
                                                        Unit = p.Unit,
                                                        ProductID = p.ID,
                                                        ProductCookingUrl = p.Cooking,
                                                        ProductName = p.Name,
                                                        ReportID = dd.ReportID,
                                                        ProductReportUrl = from productReport in context.ProductReport where productReport.ID == dd.ReportID select productReport.Description
                                                    })
                            };


                var fianlQuery = query.Select(x => new DistributionDto
                {
                    ID = x.ID,
                    FamilyID = x.FamilyID,
                    FamilyName = x.FamilyName,
                    DistributedDate = x.DistributedDate,
                    FamilyCardInfo = x.FamilyCardInfo.Select(n => new FamilyCardDto { 
                        CardKindName = n.Name,
                        TotalCount = n.TotalCount,
                        DistributedCount = n.DistributedCount
                    }),
                    DistributionDetails = x.DistributeDetail.Select(n => new DistributionDetailDto
                    {
                        DistributionID = n.DistributionID,
                        FamilyCardID = n.FamilyCardID,
                        Unit = n.Unit,
                        Amount = n.Amount,
                        ProductID = n.ProductID,
                        ProductCookingUrl = n.ProductCookingUrl,
                        ProductName = n.ProductName,
                        ReportID = n.ReportID,
                        ProductReportUrl = n.ProductReportUrl.FirstOrDefault()
                    })
                }).OrderByDescending(x => x.DistributedDate);


                var countQuery = fianlQuery.FutureCount();

                var listQuery = fianlQuery.Skip((pageIndex - 1) * pageSize).Take(pageSize).Future();

                var distributions = listQuery.ToList();
                for (int i = 0; i < distributions.Count; i++)
                {
                    distributions[i].DistributedDateStr = distributions[i].DistributedDate.ToString("yyyy年MM月dd日");
                }

                return new FrontWebSearchResult<DistributionDto> { Result = distributions, TotalCount = countQuery.Value, MemberName = memberName };
            }
        }
    }
}
