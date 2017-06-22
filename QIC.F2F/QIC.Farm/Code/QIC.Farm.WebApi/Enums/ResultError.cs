using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIC.Farm.WebApi.Enums
{
    public enum ResultError
    {
        /// <summary>
        /// 系统错误
        /// </summary>
        SystemError = 0,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// Token与服务器存在时差
        /// </summary>
        TokenServerTimeTifference,
        /// <summary>
        /// Token签名错误
        /// </summary>
        TokenSignatureError
    }
}