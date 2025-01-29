using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Extensions;
using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Core.Generation
{
    public abstract class WorldGenerator
    {
        protected readonly Map<Single, Chunk> chunkMap = new Map<float, Chunk>();

        protected GeneratorParameters parameters;

        protected WorldGenerator(GeneratorParameters parameters)
        {
            this.parameters = parameters;
        }

        protected virtual Chunk GetNeighbour(Chunk chunk, Direction direction)
        {
            return GetNeighbour(chunk.Position, direction);
        }

        protected virtual Chunk GetNeighbour(GameFrame.Core.Math.Vector2 chunkPosition, Direction direction)
        {
            var x = chunkPosition.X;
            var z = chunkPosition.Y;

            switch (direction)
            {
                case Direction.Left: x--; break;
                case Direction.Top: z++; break;
                case Direction.Right: x++; break;
                case Direction.Bottom: z--; break;
            }

            if (chunkMap.TryGetValue(x, z, out var neightbour))
            {
                return neightbour;
            }

            return default;
        }

        protected void StitchAll(Chunk chunk)
        {
            foreach (Direction possibleDirection in System.Enum.GetValues(typeof(Direction)))
            {
                TryStitch(chunk, possibleDirection);
            }
        }

        protected virtual Chunk GenerateChunk(GameFrame.Core.Math.Vector2 position, Boolean isHomeChunk = false)
        {
            var chunk = new Chunk()
            {
                Position = position,
                IsHome = isHomeChunk,
            };

            chunkMap[position.X, position.Y] = chunk;

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
            var applicableBiomes = parameters.Biomes.Where(b => fieldHeight >= b.MinHeight && fieldHeight <= b.MaxHeight).ToList();

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

                // UnityEngine.Debug.LogFormat("{0} => {1}", fieldHeight, leadingBiome.Name);

                return leadingBiome;
            }
            else if (applicableBiomes.Count == 1)
            {
                var biome = applicableBiomes[0];

                // UnityEngine.Debug.LogFormat("{0} => {1}", fieldHeight, biome.Name);

                return biome;
            }
            else
            {
                var biome = this.parameters.Biomes.FirstOrDefault(b => b.IsDefault);

                // UnityEngine.Debug.LogFormat("{0} => {1} (Default)", fieldHeight, biome.Name);

                return biome;
            }
        }

        protected void TryStitch(Chunk chunk, Direction direction)
        {
            var neighbour = GetNeighbour(chunk.Position, direction);

            if (neighbour != default)
            {
                Stitch(chunk, neighbour, direction);
            }
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

                var fieldCount = 2;

                var newheight = (field.Position.Y + opposingField.Position.Y) / fieldCount;

                var leftOvers = field.Edges & ~edgeSide;

                if (leftOvers != EdgeSide.None)
                {
                    var targetDirection = leftOvers.ToDirection();

                    var neighbourChunk1 = GetNeighbour(chunk1, targetDirection);
                    var neighbourChunk2 = GetNeighbour(chunk2, targetDirection);

                    var neighbour1Field = default(Field);
                    var neighbour2Field = default(Field);

                    if (neighbourChunk1 != default)
                    {
                        var opposingFieldEdge = field.Edges.Opposing(targetDirection);

                        neighbour1Field = neighbourChunk1.GetEdgeField(opposingFieldEdge);

                        fieldCount++;

                        newheight += (neighbour1Field.Position.Y - newheight) / (fieldCount + 1);

                        neighbourChunk1.IsUpdateRequired = true;
                    }

                    if (neighbourChunk2 != default)
                    {
                        var opposingFieldEdge2 = opposingField.Edges.Opposing(targetDirection);

                        neighbour2Field = neighbourChunk2.GetEdgeField(opposingFieldEdge2);

                        fieldCount++;
                        newheight += (neighbour2Field.Position.Y - newheight) / (fieldCount + 1);

                        neighbourChunk2.IsUpdateRequired = true;
                    }

                    if (neighbour1Field != default)
                    {
                        neighbour1Field.Position = new GameFrame.Core.Math.Vector3(neighbour1Field.Position.X, newheight, neighbour1Field.Position.Z);
                    }

                    if (neighbour2Field != default)
                    {
                        neighbour2Field.Position = new GameFrame.Core.Math.Vector3(neighbour2Field.Position.X, newheight, neighbour2Field.Position.Z);
                    }
                }

                field.Position = new GameFrame.Core.Math.Vector3(field.Position.X, newheight, field.Position.Z);
                opposingField.Position = new GameFrame.Core.Math.Vector3(opposingField.Position.X, newheight, opposingField.Position.Z);
            }

            chunk1.IsUpdateRequired = true;
            chunk2.IsUpdateRequired = true;
        }
    }
}
