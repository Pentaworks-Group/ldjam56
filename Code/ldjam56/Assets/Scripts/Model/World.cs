using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class World
    {
        public Single BiomeScale { get; set; }
        public Single TerrainSeed { get; set; }
        public float TerrainScale { get; set; }
        public Int32 ChunkSize { get; set; }
        public List<Chunk> Chunks { get; set; }
        public List<Biome> Biomes { get; set; }
    }
}
