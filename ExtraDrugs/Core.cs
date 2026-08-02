using ExtraDrugs;
using ExtraDrugs.Drugs.Crack;
using ExtraDrugs.Infrastructure;
using ExtraDrugs.Miscellaneous;
using MelonLoader;
using S1API.Lifecycle;
using S1API.Products;

[assembly: MelonInfo(typeof(ExtraDrugs.Core), "Extra Drugs", "1-BETA3", "River Card Studio")][assembly: MelonGame("TVGS", "Schedule I")]

namespace ExtraDrugs;

public class Core : MelonMod
{
    private DrugCatalog? _catalog;

    public override void OnInitializeMelon()
    {
        HarmonyInstance.PatchAll(typeof(Core).Assembly);

        _catalog = new DrugCatalog(LoggerInstance, new IDrugContentModule[]
        {
            new Crack(LoggerInstance),
        });

        CustomProductSaveProviderRegistry.Register(_catalog);
        GameLifecycle.OnPreLoad += OnPreLoad;
        GameLifecycle.OnLoadComplete += OnLoadComplete;
        LoggerInstance.Msg($"{"Extra Drugs"} {"1-BETA3"} initialized.");
    }

    public override void OnApplicationQuit()
    {
        GameLifecycle.OnPreLoad -= OnPreLoad;
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
        _catalog?.Dispose();
        _catalog = null;
    }

    private void OnPreLoad()
    {
        _catalog?.RegisterContent();
    }

    private void OnLoadComplete()
    {
        _catalog?.CompleteLoad();
    }

    //public override void OnUpdate()
    //{
        //LeanMachine.Initialize();
        //CrackInTheTimeline.Initialize();
    //}
}