using System;
using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class Biome
    {
        public String Reference { get; set; }
        public String Name { get; set; }
        public Single Seed { get; set; }
        public Single MinHeight { get; set; }
        public Single MaxHeight { get; set; }
        public Boolean IsDefault { get; set; }
        public String TextureLayerName { get; set; }

        public List<Entity> PossibleEntities { get; set; }
        public List<Entity> PossibleHazards { get; set; }
    }
}
