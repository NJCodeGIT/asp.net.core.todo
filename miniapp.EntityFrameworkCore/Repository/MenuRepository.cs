using miniapp.EntityFrameworkCore.Context;
using miniapp.EntityFrameworkCore.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miniapp.EntityFrameworkCore.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly EntityContext entityContext;
        private readonly ILogger<MenuRepository> logger;

        public MenuRepository(EntityContext entityContext, ILogger<MenuRepository> logger)
        {
            this.entityContext = entityContext;
            this.logger = logger;
        }

        public void AddEntity(object model)
        {
            try
            {
                this.logger.LogInformation("AddMenu invoked");

                this.entityContext.Add(model);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"AddMenu method failed: {ex}");
            }
        }

        public IEnumerable<Menu> GetAllMenus()
        {
            try
            {
                this.logger.LogInformation("GetAllMenus invoked");

                return this.entityContext.Menus.ToList();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"GetAllMenus method failed: {ex}");
                return null;
            }
        }

        public Menu GetMenuById(int id)
        {
            try
            {
                this.logger.LogInformation("GetMenuById invoked");

                return this.entityContext.Menus.FirstOrDefault(rw => rw.Id == id);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"GetMenuById method failed: {ex}");
                return null;
            }
        }

        public bool SaveAll()
        {
            try
            {
                this.logger.LogInformation("SaveAll invoked");

                return this.entityContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"SaveAll method failed: {ex}");
                return false;
            }
        }
    }
}
