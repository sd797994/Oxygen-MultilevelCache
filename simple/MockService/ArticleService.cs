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

        public List<Article> GetAllArticle()
        {

            return efDbContext.Article.ToList();
        }

        public async Task<List<Article>> GetAllArticleAsync()
        {
            return await efDbContext.Article.ToListAsync();
        }

        public Article GetArticleById(Article article)
        {
            return efDbContext.Article.FirstOrDefault(x => x.Id == article.Id);
        }

        public async Task<Article> GetArticleByIdAsync(Article article)
        {
            return await efDbContext.Article.FirstOrDefaultAsync(x => x.Id == article.Id);
        }
        [SystemCached]
        public List<Article> GetCacheAllArticle()
        {
            return efDbContext.Article.ToList();
        }

        [SystemCached]
        public async Task<List<Article>> GetCacheAllArticleAsync()
        {
            return await efDbContext.Article.ToListAsync();
        }

        [SystemCached]
        public Article GetCacheArticleById(Article article)
        {
            return efDbContext.Article.FirstOrDefault(x => x.Id == article.Id);
        }

        [SystemCached]
        public async Task<Article> GetCacheArticleByIdAsync(Article article)
        {
            return await efDbContext.Article.FirstOrDefaultAsync(x => x.Id == article.Id);
        }
    }
}
