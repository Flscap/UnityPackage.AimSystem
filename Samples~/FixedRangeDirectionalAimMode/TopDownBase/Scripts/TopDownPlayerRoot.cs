using Flscap.AimSystem;
using UnityEngine;

namespace Flscap.TopDown.Base
{
    public class TopDownPlayerRoot : MonoBehaviour
    {
        public TopDownInputReader Input;
        public TopDownMovement Movement;
        public Camera Camera;
        public AimVisualizer AimVisualizerPrefab;
        private AimRequest _currentAimRequest;
        private bool _isAiming;

        void Update()
        {
            Movement.Move(Input.Move);

            if (!_isAiming && Input.ActivateAbilityPressed)
            {
                StartAim();
                Input.ConsumeActivateAbility();
            }

            if (_isAiming && Input.ConfirmAimPressed)
            {
                CommitAim();
                Input.ConsumeConfirmAim();
            }
        }

        private void StartAim()
        {
            var originProvider = new TopDownTransformOriginProvider(transform);
            var directionProvider = new TopDownMouseDirectionProvider(Camera);

            var aimMode = new FixedRangeDirectionalAimMode(
                originProvider,
                directionProvider,
                range: 10f
            );

            _currentAimRequest = new AimRequest<AbilityContext, DirectionalAimData>(
                new AbilityContext
                {
                    Name = "Test ability"
                },
                (context, result) =>
                {
                    Debug.Log($"Ability: {context.Name}, Origin: {result.Origin}, Direction: {result.Direction}, Range: {result.Range}");
                });

            _isAiming = true;

            GetComponent<AimSystem.AimSystem>().StartAiming(
                _currentAimRequest,
                aimMode,
                AimVisualizerPrefab
            );
        }

        private void CommitAim()
        {
            GetComponent<AimSystem.AimSystem>().TryCommitAim(_currentAimRequest);
            _currentAimRequest = null;
            _isAiming = false;
        }


    }
}
