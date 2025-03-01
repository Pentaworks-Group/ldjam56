using System;

using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Core.Generation;
using Assets.Scripts.Extensions;
using Assets.Scripts.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.TerrainTest
{
    public class TestWorldBehaviour : MonoBehaviour
    {
        private World world;

        public GameObject templateContainer;
        public GameObject terrainContainer;

        private GameObject terrainTemplate;
        private TerrainData terrainTemplateData;

        private readonly Dictionary<String, GameObject> entityTemplates = new Dictionary<String, GameObject>();

        private readonly List<TestChunkBehaviour> chunkBehaviours = new List<TestChunkBehaviour>();
        private readonly Map<float, TestChunkBehaviour> chunkMap = new Map<float, TestChunkBehaviour>();

        private void Awake()
        {
            var world = Assets.Scripts.Base.Core.Game.State?.World;

            if (world == default)
            {
                world = new NewWorldGenerator(new WorldDefinition()
                {
                    ChunkSize = 32,
                    BiomeSeedRange = new GameFrame.Core.Math.Range(0, 1),
                    TerrainSeedRange = new GameFrame.Core.Math.Range(0, 1),
                    TerrainScale = 0.085f,
                    //TerrainHeightRange = new GameFrame.Core.Math.Range(60, 600)
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
            this.terrainTemplateData = terrainTemplate.GetComponent<Terrain>().terrainData;

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
            if (this.world != default)
            {
                foreach (var chunk in this.world.Chunks)
                {
                    LoadChunk(chunk);
                }

                UpdateNeighbors();
            }
        }

        private void LoadChunk(Chunk chunk)
        {
            var newTerrainObject = Instantiate(terrainTemplate, terrainContainer.transform);
            newTerrainObject.name = String.Format("Terrain_{0}_{1}", chunk.Position.X, chunk.Position.Y);

            var terrain = newTerrainObject.GetComponent<Terrain>();

            terrain.terrainData = terrainTemplateData.Copy();

            var collider = newTerrainObject.GetComponent<TerrainCollider>();

            collider.terrainData = terrain.terrainData;

            newTerrainObject.SetActive(true);

            var chunkBehaviour = newTerrainObject.AddComponent<TestChunkBehaviour>();
            chunkBehaviour.SetChunk(this.world, chunk);

            chunkBehaviours.Add(chunkBehaviour);
            chunkMap[chunk.Position.X, chunk.Position.Y] = chunkBehaviour;
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
            }
        }

        private TestChunkBehaviour GetNeighbour(Chunk chunk, Direction direction)
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

        private TestChunkBehaviour GetNeighbour(float x, float z)
        {
            if (chunkMap.TryGetValue(x, z, out var neightbour))
            {
                return neightbour;
            }

            return default;
        }
    }
}
