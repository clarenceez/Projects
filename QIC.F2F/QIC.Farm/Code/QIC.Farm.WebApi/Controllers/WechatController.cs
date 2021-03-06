﻿using log4net;
using QIC.Farm.Service;
using QIC.Farm.Service.Dto;
using QIC.Farm.WebApi.Common;
using QIC.Farm.WebApi.Dtos;
using QIC.Farm.WebApi.Enums;
using QIC.Infrastructure.ValidCodeManagement;
using QIC.Infrastructure.ValidCodeManagement.Enums;
using QIC.Infrastructure.WebApiFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QIC.Infrastructure.Common.Utilities;
using QIC.Infrastructure.ValidCodeManagement.Dtos;
using QIC.Infrastructure.WebFramework.Model;
using QIC.Infrastructure.Common.Safety;
using QIC.Infrastructure.WebFramework.Token;
using QIC.Infrastructure.Common.Safety.Dto;
using System.Configuration;
using QIC.Farm.WebApi.Models;
using System.IO;
using System.Net.Http.Headers;

namespace QIC.Farm.WebApi.Controllers
{
    public class WechatController : ApiController
    {
        private ILog logger = Logger.GetLogger();

        private FarmService farmService = new FarmService();

        public TokenResultDto GetToken(string openID)
        {
            TokenResultDto tokenResultDto = new TokenResultDto();
            TokenParamModel tokenParam = new TokenParamModel
            {
                SecretKey = ConfigurationManager.AppSettings["SecretKey"],
                TokenExpiredTime = Convert.ToUInt64(ConfigurationManager.AppSettings["expiredTime"])
            };
            TokenManagement<PayloadDto> tokenManagement = new TokenManagement<PayloadDto>(tokenParam.SecretKey,tokenParam.TokenExpiredTime);

            long timestamp = DateTime.Now.ToUnixTimeSpan();
            var payload = new PayloadDto
            {
                iat = timestamp,
                account = openID
            };
            TokenResult tokenResult = tokenManagement.CreatToken(timestamp, payload);
            if (tokenResult.Result != AuthResult.Success)
            {
                switch (tokenResult.Result)
                {
                    case AuthResult.OverTime:
                        tokenResultDto.Result = ResultError.TokenServerTimeTifference; break;
                    case AuthResult.SignatureError:
                        tokenResultDto.Result = ResultError.TokenSignatureError; break;
                    default:
                        tokenResultDto.Result = ResultError.SystemError; break;
                };
                return tokenResultDto;
            }
            tokenResultDto.Result = ResultError.Success;
            tokenResultDto.Token = tokenResult.Token;
            return tokenResultDto;
        }

        //登录
        [HttpGet, HttpPost]
        public WechatLoginResult Login(string code)
        {
            WechatLoginResult result = new WechatLoginResult();

            try
            {
                var openID = WechatHelper.GetWechatOpenID(code);
                //if (string.IsNullOrEmpty(openID))
                //{
                //    result.WechatLoginError = WechatLoginError.InvalidCode;
                //    return result;
                //}

                result.OpenID = "123456";// openID;
                result.Token = "Token";
                //如果已经绑定过手机号 返回token和openid,如果没有绑定 则返回Openid
                var exist = farmService.IsUserExist(openID);
                if (exist)
                {
                    result.Token = GetToken(openID).Token;
                }
                result.WechatLoginError = WechatLoginError.Success;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "Login", Param = new { code = code }, Result = result }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "Login", Param = new { code = code }, Result = result });
                }
            }
            return result;
        }


        //判断会员是否存在
        [HttpGet, HttpPost]
        public BooleanResult IsMemberExist(string openID)
        {
            BooleanResult result = BooleanResult.False;
            try
            {
                var exist = farmService.IsUserExist(openID);
                result = exist ? BooleanResult.True : BooleanResult.False;
            }
            catch (Exception ex)
            {
                result = BooleanResult.SystemError;
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "IsMemberExist", Param = new { openID = openID }, Result = result }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "IsMemberExist", Param = new { openID = openID }, Result = result });
                }
            }
            return result;
        }


        //通过手机号查询会员信息        
        [HttpGet, HttpPost]
        public FamilyInfoDto GetUserInfoByPhoneNumber(string phoneNumber)
        {
            FamilyInfoDto result = null;
            try
            {
                result = farmService.GetFamilyInfoByPhoneNumber(phoneNumber);
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetUserInfoByPhoneNumber", Param = new { phoneNumber = phoneNumber }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetUserInfoByPhoneNumber", Param = new { phoneNumber = phoneNumber }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //获取绑定用户的验证码
        [HttpGet, HttpPost]
        public CommonResult<string> GetMobileCaptchaForBindUser(string phoneNumber)
        {
            CommonResult<string> result = new CommonResult<string>();
            try
            {
                var mobileValidCode = ValidCodeFactory.GetValidCode(SendMethod.Mobile);
                var captchaResult = mobileValidCode.SendValidCode("BindUser", phoneNumber, "zh-cn");
                result.Data = captchaResult.ValidCode;
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetMobileCaptchaForBindUser", Param = new { phoneNumber = phoneNumber }, Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetMobileCaptchaForBindUser", Param = new { phoneNumber = phoneNumber }, Result = result.ToJson() });
                }
            }
            return result;
        }

        //绑定用户
        [HttpGet, HttpPost]
        public BindUserResult BindUser(BindUserModel model)
        {
            BindUserResult result = new BindUserResult();
            try
            {

                if (model.OpenID == "null" || string.IsNullOrEmpty(model.OpenID))
                {
                    result.BindUserError = BindUserError.OpenIDIsNull;
                    result.Message = "OpenID为Null";
                    return result;
                }

                //验证验证码
                var validCode = ValidCodeFactory.GetValidCode(SendMethod.Mobile);
                if (!validCode.CheckValidCode("BindUser", model.PhoneNumber, model.Captcha))
                {
                    result.BindUserError = BindUserError.CaptchaError;
                    result.Message = "验证码错误";
                    return result;
                }

                //给对应手机号的会员加上openID 和昵称
                string nickname = WechatHelper.GetWechatNickNameByOpenID(model.OpenID);

                var r = farmService.SetOpenIDByPhone(model.PhoneNumber, model.OpenID,nickname);
                if (r)
                {
                    result.Message = "绑定成功";
                    result.Token = GetToken(model.OpenID).Token;
                    result.BindUserError = BindUserError.Success;
                }
                else
                {
                    result.Message = "系统错误";
                    result.BindUserError = BindUserError.SystemError;
                }
            }
            catch (Exception ex)
            {
                result.BindUserError = BindUserError.SystemError;
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "BindUser", Param = model.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "BindUser", Param = model.ToJson(), Result = result.ToJson() });
                }
            }

            return result;
        }

        //查询某个家庭的配送单(包括配送单的详情)
        [HttpPost]
        //[Token(typeof(MemTokenHelper), false)]
        public CommonSearchResult<DistributionDto> GetDistributionListByOpenID(DistributionSearchModel model)
        {
            CommonSearchResult<DistributionDto> result = new CommonSearchResult<DistributionDto>();
            try
            {
                result = farmService.GetDistributionListByOpenID(model.OpenID, model.PageIndex, model.PageSize);
                if (result.Result == null) { result.Result = new List<DistributionDto>(); }
                return result;
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(new { Method = "GetDistributionListByOpenID", Param = model.ToJson(), Result = result.ToJson() }, ex);
                }
            }
            finally
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(new { Method = "GetDistributionListByOpenID", Param = model.ToJson(), Result = result.ToJson() });
                }
            }
            return result;
        }      
    }
}
