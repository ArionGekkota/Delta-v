using Content.Shared.Psionics.Glimmer;
using Content.Shared._DV.Psionics.Components;
using Content.Shared.Alert;
using Robust.Client.Player;
using Robust.Shared.Prototypes;

namespace Content.Client._DV.Psionics;

public sealed class GlimmerAlertSystem : EntitySystem
{
    [Dependency] private readonly AlertsSystem _alerts = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    private ProtoId<AlertPrototype> _highGlimmerAlert = "HighGlimmer";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<GlimmerChangedEvent>(OnGlimmerChanged);
    }

    private void OnGlimmerChanged(GlimmerChangedEvent eventArgs)
    {
        // Update alert on glimmer change
        if (_player.LocalSession?.AttachedEntity is not { } ent)
            return;

        if (!TryComp<PotentialPsionicComponent>(ent, out var PotentialPsionic))
            return;

        var alertProto = _prototype.Index(_highGlimmerAlert);

        if(eventArgs.Glimmer > 700)
            _alerts.ShowAlert(ent, alertProto);
        else
            _alerts.ClearAlert(ent, alertProto);
    }
}
