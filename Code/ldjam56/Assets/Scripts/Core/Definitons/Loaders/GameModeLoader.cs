using System.Collections.Generic;

using GameFrame.Core.Definitions.Loaders;

using UnityEngine;

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
                foreach (var loadedGameMode in definitions)
                {
                    var newGameMode = new GameMode()
                    {
                        Reference = loadedGameMode.Reference,
                    };

                    if (loadedGameMode.World != default)
                    {
                        newGameMode.World = new WorldDefinition()
                        {
                            Reference = loadedGameMode.World.Reference,
                            ChunkSize = loadedGameMode.World.ChunkSize,
                            TerrainScale = loadedGameMode.World.TerrainScale,
                            BiomeSeedRange = loadedGameMode.World.BiomeSeedRange,
                            TerrainSeedRange = loadedGameMode.World.TerrainSeedRange,
                            Biomes = new List<BiomeDefinition>(),
                            Entities = new List<EntityDefinition>()
                        };

                        CheckItems(loadedGameMode.World.Biomes, newGameMode.World.Biomes, this.biomeCache);
                        CheckItems(loadedGameMode.World.Entities, newGameMode.World.Entities, this.entityCache);

                        Debug.Log(newGameMode.Reference + " LoadedBiomes: " + loadedGameMode.World.Biomes?.Count + " cache: " + biomeCache.Count + " checked: " + newGameMode.World.Biomes?.Count);
                        
                        Debug.Log("cache");
                        foreach (var biome in biomeCache.Values)
                        {
                            Debug.Log(biome.Reference + " " + biome.Name + " " + biome.Hazards?.Count + " " + biome.Entities?.Count + " " + biome.SeedRange);
                        }

                        Debug.Log("loaded");
                        foreach (var biome in loadedGameMode.World.Biomes)
                        {
                            Debug.Log(biome.Reference + " " + biome.Name + " " + biome.Hazards?.Count + " " + biome.Entities?.Count + " " + biome.SeedRange);
                        }

                        Debug.Log("New");
                        foreach (var biome in newGameMode.World.Biomes)
                        {
                            Debug.Log(biome.Reference + " " + biome.Name + " " + biome.Hazards?.Count + " " + biome.Entities?.Count + " " + biome.SeedRange);
                        }
                    }

                    targetCache[loadedGameMode.Reference] = newGameMode;
                }
            }
        }
    }
}
