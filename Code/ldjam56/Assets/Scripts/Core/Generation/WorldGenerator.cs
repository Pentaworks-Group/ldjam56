using System;
using System.Collections.Generic;

using Assets.Scripts.Model;

using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Generation
{
    internal class WorldGenerator
    {
        private readonly Int32 chunkSize;
        public WorldGenerator(Int32 chunkSize = 16)
        {
            this.chunkSize = chunkSize;
        }

        public World Generate()
        {
            var world = new World()
            {
                Chunks = GenerateRootChunks()
            };

            return world;
        }

        private List<Chunk> GenerateRootChunks()
        {
            var chunks = new List<Chunk>()
            {
                GenerateChunk(new Vector2(-1, -1)),
                GenerateChunk(new Vector2(0, -1)),
                GenerateChunk(new Vector2(1, -1)),
                GenerateChunk(new Vector2(-1, 0)),
                GenerateChunk(new Vector2(0, 0)),
                GenerateChunk(new Vector2(1, 0)),
                GenerateChunk(new Vector2(-1, 1)),
                GenerateChunk(new Vector2(0, 1)),
                GenerateChunk(new Vector2(1, 1))
            };

            return chunks;
        }

        private Chunk GenerateChunk(Vector2 position)
        {
            var chunk = new Chunk()
            {
                Position = position,
                Fields = GenerateFields(),
                Entities = GenerateEntities()
            };

            return chunk;
        }

        private List<Field> GenerateFields()
        {
            var fields = new List<Field>();

            for (int z = 0; z < this.chunkSize; z++)
            {
                for (int x = 0; x < this.chunkSize; x++)
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
