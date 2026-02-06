using Flscap.AimSystem;

public sealed class FixedRangeDirectionalAimMode
    : IAimMode<DirectionalAimState>
{
    private readonly IOriginProvider _originProvider;
    private readonly IDirectionProvider _directionProvider;
    private readonly float _range;

    private readonly DirectionalAimState _state = new();

    public FixedRangeDirectionalAimMode(
        IOriginProvider originProvider,
        IDirectionProvider directionProvider,
        float range)
    {
        _originProvider = originProvider;
        _directionProvider = directionProvider;
        _range = range;
    }

    public DirectionalAimState State => _state;

    public void UpdateState(float deltaTime)
    {
        _state.Origin = _originProvider.GetOrigin();
        _state.Direction = _directionProvider.GetDirection(_state.Origin);
        _state.Range = _range;
    }

    public bool TryProjectIntent(out object aimData)
    {
        aimData = null;

        if (_state.Origin == null || _state.Direction == null || _state.Range == null)
            return false;

        aimData = new DirectionalAimResult
        {
            Origin = _state.Origin.Value,
            Direction = _state.Direction.Value.normalized,
            Range = _state.Range.Value
        };

        return true;
    }
}
