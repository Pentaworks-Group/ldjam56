using System.Collections.Generic;

using Assets.Scripts.Model;

using GameFrame.Core.Definitions.Loaders;

using UnityEngine;

namespace Assets.Scripts.Core.Definitons.Loaders
{
    public class BiomesLoader : BaseLoader<BiomeDefinition>
    {
        private readonly DefinitionCache<EntityDefinition> entityCache;

        public BiomesLoader(DefinitionCache<BiomeDefinition> targetCache, DefinitionCache<EntityDefinition> entityCache) : base(targetCache)
        {
            this.entityCache = entityCache;
        }

        protected override void OnDefinitionsLoaded(List<BiomeDefinition> definitions)
        {
            if (definitions?.Count > 0)
            {
                foreach (var loadedBiome in definitions)
                {
                    var newBiomeDefinition = new BiomeDefinition()
                    {
                        Reference = loadedBiome.Reference,
                        Name = loadedBiome.Name,
                        IsDefault = loadedBiome.IsDefault,
                        SeedRange = loadedBiome.SeedRange,
                        MinHeight = loadedBiome.MinHeight,
                        MaxHeight = loadedBiome.MaxHeight,
                        Entities = new List<EntityDefinition>(),
                        Hazards = new List<EntityDefinition>()
                    };

                    CheckItems(loadedBiome.Entities, newBiomeDefinition.Entities, entityCache);
                    CheckItems(loadedBiome.Hazards, newBiomeDefinition.Hazards, entityCache);
                    Debug.Log("loadedBiome: ref: " + loadedBiome.Name + " " + loadedBiome.Hazards.Count + " " + loadedBiome.Entities.Count + " " + loadedBiome.SeedRange);

                    targetCache[loadedBiome.Reference] = newBiomeDefinition;
                }
            }
        }
    }
}
