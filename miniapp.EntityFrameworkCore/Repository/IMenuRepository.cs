using System.Collections.Generic;
using miniapp.EntityFrameworkCore.Entities;

namespace miniapp.EntityFrameworkCore.Repository
{
    public interface IMenuRepository
    {
        IEnumerable<Menu> GetAllMenus();
        bool SaveAll();
        Menu GetMenuById(int id);
        void AddEntity(object model);
    }
}