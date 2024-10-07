using UnityEngine;

namespace Assets.Scripts.Scenes.Game.Bee
{
    public class BeeParticleBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject speedUpParticles;
        [SerializeField]
        private MoverBehaviour beeMover;



        private void OnEnable()
        {
            beeMover.SpeedUp.AddListener(EnableSpeedUpParticles);
            beeMover.NeutralSpeed.AddListener(DisableSpeedUpParticles);
        }

        private void OnDisable()
        {
            beeMover.SpeedUp.RemoveListener(EnableSpeedUpParticles);
            beeMover.NeutralSpeed.RemoveListener(DisableSpeedUpParticles);
        }

        private void EnableSpeedUpParticles(float factor)
        {
            speedUpParticles.SetActive(true);
        }


        private void DisableSpeedUpParticles()
        {
            speedUpParticles.SetActive(false);
        }
    }
}