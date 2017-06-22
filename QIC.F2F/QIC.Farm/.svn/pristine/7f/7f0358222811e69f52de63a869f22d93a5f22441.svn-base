using QIC.Farm.WebApi.Dtos;
using QIC.Farm.WebApi.Enums;
using QIC.Infrastructure.Common.Safety.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;

namespace QIC.Farm.WebApi.Common
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private log4net.ILog logger = Logger.GetLogger();
        //重写基类的异常处理方法
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            Type exceptionType = actionExecutedContext.Exception.GetType();
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.NotImplemented;
            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                httpResponseMessage.StatusCode = HttpStatusCode.RequestTimeout;
            }
            else if (exceptionType.Name == "TokenException")
            {
                TokenValidateResult dto = new TokenValidateResult();
                System.Reflection.PropertyInfo propertyInfo = exceptionType.GetProperty("AuthError"); //获取指定名称的属性
                AuthResult result = (AuthResult)propertyInfo.GetValue(actionExecutedContext.Exception, null); //获取属性值
                switch (result)
                {
                    case AuthResult.TokenInvalid:
                        dto.Result = TokenError.TokenInvalid; break;
                    case AuthResult.SignatureError:
                        dto.Result = TokenError.SignatureError; break;
                    case AuthResult.TokenDecodeError:
                        dto.Result = TokenError.TokenDecodeError; break;
                    case AuthResult.OverTime:
                        dto.Result = TokenError.OverTime; break;
                    case AuthResult.PlayloadError:
                        dto.Result = TokenError.PlayloadError; break;
                }
                httpResponseMessage.Content = new ObjectContent(typeof(TokenValidateResult), dto, new JsonMediaTypeFormatter());
            }           
            else
            {
                TokenValidateResult dto = new TokenValidateResult()
                {
                    Result = TokenError.SystemError
                };
                httpResponseMessage.Content = new ObjectContent(typeof(TokenValidateResult), dto, new JsonMediaTypeFormatter());
            }
            if (logger.IsErrorEnabled) logger.Error(new { Action = "OnException", Param = actionExecutedContext.Exception });
            actionExecutedContext.Response = httpResponseMessage;
            base.OnException(actionExecutedContext);
        }
    }
}