using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using WebApI.Models;

namespace WebApI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceDemoContext _context;

        public ProductsController(ECommerceDemoContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        [Route("GetAllProducts")]
        public PagedResult<Product> GetAllProducts(int page)
        {
            int pageSize = 10;
            var qry = _context.Product.AsNoTracking().OrderBy(p => p.ProdName);
            //var model = await PagingList.CreateAsync(qry, 10, page);
            var model = GetPagedResultForQuery(qry, page, pageSize);
            return model;
        }

        private PagedResult<Product> GetPagedResultForQuery(IQueryable<Product> query, int page, int pageSize)
        {
            var result = new PagedResult<Product>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }


        // GET: api/Products/1
        [HttpGet("{id}")]
        public IActionResult GetProduct(long id)
        {
            try
            {
                var productsDTO = from p in _context.Product
                                  join pa in _context.ProductAttribute on p.ProductId equals pa.ProductId
                                  join pal in _context.ProductAttributeLookup on pa.AttributeId equals pal.AttributeId
                                  where p.ProductId == id
                                  select new ProductsDTO
                                  {
                                      ProductId = p.ProductId,
                                      ProdCatId = p.ProdCatId,
                                      ProdName = p.ProdName,
                                      ProdDescription = p.ProdDescription,
                                      AttributeName = pal.AttributeName,
                                      AttributeValue = pa.AttributeValue
                                  };

                if (productsDTO == null) return NotFound();

                return Ok(productsDTO);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(long id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(long id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }

    }
}
