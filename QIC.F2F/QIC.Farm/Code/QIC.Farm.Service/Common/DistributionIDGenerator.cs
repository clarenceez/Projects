using QIC.Farm.Service.Model;
using QIC.Infrastructure.Cache.CacheManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QIC.Farm.Service.Common
{
    public class DistributionIDGenerator
    {
        //当前数据库最大编号：每日会重新赋值为0
        private static int CurrentMaximum = 0;

        private static ICacheManager Cache = CacheFactory.GetCache("FarmCache");
        
        private static int CacheTime = 24 * 60 * 60;

        //获取缓存key
        private static string GetCacheKey(string date)
        { 
            //计数器每天 从0开始累加 用时间作为缓存key 每天进行判断          
            return "DistributionIDCounter_" + date;
        }

        //从数据库中读取最大的配送单编号
        private static string GetMaxDistributionIDFromDB(string date)
        {
            using (var context = new Entities())
            {
                return context.Distribution.Where(x => x.ID.Contains(date)).Select(x => x.ID).Max();
            }
        }

        public static string GetDistributionID()
        {
            string date = DateTime.Now.ToString("yyMMdd");
            string cacheKey = GetCacheKey(date);

            //每日第一个配送单CurrentMaximum都会重新赋值为0
            CurrentMaximum = Cache.Get<int>(cacheKey);

            //如果 count = 0 需从数据库读取一次今日最大的ID 以确保累计器0是正确的(即今日第一个订单)
            if (CurrentMaximum == 0)
            {
                var id = GetMaxDistributionIDFromDB(date);
                //如果id不为空 则说明今天已经有过订单数据 缓存的0是错误的须重新赋值
                if (!string.IsNullOrEmpty(id)) CurrentMaximum = Convert.ToInt32(id.Substring(6, id.Length - 6));
            }

            //临时count：如果配送单取消创建，不会影响累加值，订单创建成功,则调用Increment方法
            int tempCount = CurrentMaximum + 1;
            return date + tempCount.ToString("d4");
        }

        //成功创建了配送单 则需要将计数器加1
        public static void Increment()
        {
            string date = DateTime.Now.ToString("yyMMdd");
            string cacheKey = GetCacheKey(date);
            Interlocked.Increment(ref CurrentMaximum);
            Cache.Set(cacheKey, CurrentMaximum, CacheTime, false);
        }
    }
}
