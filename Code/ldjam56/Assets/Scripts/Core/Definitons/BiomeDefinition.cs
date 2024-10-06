using System;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitons
{
    public class BiomeDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public Single Seed { get; set; }
        public Single MinHeight { get; set; }
        public Single MaxHeight { get; set; }
        public Boolean IsDefault { get; set; }
    }
}
