using System;

using System.Collections.Generic;

using Assets.Scripts.Core;
using Assets.Scripts.Core.Generation;
using Assets.Scripts.Extensions;
using Assets.Scripts.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class WorldBehaviour : MonoBehaviour
    {
        public GameObject templateContainer;
        public GameObject terrainContainer;
        public GameObject bee;
        public GameObject homeHive;

        private GameObject terrainTemplate;
        private TerrainData templateData;
        private ExistingWorldGenerator worldGenerator;

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

        public void GenerateNeighbourChunk(ChunkBehaviour startingChunk, Direction direction)
        {
            if (GetOrGenerateChunkBehaviour(startingChunk, direction, out var newNeightbour))
            {
                var isLeftNewNeighbourNew = GetOrGenerateChunkBehaviour(newNeightbour, direction.Left(), out var leftNewNeighbour);
                var isRightNewNeighbourNew = GetOrGenerateChunkBehaviour(newNeightbour, direction.Right(), out var rightNewNeighbour);

                UpdateAllNeighbours(startingChunk);
                UpdateAllNeighbours(leftNewNeighbour);
                UpdateAllNeighbours(rightNewNeighbour);
            }
        }

        public void GenerateChunkNeighbors(ChunkBehaviour chunkBehviour)
        {
            var generator = GetWorldGenerator();

            var isLeftNewChunk = GetOrGenerateChunkBehaviour(chunkBehviour, Direction.Left, out var leftNeightbour);
            var isTopNewChunk = GetOrGenerateChunkBehaviour(chunkBehviour, Direction.Top, out var topNeightbour);
            var isRightNewChunk = GetOrGenerateChunkBehaviour(chunkBehviour, Direction.Right, out var rightNeightbour);
            var isBottomNewChunk = GetOrGenerateChunkBehaviour(chunkBehviour, Direction.Bottom, out var bottomNeightbour);

            if (isLeftNewChunk || isTopNewChunk || isRightNewChunk || isBottomNewChunk)
            {
                chunkBehviour.SetNeighbours(leftNeightbour, topNeightbour, rightNeightbour, bottomNeightbour);

                if (isLeftNewChunk)
                {
                    UpdateAllNeighbours(leftNeightbour);
                }

                if (isTopNewChunk)
                {
                    UpdateAllNeighbours(topNeightbour);
                }

                if (isRightNewChunk)
                {
                    UpdateAllNeighbours(rightNeightbour);
                }

                if (isBottomNewChunk)
                {
                    UpdateAllNeighbours(bottomNeightbour);
                }
            }
        }

        private Boolean GetOrGenerateChunkBehaviour(ChunkBehaviour chunkBehviour, Direction direction, out ChunkBehaviour neighbour)
        {
            neighbour = chunkBehviour.GetNeighbour(direction);

            if (neighbour == null)
            {
                var newChunk = GetWorldGenerator().Expand(chunkBehviour.Chunk, direction);

                neighbour = SpawnChunk(newChunk);

                return true;
            }

            return false;
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
            if (this.World != default)
            {
                LoadTemplates();

                LoadWorld();
            }
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
            foreach (var chunk in this.World.Chunks)
            {
                _ = SpawnChunk(chunk);
            }

            UpdateNeighbors();
        }

        private ChunkBehaviour SpawnChunk(Chunk chunk)
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

            return chunkBehaviour;
        }

        private void UpdateNeighbors()
        {
            foreach (var chunk in chunkBehaviours)
            {
                UpdateAllNeighbours(chunk);
            }
        }

        private void UpdateAllNeighbours(ChunkBehaviour chunk)
        {
            var leftNeighbour = GetNeighbour(chunk.Chunk, Direction.Left);
            var topNeighbour = GetNeighbour(chunk.Chunk, Direction.Top);
            var rightNeighbour = GetNeighbour(chunk.Chunk, Direction.Right);
            var bottomNeighbour = GetNeighbour(chunk.Chunk, Direction.Bottom);

            chunk.SetNeighbours(leftNeighbour, topNeighbour, rightNeighbour, bottomNeighbour);
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

        private ExistingWorldGenerator GetWorldGenerator()
        {
            if (this.worldGenerator == default)
            {
                this.worldGenerator = new ExistingWorldGenerator(this.World);
            }

            return this.worldGenerator;
        }
    }
}
