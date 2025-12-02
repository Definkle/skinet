using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecificationParams specParams) : base(product =>
        (string.IsNullOrWhiteSpace(specParams.Search) || product.Name.ToLower().Contains(specParams.Search)) &&
        (specParams.Brands.Count == 0 || specParams.Brands.Contains(product.Brand)) &&
        (specParams.Types.Count == 0 || specParams.Types.Contains(product.Type))
    )
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        switch (specParams.Sort)
        {
            case "priceAsc": AddOrderBy(product => product.Price); break;
            case "priceDesc": AddOrderByDescending(product => product.Price); break;
            default: AddOrderBy(product => product.Name); break;
        }
    }
}