﻿using QIC.Farm.Service;
using QIC.Farm.Service.Common;
using QIC.Farm.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QIC.Infrastructure.Common.Utilities;
using log4net;
using QIC.Farm.WebApi.Common;
using System.Configuration;
using QIC.Farm.WebApi.Dtos;
using QIC.Farm.WebApi.Models;
using System.Web.Http.Results;
using System.Text;
using QIC.Farm.Service.Enums;

namespace QIC.Farm.WebApi.Controllers
{
    public class BackgroundController : ApiController
    {
        private ILog logger = Logger.GetLogger();

        private FarmService farmService = new FarmService();

        #region 后台登录

        [HttpGet, HttpPost]
        public LoginResult Login(string username, string password)
        {
            LoginResult result = new LoginResult();
            try
            {
                var pwd = ConfigurationManager.AppSettings[username];
                if (string.IsNullOrEmpty(pwd))
                {
                    result.Message = "用户不存在";
                    result.LoginError = Enums.LoginError.UserNotExist;
                    return result;
                }
                if (pwd != password)
                {
                    result.Message = "密码错误";
                    result.LoginError = Enums.LoginError.WrongPassword;
                    return result;
                }
                result.Message = "登录成功";
                result.LoginError = Enums.LoginError.Success;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "Login", Param = new { username = username, password = password }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "Login", Param = new { username = username, password = password }, Result = result.ToJson() });
                }
            }
            return result;
        }

        #endregion

        #region 家庭管理

        private void SetFamilyResultMessage(FamilyResult familyResult)
        {
            switch (familyResult.FamilyError)
            {
                case FamilyInfoError.MobileExist:
                    familyResult.Message = "手机号码已存在";
                    break;
                case FamilyInfoError.Success:
                    familyResult.Message = "成功";
                    break;
                case FamilyInfoError.SystemError:
                    familyResult.Message = "系统错误";
                    break;
            }
        }

        //添加家庭信息
        [HttpGet, HttpPost]
        public FamilyResult AddFamilyInfo(FamilyInfoDto familyInfo)
        {
            FamilyResult result = new FamilyResult();
            try
            {
                result = farmService.AddFamilyInfo(familyInfo);
                SetFamilyResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "AddFamilyInfo", Param = familyInfo.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "AddFamilyInfo", Param = familyInfo.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //删除家庭信息
        [HttpGet, HttpPost]
        public bool DeleteFamilyInfo(int familyID)
        {
            var result = false;
            try
            {
                result = farmService.DeleteFamilyInfo(familyID);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "DeleteFamilyInfo", Param = new { familyID = familyID }, Result = result }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "DeleteFamilyInfo", Param = new { familyID = familyID }, Result = result });
                }
            }
            return result;
        }

        //修改家庭信息
        [HttpGet, HttpPost]
        public FamilyResult UpdateFamilyInfo(FamilyInfoDto familyInfo)
        {
            FamilyResult result = new FamilyResult();
            try
            {
                result = farmService.UpdateFamilyInfo(familyInfo);
                SetFamilyResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "UpdateFamilyInfo", Param = familyInfo.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "UpdateFamilyInfo", Param = familyInfo.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //根据id查询家庭信息
        [HttpGet, HttpPost]
        public FamilyInfoDto GetFamilyInfoByID(int id)
        {
            FamilyInfoDto result = new FamilyInfoDto();
            try
            {
                result = farmService.GetFamilyInfoByID(id);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetFamilyInfoByID", Param = new { id = id }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetFamilyInfoByID", Param = new { id = id }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //根据会员名称查询家庭信息
        [HttpGet, HttpPost]
        public FamilyInfoDto GetFamilyInfoByName(string name)
        {
            FamilyInfoDto result = new FamilyInfoDto();
            try
            {
                result = farmService.GetFamilyInfoByName(name);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetFamilyInfoByName", Param = new { name = name }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetFamilyInfoByName", Param = new { name = name }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //根据会员名称模糊查询 指定数量的数据
        [HttpGet, HttpPost]
        public List<FamilyInfoDto> GetFamilyInfoListByNameFuzzy(string name, int count)
        {
            List<FamilyInfoDto> result = new List<FamilyInfoDto>();
            try
            {
                result = farmService.GetFamilyInfoListByNameFuzzy(name, count);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetFamilyInfoListByNameFuzzy", Param = new { name = name, count = count }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetFamilyInfoListByNameFuzzy", Param = new { name = name, count = count }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //按条件查询家庭信息
        [HttpGet, HttpPost]
        public CommonSearchResult<FamilyInfoDto> GetFamilyInfoList(FamilyInfoSearchDto searchParam)
        {
            CommonSearchResult<FamilyInfoDto> result = new CommonSearchResult<FamilyInfoDto>();
            try
            {
                result = farmService.GetFamilyInfoList(searchParam);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetFamilyInfoList", Param = searchParam.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetFamilyInfoList", Param = searchParam.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        #endregion

        #region 家庭收货地址管理

        //添加收货地址信息
        [HttpGet, HttpPost]
        public bool AddFamilyAddress(FamilyAddressDto familyAddress)
        {
            var result = false;
            try
            {
                result = farmService.AddFamilyAddress(familyAddress);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "AddFamilyAddress", Param = familyAddress.ToJson(), Result = result }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "AddFamilyAddress", Param = familyAddress.ToJson(), Result = result });
                }
            }
            return result;
        }

        //删除收货地址
        [HttpGet, HttpPost]
        public bool DeleteFamilyAddress(int addressID)
        {
            var result = false;
            try
            {
                result = farmService.DeleteFamilyAddress(addressID);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "DeleteFamilyAddress", Param = new { addressID = addressID }, Result = result }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "DeleteFamilyAddress", Param = new { addressID = addressID }, Result = result });
                }
            }
            return result;
        }

        //修改收货地址信息
        [HttpGet, HttpPost]
        public bool UpdateFamilyAddress(FamilyAddressDto familyAddress)
        {
            var result = false;
            try
            {
                result = farmService.UpdateFamilyAddress(familyAddress);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "UpdateFamilyAddress", Param = familyAddress.ToJson(), Result = result }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "UpdateFamilyAddress", Param = familyAddress.ToJson(), Result = result });
                }
            }
            return result;
        }

        //查询某个家庭的所有收货地址
        [HttpGet, HttpPost]
        public CommonSearchResult<FamilyAddressDto> GetFamilyAddressByFamilyID(int familyID)
        {
            CommonSearchResult<FamilyAddressDto> result = new CommonSearchResult<FamilyAddressDto>();
            try
            {
                result = farmService.GetFamilyAddressByFamilyID(familyID);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetFamilyAddressByFamilyID", Param = new { familyID = familyID }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetFamilyAddressByFamilyID", Param = new { familyID = familyID }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //条件查询所有收货地址
        [HttpGet, HttpPost]
        public CommonSearchResult<FamilyAddressDto> GetFamilyAddressList(FamilyAddressSearchDto searchParam)
        {
            CommonSearchResult<FamilyAddressDto> result = new CommonSearchResult<FamilyAddressDto>();
            try
            {
                result = farmService.GetFamilyAddressList(searchParam);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetFamilyAddressList", Param = searchParam.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetFamilyAddressList", Param = searchParam.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        #endregion

        #region 会员卡管理

        private void SetCardKindResultMessage(CardKindResult result)
        {
            switch (result.CardKindError)
            {
                case CardKindError.CardExist:
                    result.Message = "会员卡已存在";
                    break;
                case CardKindError.Success:
                    result.Message = "成功";
                    break;
                case CardKindError.SystemError:
                    result.Message = "系统错误";
                    break;
            }
        }

        //添加会员卡
        [HttpGet, HttpPost]
        public CardKindResult AddCardKind(CardKindDto cardKind)
        {
            CardKindResult result = new CardKindResult();
            try
            {
                result = farmService.AddCardKind(cardKind);
                SetCardKindResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "AddCardKind", Param = cardKind.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "AddCardKind", Param = cardKind.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //删除卡类型(注意不是正常删除 实为修改isClose状态)
        [HttpGet, HttpPost]
        public bool DeleteCardKind(int cardKindID)
        {
            var result = false;
            try
            {
                result = farmService.DeleteCardKind(cardKindID);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "DeleteCardKind", Param = new { cardKindID = cardKindID }, Result = result }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "DeleteCardKind", Param = new { cardKindID = cardKindID }, Result = result });
                }
            }
            return result;
        }

        //修改会员卡信息
        [HttpGet, HttpPost]
        public CardKindResult UpdateCardKind(CardKindDto cardKind)
        {
            CardKindResult result = new CardKindResult();
            try
            {
                result = farmService.UpdateCardKind(cardKind);
                SetCardKindResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "UpdateCardKind", Param = cardKind.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "UpdateCardKind", Param = cardKind.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //根据会员卡名称 模糊查询会员卡信息

        public List<CardKindDto> GetCardKindListByNameFuzzy(string name, int count)
        {
            List<CardKindDto> result = new List<CardKindDto>();
            try
            {
                result = farmService.GetCardKindListByNameFuzzy(name, count);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetCardKindListByNameFuzzy", Param = new { name = name, count = count }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetCardKindListByNameFuzzy", Param = new { name = name, count = count }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //条件查询所有会员卡信息
        [HttpGet, HttpPost]
        public CommonSearchResult<CardKindDto> GetCardKindList(CardKindSearchDto searchParam)
        {
            CommonSearchResult<CardKindDto> result = new CommonSearchResult<CardKindDto>();
            try
            {
                result = farmService.GetCardKindList(searchParam);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetCardKindList", Param = searchParam.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetCardKindList", Param = searchParam.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        #endregion

        #region 家庭拥有的会员卡管理

        private void SetFamilyCardResultMessage(FamilyCardResult result)
        {
            switch (result.FamilyCardError)
            {
                case FamilyCardError.FamilyCardExist:
                    result.Message = "会员卡已存在";
                    break;
                case FamilyCardError.Success:
                    result.Message = "成功";
                    break;
                case FamilyCardError.SystemError:
                    result.Message = "系统错误";
                    break;
                default:
                    break;
            }
        }

        //添加家庭会员卡
        [HttpGet, HttpPost]
        public FamilyCardResult AddFamilyCard(FamilyCardDto familyCard)
        {
            FamilyCardResult result = new FamilyCardResult();
            try
            {
                result = farmService.AddFamilyCard(familyCard);
                SetFamilyCardResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "AddFamilyCard", Param = familyCard.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "AddFamilyCard", Param = familyCard.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //删除家庭的会员卡
        [HttpGet, HttpPost]
        public bool DeleteFamilyCard(int familyCardID)
        {
            var result = false;
            try
            {
                result = farmService.DeleteFamilyCard(familyCardID);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "DeleteFamilyCard", Param = new { familyCardID = familyCardID }, Result = result }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "DeleteFamilyCard", Param = new { familyCardID = familyCardID }, Result = result });
                }
            }
            return result;
        }

        //修改家庭的会员卡信息
        [HttpGet, HttpPost]
        public FamilyCardResult UpdateFamilyCardKind(FamilyCardDto familyCard)
        {
            FamilyCardResult result = new FamilyCardResult();
            try
            {
                result = farmService.UpdateFamilyCardKind(familyCard);
                SetFamilyCardResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "UpdateCardKind", Param = familyCard.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "UpdateCardKind", Param = familyCard.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //条件查询所有家庭的会员卡信息
        [HttpGet, HttpPost]
        public CommonSearchResult<FamilyCardDto> GetFamilyCardList(FamilyCardSearchDto searchParam)
        {
            CommonSearchResult<FamilyCardDto> result = new CommonSearchResult<FamilyCardDto>();
            try
            {
                result = farmService.GetCardKindList(searchParam);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetFamilyCardList", Param = searchParam.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetFamilyCardList", Param = searchParam.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //查询某家庭的所有会员卡信息
        [HttpGet, HttpPost]
        public CommonSearchResult<FamilyCardDto> GetCardKindByFamily(int familyID)
        {
            CommonSearchResult<FamilyCardDto> result = new CommonSearchResult<FamilyCardDto>();
            try
            {
                result = farmService.GetCardKindByFamily(familyID);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetCardKindByFamily", Param = new { familyID = familyID }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetCardKindByFamily", Param = new { familyID = familyID }, Result = result.ToJson() });
                }
            }
            return result;
        }

        #endregion

        #region 农产品管理

        private void SetProductResultMessage(ProductResult result)
        {
            switch (result.ProductError)
            {
                case ProductError.ProductExist:
                    result.Message = "产品已存在";
                    break;
                case ProductError.Success:
                    result.Message = "成功";
                    break;
                case ProductError.SystemError:
                    result.Message = "系统错误";
                    break;
                default:
                    break;
            }
        }

        //添加产品信息
        [HttpGet, HttpPost]
        public ProductResult AddProduct(ProductDto product)
        {
            ProductResult result = new ProductResult();
            try
            {
                var path = HtmlReportHelper.CreateHtmlReport("ProductCooking\\" + product.Name, product.Cooking);
                product.Cooking = path;

                result = farmService.AddProduct(product);
              
                SetProductResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "AddProduct", Param = product.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "AddProduct", Param = product.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //删除产品信息（实为修改IsClosed状态）
        [HttpGet, HttpPost]
        public bool DeleteProduct(int productID)
        {
            var result = false;
            try
            {
                result = farmService.DeleteProduct(productID);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "DeleteProduct", Param = new { productID = productID }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "DeleteProduct", Param = new { productID = productID }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //修改产品信息
        [HttpGet, HttpPost]
        public ProductResult UpdateProduct(ProductDto product)
        {
            ProductResult result = new ProductResult();
            try
            {
                var path = HtmlReportHelper.CreateHtmlReport("ProductCooking\\" + product.Name, product.Cooking);
                product.Cooking = path;
                result = farmService.UpdateProduct(product);
                SetProductResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "UpdateProduct", Param = product.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "UpdateProduct", Param = product.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //获取所有有效产品
        [HttpGet, HttpPost]
        public List<ProductDto> GetAllProduct()
        {
            List<ProductDto> result = new List<ProductDto>();
            try
            {
                result = farmService.GetAllProduct();                
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetAllProduct", Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetAllProduct", Result = result.ToJson() });
                }
            }
            return result;
        }

        //查询产品做法
        [HttpGet, HttpPost]
        public string GetProductCookingStr(int id)
        {
            string str = string.Empty;
            try
            {
                var result = farmService.GetProductByID(id);
                str = HtmlReportHelper.ReaderHtmlReportContent(result.Cooking);
                return str;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetProductCookingStr", Param = new { id = id }, Result = str }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetProductCookingStr", Param = new { id = id }, Result = str });
                }
            }
            return str;
        }

        //获取产品列表
        [HttpGet, HttpPost]
        public CommonSearchResult<ProductDto> GetProductList(ProductSearchDto searchParam)
        {
            CommonSearchResult<ProductDto> result = new CommonSearchResult<ProductDto>();
            try
            {
                result = farmService.GetProductList(searchParam);               
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetProductList", Param = searchParam.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetProductList", Param = searchParam.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        #endregion

        #region 产品报告管理

        private void SetProductReportResultMessage(ProductReportResult result)
        {
            switch (result.ProductReportError)
            {
                case ProductReportError.ReportExist:
                    result.Message = "报告已存在";
                    break;
                case ProductReportError.Success:
                    result.Message = "成功";
                    break;
                case ProductReportError.SystemError:
                    result.Message = "系统错误";
                    break;
                default:
                    break;
            }
        }

        //添加产品报告信息
        [HttpGet, HttpPost]
        public ProductReportResult AddProductReport(ProductReportDto productReport)
        {
            ProductReportResult result = new ProductReportResult();
            try
            {
                var path = HtmlReportHelper.CreateHtmlReport("ProductReport\\" + productReport.Name, productReport.Description);
                productReport.Description = path;

                result = farmService.AddProductReport(productReport);              
                SetProductReportResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "AddProductReport", Param = productReport.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "AddProductReport", Param = productReport.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //删除产品报告信息（实为修改IsClosed状态）
        [HttpGet, HttpPost]
        public bool DeleteProductReport(int productReportID)
        {
            var result = false;
            try
            {
                result = farmService.DeleteProductReport(productReportID);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "DeleteProductReport", Param = new { productReportID = productReportID }, Result = result }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "DeleteProductReport", Param = new { productReportID = productReportID }, Result = result });
                }
            }
            return result;
        }

        //修改产品报告信息
        [HttpGet, HttpPost]
        public ProductReportResult UpdateProductReport(ProductReportDto productReport)
        {
            ProductReportResult result = new ProductReportResult();
            try
            {
                var path = HtmlReportHelper.CreateHtmlReport("ProductReport\\" + productReport.Name, productReport.Description);
                productReport.Description = path;
                result = farmService.UpdateProductReport(productReport);
                SetProductReportResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "UpdateProductReport", Param = productReport.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "UpdateProductReport", Param = productReport.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //获取所有有效产品报告
        [HttpGet, HttpPost]
        public List<ProductReportDto> GetAllProductReport()
        {
            List<ProductReportDto> result = new List<ProductReportDto>();
            try
            {
                result = farmService.GetAllProductReport();               
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetAllProductReport", Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetAllProductReport", Result = result.ToJson() });
                }
            }
            return result;
        }

        //获取产品报告列表
        [HttpGet, HttpPost]
        public CommonSearchResult<ProductReportDto> GetProductReportList(ProductReportSearchDto searchParam)
        {
            CommonSearchResult<ProductReportDto> result = new CommonSearchResult<ProductReportDto>();
            try
            {
                result = farmService.GetProductReportList(searchParam);               
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetProductReportList", Param = searchParam.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetProductReportList", Param = searchParam.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //查询溯源报告htmlContent
        [HttpGet, HttpPost]
        public string GetProductReportStr(int id)
        {
            string str = string.Empty;
            try
            {
                var result = farmService.GetProductReportByID(id);
                str = HtmlReportHelper.ReaderHtmlReportContent(result.Description);
                return str;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetProductReportStr", Param = new { id = id }, Result = str }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetProductReportStr", Param = new { id = id }, Result = str });
                }
            }
            return str;
        }

        #endregion

        #region 添加和修改配送

        private void SetDistributionResultMessage(DistributionResult result)
        {
            switch (result.DistributionError)
            {
                case DistributionError.Success:
                    result.Message = "成功";
                    break;
                case DistributionError.SystemError:
                    result.Message = "系统错误";
                    break;
            }
        }

        private void SetDistributionDetailResultMessage(DistributionDetailResult result)
        {
            switch (result.DistributionDetailError)
            {
                case DistributionDetailError.CountNotEnough:
                    result.Message = "该会员卡配送次数不足";
                    break;
                case DistributionDetailError.Success:
                    result.Message = "成功";
                    break;
                case DistributionDetailError.SystemError:
                    result.Message = "系统错误";
                    break;
            }
        }

        //添加配送单基本信息
        [HttpGet, HttpPost]
        public DistributionResult AddDistribution(DistributionDto distribution)
        {
            var result = new DistributionResult();
            try
            {
                result = farmService.AddDistribution(distribution);
                if (result.DistributionError == DistributionError.Success) DistributionIDGenerator.Increment();
                SetDistributionResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "AddDistribution", Param = distribution.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "AddDistribution", Param = distribution.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //添加配送单
        [HttpGet, HttpPost]
        public DistributionDetailResult AddDistributionDetail(DistributionDetailDto distributionDetail)
        {
            DistributionDetailResult result = new DistributionDetailResult();
            try
            {
                result = farmService.AddDistributionDetail(distributionDetail);
                SetDistributionDetailResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "AddDistributionDetail", Param = distributionDetail.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "AddDistributionDetail", Param = distributionDetail.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //通过单号删除配送单详情
        [HttpGet, HttpPost]
        public bool DeleteDistributionDetail(int id)
        {
            var result = false;
            try
            {
                result = farmService.DeleteDistributionDetail(id);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "DeleteDistributionDetail", Param = new { distributionID = id }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "DeleteDistributionDetail", Param = new { distributionID = id }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //获取配送单号
        [HttpGet, HttpPost]
        public string GetDistributionID()
        {
            return DistributionIDGenerator.GetDistributionID();
        }

        //添加配送单(包括配送单的详情)
        //[HttpGet, HttpPost]
        //public AddDistributionAndDetailResult AddDistributionAndDetails(DistributionModel model)
        //{
        //    AddDistributionAndDetailResult result = new AddDistributionAndDetailResult();
        //    try
        //    {
        //        try
        //        {
        //            var success = farmService.AddDistribution(model.Distribution);
        //            if (!success)
        //            {
        //                result.Message = "添加配送单失败";
        //                result.AddDistributionAndDetailError = Enums.AddDistributionAndDetailError.SystemError;
        //                return result;
        //            }
        //            else DistributionIDGenerator.Increment();
        //        }
        //        catch (Exception ex)
        //        {
        //            result.Message = "添加配送单失败";
        //            result.AddDistributionAndDetailError = Enums.AddDistributionAndDetailError.SystemError;
        //            return result;//创建出现异常
        //        }

        //        List<int> familyCardIds = new List<int>();
        //        foreach (var item in model.DistributionDetails)
        //        {
        //            //插入数据到DistributionDetail
        //            item.DistributionID = model.Distribution.ID;
        //            var r = farmService.AddDistributionDetail(item);
        //            if (r)
        //            {
        //                if (!familyCardIds.Contains(item.FamilyCardID))
        //                {
        //                    familyCardIds.Add(item.FamilyCardID);
        //                }
        //            }
        //        }

        //        //扣除家庭的会员卡配送次数
        //        farmService.UpdateTotalCount(familyCardIds);

        //        result.Message = "添加成功";
        //        result.AddDistributionAndDetailError = Enums.AddDistributionAndDetailError.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (logger.IsErrorEnabled)
        //        {
        //            logger.Error(new { Method = "AddDistributionAndDetails", Param = model.ToJson(), Result = result }, ex);
        //        }
        //    }
        //    finally
        //    {
        //        if (logger.IsInfoEnabled)
        //        {
        //            logger.Info(new { Method = "AddDistributionAndDetails", Param = model.ToJson(), Result = result });
        //        }
        //    }
        //    return result;
        //}

        //[HttpGet, HttpPost]
        //public AddDistributionAndDetailResult UpdateDistributionAndDetails(DistributionModel model)
        //{
        //    AddDistributionAndDetailResult result = new AddDistributionAndDetailResult();
        //    try
        //    {
        //        var updateSuccess = farmService.UpdateDistribution(model.Distribution);

        //        var details = farmService.GetDetailByDistributionID(model.Distribution.ID);

        //        var updateIdList = new List<long>();

        //        foreach (var detailToUpdate in model.DistributionDetails)
        //        {
        //            updateIdList.Add(detailToUpdate.ID);
        //            farmService.UpdateDistributionDetail(detailToUpdate);
        //        }

        //        foreach (var detail in details)
        //        {
        //            if (!updateIdList.Contains(detail.ID))
        //            {
        //                farmService.DeleteDistributionDetail(detail.ID);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (logger.IsErrorEnabled)
        //        {
        //            logger.Error(new { Method = "UpdateDistributionAndDetails", Param = model.ToJson(), Result = result }, ex);
        //        }
        //    }
        //    finally
        //    {
        //        if (logger.IsInfoEnabled)
        //        {
        //            logger.Info(new { Method = "UpdateDistributionAndDetails", Param = model.ToJson(), Result = result });
        //        }
        //    }
        //    return result;
        //}

        //删除配送单(详情也删除)

        [HttpGet, HttpPost]
        public bool UpdateDistribution(DistributionDto distribution)
        {
            var result = false;
            try
            {
                result = farmService.UpdateDistribution(distribution);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "UpdateDistribution", Param = distribution.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "UpdateDistribution", Param = distribution.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        [HttpGet, HttpPost]
        public bool DeleteDistribution(string id)
        {
            var result = false;
            try
            {
                var delDistributionResult = farmService.DeleteDistribution(id);
                if (delDistributionResult)
                {
                    result = farmService.DeleteDistributionDetail(id);
                }
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "DeleteDistribution", Param = new { distributionID = id }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "DeleteDistribution", Param = new { distributionID = id }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //通过id查询配送单所有信息
        [HttpGet, HttpPost]
        public DistributionDto GetDistributionByID(string id)
        {
            DistributionDto result = new DistributionDto();
            try
            {
                result = farmService.GetDistributionByID(id);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetDistributionByID", Param = new { id = id }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetDistributionByID", Param = new { id = id }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //条件查询所有配送单
        [HttpGet, HttpPost]
        public CommonSearchResult<DistributionDto> GetDistributionList(DistributionSearchDto searchParam)
        {
            CommonSearchResult<DistributionDto> result = new CommonSearchResult<DistributionDto>();
            try
            {
                result = farmService.GetDistributionList(searchParam);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetDistributionList", Param = searchParam.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetDistributionList", Param = searchParam.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //修改配送单
        [HttpGet, HttpPost]
        public DistributionDetailResult UpdateDistributionDetail(DistributionDetailDto distributionDetail)
        {
            DistributionDetailResult result = new DistributionDetailResult();
            try
            {
                result = farmService.UpdateDistributionDetail(distributionDetail);
                SetDistributionDetailResultMessage(result);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "UpdateDistributionDetail", Param = distributionDetail.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "UpdateDistributionDetail", Param = distributionDetail.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }

        //查询某个配送单的配送详情列表
        [HttpGet, HttpPost]
        public List<DistributionDetailDto> GetDetailByDistributionID(string distributionID)
        {
            List<DistributionDetailDto> result = new List<DistributionDetailDto>();
            try
            {
                result = farmService.GetDetailByDistributionID(distributionID);
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetDetailByDistributionID", Param = new { distributionID = distributionID }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetDetailByDistributionID", Param = new { distributionID = distributionID }, Result = result.ToJson() });
                }
            }
            return result;
        }

        #endregion
    }
}
