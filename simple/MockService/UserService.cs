using Oxygen.MulitlevelCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSimple.MockService
{
    internal class UserService : IUserService
    {
        [SystemCached]
        public string GetUserName(int id)
        {
            Thread.Sleep(5);//模拟数据库请求
            return $"{id}_aaa";
        }

        [SystemCached]
        public async Task<string> GetUserNameAsync(int id)
        {
            await Task.Delay(5);//模拟数据库请求
            return $"{id}_aaa";
        }
    }
}
