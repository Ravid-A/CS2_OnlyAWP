using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace CS2_OnlyAWP.Modules.Handlers;

internal static class DynamicHooks
{
    private static string[] AllowedWeapons = 
    {
        "awp",
        "knife",
        "bayonet",
    };

    public static HookResult OnWeaponCanAcquire(DynamicHook hook)
    {
        AcquireMethod acquireMethod = hook.GetParam<AcquireMethod>(2);

        if(acquireMethod == AcquireMethod.Buy)
        {
            hook.SetReturn(AcquireResult.NotAllowedForPurchase);
            return HookResult.Stop;
        }

        if(CS2_OnlyAWP.GetCSWeaponDataFromKeyFunc == null)
        {
            Utils.ThrowError("GetCSWeaponDataFromKeyFunc is null");
            return HookResult.Continue;
        }

        var vdata = CS2_OnlyAWP.GetCSWeaponDataFromKeyFunc.Invoke(-1, hook.GetParam<CEconItemView>(1).ItemDefinitionIndex.ToString());

        if(vdata == null)
        {
            Utils.ThrowError("vdata is null");
            return HookResult.Continue;
        }

        if(!IsAllowedWeapon(vdata.Name))
        {
            hook.SetReturn(AcquireResult.NotAllowedByMode);
            return HookResult.Stop;
        }

        CCSPlayerController client = hook.GetParam<CCSPlayer_ItemServices>(0).Pawn.Value!.Controller.Value!.As<CCSPlayerController>();

        if(client == null)
        {
            Utils.ThrowError("client is null");
            return HookResult.Continue;
        }

        CCSPlayer_WeaponServices cCSPlayer_WeaponServices = new CCSPlayer_WeaponServices(client.Pawn.Value!.WeaponServices!.Handle);

        if(cCSPlayer_WeaponServices == null)
        {
            Utils.ThrowError("cCSPlayer_WeaponServices is null");
            return HookResult.Continue;
        }

        var weapons = cCSPlayer_WeaponServices.MyWeapons;

        if(weapons == null)
        {
            Utils.ThrowError("weapon is null");
            return HookResult.Continue;
        }

        if(weapons.Count <= 0)
        {
            return HookResult.Continue;
        }
        
        return HookResult.Continue;
    }

    private static bool IsAllowedWeapon(string weapon)
    {
        foreach(var allowedWeapon in AllowedWeapons)
        {
            if(weapon.Contains(allowedWeapon))
            {
                return true;
            }
        }

        return false;
    }
}