using System;

using Assets.Scripts.Model;

namespace Assets.Scripts.Core.Generation
{
    public class ExistingWorldGenerator : WorldGenerator
    {
        private readonly World world;
        private readonly Map<Single, Chunk> chunkMap = new Map<float, Chunk>();

        public ExistingWorldGenerator(World world) : base(new GeneratorParameters(world))
        {
            this.world = world;

            foreach (var chunk in world.Chunks)
            {
                chunkMap[chunk.Position.X, chunk.Position.Y] = chunk;
            }
        }

        public Chunk Expand(Chunk startingChunk, Direction direction)
        {
            var position = startingChunk.Position;

            switch (direction)
            {
                case Direction.Left: position.X--; break;
                case Direction.Top: position.Y++; break;
                case Direction.Right: position.X++; break;
                case Direction.Bottom: position.Y--; break;
            }

            if (!chunkMap.TryGetValue(position.X, position.Y, out var newChunk))
            {
                newChunk = GenerateChunk(position);
                chunkMap[position.X, position.Y] = newChunk;

                Stitch(startingChunk, newChunk, direction);

                StitchAll(newChunk);

                world.Chunks.Add(newChunk);
            }

            return newChunk;
        }

        protected void StitchAll(Chunk chunk)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                TryStitch(chunk, direction);
            }
        }

        protected void TryStitch(Chunk chunk, Direction direction)
        {
            var x = chunk.Position.X;
            var z = chunk.Position.Y;

            switch (direction)
            {
                case Direction.Left: x--; break;
                case Direction.Top: z++; break;
                case Direction.Right: x++; break;
                case Direction.Bottom: z--; break;
            }

            if (chunkMap.TryGetValue(x, z, out var neightbour))
            {
                Stitch(chunk, neightbour, direction);
            }
        }
    }
}
