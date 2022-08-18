using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using event_guru_api.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace event_guru_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly ApplicationContext _db;
        public VendorController(ApplicationContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetAll()
        {
            return await _db.Vendors.ToListAsync();
        }

        [HttpGet]
        [Route("{ID:int}")]
        public async Task<ActionResult<Vendor>> Get(int ID)
        {
            Vendor? vendor = await _db.Vendors.Where(v => v.ID == ID).FirstOrDefaultAsync();
            if (vendor is null)
            {
                return Problem("Vendor not found");
            }
            return vendor;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] VendorModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }

            var vendor = _db.Vendors.Where(v => v.Name == model.Name).FirstOrDefault();
            if (vendor is not null)
            {
                ModelState.AddModelError("NameExists", "The name already exists");
            }
            var newVendor = new Vendor()
            {
                Name = model.Name,
                Location = model.Location,
                Photo = model.Photo,
                Phone = model.Phone,
                Email = model.Email,
                Description = model.Description,
                Unit = model.Unit,
                Type = model.Type,
                Price = model.Price,
                Negotiable = model.Negotiable
            };
            await _db.Vendors.AddAsync(newVendor);
            var result = await _db.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = $"Successfully added a Vendor {result.ToString()}" });
        }

        [HttpPut]
        [Route("{ID:int}")]
        public async Task<IActionResult> Edit(int ID, [FromForm] VendorModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }
            var vendor = await _db.Vendors.Where(v => v.ID == ID).FirstOrDefaultAsync();
            if (vendor is null)
            {
                return NotFound("The vendor you are trying to edit is not found");
            }
            vendor.Name = model.Name;
            vendor.Location = model.Location;
            vendor.Description = model.Description;
            vendor.Email = model.Email;
            vendor.Price = model.Price;
            vendor.Unit = model.Unit;
            vendor.Phone = model.Phone;
            vendor.Negotiable = model.Negotiable;
            vendor.Photo = model.Photo;
            vendor.Type = model.Type;

            var result = await _db.SaveChangesAsync();
            return Ok(new Response { Status = "Success", Message = "The vendor was updated successfully" });

        }

        [HttpDelete]
        [Route("{ID:int}")]
        public async Task<IActionResult> Delete(int ID)
        {
            var vendor = await _db.Vendors.Where(v => v.ID == ID).FirstOrDefaultAsync();
            if (vendor is null)
            {
                return NotFound("The vendor you are trying to delete is not found");
            }
            _db.Vendors.Remove(vendor);
            await _db.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "The vendor was deleted successfully" });
        }
    }
}
