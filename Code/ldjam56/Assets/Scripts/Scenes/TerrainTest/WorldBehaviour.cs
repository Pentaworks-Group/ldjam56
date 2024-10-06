using System;

using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Generation;
using Assets.Scripts.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.TerrainTest
{
    public class WorldBehaviour : MonoBehaviour
    {
        private World world;

        public GameObject templateContainer;
        public GameObject terrainContainer;

        private GameObject terrainTemplate;
        private TerrainData templateData;

        private readonly Dictionary<String, GameObject> entityTemplates = new Dictionary<String, GameObject>();

        private readonly List<ChunkBehaviour> chunkBehaviours = new List<ChunkBehaviour>();
        private readonly Map<float, ChunkBehaviour> chunkMap = new Map<float, ChunkBehaviour>();

        private void Awake()
        {
            var world = Assets.Scripts.Base.Core.Game.State?.World;

            if (world == default)
            {
                world = new WorldGenerator(new WorldDefinition()
                {
                    ChunkSize = 32,
                    SeedRange = new GameFrame.Core.Math.Range(1, 1)                    
                }).Generate();
            }

            this.world = world;
        }

        private void Start()
        {
            LoadTemplates();

            LoadWorld();
        }

        private void LoadTemplates()
        {
            this.terrainTemplate = templateContainer.transform.Find("TerrainTemplate").gameObject;
            this.templateData = terrainTemplate.GetComponent<Terrain>().terrainData;

            var entityTemplateContainerTransform = templateContainer.transform.Find("Entities");

            if (entityTemplateContainerTransform != null)
            {
                foreach (Transform template in entityTemplateContainerTransform)
                {
                    this.entityTemplates[template.name] = template.gameObject;
                }
            }
        }

        private void LoadWorld()
        {
            foreach (var chunk in this.world.Chunks)
            {
                var newTerrainObject = Instantiate(terrainTemplate, terrainContainer.transform);
                newTerrainObject.name = String.Format("Terrain_{0}_{1}", chunk.Position.X, chunk.Position.Y);

                var terrain = newTerrainObject.GetComponent<Terrain>();

                terrain.terrainData = TerrainDataCloner.Clone(templateData);

                var collider = newTerrainObject.GetComponent<TerrainCollider>();

                collider.terrainData = terrain.terrainData;

                newTerrainObject.SetActive(true);

                var chunkBehaviour = newTerrainObject.AddComponent<ChunkBehaviour>();
                chunkBehaviour.SetChunk(this.world, chunk);

                chunkBehaviours.Add(chunkBehaviour);
                chunkMap[chunk.Position.X, chunk.Position.Y] = chunkBehaviour;
            }

            UpdateNeighbors();
        }

        private void UpdateNeighbors()
        {
            foreach (var chunk in chunkBehaviours)
            {
                var leftNeighbour = GetNeighbour(chunk.Chunk, Direction.Left);
                var topNeighbour = GetNeighbour(chunk.Chunk, Direction.Top);
                var rightNeighbour = GetNeighbour(chunk.Chunk, Direction.Right);
                var bottomNeighbour = GetNeighbour(chunk.Chunk, Direction.Bottom);

                chunk.SetNeighbours(leftNeighbour, topNeighbour, rightNeighbour, bottomNeighbour);

                //if (chunk.LeftNeighbour == null)
                //{
                //    var neighbour = GetNeighbour(chunk.Chunk.Position.X - 1, chunk.Chunk.Position.Y);

                //    if (neighbour != null)
                //    {
                //        chunk.SetNeighbour(NeightbourDirection.Left, neighbour);
                //    }
                //}

                //if (chunk.TopNeighbour == null)
                //{
                //    var neighbour = GetNeighbour(chunk.Chunk.Position.X, chunk.Chunk.Position.Y + 1);

                //    if (neighbour != null)
                //    {
                //        chunk.SetNeighbour(NeightbourDirection.Top, neighbour);
                //    }
                //}

                //if (chunk.RightNeighbour == null)
                //{
                //    var neighbour = GetNeighbour(chunk.Chunk.Position.X + 1, chunk.Chunk.Position.Y);

                //    if (neighbour != null)
                //    {
                //        chunk.SetNeighbour(NeightbourDirection.Right, neighbour);
                //    }
                //}

                //if (chunk.BottomNeighbour == null)
                //{
                //    var neighbour = GetNeighbour(chunk.Chunk.Position.X, chunk.Chunk.Position.Y - 1);

                //    if (neighbour != null)
                //    {
                //        chunk.SetNeighbour(NeightbourDirection.Bottom, neighbour);
                //    }
                //}
            }
        }

        private ChunkBehaviour GetNeighbour(Chunk chunk, Direction direction)
        {
            var x = chunk.Position.X;
            var z = chunk.Position.Y;

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

        private ChunkBehaviour GetNeighbour(float x, float z)
        {
            if (chunkMap.TryGetValue(x, z, out var neightbour))
            {
                return neightbour;
            }

            return default;
        }
    }
}
