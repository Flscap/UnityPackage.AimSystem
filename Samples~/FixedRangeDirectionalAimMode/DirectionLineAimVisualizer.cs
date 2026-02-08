using Flscap.AimSystem;
using UnityEngine;

public sealed class DirectionLineAimVisualizer
    : AimVisualizer<IAimMode<DirectionalAimData>>
{
    [SerializeField] private LineRenderer _lineRenderer;

    private void Update()
    {
        var data = AimMode.Data;

        if (data.Origin.HasValue &&
            data.Direction.HasValue &&
            data.Range.HasValue)
        {
            _lineRenderer.enabled = true;

            Vector3 start = data.Origin.Value;
            Vector3 end = start + data.Direction.Value.normalized * data.Range.Value;

            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}
