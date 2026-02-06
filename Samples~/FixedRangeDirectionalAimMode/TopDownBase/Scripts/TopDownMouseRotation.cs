using UnityEngine;
using UnityEngine.InputSystem;

namespace Flscap.TopDown.Base
{
    public class TopDownMouseRotation : MonoBehaviour
    {
        public Camera Camera;

        void Update()
        {
            Ray ray = Camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 target = ray.GetPoint(distance);
                Vector3 dir = target - transform.position;
                dir.y = 0f;

                if (dir.sqrMagnitude > 0.001f)
                    transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }
}
