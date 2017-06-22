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
    public class CardKindDal
    {
        public CardKindResult Add(CardKindDto cardKind)
        {
            var result = new CardKindResult();
            using (var context = new Entities())
            {
                if (context.CardKind.Any(x => x.Name == cardKind.Name))
                {
                    result.CardKindError = CardKindError.CardExist;
                    return result;
                }

                context.CardKind.Add(new CardKind
                {
                    Name = cardKind.Name,
                    Description = cardKind.Description,
                    TotalCount = cardKind.TotalCount,
                    IsClosed = cardKind.IsClosed
                });

                var count = context.SaveChanges();
                if (count > 0) result.CardKindError = CardKindError.Success;
            }

            return result;
        }

        //修改isClosed状态
        public bool Delete(int id)
        {
            using (var context = new Entities())
            {
                var result = context.CardKind.Where(x => x.ID == id).Update(x => new CardKind { IsClosed = true });
                return result > 0;
            }
        }

        public CardKindResult Update(CardKindDto cardKind)
        {
            var result = new CardKindResult();
            using (var context = new Entities())
            {
                if (context.CardKind.Any(x => x.ID != cardKind.ID && x.Name == cardKind.Name))
                {
                    result.CardKindError = CardKindError.CardExist;
                    return result;
                }

                var count = context.CardKind.Where(x => x.ID == cardKind.ID).Update(x => new CardKind
                {
                    Name = cardKind.Name,
                    Description = cardKind.Description,
                    TotalCount = cardKind.TotalCount,
                    IsClosed = cardKind.IsClosed
                });

                if (count > 0) result.CardKindError = CardKindError.Success;
            }
            return result;
        }

        public List<CardKindDto> GetCardKindListByNameFuzzy(string name,int count)
        {
            name = name.Trim();
            using (var context = new Entities())
            {
                var cardKindList = context.CardKind.Where(x => x.Name.Contains(name)).Take(count).Select(x => new CardKindDto
                {
                    ID = x.ID,
                    Name = x.Name,
                    Description = x.Description,
                    IsClosed = x.IsClosed,
                    TotalCount = x.TotalCount
                }).ToList();
                return cardKindList;
            }
        }

        public CommonSearchResult<CardKindDto> GetCardKindList(CardKindSearchDto searchDto)
        {
            var name = (searchDto.Name??"").Trim();

            using (var context = new Entities())
            {
                var query = context.CardKind.AsQueryable();

                if (searchDto.IsClosed.HasValue)
                {
                    query = query.Where(x => x.IsClosed == searchDto.IsClosed.Value);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.Name.Contains(name));
                }

                var fianlQuery = query.Select(x => new CardKindDto
                {
                    ID = x.ID,
                    Name = x.Name,
                    Description = x.Description,
                    TotalCount = x.TotalCount,
                    IsClosed = x.IsClosed
                }).OrderBy(x => x.ID);

                var countQuery = fianlQuery.FutureCount();

                var listQuery = fianlQuery.Skip((searchDto.PageIndex - 1) * searchDto.PageSize).Take(searchDto.PageSize).Future();

                var cardKinds = listQuery.ToList();

                return new CommonSearchResult<CardKindDto> { Result = cardKinds, TotalCount = countQuery.Value };
            }
        }
    }
}
