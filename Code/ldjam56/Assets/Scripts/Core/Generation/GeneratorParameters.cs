using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core.Generation
{
    public class GeneratorParameters
    {
        public GeneratorParameters(WorldDefinition worldDefinition)
        {
            var terrainSeed = worldDefinition.TerrainSeedRange.GetRandom();

            this.TerrainSeed = terrainSeed;
            this.TerrainScale = worldDefinition.TerrainScale.GetValueOrDefault();
            this.ChunkSize = worldDefinition.ChunkSize.GetValueOrDefault();
            this.EdgeIndex = this.ChunkSize - 1;
            this.Biomes = ConvertBiomes(worldDefinition.Biomes);
        }

        public GeneratorParameters(World world)
        {
            this.TerrainSeed = world.TerrainSeed;
            this.TerrainScale = world.TerrainScale;
            this.ChunkSize = world.ChunkSize;
            this.EdgeIndex = world.ChunkSize - 1;
            this.Biomes = world.Biomes;
        }

        public Int32 ChunkSize { get; }
        public Int32 EdgeIndex { get; }
        public float TerrainSeed { get; }
        public float TerrainScale { get; }
        public List<Biome> Biomes { get; }

        private List<Biome> ConvertBiomes(List<BiomeDefinition> biomeDefinitions)
        {
            var biomes = new List<Biome>();

            if (biomeDefinitions?.Count > 0)
            {
                foreach (var biomeDefinition in biomeDefinitions)
                {
                    var biome = new Biome()
                    {
                        Reference = biomeDefinition.Reference,
                        Name = biomeDefinition.Name,
                        IsDefault = biomeDefinition.IsDefault.GetValueOrDefault(),
                        TextureLayerName = biomeDefinition.TextureLayerName,
                        MinHeight = biomeDefinition.MinHeight.GetValueOrDefault() * this.TerrainScale,
                        MaxHeight = biomeDefinition.MaxHeight.GetValueOrDefault() * this.TerrainScale,
                        Seed = biomeDefinition.SeedRange.GetRandom(),
                        PossibleEntities = ConvertEntities(biomeDefinition.Entities),
                        PossibleHazards = ConvertEntities(biomeDefinition.Hazards),
                    };

                    biomes.Add(biome);
                }
            }

            return biomes;
        }

        private List<Entity> ConvertEntities(List<EntityDefinition> entityDefinitions)
        {
            var entities = new List<Entity>();

            if (entityDefinitions?.Count > 0)
            {
                foreach (var entityDefinition in entityDefinitions)
                {
                    var entity = new Entity()
                    {
                        ModelReference = entityDefinition.ModelReference,
                        Chance = entityDefinition.Chance.GetValueOrDefault()
                    };

                    entities.Add(entity);
                }
            }

            return entities;
        }

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
                    CreateBiome("Grass", "GrassTerrainLayer", 0.1f, 0.9f, GetRange(0, 123456), terrainScale, new List<EntityDefinition>()
                    {
                        CreateEntity("Flower1", "Flower_1",0.005f),
                        CreateEntity("Flower2", "Flower_2",0.01f),
                        CreateEntity("Flower3", "Flower_3",0.01f),
                        CreateEntity("StoneGroup", "Stone_Few",0.0001f)
                    }, true),
                    CreateBiome("Desert", "DesertTerrainLayer", 0.5f, 0.7f, GetRange(0, 123456), terrainScale, new List<EntityDefinition>()
                    {
                        CreateEntity("Flower1", "Flower_1", 0.005f),
                        CreateEntity("StoneGroup", "Stone_Few", 0.0001f),
                        CreateEntity("StoneSingle", "Stone_Single", 0.0001f),
                    }),
                    CreateBiome("Forrest", "ForestTerrainLayer", 0.5f, 0.7f, GetRange(0, 123456), terrainScale, new List<EntityDefinition>()
                    {
                        CreateEntity("Flower1", "Flower_1", 0.025f),
                        CreateEntity("Flower2", "Flower_2", 0.005f),
                        CreateEntity("Flower3", "Flower_3", 0.0001f),
                        CreateEntity("StoneSingle", "Stone_Single", 0.0001f),
                    }),
                    CreateBiome("Water", "WaterTerrainLayer", 0.0f, 0.1f, GetRange(0, 123456), terrainScale, new List<EntityDefinition>()
                    {
                        CreateEntity("StoneSingle", "Stone_Single", 0.01f),
                    }),
                    CreateBiome("Mountain", "OtherTerrainLayer", 0.9f, 1f, GetRange(0, 123456), terrainScale, new List<EntityDefinition>()
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

        private static BiomeDefinition CreateBiome(String name, String textureLayerName, Single minHeight, Single maxHeight, GameFrame.Core.Math.Range seedRange, Single terrainScale, List<EntityDefinition> possibleEntites, Boolean isDefault = false)
        {
            return new BiomeDefinition()
            {
                Reference = name,
                Name = name,
                TextureLayerName = textureLayerName,
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
