using UnityEngine;

public class TopDownTransformOriginProvider : IOriginProvider
{
    private readonly Transform _transform;

    public TopDownTransformOriginProvider(Transform transform)
    {
        _transform = transform;
    }

    public Vector3? GetOrigin()
    {
        if (_transform == null)
            return null;

        return _transform.position;
    }
}
