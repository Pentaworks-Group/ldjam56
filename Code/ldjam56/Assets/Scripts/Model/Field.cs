using System;

namespace Assets.Scripts.Model
{
    public class Field
    {
        public GameFrame.Core.Math.Vector3 Position { get; set; }
        public Biome Biome { get; set; }
        public EdgeSide Edges { get; set; }
        public Boolean IsHome { get; set; }
    }
}
