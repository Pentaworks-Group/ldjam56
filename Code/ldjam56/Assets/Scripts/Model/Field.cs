﻿namespace Assets.Scripts.Model
{
    public class Field
    {
        public GameFrame.Core.Math.Vector3 Position { get; set; }
        public EdgeSide Edges { get; set; }
        public Biome Biome { get; set; }
    }
}
