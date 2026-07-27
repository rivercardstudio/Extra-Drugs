using ExtraDrugs;
using ExtraDrugs.Developer;
using ExtraDrugs.Miscellaneous;
using MelonLoader;

[assembly: MelonInfo(typeof(Start), "Extra Drugs", "0.1", "River Card Studio")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace ExtraDrugs
{
    public class Start : MelonMod
    {
        private readonly PhoneCallTriggers phoneCallTriggers = new();

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("We're so back my friend. And thank you for downloading my mod.");
        }

        public override void OnUpdate()
        {
            phoneCallTriggers.PhoneCall1Trigger();
        }
    }
}