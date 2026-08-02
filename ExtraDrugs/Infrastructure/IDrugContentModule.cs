using S1API.Products;

namespace ExtraDrugs.Infrastructure;

internal interface IDrugContentModule : IDisposable
{
    string ProviderDataKey { get; }

    void RegisterContent();

    void CompleteLoad();

    CustomProductDefinitionBuilder? Restore(CustomProductSaveDescriptor descriptor);
}
