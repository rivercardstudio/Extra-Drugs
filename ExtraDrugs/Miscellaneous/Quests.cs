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
    public class LeanMachine : Quest
    {
        protected override string Title => "Lean Machine";
        protected override string Description => "You make your first batch of lean.";
        protected override bool AutoBegin => false;
        protected override Sprite? QuestIcon => ImageUtils.LoadImage("River Card Studio/QuestIcon.png");

        private static LeanMachine? leanMachine;
        public static LeanMachine? Quest => leanMachine;

        public static bool initialized = false;

        public static bool liquidStationCompleted = false;
        public static bool emptyBottleCompleted = false;
        public static bool coughSyrupCompleted = false;
        public static bool cukeCompleted = false;

        private static QuestEntry? entryIntroCall;
        private static QuestEntry? entryLiquidStation;
        private static QuestEntry? entryEmptyBottle;
        private static QuestEntry? entryCoughSyrup;
        private static QuestEntry? entryCuke;
        private static QuestEntry? entryLean;

        public LeanMachine()
        {
            entryIntroCall = AddEntry("Talk to Uncle Nelson at a payphone");
            entryLiquidStation = AddEntry("Buy 1x liquid station");
            entryEmptyBottle = AddEntry("Buy 10x empty bottles");
            entryCoughSyrup = AddEntry("Buy 2x cough syrup");
            entryCuke = AddEntry("Buy 8x cuke");
            entryLean = AddEntry("Make lean at the mixing station");
        }

        public static void Initialize()
        {
            if (initialized) return;
            var mixingMania = QuestManager.Get<MixingMania>();
            if (mixingMania != null && mixingMania.QuestEntries.TrueForAll(e => e.State == QuestState.Completed))
            {
                leanMachine = (LeanMachine)QuestManager.CreateQuest<LeanMachine>();
                leanMachine.Begin();
                CallManager.QueueCall(new IntroCall());
                initialized = true;
            }
        }

        public static void CompleteIntroCall()
        {
            entryIntroCall?.Complete();
            entryLiquidStation?.Begin();
            entryEmptyBottle?.Begin();
            entryCoughSyrup?.Begin();
            entryCuke?.Begin();
        }

        public void CompleteLiquidStation()
        {
            if (liquidStationCompleted) return;

            liquidStationCompleted = true;
            entryLiquidStation?.Complete();
            CheckComplete();
        }

        public void CompleteEmptyBottle()
        {
            if (emptyBottleCompleted) return;

            emptyBottleCompleted = true;
            entryEmptyBottle?.Complete();
            CheckComplete();
        }

        public void CompleteCoughSyrup()
        {
            if (coughSyrupCompleted) return;

            coughSyrupCompleted = true;
            entryCoughSyrup?.Complete();
            CheckComplete();
        }

        public void CompleteCuke()
        {
            if (cukeCompleted) return;

            cukeCompleted = true;
            entryCuke?.Complete();
            CheckComplete();
        }

        public void CompleteLean()
        {
            entryLean?.Complete();
        }

        private void CheckComplete()
        {
            if (liquidStationCompleted && emptyBottleCompleted && coughSyrupCompleted && cukeCompleted)
            {
                entryLean?.Begin();
            }
        }
    }

    public class CrackInTheTimeline : Quest
    {
        protected override string Title => "Crack In The Timeline";
        protected override string Description => "Step your cocaine game up by turning it into crack.";
        protected override bool AutoBegin => false;
        protected override Sprite? QuestIcon => ImageUtils.LoadImage("River Card Studio/QuestIcon.png");

        public static CrackInTheTimeline? crackInTheTimeline;
        public static CrackInTheTimeline? Quest => crackInTheTimeline;

        public static bool initialized = false;
        public static bool sent = false;

        public static QuestEntry? entryPostBenziesCall;
        private static QuestEntry? entryCookCrack;
        private static QuestEntry? entryBakeCrack;

        public CrackInTheTimeline()
        {
            entryPostBenziesCall = AddEntry("Talk to Uncle Nelson at a payphone");
            entryCookCrack = AddEntry("Cook crack at the chemistry station");
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
                CallManager.QueueCall(new PostBenziesCall());
                initialized = true;
            }
        }

        public static void CompletePostBenziesCall()
        {
            if (sent) return;
            entryPostBenziesCall?.Complete();
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