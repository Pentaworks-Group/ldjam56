using System;
using System.Collections.Generic;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Model;

using GameFrame.Core.Extensions;


namespace Assets.Scripts.Core.Generation
{
    public class GeneratorParameters
    {
        public GeneratorParameters(Int32 chunkSize, float terrainSeed, float terrainScale, List<Biome> bioms)
        {
            this.TerrainSeed = terrainSeed;
            this.TerrainScale = terrainScale;
            this.ChunkSize = chunkSize;
            this.EdgeIndex = chunkSize - 1;
            this.Biomes = bioms;
        }

        public Int32 ChunkSize { get; }
        public Int32 EdgeIndex { get; }
        public float TerrainSeed { get; }
        public float TerrainScale { get; }
        public IList<Biome> Biomes { get; }

        public static GeneratorParameters FromWorldDefinition(WorldDefinition worldDefinition)
        {
            var terrainSeed = worldDefinition.TerrainSeedRange.GetRandom();

            var biomes = new List<Biome>()
            {
                CreateBiome("Grass", 0.1f, 0.9f, worldDefinition.TerrainScale,new List<Entity>() {
                    new Entity() { ModelReference = "Flower_1", Chance = 0.005f },
                    new Entity() { ModelReference = "Flower_2", Chance = 0.01f },
                    new Entity() { ModelReference = "Flower_3", Chance = 0.01f },
                    new Entity() { ModelReference = "Stone_Few", Chance = 0.0001f} }, true),
                CreateBiome("Desert", 0.5f, 0.7f, worldDefinition.TerrainScale,new List<Entity>() {
                    new Entity() { ModelReference = "Flower_1", Chance = 0.00005f},
                    new Entity() { ModelReference = "Stone_Few", Chance = 0.01f},
                    new Entity() { ModelReference = "Stone_Single", Chance = 0.0001f} }),
                CreateBiome("Forrest", 0.1f, 0.7f, worldDefinition.TerrainScale,new List<Entity>() {
                    new Entity() { ModelReference = "Flower_1", Chance = 0.025f },
                    new Entity() { ModelReference = "Flower_2", Chance = 0.005f },
                    new Entity() { ModelReference = "Flower_3", Chance = 0.0001f },
                    new Entity() { ModelReference = "Stone_Single", Chance = 0.001f} }),
                CreateBiome("Water", 0.0f, 0.1f, worldDefinition.TerrainScale,new List<Entity>() {
                    new Entity() { ModelReference = "Stone_Single", Chance = 0.01f} }),
                CreateBiome("Other", 0.9f, 1f, worldDefinition.TerrainScale,new List<Entity>() {
                    new Entity() { ModelReference = "Stone_Few", Chance = 0.0001f},
                    new Entity() { ModelReference = "Stone_Single", Chance = 0.01f}}),
            };
            //worldDefinition.Biomes.Convert();

            return new GeneratorParameters(worldDefinition.ChunkSize, terrainSeed, worldDefinition.TerrainScale, biomes);
        }

        public static GeneratorParameters FromWorld(World world)
        {
            return new GeneratorParameters(world.ChunkSize, world.TerrainSeed, world.TerrainScale, world.Biomes);
        }

        private static Biome CreateBiome(String name, Single minHeight, Single maxHeight, Single terrainScale, List<Entity> possibleEntites, Boolean isDefault = false)
        {
            return new Biome()
            {
                Name = name,
                Seed = UnityEngine.Random.Range(0, 123456),
                MinHeight = minHeight * terrainScale,
                MaxHeight = maxHeight * terrainScale,
                PossibleEntities = possibleEntites,
                IsDefault = isDefault
            };
        }
    }
}
