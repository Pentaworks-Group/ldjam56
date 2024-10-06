using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class EventEntityBehaviour : MonoBehaviour
    {
        [SerializeField]
        private WorldEventsBehaviour worldBehaviour;

        [SerializeField]
        private GameObject gotchaParticles;
        [SerializeField]
        private GameObject sphere;
        [SerializeField]
        private GameObject particles;

        private bool wasTriggered = false;

        private void OnTriggerEnter(Collider collision)
        {
            if (!wasTriggered)
            {
                wasTriggered = true;
                worldBehaviour.WasCaptured(this);
                gotchaParticles.SetActive(true);
                sphere.SetActive(false);
                particles.SetActive(false);
                Destroy(gameObject, 3f);
            }
        }



    }
}