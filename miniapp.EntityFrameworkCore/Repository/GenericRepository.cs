using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using miniapp.EntityFrameworkCore.Context;
using miniapp.EntityFrameworkCore.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace miniapp.EntityFrameworkCore.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly EntityContext context;
        private readonly ILogger<GenericRepository<T>> logger;
        private readonly IMapper mapper;
        private DbSet<T> entities;

        public GenericRepository(EntityContext context, ILogger<GenericRepository<T>> logger, IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public bool Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Add(entity);
                return this.SaveAll();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(T entity, AppUser appUser)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                if (this.IsAllowToDo(entity, appUser))
                {
                    var a = this.Entities.Update(entity);
                    return this.SaveAll();
                }
                else
                {
                    throw new InvalidOperationException("Invalid access");
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(T entity, AppUser appUser)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                if (this.IsAllowToDo(entity, appUser))
                {
                    this.Entities.Remove(entity);
                    return this.SaveAll();
                }
                else
                {
                    throw new InvalidOperationException("Invalid access");
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                var result = this.Entities.ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<T> GetAllByUser(AppUser appUser)
        {
            try
            {
                Type typeParameterType = typeof(T);
                string sp_name = "generic_GetAllByUser {0}, {1}";
                return this.GetAllByUserFromSP(appUser, sp_name, typeParameterType.Name.ToLowerInvariant());
            }
            catch
            {
                return null;
            }
        }

        private IEnumerable<T> GetAllByUserFromSP(AppUser appUser, string sp_name, string tableName)
        {
            try
            {
                return this.Entities.FromSql(sp_name, appUser.Id, tableName);
            }
            catch
            {
                return null;
            }
        }

        public DbSet<T> Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = context.Set<T>();
                }
                return entities;
            }
        }

        public bool SaveAll()
        {
            try
            {
                this.logger.LogInformation("SaveAll invoked");

                return this.context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"SaveAll method failed: {ex}");
                return false;
            }
        }

        private bool IsAllowToDo(T entity, AppUser appUser)
        {
            var myItems = this.GetAllByUser(appUser);
            return myItems.Any(rw => rw.Id == entity.Id);
        }
    }
}
