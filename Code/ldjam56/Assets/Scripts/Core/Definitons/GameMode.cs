using System.Collections.Generic;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitons
{
    public class GameMode : BaseDefinition
    {
        public WorldDefinition World { get; set; }
        public List<BiomeDefinition> Biomes { get; set; } = new List<BiomeDefinition>();
        public List<EntityDefinition> Entities { get; set; } = new List<EntityDefinition>();
    }
}
