
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Core.Attributes;
using CS2_OnlyAWP.Modules;
using CS2_OnlyAWP.Modules.Handlers;

namespace CS2_OnlyAWP;

// Possible results for CSPlayer::CanAcquire
public enum AcquireResult : int
{
    Allowed = 0,
    InvalidItem,
    AlreadyOwned,
    AlreadyPurchased,
    ReachedGrenadeTypeLimit,
    ReachedGrenadeTotalLimit,
    NotAllowedByTeam,
    NotAllowedByMap,
    NotAllowedByMode,
    NotAllowedForPurchase,
    NotAllowedByProhibition,
};

// Possible results for CSPlayer::CanAcquire
public enum AcquireMethod : int
{
    PickUp = 0,
    Buy,
};

[MinimumApiVersion(202)]
public class CS2_OnlyAWP : BasePlugin
{
    public override string ModuleName => "CS2_OnlyAWP Plugin";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Ravid";
    public override string ModuleDescription => "CS2_OnlyAWP Plugin";

    public static CS2_OnlyAWP Instance { get; private set; } = null!;

    public required MemoryFunctionWithReturn<CCSPlayer_ItemServices, CEconItemView, AcquireMethod, NativeObject, AcquireResult> CCSPlayer_CanAcquireFunc;
    public static MemoryFunctionWithReturn<int, string, CCSWeaponBaseVData>? GetCSWeaponDataFromKeyFunc;

    public override void Load(bool hotReload)
    {
        Instance = this;

        Events.RegisterEvents();

        GetCSWeaponDataFromKeyFunc = new(GameData.GetSignature("GetCSWeaponDataFromKey"));

        if(GetCSWeaponDataFromKeyFunc == null)
            Utils.ThrowError("GetCSWeaponDataFromKeyFunc is null");

        CCSPlayer_CanAcquireFunc = new(GameData.GetSignature("CCSPlayer_CanAcquire"));
        CCSPlayer_CanAcquireFunc.Hook(DynamicHooks.OnWeaponCanAcquire, HookMode.Pre);
    }
}
