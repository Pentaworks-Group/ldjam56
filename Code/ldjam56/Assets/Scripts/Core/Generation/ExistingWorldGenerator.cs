using Assets.Scripts.Model;

namespace Assets.Scripts.Core.Generation
{
    public class ExistingWorldGenerator : WorldGenerator
    {
        public ExistingWorldGenerator(World world)
        {
            this.world = world;

            Initialize(world.Seed, world.ChunkSize, world.Scale);
        }

        public void Expand(Chunk startingChunk, Direction direction)
        {

        }
    }
}
