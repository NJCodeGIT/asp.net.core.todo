using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using miniapp.EntityFrameworkCore.Entities;
using miniapp.EntityFrameworkCore.Repository;
using miniapp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace miniapp.ApiControllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ToDoApiController : Controller
    {
        private readonly IGenericRepository<ToDo> genericRepository;
        private readonly ILogger<ToDoApiController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private Task<AppUser> GetCurrentUserAsync() => this.userManager.GetUserAsync(HttpContext.User);

        public ToDoApiController(IGenericRepository<ToDo> genericRepository, ILogger<ToDoApiController> logger, IMapper mapper,
            UserManager<AppUser> userManager)
        {
            this.genericRepository = genericRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        // ActionResult<IEnumerable<T>> pattern can use for public APIs for better documentation 
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<ToDo>>> Get()
        {
            try
            {
                var user = await GetCurrentUserAsync();
                var filteredList = this.genericRepository.GetAll().Where(rw => rw.CreatedBy.Id == user.Id || rw.ModifiedBy.Id == user.Id);
                return Ok(this.mapper.Map<IEnumerable<ToDo>, IEnumerable<ToDoViewModel>>(this.genericRepository.GetAll()));
            }
            catch (Exception ex)
            {
                this.logger.LogError($"ToDo GET request failed: {ex}");
                return BadRequest("Bad Request");
            }
        }

        //[HttpGet("{id:int}")]
        //[ProducesResponseType(200)]
        //public async Task<IActionResult> Get(int id)
        //{
        //    try
        //    {
        //        var user = await GetCurrentUserAsync();
        //        var ToDo = this.genericRepository.GetById(id);

        //        if (ToDo != null) return Ok(this.mapper.Map<ToDo, ToDoViewModel>(ToDo));
        //        else return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.logger.LogError($"ToDo GET request failed: {ex}");
        //        return BadRequest("Bad Request");
        //    }

        //}

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody]ToDoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = false;
                    var newModel = this.mapper.Map<ToDoViewModel, ToDo>(model);
                    var user = await GetCurrentUserAsync();

                    newModel.ModifiedBy = user;

                    if (newModel.Id > 0)
                    {
                        result = this.genericRepository.Update(newModel, user);
                    }
                    else
                    {
                        newModel.CreatedBy = user;
                        result = this.genericRepository.Insert(newModel);
                    }

                    if (result)
                    {
                        return Created($"/api/ToDo/{newModel.Id}", this.mapper.Map<ToDo, ToDoViewModel>(newModel));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"ToDo Post request failed: {ex}");
            }

            return BadRequest("Failed to save data");
        }

        [HttpDelete()]
        public async Task<IActionResult> Delete([FromBody]ToDoViewModel model)
        {
            try
            {
                var entityModel = this.mapper.Map<ToDoViewModel, ToDo>(model);
                var user = await GetCurrentUserAsync();

                if (this.genericRepository.Delete(entityModel, user))
                {
                    return Ok(this.mapper.Map<IEnumerable<ToDo>, IEnumerable<ToDoViewModel>>(this.genericRepository.GetAll()));
                }

            }
            catch (Exception ex)
            {
                this.logger.LogError($"ToDo Post request failed: {ex}");
            }

            return BadRequest("Failed to delete");
        }

    }
}