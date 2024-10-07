using Assets.Scripts.Scenes.Game;

using UnityEngine;

namespace Assets.Scripts
{
	public class HiveBehaviour : MonoBehaviour
	{
        [SerializeField]
        private WorldEventsBehaviour worldBehaviour;

        private void OnTriggerEnter(Collider other)
        {
            worldBehaviour.BeeCameHome();
        }
    }
}
