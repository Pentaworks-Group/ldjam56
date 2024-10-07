using Assets.Scripts.Scenes.Game.Bee;

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

            if (!wasTriggered && collision.gameObject.layer == 9)
            {
                wasTriggered = true;
                worldBehaviour.WasCaptured(this);

                var boosterBehaviour = collision.gameObject.GetComponent<BoosterBehaviour>();
                boosterBehaviour.AddBoostPower(1);

                gotchaParticles.SetActive(true);
                sphere.SetActive(false);
                particles.SetActive(false);
                Destroy(gameObject, 3f);
            }
        }



    }
}