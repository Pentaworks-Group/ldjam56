using Assets.Scripts.Model;

namespace Assets.Scripts.Core.Generation
{
    public class ExistingWorldGenerator : WorldGenerator
    {
        private readonly World world;

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
    }
}
