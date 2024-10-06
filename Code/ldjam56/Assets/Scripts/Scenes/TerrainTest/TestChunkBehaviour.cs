using System;

using Assets.Scripts.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.TerrainTest
{
    public class TestChunkBehaviour : MonoBehaviour
    {
        private Terrain terrain;

        public World World { get; private set; }
        public Chunk Chunk { get; private set; }

        public TestChunkBehaviour LeftNeighbour { get; private set; }
        public TestChunkBehaviour TopNeighbour { get; private set; }
        public TestChunkBehaviour RightNeighbour { get; private set; }
        public TestChunkBehaviour BottomNeighbour { get; private set; }

        public void SetChunk(World world, Chunk chunk)
        {
            this.World = world;
            this.Chunk = chunk;

            if (chunk != null)
            {
                this.terrain = GetComponent<Terrain>();

                transform.position = new Vector3(chunk.Position.X * terrain.terrainData.size.x, 0, chunk.Position.Y * terrain.terrainData.size.z);

                LoadFields();
            }
        }

        public void SetNeighbours(TestChunkBehaviour leftNeighbour, TestChunkBehaviour topNeighbour, TestChunkBehaviour rightNeighbour, TestChunkBehaviour bottomNeighbour)
        {
            var leftTerrain = GetTerrain(leftNeighbour);
            var topTerrain = GetTerrain(topNeighbour);
            var rightTerrain = GetTerrain(rightNeighbour);
            var bottomTerrain = GetTerrain(bottomNeighbour);

            this.terrain.SetNeighbors(leftTerrain, topTerrain, rightTerrain, bottomTerrain);
        }

        public void SetNeighbour(Direction direction, TestChunkBehaviour neighbour, Boolean isReverse = false)
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
            var heightMap = new float[this.terrain.terrainData.heightmapResolution, this.terrain.terrainData.heightmapResolution];
            var heightFieldSize = this.terrain.terrainData.heightmapResolution / World.ChunkSize;

            var alphaMap = this.terrain.terrainData.GetAlphamaps(0, 0, this.terrain.terrainData.alphamapWidth, this.terrain.terrainData.alphamapHeight);

            var alphaFieldSizeX = this.terrain.terrainData.alphamapWidth / World.ChunkSize;
            var alphaFieldSizeZ = this.terrain.terrainData.alphamapHeight / World.ChunkSize;

            foreach (var field in Chunk.Fields)
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

            this.terrain.terrainData.SetHeights(0, 0, heightMap);
            this.terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
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

        private Terrain GetTerrain(TestChunkBehaviour chunkBehaviour)
        {
            if (chunkBehaviour != null)
            {
                return chunkBehaviour.terrain;
            }

            return null;
        }
    }
}
