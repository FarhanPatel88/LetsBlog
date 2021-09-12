using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsBlog.Models.Blog;

namespace LetsBlog.Repository
{
    public interface IBlogRepository
    {
        public Task<Blog> UpsertAsync(BlogCreate blogCreate, int applicationUserId); 

        public Task<PagedResults<Blog>> GetAllAsync(BlogPaging blogPaging);

        public Task<List<Blog>> GetAllByUserIdAsync(int applicationUserId);

        public Task<List<Blog>> GetAllFamousAsync();

        public Task<Blog> GetAsync(int blogId);

        public Task<int> DeleteAsync(int blogId);
    }
}
