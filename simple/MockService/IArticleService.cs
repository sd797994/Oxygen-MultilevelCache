using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSimple.MockService
{

    internal interface IArticleService
    {
        string GetUserArticleTitle(string username);
        Task<string> GetUserArticleTitleAsync(string username);
    }
}
