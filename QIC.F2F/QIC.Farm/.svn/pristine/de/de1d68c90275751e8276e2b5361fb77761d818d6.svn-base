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
    public class FamilyCardDal
    {
        public FamilyCardResult Add(FamilyCardDto familyCard)
        {
            var result = new FamilyCardResult();
            using (var context = new Entities())
            {
                if (context.FamilyCard.Any(x => x.FamilyID == familyCard.FamilyID && x.CardKindID == familyCard.CardKindID))
                {
                    result.FamilyCardError = FamilyCardError.FamilyCardExist;//该家庭已添加过该类型的卡
                    return result;
                }

                var cardKind = context.CardKind.FirstOrDefault(x => x.ID == familyCard.CardKindID);

                if (cardKind != null)
                {
                    context.FamilyCard.Add(new FamilyCard
                    {
                        FamilyID = familyCard.FamilyID,
                        CardKindID = familyCard.CardKindID,
                        IsValid = familyCard.IsValid,
                        TotalCount = cardKind.TotalCount
                    });
                }
                var count = context.SaveChanges();
                if (count > 0) result.FamilyCardError = FamilyCardError.Success;
            }
            return result;
        }

        public bool Delete(int id)
        {
            using (var context = new Entities())
            {
                var result = context.FamilyCard.Where(x => x.ID == id).Delete();
                return result > 0;
            }
        }

        public FamilyCardResult Update(FamilyCardDto familyCard)
        {
            var result = new FamilyCardResult();
            using (var context = new Entities())
            {
                if (context.FamilyCard.Any(x => x.ID != familyCard.ID && x.FamilyID == familyCard.FamilyID && x.CardKindID == familyCard.CardKindID))
                {
                    result.FamilyCardError = FamilyCardError.FamilyCardExist;
                    return result;
                }

                var count = context.FamilyCard.Where(x => x.ID == familyCard.ID).Update(x => new FamilyCard
                {
                    FamilyID = familyCard.FamilyID,
                    CardKindID = familyCard.CardKindID,
                    DistributedCount = familyCard.DistributedCount,
                    IsValid = familyCard.IsValid,
                    TotalCount = familyCard.TotalCount
                });

                if (count > 0) result.FamilyCardError = FamilyCardError.Success;
            }
            return result;
        }

        //判断配送次数是否充足
        public bool IsTotalCountEnough(int id)
        {
            var result = false;
            using (var context = new Entities())
            {
                var familyCard = context.FamilyCard.FirstOrDefault(x => x.ID == id);
                if (familyCard != null)
                {
                    if(familyCard.TotalCount > familyCard.DistributedCount) result = true;
                    else result = false;
                }
                return result;
            }
        }

        //更新配送次数
        public void UpdateDestritedCount(int id, bool add)
        {
            var symbol = add ? "+" : "-";
            using (var context = new Entities())
            {               
                context.Database.ExecuteSqlCommand(string.Format("update FamilyCard  set DistributedCount = (case DistributedCount when TotalCount then DistributedCount else DistributedCount {0} 1 end) where id = {1} and IsValid = 1", symbol, id));
                context.FamilyCard.Where(x => x.ID == id && x.DistributedCount == x.TotalCount).Update(x => new FamilyCard { IsValid = false });
            }
        }


        //查询对应家庭的所有会员卡信息
        public CommonSearchResult<FamilyCardDto> GetFamilyCardByFamily(int familyID)
        {
            using (var context = new Entities())
            {

                var query = from f in context.FamilyInfo
                            where f.ID == familyID
                            join fc in context.FamilyCard on f.ID equals fc.FamilyID
                            join c in context.CardKind on fc.CardKindID equals c.ID
                            where fc.IsValid == true
                            select new
                            {
                                ID = fc.ID,
                                FamilyName = f.Name,
                                CardKindName = c.Name,
                                DistributedCount = fc.DistributedCount,
                                IsValid = fc.IsValid,
                                TotalCount = fc.TotalCount,
                                CreateTime = fc.CreateTime
                            };

                var fianlQuery = query.Select(x => new FamilyCardDto
                {
                    ID = x.ID,
                    FamilyName = x.FamilyName,
                    CardKindName = x.CardKindName,
                    DistributedCount = x.DistributedCount,
                    IsValid = x.IsValid,
                    TotalCount = x.TotalCount,
                    CreateTime = x.CreateTime
                }).OrderBy(x => x.ID);

                var countQuery = fianlQuery.FutureCount();

                var listQuery = fianlQuery.Future();

                var familyCards = listQuery.ToList();

                return new CommonSearchResult<FamilyCardDto> { Result = familyCards, TotalCount = countQuery.Value };
            }
        }

        public CommonSearchResult<FamilyCardDto> GetFamilyCardList(FamilyCardSearchDto searchDto)
        {
            var familyName = (searchDto.FamilyName ?? "").Trim();
            var cardKindName = (searchDto.CardKindName ?? "").Trim();

            using (var context = new Entities())
            {
                var query = from f in context.FamilyInfo
                            join fc in context.FamilyCard on f.ID equals fc.FamilyID
                            join c in context.CardKind on fc.CardKindID equals c.ID
                            select new
                            {
                                ID = fc.ID,
                                FamilyID = f.ID,
                                FamilyName = f.Name,
                                CardKindID = c.ID,
                                CardKindName = c.Name,
                                DistributedCount = fc.DistributedCount,
                                IsValid = fc.IsValid,
                                TotalCount = fc.TotalCount,
                                CreateTime = fc.CreateTime
                            };

                if (!string.IsNullOrEmpty(familyName))
                {
                    query = query.Where(x => x.FamilyName.Contains(familyName));
                }

                if (!string.IsNullOrEmpty(cardKindName))
                {
                    query = query.Where(x => x.CardKindName.Contains(cardKindName));
                }

                if (searchDto.StartTime.HasValue)
                {
                    query = query.Where(x => DateTime.Compare(x.CreateTime, searchDto.StartTime.Value) >= 0);
                }

                if (searchDto.EndTime.HasValue)
                {
                    query = query.Where(x => DateTime.Compare(x.CreateTime, searchDto.EndTime.Value) <= 0);
                }

                var fianlQuery = query.Select(x => new FamilyCardDto
                {
                    ID = x.ID,
                    FamilyID = x.FamilyID,
                    FamilyName = x.FamilyName,
                    CardKindID = x.CardKindID,
                    CardKindName = x.CardKindName,
                    DistributedCount = x.DistributedCount,
                    IsValid = x.IsValid,
                    TotalCount = x.TotalCount,
                    CreateTime = x.CreateTime
                }).OrderBy(x => x.ID);

                var countQuery = fianlQuery.FutureCount();

                var listQuery = fianlQuery.Skip((searchDto.PageIndex - 1) * searchDto.PageSize).Take(searchDto.PageSize).Future();

                var familyCards = listQuery.ToList();

                return new CommonSearchResult<FamilyCardDto> { Result = familyCards, TotalCount = countQuery.Value };
            }
        }
    }
}
