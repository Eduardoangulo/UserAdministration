using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserAdministration.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        //Task<T> GetByIdAsync(int id);
        //Task<IReadOnlyList<T>> GetAllAsync();
        int Add(T entity);
        int Update(T entity);
        int DeleteUser(T entity);

        List<T> GetUsersAvailable(string state);

        //Task<int> UpdateAsync(T entity);
        //Task<int> DeleteAsync(int id);
    }
}
