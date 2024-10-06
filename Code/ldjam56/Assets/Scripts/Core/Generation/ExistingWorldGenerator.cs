using Assets.Scripts.Model;

namespace Assets.Scripts.Core.Generation
{
    public class ExistingWorldGenerator : WorldGenerator
    {
        private readonly World world;

        public ExistingWorldGenerator(World world)
        {
            this.world = world;

            Initialize(GeneratorParameters.FromWorld(world));
        }

        public void Expand(Chunk startingChunk, Direction direction)
        {

        }
    }
}
