using Flscap.AimSystem;

public sealed class FixedRangeDirectionalAimMode
    : IAimMode<DirectionalAimData>
{
    private readonly IOriginProvider _originProvider;
    private readonly IDirectionProvider _directionProvider;
    private readonly float _range;

    private readonly DirectionalAimData _data = new();

    public FixedRangeDirectionalAimMode(
        IOriginProvider originProvider,
        IDirectionProvider directionProvider,
        float range)
    {
        _originProvider = originProvider;
        _directionProvider = directionProvider;
        _range = range;
    }

    public DirectionalAimData Data => _data;

    public void UpdateState(float deltaTime)
    {
        _data.Origin = _originProvider.GetOrigin();
        _data.Direction = _directionProvider.GetDirection(_data.Origin);
        _data.Range = _range;
    }

    public bool TryProjectIntent(out object aimData)
    {
        if (_data.Origin == null || _data.Direction == null || _data.Range == null)
        {
            aimData = null;
            return false;
        }

        // No transformation. This is the result.
        aimData = _data;
        return true;
    }
}
