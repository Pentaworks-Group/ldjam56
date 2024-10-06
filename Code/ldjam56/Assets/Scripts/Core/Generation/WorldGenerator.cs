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
        protected Int32 seed;
        protected float scale;
        protected Int32 chunkSize;
        protected Int32 edgeIndex;

        protected World world;

        protected void Initialize(Int32 seed, Int32 chunkSize, float scale)
        {
            this.seed = seed;
            this.scale = scale;
            this.chunkSize = chunkSize;
            this.edgeIndex = chunkSize - 1;
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

        protected virtual Chunk GenerateChunk(Vector2 position)
        {
            var chunk = new Chunk()
            {
                Position = position,
                Fields = GenerateFields(position),
                Entities = GenerateEntities()
            };

            return chunk;
        }

        protected virtual List<Field> GenerateFields(Vector2 chunkPosition)
        {
            var fields = new List<Field>();

            var chunkOffsetX = (chunkPosition.X * chunkSize);
            var chunkOffsetZ = (chunkPosition.Y * chunkSize);

            for (int z = 0; z < this.chunkSize; z++)
            {
                for (int x = 0; x < this.chunkSize; x++)
                {
                    float xCoord = (chunkOffsetX + x) * world.Scale;
                    float yCoord = (chunkOffsetZ + z) * world.Scale;

                    float y = UnityEngine.Mathf.PerlinNoise(xCoord + seed, yCoord + seed) * world.Scale;

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

        private List<Entity> GenerateEntities()
        {
            var entities = new List<Entity>();

            // empty for now

            return entities;
        }
    }
}
