using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitons;

namespace Assets.Scripts.Core.Generation
{
    public class GeneratorParameters
    {
        public GeneratorParameters(Int32 chunkSize, float terrainSeed, float terrainScale, List<BiomeDefinition> biomes)
        {
            this.TerrainSeed = terrainSeed;
            this.TerrainScale = terrainScale;
            this.ChunkSize = chunkSize;
            this.EdgeIndex = chunkSize - 1;
            this.Biomes = biomes;
        }

        public Int32 ChunkSize { get; }
        public Int32 EdgeIndex { get; }
        public float TerrainSeed { get; }
        public float TerrainScale { get; }
        public List<BiomeDefinition> Biomes { get; }

        internal static WorldDefinition CreateTest()
        {
            var terrainScale = 0.085f;

            return new WorldDefinition()
            {
                ChunkSize = 32,
                BiomeSeedRange = new GameFrame.Core.Math.Range(0, 1),
                TerrainSeedRange = new GameFrame.Core.Math.Range(0, 1),
                TerrainScale = terrainScale,
                Biomes = new List<BiomeDefinition>()
                {
                    CreateBiome("Grass", 0.1f, 0.9f, GetRange(0, 123456), terrainScale, new List<EntityDefinition>()
                    {
                        CreateEntity("Flower1", "Flower_1",0.005f),
                        CreateEntity("Flower2", "Flower_2",0.01f),
                        CreateEntity("Flower3", "Flower_3",0.01f),
                        CreateEntity("StoneGroup", "Stone_Few",0.0001f)
                    }, true),
                    CreateBiome("Desert", 0.5f, 0.7f, GetRange(0, 123456), terrainScale, new List<EntityDefinition>()
                    {
                        CreateEntity("Flower1", "Flower_1", 0.005f),
                        CreateEntity("StoneGroup", "Stone_Few", 0.0001f),
                        CreateEntity("StoneSingle", "Stone_Single", 0.0001f),
                    }),
                    CreateBiome("Forrest", 0.5f, 0.7f, GetRange(0, 123456), terrainScale, new List<EntityDefinition>()
                    {
                        CreateEntity("Flower1", "Flower_1", 0.025f),
                        CreateEntity("Flower2", "Flower_2", 0.005f),
                        CreateEntity("Flower3", "Flower_3", 0.0001f),
                        CreateEntity("StoneSingle", "Stone_Single", 0.0001f),
                    }),
                    CreateBiome("Water", 0.0f, 0.1f, GetRange(0, 123456), terrainScale, new List<EntityDefinition>()
                    {
                        CreateEntity("StoneSingle", "Stone_Single", 0.01f),
                    }),
                    CreateBiome("Mountain", 0.9f, 1f, GetRange(0, 123456), terrainScale, new List<EntityDefinition>()
                    {
                        CreateEntity("StoneGroup", "Stone_Few", 0.0001f),
                        CreateEntity("StoneSingle", "Stone_Single", 0.01f),
                    }),
                }
            };
        }

        private static GameFrame.Core.Math.Range GetRange(Single min, Single max)
        {
            return new GameFrame.Core.Math.Range(min, max);
        }

        private static BiomeDefinition CreateBiome(String name, Single minHeight, Single maxHeight, GameFrame.Core.Math.Range seedRange, Single terrainScale, List<EntityDefinition> possibleEntites, Boolean isDefault = false)
        {
            return new BiomeDefinition()
            {
                Reference = name,
                Name = name,
                SeedRange = seedRange,
                MinHeight = minHeight * terrainScale,
                MaxHeight = maxHeight * terrainScale,
                Entities = possibleEntites,
                IsDefault = isDefault
            };
        }

        private static EntityDefinition CreateEntity(String reference, String modelReference, float chance)
        {
            return new EntityDefinition()
            {
                Reference = reference,
                Chance = chance,
                ModelReference = modelReference
            };
        }
    }
}
