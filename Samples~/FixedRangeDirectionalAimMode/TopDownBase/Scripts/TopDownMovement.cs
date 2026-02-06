using UnityEngine;

namespace Flscap.TopDown.Base
{
    public class TopDownMovement : MonoBehaviour
    {
        public float Speed = 5f;

        public void Move(Vector2 input)
        {
            Vector3 delta = new Vector3(input.x, 0f, input.y);
            transform.Translate(delta * Speed * Time.deltaTime, Space.World);
        }
    }
}
