#if IL2CPPMELON
using Il2CppScheduleOne.Calling;
using Il2CppScheduleOne.ScriptableObjects;
#elif MONOMELON
using ScheduleOne.ScriptableObjects;
#endif
using S1API.Entities;
using S1API.Entities.NPCs;
using S1API.PhoneCalls;

namespace ExtraDrugs.Miscellaneous
{
    public class IntroCall : PhoneCallDefinition
    {
        public static PhoneCallData? IntroCallData;

        public IntroCall() : base(NPC.Get<UncleNelson>())
        {
            IntroCallData = S1PhoneCallData;
            AddStage(
                "You can use the mixing station for mixing, but you can also use it to make other stuff such as edible mixtures, but also <h1>lean</h>. " +
                "It's something you easily get done, and you can do it pretty fast and cheap. "
                );
            AddStage(
                "Head over to the local Gas-Mart, grab yourself some <h1>cough syrup</h> and <h1>cuke</h>. " +
                "Mix those ingredients together in the mixing station. "
                );
            AddStage(
                "Then go to a hardware store, and buy a <h1>liquid station</h> and some <h1>empty bottles</h>. " +
                "You can use the liquid station to package lean"
                );
            Completed();
        }
    }

    public class PostBenziesCall : PhoneCallDefinition
    {
        public static PhoneCallData? PostBenziesCallData;

        public PostBenziesCall() : base(NPC.Get<UncleNelson>())
        {
            PostBenziesCallData = S1PhoneCallData;
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