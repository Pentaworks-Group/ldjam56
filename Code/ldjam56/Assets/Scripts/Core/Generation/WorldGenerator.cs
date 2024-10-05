using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Generation
{
    internal class WorldGenerator
    {
        public World Generate()
        {
            var world = new World()
            {
                Chunks = GenerateChunks(new GameFrame.Core.Math.Vector2(0, 0), 3, 3)
            };

            return world;
        }

        private List<Chunk> GenerateChunks(GameFrame.Core.Math.Vector2 rootPosition, Int32 columns, Int32 rows)
        {
            var chunks = new List<Chunk>();

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    chunks.Add(GenerateChunk(new Vector2(rootPosition.X + i, rootPosition.Y + j)));
                }
            }

            return chunks;
        }

        private Chunk GenerateChunk(Vector2 position)
        {
            var chunk = new Chunk()
            {
                Position = position,
                Fields = GenerateFields(16, 16),
                Entities = GenerateEntities()
            };

            return chunk;
        }

        private List<Field> GenerateFields(Int32 columns, Int32 rows)
        {
            var fields = new List<Field>();

            for (int z = 0; z < rows; z++)
            {
                for (int x = 0; x < columns; x++)
                {
                    var y = 0; // height of the field.

                    var field = new Field()
                    {
                        Position = new Vector3(x, y, z)
                    };

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
