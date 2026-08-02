#if IL2CPPMELON
using Il2CppScheduleOne.Calling;
using Il2CppScheduleOne.ScriptableObjects;
using Il2CppInterop.Runtime;
#elif MONOMELON
using ScheduleOne.Calling;
using ScheduleOne.ScriptableObjects;
#endif
using HarmonyLib;
using ExtraDrugs.Miscellaneous;

[HarmonyPatch(typeof(PayPhone), "OnCallCompleted")]
class PayPhonePatch
{
    [HarmonyPostfix]
    static void Postfix(PhoneCallData data)
    {
        if (data == null)
            return;

        if (data == IntroCall.IntroCallData)
        {
            LeanMachine.CompleteIntroCall();
        }
        else if (data == PostBenziesCall.PostBenziesCallData)
        {
            CrackInTheTimeline.CompletePostBenziesCall();
        }
    }
}