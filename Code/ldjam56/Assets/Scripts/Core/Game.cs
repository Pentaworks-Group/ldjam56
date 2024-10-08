using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Constants;
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
                CurrentScene = SceneNames.Game
            };

            var worldGenerator = new NewWorldGenerator(mode.World);

            gameState.World = worldGenerator.Generate();

            if (gameState.Bee == default)
            {
                gameState.Bee = new Model.Bee();
            }

            Debug.Log("GameState Biomes: " + gameState.World.Biomes.Count);
            return gameState;
        }

        protected override PlayerOptions InitializePlayerOptions()
        {
            return new PlayerOptions()
            {
                EffectsVolume = 0.9f,
                AmbienceVolume = 0.1f,
                BackgroundVolume = 0.05f,
                ShowTouchPads = SystemInfo.deviceType == DeviceType.Handheld
            };
        }

        protected override void RegisterScenes()
        {
            RegisterScenes(Constants.Scenes.GetAll());
        }

        protected override void InitializeAudioClips()
        {
            InitializeButtonEffects();
        }

        private void InitializeButtonEffects()
        {
            this.buttonAudioClips.Add(GameFrame.Base.Resources.Manager.Audio.Get("Buzz_1"));
            this.buttonAudioClips.Add(GameFrame.Base.Resources.Manager.Audio.Get("Buzz_2"));
        }

        protected override IEnumerator LoadDefintions()
        {
            var entityCache = new DefinitionCache<EntityDefinition>();
            var biomeCache = new DefinitionCache<BiomeDefinition>();

            yield return new DefinitionLoader<EntityDefinition>(entityCache).LoadDefinitions("Entities.json");
            yield return new BiomesLoader(biomeCache, entityCache).LoadDefinitions("Biomes.json");
            yield return new GameModeLoader(this.gameModeCache, biomeCache, entityCache).LoadDefinitions("GameModes.json");
            Debug.Log("loaded definitions");
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
