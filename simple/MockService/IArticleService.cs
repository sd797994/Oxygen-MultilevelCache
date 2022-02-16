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
        Article GetArticleById(Article article);
        Task<Article> GetArticleByIdAsync(Article article);
        List<Article> GetAllArticle();
        Task<List<Article>> GetAllArticleAsync();
        Article GetCacheArticleById(Article article);
        Task<Article> GetCacheArticleByIdAsync(Article article);
        List<Article> GetCacheAllArticle();
        Task<List<Article>> GetCacheAllArticleAsync();
    }
}
