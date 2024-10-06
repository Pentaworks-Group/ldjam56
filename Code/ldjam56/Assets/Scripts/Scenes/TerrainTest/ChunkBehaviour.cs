using System;

using Assets.Scripts.Model;

using UnityEngine;

namespace Assets.Scripts.Scenes.TerrainTest
{
    public class ChunkBehaviour : MonoBehaviour
    {
        private Terrain terrain;

        public World World { get; private set; }
        public Chunk Chunk { get; private set; }

        public ChunkBehaviour LeftNeighbour { get; private set; }
        public ChunkBehaviour TopNeighbour { get; private set; }
        public ChunkBehaviour RightNeighbour { get; private set; }
        public ChunkBehaviour BottomNeighbour { get; private set; }

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
            var heightMap = new float[this.terrain.terrainData.heightmapResolution, this.terrain.terrainData.heightmapResolution];

            var fieldSizeX = this.terrain.terrainData.heightmapResolution / World.ChunkSize;
            var fieldSizeZ = fieldSizeX;

            foreach (var field in Chunk.Fields)
            {
                var rangeXStart = fieldSizeX * (Int32)field.Position.X;
                var rangeXEnd = rangeXStart + fieldSizeX;

                if (rangeXEnd + 1 == this.terrain.terrainData.heightmapResolution)
                {
                    rangeXEnd++;
                }

                var rangeZStart = fieldSizeZ * (Int32)field.Position.Z;
                var rangeZEnd = rangeZStart + fieldSizeZ;

                if (rangeZEnd + 1 == this.terrain.terrainData.heightmapResolution)
                {
                    rangeZEnd++;
                }

                for (int z = rangeZStart; z < rangeZEnd; z++)
                {
                    for (int x = rangeXStart; x < rangeXEnd; x++)
                    {
                        heightMap[z, x] = field.Position.Y;
                    }
                }
            }

            var height1 = heightMap[0, this.terrain.terrainData.heightmapResolution - 1];
            var height2 = heightMap[this.terrain.terrainData.heightmapResolution - 1, 0];
            var height3 = heightMap[this.terrain.terrainData.heightmapResolution - 1, this.terrain.terrainData.heightmapResolution - 1];
            var height4 = heightMap[0, 0];

            this.terrain.terrainData.SetHeights(0, 0, heightMap);
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
