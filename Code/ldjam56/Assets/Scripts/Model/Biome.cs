using System;

using GameFrame.Core.Media;

namespace Assets.Scripts.Model
{
    public class Biome
    {
        public String Name { get; set; }
        public Color Color { get; set; }
        public Single Seed { get; set; }
        public Single MinHeight { get; set; }
        public Single MaxHeight { get; set; }
    }
}
