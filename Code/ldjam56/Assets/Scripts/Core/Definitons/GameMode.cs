using System;
using System.Collections.Generic;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitons
{
    public class GameMode : BaseDefinition
    {
        public String Name { get; set; }
        public WorldDefinition World { get; set; }

        public List<BiomeDefinition> Biomes { get; set; }
        public List<EntityDefinition> Entities { get; set; }
    }
}
