using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Extensions;
using Assets.Scripts.Model;

using GameFrame.Core.Extensions;
using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Generation
{
    internal class WorldGenerator
    {
        private const float WorldHeight = 600;

        private readonly Int32 chunkSize;
        private readonly Int32 edgeIndex;
        private readonly Int32 seed;

        public WorldGenerator(WorldDefinition worldDefinition)
        {
            this.chunkSize = worldDefinition.ChunkSize;
            this.edgeIndex = chunkSize - 1;

            //this.seed = (Int32)worldDefinition.SeedRange.GetRandom();
            this.seed = 1;
        }

        public World Generate()
        {
            var world = new World()
            {
                Seed = seed,
                ChunkSize = chunkSize,
                Chunks = GenerateRootChunks()
            };

            return world;
        }

        private List<Chunk> GenerateRootChunks()
        {
            var bottomLeft = GenerateChunk(new Vector2(0, 0));
            var bottomCenter = GenerateChunk(new Vector2(1, 0));
            var bottomRight = GenerateChunk(new Vector2(2, 0));
            var middleLeft = GenerateChunk(new Vector2(0, 1));
            var middleCenter = GenerateChunk(new Vector2(1, 1));
            var middleRight = GenerateChunk(new Vector2(2, 1));
            var topLeft = GenerateChunk(new Vector2(0, 2));
            var topCenter = GenerateChunk(new Vector2(1, 2));
            var topRight = GenerateChunk(new Vector2(2, 2));

            Stitch(bottomLeft, bottomCenter, Direction.Right);
            Stitch(bottomCenter, bottomRight, Direction.Right);

            Stitch(middleLeft, middleCenter, Direction.Right);
            Stitch(middleCenter, middleRight, Direction.Right);

            Stitch(topLeft, topCenter, Direction.Right);
            Stitch(topCenter, topRight, Direction.Right);

            Stitch(bottomLeft, middleLeft, Direction.Top);
            Stitch(middleLeft, topLeft, Direction.Top);

            Stitch(bottomCenter, middleCenter, Direction.Top);
            Stitch(middleCenter, topCenter, Direction.Top);

            Stitch(bottomRight, middleRight, Direction.Top);
            Stitch(middleRight, topRight, Direction.Top);

            var chunks = new List<Chunk>()
            {
                bottomLeft,
                bottomCenter,
                bottomRight,
                middleLeft,
                middleCenter,
                middleRight,
                topLeft,
                topCenter,
                topRight
            };

            return chunks;
        }

        private void Stitch(Chunk chunk1, Chunk chunk2, Direction direction)
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
                        opposingField = chunk2Fields.FirstOrDefault(f => f.Position.X == edgeIndex && f.Position.Z == field.Position.Z);
                        break;

                    case Direction.Top:
                        opposingField = chunk2Fields.FirstOrDefault(f => f.Position.X == field.Position.X && f.Position.Z == 0);
                        break;

                    case Direction.Right:
                        opposingField = chunk2Fields.FirstOrDefault(f => f.Position.X == 0 && f.Position.Z == field.Position.Z);

                        break;
                    case Direction.Bottom:
                        opposingField = chunk2Fields.FirstOrDefault(f => f.Position.X == field.Position.X && f.Position.Z == edgeIndex);

                        break;
                }

                var newheight = (field.Position.Y + opposingField.Position.Y) / 2;

                field.Position = new GameFrame.Core.Math.Vector3(field.Position.X, newheight, field.Position.Z);
                opposingField.Position = new GameFrame.Core.Math.Vector3(opposingField.Position.X, newheight, opposingField.Position.Z);
            }
        }

        private Chunk GenerateChunk(Vector2 position)
        {
            var chunk = new Chunk()
            {
                Position = position,
                Fields = GenerateFields(position),
                Entities = GenerateEntities()
            };

            return chunk;
        }

        private List<Field> GenerateFields(Vector2 chunkPosition)
        {
            var fields = new List<Field>();

            float scale = 0.15f;

            var chunkOffsetX = (chunkPosition.X * chunkSize);
            var chunkOffsetZ = (chunkPosition.Y * chunkSize);

            for (int z = 0; z < this.chunkSize; z++)
            {
                for (int x = 0; x < this.chunkSize; x++)
                {
                    float xCoord = (chunkOffsetX + x) * scale;
                    float yCoord = (chunkOffsetZ + z) * scale;

                    float y = UnityEngine.Mathf.PerlinNoise(xCoord + seed, yCoord + seed) * scale;

                    var field = new Field()
                    {
                        Position = new GameFrame.Core.Math.Vector3(x, y, z)
                    };

                    if (z == 0)
                    {
                        field.Edges = EdgeSide.Bottom;
                    }
                    else if (z == edgeIndex)
                    {
                        field.Edges = EdgeSide.Top;
                    }

                    if (x == 0)
                    {
                        field.Edges |= EdgeSide.Left;
                    }
                    else if (x == edgeIndex)
                    {
                        field.Edges |= EdgeSide.Right;
                    }

                    fields.Add(field);
                }
            }

            return fields;
        }

        private float GetRandomHeight()
        {
            var random = UnityEngine.Random.Range(0, 1000);

            if (random > 950)
            {
                return UnityEngine.Random.Range(0, 128);
            }

            return 0;
        }

        private List<Entity> GenerateEntities()
        {
            var entities = new List<Entity>();

            // empty for now

            return entities;
        }

        private float GetNormalized(float fieldHeight)
        {
            if (fieldHeight > 0)
            {
                return fieldHeight / WorldHeight;
            }

            return default;
        }

        private Vector2 CalculatePosition(Vector2 currentPosition, Direction direction)
        {
            switch (direction)
            {
                case Direction.Left: return new Vector2(currentPosition.X - 1, currentPosition.Y);
                case Direction.Top: return new Vector2(currentPosition.X, currentPosition.Y + 1);
                case Direction.Right: return new Vector2(currentPosition.X + 1, currentPosition.Y);
                case Direction.Bottom: return new Vector2(currentPosition.X + 1, currentPosition.Y);
                default:
                    throw new NotSupportedException("wut?");
            }
        }
    }
}
