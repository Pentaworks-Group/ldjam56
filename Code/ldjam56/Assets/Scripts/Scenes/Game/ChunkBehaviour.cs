using System;
using System.Collections.Generic;

using Assets.Scripts.Model;
using Assets.Scripts.Scenes.Game.Hazards;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class ChunkBehaviour : MonoBehaviour
    {
        private readonly Dictionary<String, Int32> layerNameCache = new Dictionary<String, Int32>();
        private Terrain terrain;

        public WorldBehaviour WorldBehaviour { get; private set; }
        public Chunk Chunk { get; private set; }

        public ChunkBehaviour LeftNeighbour { get; private set; }
        public ChunkBehaviour TopNeighbour { get; private set; }
        public ChunkBehaviour RightNeighbour { get; private set; }
        public ChunkBehaviour BottomNeighbour { get; private set; }

        //private readonly Map<float, TextMeshPro> fieldMap = new Map<float, TextMeshPro>();

        public void SetChunk(WorldBehaviour worldBehaviour, Chunk chunk)
        {
            this.WorldBehaviour = worldBehaviour;
            this.Chunk = chunk;

            if (chunk != null)
            {
                this.terrain = GetComponent<Terrain>();

                for (int layerIndex = 0; layerIndex < this.terrain.terrainData.terrainLayers.Length; layerIndex++)
                {
                    var layer = this.terrain.terrainData.terrainLayers[layerIndex];

                    layerNameCache[layer.name] = layerIndex;
                }

                var positionX = chunk.Position.X * terrain.terrainData.size.x - 1;
                var positionZ = chunk.Position.Y * terrain.terrainData.size.z - 1;

                transform.position = new Vector3(positionX, 0, positionZ);

                LoadFields();
            }
        }

        public void SetNeighbours(ChunkBehaviour leftNeighbour, ChunkBehaviour topNeighbour, ChunkBehaviour rightNeighbour, ChunkBehaviour bottomNeighbour)
        {
            var leftTerrain = GetTerrain(leftNeighbour);
            this.LeftNeighbour = leftNeighbour;

            var topTerrain = GetTerrain(topNeighbour);
            this.TopNeighbour = topNeighbour;

            var rightTerrain = GetTerrain(rightNeighbour);
            this.RightNeighbour = rightNeighbour;

            var bottomTerrain = GetTerrain(bottomNeighbour);
            this.BottomNeighbour = bottomNeighbour;

            this.terrain.SetNeighbors(leftTerrain, topTerrain, rightTerrain, bottomTerrain);
        }

        public ChunkBehaviour GetNeighbour(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left: return this.LeftNeighbour;
                case Direction.Top: return this.TopNeighbour;
                case Direction.Right: return this.RightNeighbour;
                case Direction.Bottom: return this.BottomNeighbour;
            }

            throw new NotSupportedException("Wut?");
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

        public void RefreshHeightMap()
        {
            var heightMap = this.terrain.terrainData.GetHeights(0, 0, this.terrain.terrainData.heightmapResolution, this.terrain.terrainData.heightmapResolution);

            var heightFieldSize = this.terrain.terrainData.heightmapResolution / WorldBehaviour.World.ChunkSize;

            foreach (var field in Chunk.Fields)
            {
                DrawHeightMap(field, heightFieldSize, ref heightMap);
            }

            this.terrain.terrainData.SetHeights(0, 0, heightMap);
        }

        private void DrawField(Field field, Vector3 fieldSize)
        {
            var fieldActualHeight = UnityEngine.Mathf.Lerp(0, this.terrain.terrainData.size.y, field.Position.Y);

            //if (field.Edges != EdgeSide.None)
            //{
            //var fieldObject = WorldBehaviour.GetTemplateCopy("FieldTemplate", this.transform);

            //var rectTransform = fieldObject.GetComponent<RectTransform>();

            //rectTransform.localPosition = new UnityEngine.Vector3(field.Position.X * fieldSize.x + (fieldSize.x / 2), fieldActualHeight * 1.25f, field.Position.Z * fieldSize.z + (fieldSize.z / 2));
            //rectTransform.sizeDelta = fieldSize;

            //var text = fieldObject.transform.Find("Text").GetComponent<TextMeshPro>();

            //fieldMap[field.Position.X, field.Position.Z] = text;

            //text.text = String.Format("{0}", fieldActualHeight);
            //}

            if (field.IsHome != default)
            {
                var house = WorldBehaviour.GetTemplateCopy("BeeHive", this.transform, false);

                var centerX = field.Position.X * fieldSize.x;
                var centerY = field.Position.Z * fieldSize.z;


                house.transform.localPosition = new UnityEngine.Vector3(centerX, fieldActualHeight, centerY);
                house.SetActive(true);

                WorldBehaviour.homeHive = house;
                WorldBehaviour.bee.transform.position = new UnityEngine.Vector3(centerX, fieldActualHeight + 1, centerY - 3);
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

            if (field.Biome?.PossibleEntities?.Count > 0)
            {
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
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 9)
            {
                this.WorldBehaviour.GenerateChunkNeighbors(this);

                if (TryGetComponent(out BoxCollider boxCollider))
                {
                    Destroy(boxCollider);
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

            var thirdOfSize = fieldSize.x / 3;

            var position = new UnityEngine.Vector3(centerX + UnityEngine.Random.Range(thirdOfSize, 2 * thirdOfSize), yPosition, centerY + UnityEngine.Random.Range(thirdOfSize, 2 * thirdOfSize));

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
            model.transform.position = position;

            //Debug.Log("Placed at: " + position);
            var hazardBehaviour = model.GetComponent<HazardBaseBehaviour>();
            hazardBehaviour.Init();
        }

        private void DrawHeightMap(Field field, Int32 heightFieldSize, ref Single[,] heightMap)
        {
            //if (fieldMap.TryGetValue(field.Position.X, field.Position.Z, out var textMesh))
            //{
            //    textMesh.text = String.Format("{0}", field.Position.Y);
            //}

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
            if (layerNameCache.TryGetValue(biome.TextureLayerName, out var layerIndex))
            {
                return layerIndex;
            }

            throw new NotSupportedException($"Layer '{biome.TextureLayerName}' not found in terrain data!");
        }

        private Terrain GetTerrain(ChunkBehaviour chunkBehaviour)
        {
            if (chunkBehaviour != null)
            {
                return chunkBehaviour.terrain;
            }

            return null;
        }

        private void Update()
        {
            if (this.terrain != null && Chunk != default && Chunk.IsUpdateRequired)
            {
                //Debug.LogFormat("Refreshing Chunk ( {0} - {1} )", this.Chunk.Position.X, this.Chunk.Position.Y);

                Chunk.IsUpdateRequired = false;
                RefreshHeightMap();
            }
        }
    }
}
