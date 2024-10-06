using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Extensions;
using Assets.Scripts.Model;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Generation
{
    public abstract class WorldGenerator
    {
        protected GeneratorParameters parameters;

        protected void Initialize(GeneratorParameters parameters)
        {
            this.parameters = parameters;
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

        protected virtual Chunk GenerateChunk(Vector2 position)
        {
            var chunk = new Chunk()
            {
                Position = position,
                Entities = GenerateEntities()
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
                    //var worldPositionX = (chunkOffsetX + x);
                    //var worldPositionZ = (chunkOffsetZ + z);

                    //float xCoord = worldPositionX * parameters.TerrainScale;
                    //float yCoord = worldPositionZ * parameters.TerrainScale;

                    //float y = UnityEngine.Mathf.PerlinNoise(xCoord + parameters.TerrainSeed, yCoord + parameters.TerrainSeed) * parameters.TerrainScale;

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

            return fields;
        }

        private Biome GetBiome(float worldPositionX, float worldPositionZ, Single fieldHeight)
        {
            float biomeOffsetX = worldPositionX;
            float biomeOffsetZ = worldPositionZ;

            var newValue = fieldHeight * 100f;

            var applicableBiomes = parameters.Biomes.Where(b => fieldHeight >= b.MinHeight && fieldHeight <= b.MaxHeight).ToList();

            if (applicableBiomes.Count > 1)
            {
                var leadingBiome = default(Biome);
                var currentLeader = 0f;

                foreach (var biome in applicableBiomes)
                {
                    var test1 = (worldPositionX + biome.Seed + 0.5f) / 5;
                    var test2 = (biomeOffsetZ + biome.Seed + 0.5f) / 5;

                    var biomeSample = UnityEngine.Mathf.PerlinNoise(test1, test2);

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

            throw new NotSupportedException("Wut");
        }

        private List<Entity> GenerateEntities()
        {
            var entities = new List<Entity>();

            // empty for now

            return entities;
        }
    }
}
