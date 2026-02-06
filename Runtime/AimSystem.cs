using UnityEngine;

namespace Flscap.AimSystem
{
    public class AimSystem : MonoBehaviour
    {
        private AimRequest _aimRequest;
        private IAimMode _aimMode;
        private AimVisualizer _activeVisualizer;

        public void StartAiming(AimRequest aimRequest, IAimMode aimMode, AimVisualizer visualizerPrefab)
        {
            ResetState();

            _aimRequest = aimRequest;
            _aimMode = aimMode;

            if (visualizerPrefab != null)
            {
                _activeVisualizer = Instantiate(visualizerPrefab);
                _activeVisualizer.InitializeInternal(_aimMode);
            }
        }

        private void Update()
        {
            _aimMode?.UpdateState(Time.deltaTime);
        }

        public void TryCommitAim(AimRequest requestToCommit)
        {
            if (_aimRequest == requestToCommit && _aimMode != null && _aimMode.TryProjectIntent(out var aimData))
            {
                _aimRequest.Commit(aimData);
                ResetState();
            }
        }

        private void ResetState()
        {
            if (_activeVisualizer != null)
            {
                _activeVisualizer.Cleanup();
                _activeVisualizer = null;
            }

            _aimRequest = null;
            _aimMode = null;
        }
    }
}
