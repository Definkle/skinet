using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams specParams) : base(x =>
        (string.IsNullOrWhiteSpace(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
        (string.IsNullOrWhiteSpace(specParams.Brand) || x.Brand == specParams.Brand) &&
        (string.IsNullOrWhiteSpace(specParams.Type) || x.Type == specParams.Type)
    )
    {
        switch (specParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
