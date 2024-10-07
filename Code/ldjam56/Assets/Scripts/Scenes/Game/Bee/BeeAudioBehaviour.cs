using System;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game.Bee
{
    public class BeeAudioBehaviour : MonoBehaviour
    {
        private Boolean isStarted;

        public Model.Bee bee;
        public AudioSource beeAudioSource;

        private void Start()
        {
            this.bee = Base.Core.Game.State?.Bee;

            this.beeAudioSource.volume = GameFrame.Base.Audio.Ambience.Volume;
            GameFrame.Base.Audio.Ambience.VolumeChanged.AddListener(OnAmbienceVolumeChanged);
            Base.Core.Game.OnPauseToggled.AddListener(OnGamePauseToggled);
        }

        private void OnGamePauseToggled(Boolean isPaused)
        {
            if (isPaused)
            {
                this.beeAudioSource.Pause();
            }
            else
            {
                this.beeAudioSource.UnPause();
            }
        }

        private void OnAmbienceVolumeChanged(Single volume)
        {
            this.beeAudioSource.volume = volume;
        }

        private void Update()
        {
            if (this.bee != null)
            {
                if (!isStarted)
                {
                    isStarted = true;

                    this.beeAudioSource.volume = GameFrame.Base.Audio.Ambience.Volume;
                    this.beeAudioSource.Play();
                }
            }
        }
    }
}
