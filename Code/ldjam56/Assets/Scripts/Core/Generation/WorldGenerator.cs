using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Extensions;
using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core.Generation
{
    public abstract class WorldGenerator
    {
        protected GeneratorParameters parameters;
        protected List<Biome> biomes;

        protected void Initialize(GeneratorParameters parameters)
        {
            this.parameters = parameters;

            this.biomes = ConvertBiomes(parameters.Biomes);
        }

        protected void Stitch(Chunk chunk1, Chunk chunk2, Direction direction)
        {
            var edgeSide = direction.ToEdge();
            var opposingEdge = edgeSide.Opposing(direction);

            var chunk1Fields = chunk1.GetEdgeFields(edgeSide);
            var chunk2Fields = chunk2.GetEdgeFields(opposingEdge);

            foreach (var field in chunk1Fields)
            {
                var opposingField = default(Field);

                switch (direction)
                {
                    default:
                    case Direction.Left:
                        opposingField = chunk2Fields.FirstOrDefault(f => f.Position.X == parameters.EdgeIndex && f.Position.Z == field.Position.Z);
                        break;

                    case Direction.Top:
                        opposingField = chunk2Fields.FirstOrDefault(f => f.Position.X == field.Position.X && f.Position.Z == 0);
                        break;

                    case Direction.Right:
                        opposingField = chunk2Fields.FirstOrDefault(f => f.Position.X == 0 && f.Position.Z == field.Position.Z);

                        break;
                    case Direction.Bottom:
                        opposingField = chunk2Fields.FirstOrDefault(f => f.Position.X == field.Position.X && f.Position.Z == parameters.EdgeIndex);

                        break;
                }

                var newheight = (field.Position.Y + opposingField.Position.Y) / 2;

                field.Position = new GameFrame.Core.Math.Vector3(field.Position.X, newheight, field.Position.Z);
                opposingField.Position = new GameFrame.Core.Math.Vector3(opposingField.Position.X, newheight, opposingField.Position.Z);
            }
        }

        protected float fieldMinHeight = float.MaxValue;
        protected float fieldMaxHeight;

        protected virtual Chunk GenerateChunk(GameFrame.Core.Math.Vector2 position, Boolean isHomeChunk = false)
        {
            var chunk = new Chunk()
            {
                Position = position,
                IsHome = isHomeChunk,
            };

            chunk.Fields = GenerateFields(chunk);

            return chunk;
        }

        protected virtual List<Field> GenerateFields(Chunk chunk)
        {
            var fields = new List<Field>();

            var chunkOffsetX = (chunk.Position.X * parameters.ChunkSize);
            var chunkOffsetZ = (chunk.Position.Y * parameters.ChunkSize);

            for (int z = 0; z < this.parameters.ChunkSize; z++)
            {
                for (int x = 0; x < this.parameters.ChunkSize; x++)
                {
                    var worldPositionX = (chunkOffsetX + x);
                    var worldPositionZ = (chunkOffsetZ + z);

                    float xCoord = worldPositionX * parameters.TerrainScale;
                    float yCoord = worldPositionZ * parameters.TerrainScale;

                    float y = UnityEngine.Mathf.PerlinNoise(xCoord + parameters.TerrainSeed, yCoord + parameters.TerrainSeed) * parameters.TerrainScale;

                    var biome = GetBiome(worldPositionX, worldPositionZ, y);

                    var field = new Field()
                    {
                        Position = new GameFrame.Core.Math.Vector3(x, y, z),
                        Biome = biome
                    };

                    this.fieldMinHeight = Math.Min(this.fieldMinHeight, y);
                    this.fieldMaxHeight = Math.Max(this.fieldMaxHeight, y);

                    if (z == 0)
                    {
                        field.Edges = EdgeSide.Bottom;
                    }
                    else if (z == parameters.EdgeIndex)
                    {
                        field.Edges = EdgeSide.Top;
                    }

                    if (x == 0)
                    {
                        field.Edges |= EdgeSide.Left;
                    }
                    else if (x == parameters.EdgeIndex)
                    {
                        field.Edges |= EdgeSide.Right;
                    }

                    fields.Add(field);
                }
            }

            if (chunk.IsHome)
            {
                var homeField = fields.GetRandomEntry();

                homeField.IsHome = true;
            }

            return fields;
        }

        private Biome GetBiome(float worldPositionX, float worldPositionZ, Single fieldHeight)
        {
            var newValue = fieldHeight * 100f;

            var applicableBiomes = this.biomes.Where(b => fieldHeight >= b.MinHeight && fieldHeight <= b.MaxHeight).ToList();

            if (applicableBiomes.Count > 1)
            {
                var leadingBiome = default(Biome);
                var currentLeader = 0f;

                foreach (var biome in applicableBiomes)
                {
                    var xSeed = (worldPositionX + biome.Seed + 0.5f) / 5;
                    var ySeed = (worldPositionZ + biome.Seed + 0.5f) / 5;

                    var biomeSample = UnityEngine.Mathf.PerlinNoise(xSeed, ySeed);

                    if (biomeSample > currentLeader)
                    {
                        currentLeader = biomeSample;
                        leadingBiome = biome;
                    }
                }

                return leadingBiome;
            }
            else if (applicableBiomes.Count == 1)
            {
                return applicableBiomes[0];
            }
            else
            {
                return this.biomes.FirstOrDefault(b => b.IsDefault);
            }
        }

        private List<Biome> ConvertBiomes(List<BiomeDefinition> biomeDefinitions)
        {
            var biomes = new List<Biome>();

            if (biomeDefinitions?.Count > 0)
            {
                foreach (var biomeDefinition in biomeDefinitions)
                {
                    var biome = new Biome()
                    {
                        IsDefault = biomeDefinition.IsDefault.GetValueOrDefault(),
                        Name = biomeDefinition.Name,
                        MinHeight = biomeDefinition.MinHeight.GetValueOrDefault() * this.parameters.TerrainScale,
                        MaxHeight = biomeDefinition.MaxHeight.GetValueOrDefault() * this.parameters.TerrainScale,
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
    }
}
