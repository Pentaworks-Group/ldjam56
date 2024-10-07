using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class Biome
    {
        public String Name { get; set; }
        public Single Seed { get; set; }
        public Single MinHeight { get; set; }
        public Single MaxHeight { get; set; }
        public Boolean IsDefault { get; set; }

        public List<Entity> PossibleEntities { get; set; }
        public List<Entity> PossibleHazards { get; set; }
    }
}
