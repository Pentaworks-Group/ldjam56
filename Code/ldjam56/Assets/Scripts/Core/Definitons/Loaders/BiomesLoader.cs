using System.Collections.Generic;

using GameFrame.Core.Definitions.Loaders;

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
                    var newGameMode = new BiomeDefinition()
                    {
                        Reference = loadedBiome.Reference,
                        Name = loadedBiome.Name,
                        IsDefault = loadedBiome.IsDefault,
                        SeedRange = loadedBiome.SeedRange,
                        HeightRange = loadedBiome.HeightRange,
                    };

                    CheckItems(loadedBiome.Entities, newGameMode.Entities, entityCache);

                    targetCache[loadedBiome.Reference] = newGameMode;
                }
            }
        }
    }
}
