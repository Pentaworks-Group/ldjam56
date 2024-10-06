using System.Collections.Generic;

using GameFrame.Core.Definitions.Loaders;

namespace Assets.Scripts.Core.Definitons.Loaders
{
    public class GameModeLoader : BaseLoader<GameMode>
    {
        private readonly DefinitionCache<BiomeDefinition> biomeCache;
        private readonly DefinitionCache<EntityDefinition> entityCache;

        public GameModeLoader(DefinitionCache<GameMode> targetCache, DefinitionCache<BiomeDefinition> biomeCache, DefinitionCache<EntityDefinition> entityCache) : base(targetCache)
        {
            this.biomeCache = biomeCache;
            this.entityCache = entityCache;
        }

        protected override void OnDefinitionsLoaded(List<GameMode> definitions)
        {
            if (definitions?.Count > 0)
            {
                foreach (var loadedBiome in definitions)
                {
                    var newGameMode = new GameMode()
                    {
                        Reference = loadedBiome.Reference,
                    };

                    CheckItems(loadedBiome.Biomes, newGameMode.Biomes, this.biomeCache);
                    CheckItems(loadedBiome.Entities, newGameMode.Entities, this.entityCache);

                    targetCache[loadedBiome.Reference] = newGameMode;
                }
            }
        }
    }
}
