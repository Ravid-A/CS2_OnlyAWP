using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;

namespace CS2_OnlyAWP.Modules;

internal static class Utils
{

    public static void ThrowError(string msg)
    {
        throw new Exception(msg);
    }

    public static void SetPlayerArmor(CCSPlayerController playerController)
    {
        ConVar? mp_free_armor = ConVar.Find("mp_free_armor");

        if(mp_free_armor == null)
            ThrowError("mp_free_armor is null");

        int armorValue = mp_free_armor!.GetPrimitiveValue<int>();

        switch(armorValue)
        {
            case 1:
            {
                SetClientArmor(playerController, 100);
                SetClientHelmet(playerController, false);
                break;
            }
            
            case 2:
            {
                SetClientArmor(playerController, 100);
                SetClientHelmet(playerController, true);
                break;
            }
            
            default:
            {
                SetClientArmor(playerController, 0);
                SetClientHelmet(playerController, false);
                break;
            }
        }
    }

    private static void SetClientArmor(CCSPlayerController playerController, int armorValue)
    {
        playerController.PawnArmor = armorValue;
    }

    private static void SetClientHelmet(CCSPlayerController playerController, bool hasHelmet)
    {
        playerController.PawnHasHelmet = hasHelmet;
    }
}
