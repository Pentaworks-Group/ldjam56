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
            Debug.LogFormat("GameMode loading completed");

            if (definitions?.Count > 0)
            {
                foreach (var loadedGameMode in definitions)
                {
                    var newGameMode = new GameMode()
                    {
                        Reference = loadedGameMode.Reference,
                        Name = loadedGameMode.Name,
                        IsReferenced = loadedGameMode.IsReferenced,
                        //TestFlag = loadedGameMode.TestFlag,
                        IsLoadingRequired = loadedGameMode.IsLoadingRequired,
                        //    Biomes = new List<BiomeDefinition>(),
                        //    Entities = new List<EntityDefinition>()
                    };

                    Debug.LogFormat("GameMode: {0} => {1}", loadedGameMode.Name, loadedGameMode.TestFlag);

                    if (loadedGameMode.Biomes?.Count > 0)
                    {
                        Debug.LogFormat("Biomes loaded: {0}", loadedGameMode.Biomes.Count);

                        foreach (var biome in loadedGameMode.Biomes)
                        {
                            Debug.LogFormat("Biome: {0} => {1} / {2} / {3}", biome.Reference, biome.IsReferenced, biome.TestFlag, biome.IsLoadingRequired);
                        }
                    }
                    else
                    {
                        Debug.LogFormat("Biomes not loaded correctly");
                    }

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

                        //Dirty work-around
                        CheckItems(loadedGameMode.Biomes, newGameMode.World.Biomes, this.biomeCache);
                        CheckItems(loadedGameMode.Entities, newGameMode.World.Entities, this.entityCache);

                        //CheckItems(loadedGameMode.World.Biomes, newGameMode.World.Biomes, this.biomeCache);
                        //CheckItems(loadedGameMode.World.Entities, newGameMode.World.Entities, this.entityCache);
                    }

                    targetCache[loadedGameMode.Reference] = newGameMode;
                }
            }
        }
    }
}
