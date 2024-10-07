using System;

using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class WorldBehaviour : MonoBehaviour
    {
        public GameObject templateContainer;
        public GameObject terrainContainer;
        public GameObject bee;

        private GameObject terrainTemplate;
        private TerrainData templateData;

        private readonly Dictionary<String, GameObject> entityTemplates = new Dictionary<String, GameObject>();

        private readonly List<ChunkBehaviour> chunkBehaviours = new List<ChunkBehaviour>();
        private readonly Map<float, ChunkBehaviour> chunkMap = new Map<float, ChunkBehaviour>();
        public World World { get; private set; }

        public GameObject GetTemplateCopy(String templateRefernce, Transform parentTransform, Boolean inWorldSpace = true)
        {
            if (entityTemplates.TryGetValue(templateRefernce, out var template))
            {
                var copy = Instantiate(template, parentTransform, inWorldSpace);

                return copy;
            }

            return null;
        }

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(AfterGameInstantiation);
        }

        private void AfterGameInstantiation()
        {
            this.World = Base.Core.Game.State?.World;
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
                    if (template.gameObject.layer != 8)
                    {
                        throw new Exception("Invalid layer " + template.gameObject.layer + " for " + template.name);
                    }
                    this.entityTemplates[template.name] = template.gameObject;
                }
            }
        }
        private void LoadWorld()
        {
            if (this.World != default)
            {
                foreach (var chunk in this.World.Chunks)
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

            terrain.terrainData = Terrains.TerrainDataCloner.Clone(templateData);

            var collider = newTerrainObject.GetComponent<TerrainCollider>();

            collider.terrainData = terrain.terrainData;

            newTerrainObject.SetActive(true);

            var chunkBehaviour = newTerrainObject.AddComponent<ChunkBehaviour>();
            chunkBehaviour.SetChunk(this, chunk);

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
