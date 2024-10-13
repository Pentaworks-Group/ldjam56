using Assets.Scripts.Constants;
using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Generation;
using Assets.Scripts.Model;

namespace Assets.Scripts.Core
{
    internal class GameStateConverter
    {
        private GameMode mode;

        public GameStateConverter(GameMode mode)
        {
            this.mode = mode;
        }

        public GameState Convert()
        {
            var gameState = new GameState()
            {
                GameMode = mode,
                CurrentScene = SceneNames.Game,
                Bee = ConvertBee()
            };

            var worldGenerator = new NewWorldGenerator(mode.World);

            gameState.World = worldGenerator.Generate();

            return gameState;
        }

        private Bee ConvertBee()
        {
            if (mode.Bee != default)
            {
                var beeDefinition = mode.Bee;

                var bee = new Bee()
                {
                    BoostConsumption = beeDefinition.BoostConsumption,
                    BoostStrength = beeDefinition.BoostStrength,
                    BoostRemaining = beeDefinition.BoostRemaining,
                    BoostBarMaximum = beeDefinition.BoostBarMaximum
                };

                return bee;
            }

            throw new System.Exception("Failed to load Bee!");
        }
    }
}