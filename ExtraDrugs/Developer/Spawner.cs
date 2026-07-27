using ExtraDrugs.Miscellaneous;
using MelonLoader;
using S1API.Cartel;
using UnityEngine;

namespace ExtraDrugs.Developer
{
    public class PhoneCallTrigger
    {
        public void Spawner()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                MelonLogger.Msg("Pressed O");
            }
        }
    }
}