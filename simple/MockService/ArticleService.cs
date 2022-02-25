using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.EfDataAccess;
using Microsoft.EntityFrameworkCore;
using Oxygen.MulitlevelCache;

namespace AppSimple.MockService
{
    public class ArticleService : IArticleService
    {
        private readonly EfDbContext efDbContext;
        public ArticleService(EfDbContext efDbContext)
        {
            this.efDbContext = efDbContext;
        }

        public List<Account> GetAllArticle()
        {

            return efDbContext.Account.ToList();
        }

        public async Task<List<Account>> GetAllArticleAsync()
        {
            return await efDbContext.Account.ToListAsync();
        }

        public Account GetArticleById(Account article)
        {
            return efDbContext.Account.FirstOrDefault(x => x.Id == article.Id);
        }

        public async Task<Account> GetArticleByIdAsync(Account article)
        {
            return await efDbContext.Account.FirstOrDefaultAsync(x => x.Id == article.Id);
        }
        [SystemCached]
        public List<Account> GetCacheAllArticle()
        {
            return efDbContext.Account.ToList();
        }

        [SystemCached]
        public async Task<List<Account>> GetCacheAllArticleAsync()
        {
            return await efDbContext.Account.ToListAsync();
        }

        [SystemCached]
        public Account GetCacheArticleById(Account article)
        {
            return efDbContext.Account.FirstOrDefault(x => x.Id == article.Id);
        }

        [SystemCached]
        public async Task<Account> GetCacheArticleByIdAsync(Account article)
        {
            return await efDbContext.Account.FirstOrDefaultAsync(x => x.Id == article.Id);
        }
    }
}
