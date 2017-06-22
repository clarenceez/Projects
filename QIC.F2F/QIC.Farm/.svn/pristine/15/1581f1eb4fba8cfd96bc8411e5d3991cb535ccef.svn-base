using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QIC.Farm.WebApi.Common
{
    public class Logger
    {
        static Logger()
        {
            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "App_Data\\Configurations\\log4net.config");
            if (System.IO.File.Exists(path))
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            }
            else
            {
                log4net.Config.XmlConfigurator.Configure();
            }
        }

        public static ILog GetLogger()
        {
            return LogManager.GetLogger("FarmApiLogger");
        }  
    }
}