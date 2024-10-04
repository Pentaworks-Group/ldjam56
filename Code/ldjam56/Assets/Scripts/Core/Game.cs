using GameFrame.Core;

using UnityEngine;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.Game<GameState, PlayerOptions>
    {
        protected override GameState InitializeGameState()
        {
            throw new System.NotImplementedException();
        }

        protected override PlayerOptions InitialzePlayerOptions()
        {
            throw new System.NotImplementedException();
        }
    }
}
