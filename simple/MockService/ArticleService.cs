using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oxygen.MulitlevelCache;

namespace AppSimple.MockService
{
    internal class ArticleService : IArticleService
    {
        [SystemCached]
        public string GetUserArticleTitle(string username)
        {
            Thread.Sleep(5);//模拟数据库请求
            return $"{username}_article1";
        }

        [SystemCached]
        public async Task<string> GetUserArticleTitleAsync(string username)
        {
            await Task.Delay(5);//模拟数据库请求
            return $"{username}_article1";
        }
    }
}
