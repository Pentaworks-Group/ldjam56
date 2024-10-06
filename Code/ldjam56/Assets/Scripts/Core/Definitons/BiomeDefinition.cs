using System;
using System.Collections.Generic;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitons
{
    public class BiomeDefinition : BaseDefinition
    {
        public String Name { get; set; }
        public GameFrame.Core.Math.Range SeedRange { get; set; }
        public GameFrame.Core.Math.Range HeightRange { get; set; }
        public Boolean IsDefault { get; set; }
        public List<EntityDefinition> Entities { get; set; } = new List<EntityDefinition>();
    }
}
