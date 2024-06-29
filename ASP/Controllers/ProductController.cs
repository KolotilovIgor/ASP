using Microsoft.AspNetCore.Mvc;
using ASP.Data;
using ASP.Models;
using System.Collections.Generic;

namespace ASP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        public ActionResult<int> AddProduct(string name, string description, decimal price)
        {
            using (StorageContext storageContext = new StorageContext())
            {
                if (storageContext.Products.Any(p => p.Name == name))
                    return StatusCode(409);

                var product = new Product() { Name = name, Description = description, Price = price };
                storageContext.Products.Add(product);
                storageContext.SaveChanges();
                return Ok(product.Id);
            }
            
        }
        [HttpGet("get_all_product")]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            IEnumerable<Product> list;
            using (StorageContext storageContext = new StorageContext())
            {
                list = storageContext.Products.Select(p => new Product { Name = p.Name, Description = p.Description, Price = p.Price }).ToList();
                return Ok(list);
            }
        }
        [HttpDelete("delete_product/{id}")]
        public ActionResult DeleteProduct(int id)
        {
            using (var storageContext = new StorageContext())
            {
                var product = storageContext.Products.FirstOrDefault(p => p.Id == id);
                if (product == null) return NotFound();

                storageContext.Products.Remove(product);
                storageContext.SaveChanges();
                return Ok();
            }
        }

        [HttpDelete("delete_group/{id}")]
        public ActionResult DeleteGroup(int id)
        {
            using (var storageContext = new StorageContext())
            {
                var group = storageContext.ProductGroup.FirstOrDefault(pg => pg.Id == id);
                if (group == null) return NotFound();

                storageContext.ProductGroup.Remove(group);
                storageContext.SaveChanges();
                return Ok();
            }
        }

        [HttpPut("update_price/{id}")]
        public ActionResult UpdatePrice(int id, decimal newPrice)
        {
            using (var storageContext = new StorageContext())
            {
                var product = storageContext.Products.FirstOrDefault(p => p.Id == id);
                if (product == null) return NotFound();

                product.Price = newPrice;
                storageContext.SaveChanges();
                return Ok();
            }
        }
    }
}
