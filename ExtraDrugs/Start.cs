using ExtraDrugs;
using ExtraDrugs.Miscellaneous;
using MelonLoader;

[assembly: MelonInfo(typeof(Start), "Extra Drugs", "1-BETA2", "River Card Studio")][assembly: MelonGame("TVGS", "Schedule I")]

namespace ExtraDrugs
{
    public class Start : MelonMod
    {
        public override void OnUpdate()
        {
            LeanMachine.Initialize();
            CrackInTheTimeline.Initialize();
        }
    }
}