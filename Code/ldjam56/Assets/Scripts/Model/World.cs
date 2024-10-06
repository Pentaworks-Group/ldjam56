using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class World
    {
        public Int32 Seed { get; set; }
        public float Scale { get; set; }
        public Int32 ChunkSize { get; set; }
        public List<Chunk> Chunks { get; set; }
    }
}
