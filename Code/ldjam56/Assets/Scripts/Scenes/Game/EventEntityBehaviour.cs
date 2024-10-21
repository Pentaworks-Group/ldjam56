using System.Collections.Generic;

using Assets.Scripts.Scenes.Game.Bee;

using GameFrame.Core.Extensions;

using Unity.VisualScripting;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class EventEntityBehaviour : MonoBehaviour
    {
        private static IList<AudioClip> nomSounds;

        [SerializeField]
        private WorldEventsBehaviour worldBehaviour;

        [SerializeField]
        private GameObject gotchaParticles;
        [SerializeField]
        private GameObject sphere;
        [SerializeField]
        private GameObject particles;

        private bool wasTriggered = false;

        private void Awake()
        {
            if (nomSounds == default)
            {
                nomSounds = new List<AudioClip>()
                {
                    GameFrame.Base.Resources.Manager.Audio.Get("NamNam_1"),
                    GameFrame.Base.Resources.Manager.Audio.Get("NamNam_2"),
                };
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (!wasTriggered && collision.gameObject.layer == 9)
            {
                wasTriggered = true;

                worldBehaviour.WasCaptured(this);

                if (!collision.gameObject.TryGetComponent(out BoosterBehaviour boosterBehaviour))
                {
                    if (!collision.transform.parent.parent.TryGetComponent(out boosterBehaviour))
                    {
                        throw new System.Exception("Not Booster found!");
                    }
                }

                boosterBehaviour.AddBoostPower(1);

                gotchaParticles.SetActive(true);
                sphere.SetActive(false);
                particles.SetActive(false);

                Destroy(gameObject, 3f);

                GameFrame.Base.Audio.Effects.Play(nomSounds.GetRandomEntry());
            }
        }

        //public void Hide()
        //{
        //    sphere.SetActive(false);
        //    particles.SetActive(false);
        //}

        //public void Show()
        //{
        //    sphere.SetActive(true);
        //    particles.SetActive(true);

        //}
    }
}