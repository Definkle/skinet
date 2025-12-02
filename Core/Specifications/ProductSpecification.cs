using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string? brand, string? type, string? sort) : base(product =>
        (string.IsNullOrEmpty(brand) || product.Brand == brand) &&
        (string.IsNullOrEmpty(type) || product.Type == type)
    )
    {
        switch (sort)
        {
            case "priceAsc": AddOrderBy(product => product.Price); break;
            case "priceDesc": AddOrderByDescending(product => product.Price); break;
            default: AddOrderBy(product => product.Name); break;
        }
    }
}