using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using miniapp.EntityFrameworkCore.Entities;

namespace miniapp.EntityFrameworkCore.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        DbSet<T> Entities { get; }
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllByUser(AppUser appUser);
        bool Delete(T entity, AppUser appUser);
        T GetById(object id);
        bool Insert(T entity);
        bool Update(T entity, AppUser appUser);
        bool SaveAll();
    }
}