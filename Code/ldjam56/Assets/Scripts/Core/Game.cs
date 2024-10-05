using System.Collections;
using System.Collections.Generic;

using GameFrame.Core.Extensions;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        private readonly List<UnityEngine.AudioClip> buttonAudioClips = new List<AudioClip>();

        public override void PlayButtonSound()
        {
            GameFrame.Base.Audio.Effects.Play(this.buttonAudioClips.GetRandomEntry());
        }

        protected override GameState InitializeGameState()
        {
            return new GameState();
        }

        protected override PlayerOptions InitialzePlayerOptions()
        {
            return new PlayerOptions();
        }

        protected override void InitializeAudioClips()
        {
            InitializeButtonEffects();
        }

        private void InitializeButtonEffects()
        {
            this.buttonAudioClips.Add(GameFrame.Base.Resources.Manager.Audio.Get("Button"));

            //this.buttonAudioClips.Add(GameFrame.Base.Resources.Manager.Audio.Get("ButtonEffect1"));
            //this.buttonAudioClips.Add(GameFrame.Base.Resources.Manager.Audio.Get("ButtonEffect2"));
        }

        protected override IEnumerator LoadDefintions()
        {
            yield break;
        }

        protected override void OnGameStartup()
        { }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void GameStart()
        {
            Base.Core.Game.Startup();
        }
    }
}
