namespace BossMod.Endwalker.VariantCriterion.V3AloAloIsland.V31QuaQua;

public enum OID : uint
{
    Quaqua = 0x40BA,
    Helper = 0x233C,
    AethericCharge = 0x40BB,
    ScaldingWaves = 0x40BE,
    ScaldingWaves1 = 0x4134,
}

public enum AID : uint
{
    MadeMagic = 35732, // 40BA->self, 5.0s cast, range 50 circle
    ArcaneArmaments = 35720, // 40BA->self, 8.0s cast, single-target
    _Weaponskill_ = 35721, // 233C->self, no cast, single-target
    RavagingAxe = 35722, // 40BB->self, 2.0s cast, range 14 circle
    RingingQuoits = 35723, // 40BB->self, 2.0s cast, range 5-18 donut
    ArcaneArmaments1 = 35724, // 40BA->self, 5.0s cast, single-target
    _Weaponskill_HammerLanding = 35725, // 40BA->location, 8.0s cast, range 40 circle
    _Weaponskill_HammerLanding1 = 35726, // 40BA->location, no cast, range 40 circle
    _Weaponskill_VioletStorm = 35733, // 40BA->self, 5.5s cast, range 32 120.000-degree cone
    _Weaponskill_Howl = 35734, // 40BA->self, 4.0s cast, single-target
    _Weaponskill_ScaldingWaves = 35735, // 40BE->self, 5.0s cast, range 50 width 8 rect
    _Weaponskill_ScaldingWaves1 = 35736, // 4134->self, no cast, range 50 width 4 rect
}

[SkipLocalsInit]
sealed class QuaquaStates : StateMachineBuilder
{
    public QuaquaStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<ArcaneArmaments>();

    }
}

[ModuleInfo(BossModuleInfo.Maturity.WIP,
    StatesType = typeof(QuaquaStates),
    ConfigType = null, // replace null with typeof(QuaquaConfig) if applicable
    ObjectIDType = typeof(OID),
    ActionIDType = null, // replace null with typeof(AID) if applicable
    StatusIDType = null, // replace null with typeof(SID) if applicable
    TetherIDType = null, // replace null with typeof(TetherID) if applicable
    IconIDType = null, // replace null with typeof(IconID) if applicable
    PrimaryActorOID = (uint)OID.Quaqua,
    Contributors = "",
    Expansion = BossModuleInfo.Expansion.Endwalker,
    Category = BossModuleInfo.Category.VariantCriterion,
    GroupType = BossModuleInfo.GroupType.CFC,
    GroupID = 961u,
    NameID = 12527u,
    SortOrder = 1,
    PlanLevel = 0)]
[SkipLocalsInit]
public sealed class Quaqua(WorldState ws, Actor primary) : BossModule(ws, primary, new(-538f, 94f), new ArenaBoundsCircle(20f));

sealed class ArcaneArmaments(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = [];
    public override ReadOnlySpan<AOEInstance> ActiveAOEs(int slot, Actor actor)
    {
        var aoes = CollectionsMarshal.AsSpan(_aoes);
        var len = aoes.Length;

        if (len == 0)
            return [];

        List<AOEInstance> upcoming = [];
        var firstAct = aoes[0].Activation;

        for (var i = 0; i < len; i++)
        {
            if (aoes[i].Activation == firstAct)
            {
                upcoming.Add(aoes[i]);
            }
        }

        return CollectionsMarshal.AsSpan(upcoming);
    }
    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if (caster.OID == (uint)OID.AethericCharge)
        {
            if (spell.Action.ID == (uint)AID.RavagingAxe)
            {
                _aoes.Add(new(new AOEShapeCircle(14f), actor.Position, activation: WorldState.CurrentTime.AddSeconds(7.7d)));
            }
            if (actor.CastInfo.Action.ID == (uint)AID.RingingQuoits)
            {
                _aoes.Add(new(new AOEShapeDonut(5f, 18f), actor.Position, activation: WorldState.CurrentTime.AddSeconds(7.7d)));
            }
        }
    }
}
