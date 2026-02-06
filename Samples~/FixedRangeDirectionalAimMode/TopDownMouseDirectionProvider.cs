using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownMouseDirectionProvider : IDirectionProvider
{
    private readonly Camera _camera;
    private readonly float _groundY;

    public TopDownMouseDirectionProvider(Camera camera, float groundY = 0f)
    {
        _camera = camera;
        _groundY = groundY;
    }

    public Vector3? GetDirection(Vector3? origin)
    {
        if (!origin.HasValue || _camera == null)
            return null;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePos);

        Plane plane = new Plane(Vector3.up, new Vector3(0f, _groundY, 0f));

        if (!plane.Raycast(ray, out float distance))
            return null;

        Vector3 hitPoint = ray.GetPoint(distance);
        Vector3 dir = hitPoint - origin.Value;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
            return null;

        return dir.normalized;
    }
}
