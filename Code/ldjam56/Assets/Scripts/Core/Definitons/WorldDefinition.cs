using System;
using System.Collections.Generic;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitons
{
    public class WorldDefinition : BaseDefinition
    {
        public float TerrainScale { get; set; }
        public Int32 ChunkSize { get; set; }
        public GameFrame.Core.Math.Range BiomeSeedRange { get; set; }
        public GameFrame.Core.Math.Range TerrainSeedRange { get; set; }
        public List<BiomeDefinition> Biomes { get; set; } = new List<BiomeDefinition>();
        public List<EntityDefinition> Entities { get; set; } = new List<EntityDefinition>();
    }
}
