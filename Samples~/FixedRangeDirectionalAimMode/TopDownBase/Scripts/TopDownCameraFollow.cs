using UnityEngine;

namespace Flscap.TopDown
{
    public class TopDownCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 12f, -6f);
        [SerializeField] private float followSpeed = 10f;

        void LateUpdate()
        {
            if (!target) return;

            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(
                transform.position,
                desiredPosition,
                followSpeed * Time.deltaTime
            );
        }
    }
}
