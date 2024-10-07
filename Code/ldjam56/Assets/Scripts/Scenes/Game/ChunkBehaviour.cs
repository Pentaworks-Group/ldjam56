using System;

using Assets.Scripts.Model;
using Assets.Scripts.Scenes.Game.Hazards;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class ChunkBehaviour : MonoBehaviour
    {
        private Terrain terrain;

        public WorldBehaviour WorldBehaviour { get; private set; }
        public Chunk Chunk { get; private set; }

        public ChunkBehaviour LeftNeighbour { get; private set; }
        public ChunkBehaviour TopNeighbour { get; private set; }
        public ChunkBehaviour RightNeighbour { get; private set; }
        public ChunkBehaviour BottomNeighbour { get; private set; }

        public void SetChunk(WorldBehaviour worldBehaviour, Chunk chunk)
        {
            this.WorldBehaviour = worldBehaviour;
            this.Chunk = chunk;

            if (chunk != null)
            {
                this.terrain = GetComponent<Terrain>();

                transform.position = new Vector3(chunk.Position.X * terrain.terrainData.size.x, 0, chunk.Position.Y * terrain.terrainData.size.z);

                LoadFields();
            }
        }

        public void SetNeighbours(ChunkBehaviour leftNeighbour, ChunkBehaviour topNeighbour, ChunkBehaviour rightNeighbour, ChunkBehaviour bottomNeighbour)
        {
            var leftTerrain = GetTerrain(leftNeighbour);
            var topTerrain = GetTerrain(topNeighbour);
            var rightTerrain = GetTerrain(rightNeighbour);
            var bottomTerrain = GetTerrain(bottomNeighbour);

            this.terrain.SetNeighbors(leftTerrain, topTerrain, rightTerrain, bottomTerrain);
        }

        public void SetNeighbour(Direction direction, ChunkBehaviour neighbour, Boolean isReverse = false)
        {
            switch (direction)
            {
                case Direction.Left:
                    this.LeftNeighbour = neighbour;

                    if (neighbour != null)
                    {
                        this.terrain.SetNeighbors(neighbour.terrain, terrain.topNeighbor, terrain.rightNeighbor, terrain.bottomNeighbor);

                        if (!isReverse)
                        {
                            neighbour.SetNeighbour(Direction.Right, this, true);
                        }
                    }
                    break;

                case Direction.Top:
                    this.TopNeighbour = neighbour;

                    if (neighbour != null)
                    {
                        this.terrain.SetNeighbors(terrain.leftNeighbor, neighbour.terrain, terrain.rightNeighbor, terrain.bottomNeighbor);

                        if (!isReverse)
                        {
                            neighbour.SetNeighbour(Direction.Bottom, this, true);
                        }
                    }
                    break;
                case Direction.Right:
                    this.RightNeighbour = neighbour;

                    if (neighbour != null)
                    {
                        this.terrain.SetNeighbors(terrain.leftNeighbor, terrain.topNeighbor, neighbour.terrain, terrain.bottomNeighbor);

                        if (!isReverse)
                        {
                            neighbour.SetNeighbour(Direction.Left, this, true);
                        }
                    }
                    break;
                case Direction.Bottom:
                    this.BottomNeighbour = neighbour;

                    if (neighbour != null)
                    {
                        this.terrain.SetNeighbors(terrain.leftNeighbor, terrain.topNeighbor, terrain.rightNeighbor, neighbour.terrain);

                        if (!isReverse)
                        {
                            neighbour.SetNeighbour(Direction.Top, this, true);
                        }
                    }
                    break;
            }
        }

        private void LoadFields()
        {
            var fieldSize = (this.terrain.terrainData.size / this.WorldBehaviour.World.ChunkSize);

            var heightMap = new float[this.terrain.terrainData.heightmapResolution, this.terrain.terrainData.heightmapResolution];
            var heightFieldSize = this.terrain.terrainData.heightmapResolution / WorldBehaviour.World.ChunkSize;

            var alphaMap = this.terrain.terrainData.GetAlphamaps(0, 0, this.terrain.terrainData.alphamapWidth, this.terrain.terrainData.alphamapHeight);

            var alphaFieldSizeX = this.terrain.terrainData.alphamapWidth / WorldBehaviour.World.ChunkSize;
            var alphaFieldSizeZ = this.terrain.terrainData.alphamapHeight / WorldBehaviour.World.ChunkSize;

            foreach (var field in Chunk.Fields)
            {
                DrawField(field, fieldSize);
                DrawHeightMap(field, heightFieldSize, ref heightMap);
                DrawAlphaMap(field, alphaFieldSizeX, alphaFieldSizeZ, ref alphaMap);
            }

            this.terrain.terrainData.SetHeights(0, 0, heightMap);
            this.terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
        }

        private void DrawField(Field field, Vector3 fieldSize)
        {
            if (field.IsHome != default)
            {
                var house = WorldBehaviour.GetTemplateCopy("BeeHive", this.transform, false);

                var centerX = field.Position.X * fieldSize.x;
                var centerY = field.Position.Z * fieldSize.z;

                var test = UnityEngine.Mathf.Lerp(0, this.terrain.terrainData.size.y, field.Position.Y);

                house.transform.localPosition = new UnityEngine.Vector3(centerX, test, centerY);
                house.SetActive(true);

                WorldBehaviour.homeHive = house;
                WorldBehaviour.bee.transform.position = new UnityEngine.Vector3(centerX, test + 1, centerY - 1);
            }
            else
            {
                SelectAndPlaceEntity(field, fieldSize);
                SelectAndPlaceHazard(field, fieldSize);  
            }
        }

        private void SelectAndPlaceEntity(Field field, Vector3 fieldSize)
        {
            var rand = UnityEngine.Random.value;
            foreach (var entity in field.Biome.PossibleEntities)
            {
                rand -= entity.Chance;
                if (rand <= 0)
                {
                    PlaceEntity(entity, field, fieldSize);
                    break;
                }
            }
        }


        private void PlaceEntity(Entity entity, Field field, Vector3 fieldSize)
        {
            var position = GetPosition(field, fieldSize);

            var model = WorldBehaviour.GetTemplateCopy(entity.ModelReference, this.transform, false);
            var rotationQuater = Quaternion.Euler(0, UnityEngine.Random.value * 360, 0);
            var scale = UnityEngine.Random.Range(.9f, 1.1f);
            model.transform.localScale *= scale;
            model.transform.SetLocalPositionAndRotation(position, rotationQuater);
            model.SetActive(true);
        }

        private Vector3 GetPosition(Field field, Vector3 fieldSize)
        {
            var centerX = field.Position.X * fieldSize.x;
            var centerY = field.Position.Z * fieldSize.z;
            var yPosition = UnityEngine.Mathf.Lerp(0, this.terrain.terrainData.size.y, field.Position.Y);
            var position = new UnityEngine.Vector3(centerX + UnityEngine.Random.Range(fieldSize.x / 3, 2 * fieldSize.x / 3), yPosition, centerY + UnityEngine.Random.Range(fieldSize.x / 3, 2 * fieldSize.x / 3));
            return position;
        }

        private void SelectAndPlaceHazard(Field field, Vector3 fieldSize)
        {
            var rand = UnityEngine.Random.value;
            foreach (var hazard in field.Biome.PossibleHazards)
            {
                rand -= hazard.Chance;
                if (rand <= 0)
                {
                    PlaceHazard(hazard, field, fieldSize);
                    break;
                }
            }
        }
        private void PlaceHazard(Entity entity, Field field, Vector3 fieldSize)
        {
            var position = GetPosition(field, fieldSize);
            var model = WorldBehaviour.GetTemplateCopy(entity.ModelReference, this.transform, false);
            model.SetActive(true);

            var hazardBehaviour = model.GetComponent<HazardBaseBehaviour>();
            hazardBehaviour.Init();

        }

        private void DrawHeightMap(Field field, Int32 heightFieldSize, ref Single[,] heightMap)
        {
            // HeightMap
            var heightRangeXStart = heightFieldSize * (Int32)field.Position.X;
            var heightRangeXEnd = heightRangeXStart + heightFieldSize;

            if (heightRangeXEnd + 1 == this.terrain.terrainData.heightmapResolution)
            {
                heightRangeXEnd++;
            }

            var heightRangeZStart = heightFieldSize * (Int32)field.Position.Z;
            var heightRangeZEnd = heightRangeZStart + heightFieldSize;

            if (heightRangeZEnd + 1 == this.terrain.terrainData.heightmapResolution)
            {
                heightRangeZEnd++;
            }

            for (int z = heightRangeZStart; z < heightRangeZEnd; z++)
            {
                for (int x = heightRangeXStart; x < heightRangeXEnd; x++)
                {
                    heightMap[z, x] = field.Position.Y;
                }
            }
        }

        private void DrawAlphaMap(Field field, Int32 alphaFieldSizeX, Int32 alphaFieldSizeZ, ref Single[,,] alphaMap)
        {
            // AlphaMap
            if (!field.Biome.IsDefault)
            {
                var alphaRangeXStart = alphaFieldSizeX * (Int32)field.Position.X;
                var alphaRangeXEnd = alphaRangeXStart + alphaFieldSizeX;

                var alphaRangeZStart = alphaFieldSizeZ * (Int32)field.Position.Z;
                var alphaRangeZEnd = alphaRangeZStart + alphaFieldSizeZ;

                var layerIndex = GetLayerIndex(field.Biome);

                for (int z = alphaRangeZStart; z < alphaRangeZEnd; z++)
                {
                    for (int x = alphaRangeXStart; x < alphaRangeXEnd; x++)
                    {
                        alphaMap[z, x, layerIndex] = 1f;
                    }
                }
            }
        }

        private Int32 GetLayerIndex(Biome biome)
        {
            switch (biome.Name)
            {
                case "Forrest":
                    return 1;

                case "Desert":
                    return 2;

                case "Water":
                    return 3;

                case "Other":
                    return 4;

                default:
                case "Grass":
                    return 0;
            }
        }

        private Terrain GetTerrain(ChunkBehaviour chunkBehaviour)
        {
            if (chunkBehaviour != null)
            {
                return chunkBehaviour.terrain;
            }

            return null;
        }
    }
}
