using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Definitons.Loaders;
using Assets.Scripts.Core.Generation;

using GameFrame.Core.Definitions.Loaders;
using GameFrame.Core.Extensions;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        private readonly DefinitionCache<GameMode> gameModeCache = new DefinitionCache<GameMode>();

        private readonly List<UnityEngine.AudioClip> buttonAudioClips = new List<AudioClip>();

        public override void PlayButtonSound()
        {
            GameFrame.Base.Audio.Effects.Play(this.buttonAudioClips.GetRandomEntry());
        }

        protected override GameState InitializeGameState()
        {
            var mode = this.gameModeCache.Values.First();

            var gameState = new GameState()
            {
                GameMode = mode,
            };

            var worldGenerator = new NewWorldGenerator(mode.World);

            gameState.World = worldGenerator.Generate();

            return gameState;
        }

        protected override PlayerOptions InitialzePlayerOptions()
        {
            return new PlayerOptions()
            {
                EffectsVolume = 0.7f,
                AmbienceVolume = 0.2f,
                BackgroundVolume = 0.15f,
                ShowTouchPads = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            };
        }

        protected override void InitializeAudioClips()
        {
            InitializeBackgroundMusic();
            InitializeAmbienceSounds();
            InitializeButtonEffects();
        }

        private void InitializeBackgroundMusic()
        { }

        private void InitializeAmbienceSounds()
        {
            var ambienceTracks = new List<AudioClip>()
            {
                GameFrame.Base.Resources.Manager.Audio.Get("WoodSound")
            };

            GameFrame.Base.Audio.Ambience.Play(ambienceTracks);
        }

        private void InitializeButtonEffects()
        {
            this.buttonAudioClips.Add(GameFrame.Base.Resources.Manager.Audio.Get("Button"));

            //this.buttonAudioClips.Add(GameFrame.Base.Resources.Manager.Audio.Get("ButtonEffect1"));
            //this.buttonAudioClips.Add(GameFrame.Base.Resources.Manager.Audio.Get("ButtonEffect2"));
        }

        protected override IEnumerator LoadDefintions()
        {
            //var filePath = $"{Application.streamingAssetsPath}/GameFields.json";
            //var filePath2 = $"{Application.streamingAssetsPath}/GameModes.json";

            var entityCache = new DefinitionCache<EntityDefinition>();
            var biomeCache = new DefinitionCache<BiomeDefinition>();

            yield return new DefinitionLoader<EntityDefinition>(entityCache).LoadDefinitions("Entities.json");
            yield return new BiomesLoader(biomeCache, entityCache).LoadDefinitions("Biomes.json");
            yield return new GameModeLoader(this.gameModeCache, biomeCache, entityCache).LoadDefinitions("GameModes.json");

            Debug.Log("Done loading Definitions");
            Debug.Log(entityCache.Values.Count);
            Debug.Log(biomeCache.Values.Count);
            Debug.Log(gameModeCache.Values.Count);
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
