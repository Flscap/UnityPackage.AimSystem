using UnityEngine;
using UnityEngine.InputSystem;

namespace Flscap.TopDown.Base
{
    public class TopDownInputReader : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public bool ActivateAbilityPressed { get; private set; }
        public bool ConfirmAimPressed { get; private set; }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        public void OnActivateAbility(InputAction.CallbackContext context)
        {
            Debug.Log("OnActivateAbility");
            if (context.performed)
                ActivateAbilityPressed = true;
        }

        public void OnConfirmAim(InputAction.CallbackContext context)
        {
            Debug.Log("OnActivateAbility");
            if (context.performed)
                ConfirmAimPressed = true;
        }
        
        public void ConsumeActivateAbility()
        {
            ActivateAbilityPressed = false;
        }

        public void ConsumeConfirmAim()
        {
            ConfirmAimPressed = false;
        }
    }
}