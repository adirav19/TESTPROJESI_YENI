using System.Collections.Generic;
using System.Threading.Tasks;

namespace TESTPROJESI.Repositories.Interfaces
{
    /// <summary>
    /// 🎯 Tüm CRUD operasyonları için genel repository interface
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        // 📋 Listeleme
        Task<List<T>> GetAllAsync(string queryParams = null);

        // 🔍 Tekil kayıt
        Task<T?> GetByIdAsync(string id);

        // ➕ Ekleme
        Task<T> CreateAsync(T entity);
         
        // ✏️ Güncelleme
        Task<T> UpdateAsync(string id, T entity);

        // 🗑️ Silme
        Task<bool> DeleteAsync(string id);
    }
}