using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitons
{
    public class WorldDefinition : BaseDefinition
    {
        public BeeDefinition Bee { get; set; }
        public float? TerrainScale { get; set; }
        public Int32? ChunkSize { get; set; }
        public GameFrame.Core.Math.Range BiomeSeedRange { get; set; }
        public GameFrame.Core.Math.Range TerrainSeedRange { get; set; }
        public List<BiomeDefinition> Biomes { get; set; }
        public List<EntityDefinition> Entities { get; set; }
    }
}
