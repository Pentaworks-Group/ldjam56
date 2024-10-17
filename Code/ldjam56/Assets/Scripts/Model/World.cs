using System;
using System.Collections.Generic;

using GameFrame.Core.Math;

namespace Assets.Scripts.Model
{
    public class World
    {
        public Single TerrainSeed { get; set; }
        public float TerrainScale { get; set; }
        public Int32 ChunkSize { get; set; }
        public List<Chunk> Chunks { get; set; }
        public List<Biome> Biomes { get; set; }
        public Vector3 SunAngles { get; set; }
        public Vector3 MoonAngles { get; set; }
    }
}
