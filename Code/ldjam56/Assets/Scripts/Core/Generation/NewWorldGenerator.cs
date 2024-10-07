using System.Collections.Generic;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Model;

using Frame = GameFrame.Core.Math;

namespace Assets.Scripts.Core.Generation
{
    public class NewWorldGenerator : WorldGenerator
    {
        private readonly WorldDefinition worldDefinition;
        private World world;

        public NewWorldGenerator(WorldDefinition worldDefinition) : base(new GeneratorParameters(worldDefinition))
        {
            this.worldDefinition = worldDefinition;
        }

        public World Generate()
        {
            this.world = new World()
            {
                TerrainScale = parameters.TerrainScale,
                TerrainSeed = parameters.TerrainSeed,
                ChunkSize = parameters.ChunkSize,
            };

            this.world.Chunks = GenerateRootChunks();

            UnityEngine.Debug.LogFormat("Min: {0} Max: {1}", this.fieldMinHeight, this.fieldMaxHeight);

            return this.world;
        }

        private List<Chunk> GenerateRootChunks()
        {
            var bottomLeft = GenerateChunk(new Frame.Vector2(-1, -1));
            var bottomCenter = GenerateChunk(new Frame.Vector2(0, -1));
            var bottomRight = GenerateChunk(new Frame.Vector2(1, -1));
            var middleLeft = GenerateChunk(new Frame.Vector2(-1, 0));
            var middleCenter = GenerateChunk(new Frame.Vector2(0, 0), true);

            var middleRight = GenerateChunk(new Frame.Vector2(1, 0));
            var topLeft = GenerateChunk(new Frame.Vector2(-1, 1));
            var topCenter = GenerateChunk(new Frame.Vector2(0, 1));
            var topRight = GenerateChunk(new Frame.Vector2(1, 1));

            Stitch(bottomLeft, bottomCenter, Direction.Right);
            Stitch(bottomCenter, bottomRight, Direction.Right);

            Stitch(middleLeft, middleCenter, Direction.Right);
            Stitch(middleCenter, middleRight, Direction.Right);

            Stitch(topLeft, topCenter, Direction.Right);
            Stitch(topCenter, topRight, Direction.Right);

            Stitch(bottomLeft, middleLeft, Direction.Top);
            Stitch(middleLeft, topLeft, Direction.Top);

            Stitch(bottomCenter, middleCenter, Direction.Top);
            Stitch(middleCenter, topCenter, Direction.Top);

            Stitch(bottomRight, middleRight, Direction.Top);
            Stitch(middleRight, topRight, Direction.Top);

            var chunks = new List<Chunk>()
            {
                bottomLeft,
                bottomCenter,
                bottomRight,
                middleLeft,
                middleCenter,
                middleRight,
                topLeft,
                topCenter,
                topRight
            };

            return chunks;
        }
    }
}
