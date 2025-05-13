// filepath: /home/sprudel/goblin-market/goblin-api/Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using goblin_api.Data;
using goblin_api.Models;
using goblin_api.DTOs;

namespace goblin_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductsController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    EAN = p.EAN,
                    StorageLocation = p.StorageLocation,
                    ExpiryDate = p.ExpiryDate,
                    OpenedAt = p.OpenedAt,
                    ShelfLifeAfterOpening = p.ShelfLifeAfterOpening
                })
                .ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                EAN = product.EAN,
                StorageLocation = product.StorageLocation,
                ExpiryDate = product.ExpiryDate,
                OpenedAt = product.OpenedAt,
                ShelfLifeAfterOpening = product.ShelfLifeAfterOpening
            };
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(long id, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = updateProductDto.Name;
            product.EAN = updateProductDto.EAN;
            product.StorageLocation = updateProductDto.StorageLocation;
            // Convert dates to UTC before saving
            product.ExpiryDate = updateProductDto.ExpiryDate.HasValue 
                ? DateTime.SpecifyKind(updateProductDto.ExpiryDate.Value, DateTimeKind.Utc) 
                : (DateTime?)null;
            product.OpenedAt = updateProductDto.OpenedAt.HasValue 
                ? DateTime.SpecifyKind(updateProductDto.OpenedAt.Value, DateTimeKind.Utc) 
                : (DateTime?)null;
            product.ShelfLifeAfterOpening = updateProductDto.ShelfLifeAfterOpening;

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

        // PUT: api/Products/5/open
        [HttpPut("{id}/open")]
        public async Task<IActionResult> OpenProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            if (product.OpenedAt.HasValue)
            {
                // Product is already open, return conflict or bad request
                return BadRequest(new { message = "Product is already open." });
            }

            product.OpenedAt = DateTime.UtcNow; // Set to current UTC time
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

            // Return the updated product or just NoContent
            // For now, returning NoContent is fine, or you can return the updated ProductDto
            return NoContent(); 
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                EAN = createProductDto.EAN,
                StorageLocation = createProductDto.StorageLocation,
                // Convert dates to UTC before saving
                ExpiryDate = createProductDto.ExpiryDate.HasValue 
                    ? DateTime.SpecifyKind(createProductDto.ExpiryDate.Value, DateTimeKind.Utc) 
                    : (DateTime?)null,
                OpenedAt = createProductDto.OpenedAt.HasValue 
                    ? DateTime.SpecifyKind(createProductDto.OpenedAt.Value, DateTimeKind.Utc) 
                    : (DateTime?)null,
                ShelfLifeAfterOpening = createProductDto.ShelfLifeAfterOpening
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, new ProductDto 
            { 
                Id = product.Id,
                Name = product.Name,
                EAN = product.EAN,
                StorageLocation = product.StorageLocation,
                ExpiryDate = product.ExpiryDate,
                OpenedAt = product.OpenedAt,
                ShelfLifeAfterOpening = product.ShelfLifeAfterOpening
            });
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
