using Flscap.AimSystem;
using UnityEngine;

public sealed class DirectionLineAimVisualizer
    : AimVisualizer<IAimMode<DirectionalAimState>>
{
    [SerializeField] private LineRenderer _lineRenderer;

    private void Update()
    {
        var state = AimMode.State;

        if (state.Origin.HasValue &&
            state.Direction.HasValue &&
            state.Range.HasValue)
        {
            _lineRenderer.enabled = true;

            Vector3 start = state.Origin.Value;
            Vector3 end = start + state.Direction.Value.normalized * state.Range.Value;

            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}
