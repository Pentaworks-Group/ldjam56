using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

using GameFrame.Core.Extensions;
using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Generation
{
    public class NewWorldGenerator : WorldGenerator
    {
        private readonly WorldDefinition worldDefinition;

        public NewWorldGenerator(WorldDefinition worldDefinition)
        {
            this.worldDefinition = worldDefinition;

            Initialize((Int32)worldDefinition.SeedRange.GetRandom(),worldDefinition.ChunkSize, worldDefinition.Scale);
        }

        public World Generate()
        {
            this.world = new World()
            {
                Scale = this.scale,
                Seed = this.seed,
                ChunkSize = this.chunkSize,
            };

            this.world.Chunks = GenerateRootChunks();

            return this.world;
        }

        private List<Chunk> GenerateRootChunks()
        {
            var bottomLeft = GenerateChunk(new Vector2(-1, -1));
            var bottomCenter = GenerateChunk(new Vector2(0, -1));
            var bottomRight = GenerateChunk(new Vector2(1, -1));
            var middleLeft = GenerateChunk(new Vector2(-1, 0));
            var middleCenter = GenerateChunk(new Vector2(0, 0));
            var middleRight = GenerateChunk(new Vector2(1, 0));
            var topLeft = GenerateChunk(new Vector2(-1, 1));
            var topCenter = GenerateChunk(new Vector2(0, 1));
            var topRight = GenerateChunk(new Vector2(1, 1));

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
