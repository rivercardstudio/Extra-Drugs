#if IL2CPPMELON
using Il2CppScheduleOne.Calling;
using Il2CppScheduleOne.ScriptableObjects;
using Il2CppScheduleOne.ItemFramework;
#elif MONOMELON
using ScheduleOne.Calling;
using ScheduleOne.ScriptableObjects;
using ScheduleOne.ItemFramework;
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
            InventoryPatch.active = true;
            SetStoredItemPatch.active = true;
            CrackInTheTimeline.CompletePostBenziesCall();
        }
    }
}

public static class InventoryPatch
{
    public static bool active = false;
    public static void CheckForCrack(ItemInstance item, int amount)
    {
        if (item == null)
            return;

        if (!active)
            return;

        string id = item.ID.ToLower();

        if (id == "rivercardstudio.extradrugs:products/crack" ||
            id == "crack")
        {
            CrackInTheTimeline.CompleteCookCrack();
        }
    }
}

[HarmonyPatch(typeof(ItemSlot), "ChangeQuantity")]
public class ChangeQuantityPatch
{
    [HarmonyPostfix]
    public static void Postfix(ItemSlot __instance, int change)
    {
        if (change <= 0)
            return;

        InventoryPatch.CheckForCrack(__instance.ItemInstance, change);
    }
}

[HarmonyPatch(typeof(ItemSlot), "SetStoredItem")]
public class SetStoredItemPatch
{
    public static bool active = false;

    [HarmonyPostfix]
    public static void Postfix(ItemSlot __instance, ItemInstance instance)
    {
        if (instance == null)
            return;

        if (!active)
            return;

        string id = instance.ID.ToLower();

        if (id == "rivercardstudio.extradrugs:products/crack" ||
            id == "crack")
        {
            CrackInTheTimeline.CompleteCookCrack();
        }
    }
}