using System.Collections;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        protected override GameState InitializeGameState()
        {
            return new GameState();
        }

        protected override PlayerOptions InitialzePlayerOptions()
        {
            return new PlayerOptions();
        }

        protected override void InitializeAudioClips()
        { }

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
