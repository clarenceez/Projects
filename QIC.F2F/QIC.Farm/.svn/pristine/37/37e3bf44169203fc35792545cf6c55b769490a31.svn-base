using QIC.Infrastructure.Common.Safety;
using QIC.Infrastructure.Common.Safety.Dto;
using QIC.Infrastructure.TokenSafetyParamClient;
using QIC.Infrastructure.TokenSafetyParamClient.TokenService;
using QIC.Infrastructure.WebFramework.Model;
using System;
using System.Web;
using QIC.Infrastructure.WebFramework.Token;
using System.Configuration;

namespace QIC.Farm.WebApi.Common
{
    public abstract class TokenHelper : ITokenHelper
    {
        protected byte mcID;

        protected int scID;

        private string agentKey = "QIC";

        private string tokenKey = "Authorization";

        private string tokenHeaderValue;

        private string agentHeaderValue;

        private TokenManagement<PayloadDto> tokerManager;

        private PayloadResult<PayloadDto> payload;

        private bool allowTouristMode;

        protected virtual string AgentKey
        {
            get
            {
                return this.agentKey;
            }
        }

        protected virtual string TokenKey
        {
            get
            {
                return this.tokenKey;
            }
        }

        private TokenHelper(bool allowTourist)
        {
            this.allowTouristMode = allowTourist;
        }

        public TokenHelper(HttpContext context, bool allowTourist)
        {
            this.allowTouristMode = allowTourist;
            this.AnalyzeParam(context);
        }

        public TokenHelper(HttpContextBase context, bool allowTourist)
        {
            this.allowTouristMode = allowTourist;
            this.AnalyzeParam(context);
        }


        public virtual void TokenVerification()
        {
            if (this.tokerManager == null)
            {
                throw new ArgumentException("Toker manager is null.", "tokerManager");
            }
            this.payload = this.tokerManager.ValidateToken(this.tokenHeaderValue);
            if (this.payload.Result != AuthResult.Success)
            {
                TokenException.ThrowTokenException(this.payload.Result);
            }
            PayloadDto dto = this.payload.Payload;
            this.CheckOpenID(dto);
        }

        protected virtual void CheckOpenID(PayloadDto dto)
        {
            if (dto.tourist)
            {
                if (this.allowTouristMode)
                {
                    return;
                }
                TokenException.ThrowTokenException(AuthResult.RefuseTourist);
            }
            if (string.IsNullOrEmpty(dto.account))
            {
                TokenException.ThrowTokenException(AuthResult.AccountInvalid);
            }
        }

        public virtual PayloadResult<PayloadDto> GetPayload()
        {
            if (this.payload != null)
            {
                return this.payload;
            }
            if (this.tokerManager == null)
            {
                throw new ArgumentException("Toker manager is null.", "tokerManager");
            }
            return this.tokerManager.ValidateToken(this.tokenHeaderValue);
        }

        private void GetTokenManagement()
        {
            TokenParamModel tokenParam = new TokenParamModel
            {
                SecretKey = ConfigurationManager.AppSettings["SecretKey"],
                TokenExpiredTime = Convert.ToUInt64(ConfigurationManager.AppSettings["expiredTime"])
            };
            this.tokerManager = new TokenManagement<PayloadDto>(tokenParam.SecretKey,tokenParam.TokenExpiredTime);
        }

        private void AnalyzeParam(HttpContext context)
        {
            string value = context.Request.Headers[this.TokenKey];
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Token", "Token code is null.");
            }
            string value2 = context.Request.Headers[this.AgentKey];
            if (string.IsNullOrEmpty(value2))
            {
                throw new ArgumentNullException("AgentCode", "Agent code is null.");
            }
            this.tokenHeaderValue = value;
            this.agentHeaderValue = value2;
            this.GetTokenManagement();
        }

        private void AnalyzeParam(HttpContextBase context)
        {
            string value = context.Request.Headers[this.TokenKey];
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("Token", "Token code is null.");
            }
            string value2 = context.Request.Headers[this.AgentKey];
            if (string.IsNullOrEmpty(value2))
            {
                throw new ArgumentNullException("AgentCode", "Agent code is null.");
            }
            this.tokenHeaderValue = value;
            this.agentHeaderValue = value2;
            this.GetTokenManagement();
        }       
    }
}
