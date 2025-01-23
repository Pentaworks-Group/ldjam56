using System.Collections.Generic;

using Assets.Scripts.Scenes.Game.Bee;

using GameFrame.Core.Extensions;

using Unity.VisualScripting;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class SporeEntityBehaviour : MonoBehaviour
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

        [SerializeField]
        private float respawnTime = 5f;

        private MumbleBeeBehaviour targededBy;
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
                if (!collision.gameObject.TryGetComponent(out BoosterBehaviour boosterBehaviour))
                {
                    if (!collision.transform.parent.parent.TryGetComponent(out boosterBehaviour))
                    {
                        throw new System.Exception("Not Booster found!");
                    }
                }

                GetEaten(boosterBehaviour);

            }
        }



        public void GetEaten(BoosterBehaviour eater)
        {
            wasTriggered = true;

            worldBehaviour.WasCaptured(this);

            eater.AddBoostPower(1);
            gotchaParticles.SetActive(true);
            sphere.SetActive(false);
            particles.SetActive(false);
            GameFrame.Base.Audio.Effects.Play(nomSounds.GetRandomEntry());
        }

        public void Respawn()
        {
            gotchaParticles.SetActive(false);
            sphere.SetActive(true);
            particles.SetActive(true);
            wasTriggered = false;
        }

        public float GetRespawnTime()
        {
            return respawnTime;
        }
    }
}