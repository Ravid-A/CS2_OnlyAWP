using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;

namespace CS2_OnlyAWP.Modules.Handlers;

internal static class Events
{
    public static void RegisterEvents()
    {
        CS2_OnlyAWP.Instance.RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
    }

    private static HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var playerController = @event.Userid;

        if (playerController == null! || !playerController.IsValid)
        {
            return HookResult.Continue;
        }

        playerController.RemoveWeapons();

        playerController.GiveNamedItem(CsItem.AWP);
        playerController.GiveNamedItem(CsItem.Knife);

        Utils.SetPlayerArmor(playerController);

        return HookResult.Continue;
    }
}
