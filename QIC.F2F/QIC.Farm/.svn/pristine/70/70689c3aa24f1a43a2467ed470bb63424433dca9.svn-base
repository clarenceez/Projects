using QIC.Farm.Service.Dto;
using QIC.Farm.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace QIC.Farm.Service.Dal
{
    public class FamilyAddressDal
    {
        public bool Add(FamilyAddressDto familyAddress)
        {
            using (var context = new Entities())
            {
                context.FamilyAddress.Add(new FamilyAddress
                {
                    Contacter = familyAddress.Contacter,
                    Phone = familyAddress.Phone,
                    Address = familyAddress.Address,
                    FamilyID = familyAddress.FamilyID,
                    PostCode = familyAddress.PostCode,
                    IsClosed = familyAddress.IsClosed
                });

                var count = context.SaveChanges();
                return count > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var context = new Entities())
            {
                var result = context.FamilyAddress.Where(x => x.ID == id).Delete();
                return result > 0;
            }
        }

        public bool Update(FamilyAddressDto familyAddress)
        {
            var result = new FamilyCardResult();
            using (var context = new Entities())
            {
                var count = context.FamilyAddress.Where(x => x.ID == familyAddress.ID).Update(x => new FamilyAddress
                {
                    Contacter = familyAddress.Contacter,
                    Phone = familyAddress.Phone,
                    Address = familyAddress.Address,
                    FamilyID = familyAddress.FamilyID,
                    PostCode = familyAddress.PostCode,
                    IsClosed = familyAddress.IsClosed
                });

                return count > 0;
            }
        }

        public CommonSearchResult<FamilyAddressDto> GetAddressListByFamilyID(int familyID)
        {
            using (var context = new Entities())
            {
                var query = from f in context.FamilyInfo
                            join fa in context.FamilyAddress on f.ID equals fa.FamilyID
                            where fa.FamilyID == familyID
                            select new
                            {
                                ID = fa.ID,
                                FamilyName = f.Name,
                                Contacter = fa.Contacter,
                                Phone = fa.Phone,
                                Address = fa.Address,
                                PostCode = fa.PostCode,
                                IsClosed = fa.IsClosed
                            };

                var fianlQuery = query.Select(x => new FamilyAddressDto
                {
                    ID = x.ID,
                    FamilyName = x.FamilyName,
                    Contacter = x.Contacter,
                    Phone = x.Phone,
                    Address = x.Address,
                    PostCode = x.PostCode,
                    IsClosed = x.IsClosed
                }).OrderBy(x => x.ID);

                var countQuery = fianlQuery.FutureCount();

                var listQuery = fianlQuery.Future();

                var addressList = listQuery.ToList();

                return new CommonSearchResult<FamilyAddressDto> { Result = addressList, TotalCount = countQuery.Value };

            }
        }

        public CommonSearchResult<FamilyAddressDto> GetFamilyAddressList(FamilyAddressSearchDto searchDto)
        {
            var familyName = (searchDto.FamilyName ?? "").Trim();

            using (var context = new Entities())
            {
                var query = from f in context.FamilyInfo
                            join fa in context.FamilyAddress on f.ID equals fa.FamilyID
                            select new
                            {
                                ID = fa.ID,
                                FamilyName = f.Name,
                                Contacter = fa.Contacter,
                                Phone = fa.Phone,
                                Address = fa.Address,
                                PostCode = fa.PostCode,
                                IsClosed = fa.IsClosed
                            };

                if (searchDto.IsClosed.HasValue)
                {
                    query = query.Where(x => x.IsClosed == searchDto.IsClosed.Value);
                }

                if (!string.IsNullOrEmpty(familyName))
                {
                    query = query.Where(x => x.FamilyName.Contains(familyName));
                }

                var fianlQuery = query.Select(x => new FamilyAddressDto
                {
                    ID = x.ID,
                    FamilyName = x.FamilyName,
                    Contacter = x.Contacter,
                    Phone = x.Phone,
                    Address = x.Address,
                    PostCode = x.PostCode,
                    IsClosed = x.IsClosed
                }).OrderBy(x => x.ID);

                var countQuery = fianlQuery.FutureCount();

                var listQuery = fianlQuery.Skip((searchDto.PageIndex - 1) * searchDto.PageSize).Take(searchDto.PageSize).Future();

                var addressList = listQuery.ToList();

                return new CommonSearchResult<FamilyAddressDto> { Result = addressList, TotalCount = countQuery.Value };
            }
        }
    }
}
