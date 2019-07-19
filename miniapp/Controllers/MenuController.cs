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

namespace miniapp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MenuController : Controller
    {
        private readonly IMenuRepository menuRepository;
        private readonly ILogger<MenuController> logger;
        private readonly IMapper mapper;

        public MenuController(IMenuRepository menuRepository, ILogger<MenuController> logger, IMapper mapper)
        {
            this.menuRepository = menuRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        // ActionResult<IEnumerable<T>> pattern can use for public APIs for better documentation 
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<Menu>> Get()
        {
            try
            {
                return Ok(this.mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(this.menuRepository.GetAllMenus()));
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Menu GET request failed: {ex}");
                return BadRequest("Bad Request");
            }

        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        public IActionResult Get(int id)
        {
            try
            {
                var menu = this.menuRepository.GetMenuById(id);

                if (menu != null) return Ok(this.mapper.Map<Menu, MenuViewModel>(menu));
                else return NotFound();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Menu GET request failed: {ex}");
                return BadRequest("Bad Request");
            }

        }

        [HttpPost()]
        public IActionResult Post([FromBody]MenuViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newModel = this.mapper.Map<MenuViewModel, Menu>(model);
                    this.menuRepository.AddEntity(newModel);
                    if (this.menuRepository.SaveAll())
                    {
                        return Created($"/api/menu/{newModel.Id}", this.mapper.Map<Menu, MenuViewModel>(newModel));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }


            }
            catch (Exception ex)
            {
                this.logger.LogError($"Menu Post request failed: {ex}");
            }

            return BadRequest("Failed to save new menu");
        }

    }
}