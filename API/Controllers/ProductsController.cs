using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IGenericRepository<Product> productRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
        [FromQuery] ProductSpecificationParams productSpecificationParams)
    {
        var specification = new ProductSpecification(productSpecificationParams);

        return await CreatePagedResult(productRepository, specification, productSpecificationParams.PageIndex,
            productSpecificationParams.PageSize);
    }

    [HttpGet("{id:int}")] // api/products/1
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        return product == null ? NotFound() : product;
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var specification = new BrandListSpecification();
        var products = await productRepository.GetAllWithSpec(specification);
        return Ok(products);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var specification = new TypeListSpecification();
        var products = await productRepository.GetAllWithSpec(specification);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepository.Add(product);

        if (await productRepository.SaveAllAsync())
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);

        await productRepository.SaveAllAsync();
        return BadRequest("Error creating the product");
    }

    [HttpPut("{id:int}")] // api/products/1
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !_DoesProductExists(id))
            return BadRequest("Cannot update this product!");

        productRepository.Update(product);
        if (await productRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Error updating the product");
    }

    [HttpDelete("{id:int}")] // api/products/1
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var productToRemove = await productRepository.GetByIdAsync(id);

        if (productToRemove == null) return NotFound();

        productRepository.Remove(productToRemove);
        if (await productRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Error deleting the product");
    }

    private bool _DoesProductExists(int id)
    {
        return productRepository.Exists(id);
    }
}