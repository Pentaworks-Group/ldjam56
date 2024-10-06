using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core.Generation
{
    public class GeneratorParameters
    {
        public GeneratorParameters(Int32 chunkSize, float terrainSeed, float terrainScale, Single biomeScale, List<Biome> bioms)
        {
            this.TerrainSeed = terrainSeed;
            this.TerrainScale = terrainScale;
            this.ChunkSize = chunkSize;
            this.EdgeIndex = chunkSize - 1;
            this.BiomeScale = biomeScale;
            this.Biomes = bioms;
        }

        public Int32 ChunkSize { get; }
        public Int32 EdgeIndex { get; }
        public float TerrainSeed { get; }
        public float TerrainScale { get; }
        public IList<Biome> Biomes { get; }
        public Single BiomeScale { get; }

        public static GeneratorParameters FromWorldDefinition(WorldDefinition worldDefinition)
        {
            var terrainSeed = worldDefinition.TerrainSeedRange.GetRandom();
            var biomeScale = 1f;

            var biomes = new List<Biome>()
            {
                CreateBiome("Grass", 0.1f, 0.9f, worldDefinition.TerrainScale, true),
                CreateBiome("Desert", 0.5f, 0.7f, worldDefinition.TerrainScale),
                CreateBiome("Forrest", 0.1f, 0.7f, worldDefinition.TerrainScale),
                CreateBiome("Water", 0.0f, 0.1f, worldDefinition.TerrainScale),
                CreateBiome("Other", 0.9f, 1f, worldDefinition.TerrainScale),
            };
            //worldDefinition.Biomes.Convert();

            return new GeneratorParameters(worldDefinition.ChunkSize, terrainSeed, worldDefinition.TerrainScale, biomeScale, biomes);
        }

        public static GeneratorParameters FromWorld(World world)
        {
            return new GeneratorParameters(world.ChunkSize, world.TerrainSeed, world.TerrainScale, world.BiomeScale, world.Biomes);
        }

        private static Biome CreateBiome(String name, Single minHeight, Single maxHeight, Single terrainScale, Boolean isDefault = false)
        {
            return new Biome()
            {
                Name = name,
                Seed = UnityEngine.Random.Range(0, 123456),
                MinHeight = minHeight * terrainScale,
                MaxHeight = maxHeight * terrainScale,
                IsDefault = isDefault
            };
        }
    }
}
