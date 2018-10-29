using System;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio2.DatabaseModel;
using System.Linq;
using WebService.Models;
using ProjectPortfolio2;

namespace WebService.Controllers
{
    [Route("api/owners")]
    [ApiController]
    public class OwnersController : Controller
    {
        private readonly IDataService _dataService;
        public OwnersController(IDataService dataService)
        {
            _dataService = dataService;
        }
        [HttpGet(Name = nameof(GetOwners))]
        public IActionResult GetOwners()
        {
            var owners = _dataService.GetOwners().Select(CreateOwnerModel);
            var result = new
            {
                Items = owners
            };
            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetOwner))]
        public IActionResult GetOwner(int id)
        {
            var owner = _dataService.GetOwner(id);
            if (owner == null) return NotFound();
            var model = CreateOwnerModel(owner);
            return Ok(model);
        }

        private OwnerModel CreateOwnerModel(Owner owner)
        {
            var model = new OwnerModel
            {
                DisplayName = owner.DisplayName,
                Location = owner.Location,
                CreationDate = owner.CreationDate,
                Age = owner.Age
            };
            return model;
        }
    }
}
