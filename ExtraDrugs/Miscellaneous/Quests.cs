using S1API.Entities;
using S1API.Entities.NPCs.Docks;
using S1API.PhoneCalls;
using S1API.Quests;
using S1API.Quests.Constants;
using S1API.Quests.Identifiers;
using S1API.Utils;
using UnityEngine;

namespace ExtraDrugs.Miscellaneous
{
    public class CrackInTheTimeline : Quest
    {
        protected override string Title => "Crack In The Timeline";
        protected override string Description => "Step your cocaine game up by turning it into crack.";
        protected override bool AutoBegin => false;
        protected override Sprite? QuestIcon => ImageUtils.LoadImage("River Card Studio/QuestIcon.png");

        private static CrackInTheTimeline? crackInTheTimeline;
        public static CrackInTheTimeline? Quest => crackInTheTimeline;

        public static bool initialized = false;
        public static bool sent = false;

        private static QuestEntry? entryIntroCall;
        private static QuestEntry? entryCookCrack;
        private static QuestEntry? entryBakeCrack;

        public CrackInTheTimeline()
        {
            entryIntroCall = AddEntry("Talk to Uncle Nelson at a payphone");
            entryCookCrack = AddEntry("Cook liquid crack at the chemistry station");
            entryBakeCrack = AddEntry("Bake the liquid crack with the lab oven");
        }

        public static void Initialize()
        {
            if (initialized) return;
            var defeatCartel = QuestManager.Get<DefeatCartel>();
            if (defeatCartel != null && defeatCartel.QuestEntries.TrueForAll(e => e.State == QuestState.Completed))
            {
                crackInTheTimeline = (CrackInTheTimeline)QuestManager.CreateQuest<CrackInTheTimeline>();
                crackInTheTimeline.Begin();
                CallManager.QueueCall(new IntroCall());
                initialized = true;
            }
        }

        public static void CompleteIntroCall()
        {
            if (sent) return;
            entryIntroCall?.Complete();
            entryCookCrack?.Begin();
            NPC.Get<SalvadorMoreno>()?.SendTextMessage(
                "Hey, Uncle Nelson told me you'd be interested in making crack. " + "\n" + "\n" +
                "Get 10g of cocaine, 1 packet of baking soda, and 1 jug of horse semen. " + "\n" + "\n" +
                "Cook those 3 ingredients in the chemistry station, this produces liquid crack. " + "\n" + "\n" +
                "Put the liquid crack in the lab oven, and hammer it once done."
                );
            sent = true;
        }

        public static void CompleteCookCrack()
        {
            entryCookCrack?.Complete();
            entryBakeCrack?.Begin();
        }

        public static void CompleteBakeCrack()
        {
            entryBakeCrack?.Complete();
            crackInTheTimeline?.Complete();
        }
    }
}