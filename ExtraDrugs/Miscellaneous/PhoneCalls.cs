using S1API.Entities;
using S1API.Entities.NPCs;
using S1API.PhoneCalls;
using ScheduleOne.ScriptableObjects;

namespace ExtraDrugs.Miscellaneous
{
    public class IntroCall : PhoneCallDefinition
    {
        public static PhoneCallData? IntroCallData;

        public IntroCall() : base(NPC.Get<UncleNelson>())
        {
            IntroCallData = S1PhoneCallData;
            AddStage(
                "And that's how we deal with pests, <h1>Thomas Benzies</h> deserved it. " +
                "I can't lie, he had it coming. " +
                "Now that he's gone, the market's all yours. " +
                "You want to make more money, right?"
                );
            AddStage(
                "You could talk to <h1>Salvador Moreno</h>, I believe he can help you. " +
                "Not only does he supply coca seeds, he also cooks crack in his free time. " +
                "He'll send you the recipe."
                );
            Completed();
        }
    }
}