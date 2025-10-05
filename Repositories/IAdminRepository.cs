using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Web.Data;
using Web.Models;

namespace Web.Repositories
{
    public interface IAdminRepository
    {
        Task<bool> IsAdminAsync(User user);
        bool IsAdmin(User user);
        Task AddAdminRoleAsync(string email);
        Task<List<User>> GetAllAdminsAsync();
        Task RemoveAdminRoleAsync(string email);
        //Task<bool> AddProduct(AddProduct model);
        Task<List<TheLoai>> GetTheLoais();
        Task<bool> AddProductAsync(AddProduct model, IWebHostEnvironment environment);
    }
}