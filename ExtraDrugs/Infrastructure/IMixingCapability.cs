using S1API.Products;

namespace ExtraDrugs.Infrastructure;

internal interface IMixingCapability
{
    void RegisterMixing(ProductKind productKind);
}