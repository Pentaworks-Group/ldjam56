using Assets.Scripts.Model;

namespace Assets.Scripts.Core.Generation
{
    public class ExistingWorldGenerator : WorldGenerator
    {
        private readonly World world;

        public ExistingWorldGenerator(World world) : base(new GeneratorParameters(world))
        {
            this.world = world;
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

            var newChunk = GenerateChunk(position);
                        
            Stitch(startingChunk, newChunk, direction);

            world.Chunks.Add(newChunk);

            return newChunk;
        }
    }
}
