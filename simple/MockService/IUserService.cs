using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSimple.MockService
{
    internal interface IUserService
    {
        string GetUserName(int id);
        Task<string> GetUserNameAsync(int id);
    }
}
