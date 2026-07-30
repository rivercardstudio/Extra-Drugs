using HarmonyLib;
using ScheduleOne.Calling;
using ScheduleOne.ScriptableObjects;

namespace ExtraDrugs.Miscellaneous
{
    [HarmonyPatch(typeof(PayPhone), "OnCallCompleted")]
    class PayPhonePatch
    {
        [HarmonyPostfix]
        static void Postfix(PhoneCallData data)
        {
            if (ReferenceEquals(data, IntroCall.IntroCallData))
            {
                CrackInTheTimeline.CompleteIntroCall();
            }
        }
    }
}