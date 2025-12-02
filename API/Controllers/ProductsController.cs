using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);
        return Ok(await repo.ListAsync(spec));
    }

    [HttpGet("{id:int}")] // api/products/1
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        return product == null ? NotFound() : product;
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await repo.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        return Ok(await repo.ListAsync(spec));
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);

        if (await repo.SaveAllAsync())
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);

        return BadRequest("Error creating the product");
    }

    [HttpPut("{id:int}")] // api/products/1
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !repo.Exists(id))
            return BadRequest("Cannot update this product!");

        repo.Update(product);
        if (await repo.SaveAllAsync()) return NoContent();

        return BadRequest("Error updating the product");
    }

    [HttpDelete("{id:int}")] // api/products/1
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var productToRemove = await repo.GetByIdAsync(id);

        if (productToRemove == null) return NotFound();

        repo.Delete(productToRemove);
        if (await repo.SaveAllAsync()) return NoContent();

        return BadRequest("Error deleting the product");
    }
}