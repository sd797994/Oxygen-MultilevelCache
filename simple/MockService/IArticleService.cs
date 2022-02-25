using Infrastructure.EfDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSimple.MockService
{

    public interface IArticleService
    {
        Account GetArticleById(Account article);
        Task<Account> GetArticleByIdAsync(Account article);
        List<Account> GetAllArticle();
        Task<List<Account>> GetAllArticleAsync();
        Account GetCacheArticleById(Account article);
        Task<Account> GetCacheArticleByIdAsync(Account article);
        List<Account> GetCacheAllArticle();
        Task<List<Account>> GetCacheAllArticleAsync();
    }
}
