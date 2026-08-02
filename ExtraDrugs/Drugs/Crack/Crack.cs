using S1 = ScheduleOne;
using S1Equipping = ScheduleOne.Equipping;
using S1ItemFramework = ScheduleOne.ItemFramework;
using S1Product = ScheduleOne.Product;
using ExtraDrugs.Infrastructure;
using MelonLoader;
using S1API.Console;
using S1API.Items;
using S1API.Products;
using S1API.Stations;
using UnityEngine;

namespace ExtraDrugs.Drugs.Crack;

internal sealed class Crack : IDrugContentModule, IMixingCapability
{
    internal const string ProductKindId = "rivercardstudio.extradrugs:crack";
    internal const string ProductId = "rivercardstudio.extradrugs:products/crack";
    internal const string RecipeId = "rivercardstudio.extradrugs:crack-synthesis";
    internal const string ProviderData = "crack";

    private const string BrickPackagingId = "brick";
    private const string CrackResource =
        "ExtraDrugs.Assets.Models.crack_rock.glb";


    private readonly MelonLogger.Instance _logger;
    private readonly EmbeddedGlbAsset _crackRock =
        new EmbeddedGlbAsset(CrackResource, "ExtraDrugs_CrackRock");

    private ProductKind? _productKind;
    private ProductPresentationProfile? _presentationProfile;
    private ProductPackagingContentProfile? _baggieProfile;
    private ProductPackagingContentProfile? _jarProfile;
    private ProductPackagingContentProfile? _brickProfile;
    private CustomProductDefinition? _definition;
    private ChemistryStationRecipe? _recipe;
    private ProductKindMetadata? _metadata;
    private ProductMixingProfile? _mixingProfile;
    private GameObject? _consumptionSource;

    internal Crack(MelonLogger.Instance logger)
    {
        _logger = logger;
    }

    public string ProviderDataKey => ProviderData;

    public void RegisterContent()
    {
        if (_definition != null)
            return;

        ProductDefinition template =
            ItemManager.GetDefinition("cocaine") as ProductDefinition ??
            throw new InvalidOperationException(
                "The native 'cocaine' product scaffold is unavailable.");
        PackagingDefinition baggie =
            ItemManager.GetDefinition("baggie") as PackagingDefinition ??
            throw new InvalidOperationException(
                "The native 'baggie' packaging definition is unavailable.");
        PackagingDefinition jar =
            ItemManager.GetDefinition("jar") as PackagingDefinition ??
            throw new InvalidOperationException(
                "The native 'jar' packaging definition is unavailable.");
        PackagingDefinition brick =
            ItemManager.GetDefinition(BrickPackagingId) as PackagingDefinition ??
            throw new InvalidOperationException(
                "The native 'brick' packaging definition is unavailable.");

        _productKind ??=
            new ProductKindBuilder(ProductKindId)
                .WithCompatibilityDrugType(DrugType.Cocaine)
                .Build();

        GameObject crackSource = _crackRock.GetOrLoad();
        EnsurePresentationRegistered(template, crackSource);
        EnsurePackagingRegistered(crackSource);
        RegisterMixing(_productKind);

        _definition = CreateBuilder(template, baggie, jar, brick).Build();
        _definition.Discover();

        ConsoleItemAliases.Register("crack", ProductId);

        _recipe ??=
            ChemistryStationRecipes.CreateAndRegister(builder => builder
                .WithRecipeId(RecipeId)
                .WithTitle("Crack \u2060")
                .WithCookTimeMinutes(240)
                .WithTemperature(300f, 20f)
                .WithFinalLiquidColor(new Color(0.906f, 0.853f, 0.631f))
                .WithCalculationMethod(QualityCalculationMethod.Additive)
                .WithIngredient("iodine", 1)
                .WithIngredient("cocaine", 10)
                .WithIngredient("horsesemen", 1)
                .WithProduct(ProductId, 20));
    }

    public void CompleteLoad()
    {
        if (_definition == null || _productKind == null)
            return;

        if (_metadata == null)
        {
            Sprite icon = _definition.Icon;
            if (icon == null)
            {
                _logger.Warning(
                    "Crack icon generation has not completed; Product Manager metadata will retry on the next load.");
            }
            else
            {
                _metadata =
                    new ProductKindMetadataBuilder(_productKind)
                        .WithDisplayName("Crack")
                        .WithColor(new Color(0.906f, 0.853f, 0.631f))
                        .WithIcon(icon)
                        .WithSortOrder(5)
                        .WithSearchAliases("crack", "yeo")
                        .WithProductManagerVisibility()
                        .Build();
            }
        }
    }

    public CustomProductDefinitionBuilder? Restore(
        CustomProductSaveDescriptor descriptor)
    {
        ProductDefinition template =
            ItemManager.GetDefinition("cocaine") as ProductDefinition ??
            throw new InvalidOperationException(
                "Cannot restore Crack without the native cocaine product scaffold.");
        PackagingDefinition baggie =
            ItemManager.GetDefinition("baggie") as PackagingDefinition ??
            throw new InvalidOperationException(
                "Cannot restore Crack without native baggie packaging.");
        PackagingDefinition jar =
            ItemManager.GetDefinition("jar") as PackagingDefinition ??
            throw new InvalidOperationException(
                "Cannot restore Crack without native jar packaging.");
        PackagingDefinition brick =
            ItemManager.GetDefinition(BrickPackagingId) as PackagingDefinition ??
            throw new InvalidOperationException(
                "Cannot restore Crack without native brick packaging.");

        _productKind ??=
            new ProductKindBuilder(ProductKindId)
                .WithCompatibilityDrugType(DrugType.Cocaine)
                .Build();

        GameObject crackSource = _crackRock.GetOrLoad();
        EnsurePresentationRegistered(template, crackSource);
        EnsurePackagingRegistered(crackSource);
        return CreateBuilder(template, baggie, jar, brick);
    }

    public void Dispose()
    {
        if (_consumptionSource != null)
            UnityEngine.Object.Destroy(_consumptionSource);

        _consumptionSource = null;
        _crackRock.Dispose();
    }

    public void RegisterMixing(ProductKind productKind)
    {
        _mixingProfile ??=
            new ProductMixingProfileBuilder(productKind)
                .WithMixerMap(ProductMixingMap.Cocaine)
                .WithOutputFactoryCompatibility(
                    "rivercardstudio.extradrugs:mixing/crack",
                    version: 1)
                .WithOutputFactory(input =>
                    new ProductMixingOutputDefinition(
                        input.MixName,
                        input.SourceKind,
                        Math.Min(999f, input.SourcePrice + 10f)))
                .Build();
    }

    private CustomProductDefinitionBuilder CreateBuilder(
        ProductDefinition template,
        PackagingDefinition baggie,
        PackagingDefinition jar,
        PackagingDefinition brick)
    {
        ProductKind kind = _productKind ??
            throw new InvalidOperationException("Crack product kind is not registered.");

        return CustomProductItemCreator
            .CreateBuilder(ProductId, kind)
            .WithName("Crack")
            .WithDescription("Named after crackheads.")
            .WithProductPrice(100f)
            .WithLegalStatus(LegalStatus.Illegal)
            .WithBaseAddictiveness(0.6f)
            .WithDefaultQuality(Quality.Standard)
            .WithRepresentationsFrom(template)
            .WithValidPackaging(baggie, jar, brick)
            .WithEffectDurations(playerSeconds: 360, npcSeconds: 720)
            .WithNativeMixerMap(ProductMixingMap.Cocaine)
            .WithSaveProvider(
                DrugCatalog.SaveProviderId,
                DrugCatalog.SaveProviderVersion,
                ProviderData);
    }

    private void EnsurePresentationRegistered(
        ProductDefinition template,
        GameObject crackSource)
    {
        ProductPresentationTransform crackPose =
            new ProductPresentationTransform(
                Vector3.zero,
                (
                    Quaternion.Euler(78f, 0f, -8f) *
                    Quaternion.Euler(0f, 0f, 0f)
                ).eulerAngles,
                Vector3.one * 0.01f);
        ProductPresentationTransform heldPillPose =
            new ProductPresentationTransform(
                crackPose.LocalPosition,
                (
                    Quaternion.Euler(0f, 0f, 0f) *
                    Quaternion.Euler(0f, 0f, 0f)
                ).eulerAngles,
                crackPose.LocalScale);

        _consumptionSource ??= CreateConsumptionSource(crackSource, crackPose);
        _presentationProfile ??=
            new ProductPresentationProfileBuilder()
                .WithLooseVisual(() => crackSource, crackPose)
                .WithHeldVisual(() => crackSource, heldPillPose)
                .WithFunctionalProductConvexMeshColliders()
                .WithGeneratedIconFromLooseVisual(
                    size: 512,
                    fitToCamera: true,
                    cameraFill: 0.78f)
                .WithConsumptionPrefab(() => _consumptionSource)
                .Require(
                    ProductPresentationContext.Loose,
                    ProductPresentationContext.Stored,
                    ProductPresentationContext.Held,
                    ProductPresentationContext.Station,
                    ProductPresentationContext.FunctionalProduct,
                    ProductPresentationContext.Icon,
                    ProductPresentationContext.Consumption)
                .Build();

        ProductPresentationProfileRegistry.RegisterForProduct(
            "rivercardstudio.extradrugs",
            "rivercardstudio.extradrugs:products/crack",
            _presentationProfile);
        ProductPresentationProfileRegistry.RegisterForProductKind(
            "rivercardstudio.extradrugs",
            _productKind ??
                throw new InvalidOperationException(
                    "Crack product kind is not registered."),
            _presentationProfile);
    }

    private void EnsurePackagingRegistered(GameObject crackSource)
    {
        _baggieProfile ??=
            new ProductPackagingContentProfileBuilder()
                .WithContent(() => crackSource)
                .AddPlacement(
                    new ProductPresentationTransform(
                        new Vector3(0f, -0.002f, 0f),
                        new Vector3(78f, 0f, 0f),
                        Vector3.one * 0.01f))
                .Build();
        _jarProfile ??=
            new ProductPackagingContentProfileBuilder()
                .WithContent(() => crackSource)
                .AddPlacements(
                    JarPlacement(-0.04f, 0.02f, 12f),
                    JarPlacement(0f, 0.02f, -28f),
                    JarPlacement(0.04f, 0.02f, 38f),
                    JarPlacement(-0.035f, 0.06f, -18f),
                    JarPlacement(0.035f, 0.06f, 22f),
                    JarPlacement(-0.04f, 0.095f, 48f),
                    JarPlacement(0f, 0.095f, -42f),
                    JarPlacement(0.04f, 0.095f, 8f),
                    JarPlacement(-0.02f, 0.13f, 65f),
                    JarPlacement(0.025f, 0.13f, -62f))
                .Build();
        Material brickMaterial = GetCrackRockMaterial(crackSource);
        _brickProfile ??=
            new ProductPackagingContentProfileBuilder()
                .WithNativeFilledVisualScaffold(
                    ProductPackagingVisualTemplate.Cocaine,
                    clone => ApplyCrackBrickMaterial(clone, brickMaterial))
                .Build();

        ProductPackagingContentProfileRegistry.Register(
            "rivercardstudio.extradrugs",
            "rivercardstudio.extradrugs:products/crack",
            "baggie",
            _baggieProfile);
        ProductPackagingContentProfileRegistry.Register(
            "rivercardstudio.extradrugs",
            "rivercardstudio.extradrugs:products/crack",
            "jar",
            _jarProfile);
        ProductPackagingContentProfileRegistry.Register(
            "rivercardstudio.extradrugs",
            "rivercardstudio.extradrugs:products/crack",
            BrickPackagingId,
            _brickProfile);
        ProductPackagingContentProfileRegistry.RegisterForProductKind(
            "rivercardstudio.extradrugs",
            "rivercardstudio.extradrugs:crack",
            "baggie",
            _baggieProfile);
        ProductPackagingContentProfileRegistry.RegisterForProductKind(
            "rivercardstudio.extradrugs",
            "rivercardstudio.extradrugs:crack",
            "jar",
            _jarProfile);
        ProductPackagingContentProfileRegistry.RegisterForProductKind(
            "rivercardstudio.extradrugs",
            "rivercardstudio.extradrugs:crack",
            BrickPackagingId,
            _brickProfile);
    }

    private static Material GetCrackRockMaterial(GameObject crackSource)
    {
        foreach (Renderer renderer in
                 crackSource.GetComponentsInChildren<Renderer>(true))
        {
            foreach (Material material in renderer.sharedMaterials)
            {
                if (material != null)
                    return material;
            }
        }

        throw new InvalidOperationException(
            "The Crack rock asset does not provide a material for brick visuals.");
    }

    private static void ApplyCrackBrickMaterial(
        GameObject scaffold,
        Material material)
    {
        bool customized = false;
        foreach (Renderer renderer in
                 scaffold.GetComponentsInChildren<Renderer>(true))
        {
            if (!renderer.name.StartsWith(
                    "Brick_LOD",
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            Material[] materials = renderer.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = material;
            renderer.sharedMaterials = materials;
            customized = true;
        }

        if (!customized)
        {
            throw new InvalidOperationException(
                "The native cocaine brick scaffold did not contain Brick_LOD renderers.");
        }
    }

    private static ProductPresentationTransform JarPlacement(
        float x,
        float y,
        float zRotation)
    {
        return new ProductPresentationTransform(
            new Vector3(x, y, 0f),
            new Vector3(78f, 0f, zRotation),
            Vector3.one * 0.02f);
    }

    private static GameObject CreateConsumptionSource(
        GameObject crackSource,
        ProductPresentationTransform crackPose)
    {
        S1Product.ProductDefinition nativeTemplate =
            GetNativeProductDefinition("meth") ??
            throw new InvalidOperationException(
                "Cannot create the Crack consumption prefab without the native meth scaffold.");
        GameObject consumptionSource =
            UnityEngine.Object.Instantiate(
                nativeTemplate.ConsumeAnimation.gameObject);
        consumptionSource.name = "ExtraDrugs_Crack_Consumption";
        UnityEngine.Object.DontDestroyOnLoad(consumptionSource);
        consumptionSource.transform.position = new Vector3(0f, -20000f, 0f);

        GameObject consumeCrack = new GameObject("ConsumeCrack");
        consumeCrack.name = "ExtraDrugs_Crack_Consumption_Visual";
        consumeCrack.transform.SetParent(consumptionSource.transform, false);
        consumeCrack.transform.localPosition = crackPose.LocalPosition;
        consumeCrack.transform.localEulerAngles = crackPose.LocalEulerAngles;
        consumeCrack.transform.localScale = crackPose.LocalScale;
        consumeCrack.SetActive(true);
        consumptionSource.SetActive(true);
        return consumptionSource;
    }

    private static S1Product.ProductDefinition? GetNativeProductDefinition(
        string itemId)
    {
        return S1.Registry.GetItem(itemId) as S1Product.ProductDefinition;
    }

    private static S1ItemFramework.QualityItemDefinition?
        GetNativeQualityDefinition(string itemId)
    {
        return S1.Registry.GetItem(itemId) as
            S1ItemFramework.QualityItemDefinition;
    }

    private static S1Equipping.Equippable_Viewmodel? AsViewmodel(
        S1Equipping.Equippable equippable)
    {
        return equippable as S1Equipping.Equippable_Viewmodel;
    }
}