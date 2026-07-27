using MelonLoader;
using S1API.Entities;
using S1API.Entities.NPCs;
using S1API.PhoneCalls;
using S1API.Quests;
using S1API.Quests.Identifiers;

namespace ExtraDrugs.Miscellaneous
{
    public class PhoneCall1 : PhoneCallDefinition
    {
        public PhoneCall1() : base(NPC.Get<UncleNelson>())
        {
            AddStage("You finally finished off <h1>the Benzies</h>, and you've also built up a customer base. I haven't said this often, but I'm proud of you, nephew.");
            AddStage("My trial just finished, and I'm going in the pen for a while. I know you can do this alone, I believe in you.");
            AddStage("If you want to expand even further, you can start manufacturing <h1>MDMA</h>. There's this guy, <h1>Jean Redneck</h> . I believe he can hook you up with some precursors.");
            AddStage("I gotta go now, I'll miss you nephew.");
            Completed();
        }
    }

    public class PhoneCallTriggers
    {
        public bool isPhoneCall1Done;

        public void PhoneCall1Trigger()
        {
            if (isPhoneCall1Done)
                return;

            var quest = QuestManager.Get<DefeatCartel>();

            if (quest != null)
            {
                quest.OnComplete += OnQuestCompleted;
                isPhoneCall1Done = true;
            }
        }

        private void OnQuestCompleted()
        {
            MelonLogger.Msg("Queueing phone call.");
            CallManager.QueueCall(new PhoneCall1());
        }
    }
}