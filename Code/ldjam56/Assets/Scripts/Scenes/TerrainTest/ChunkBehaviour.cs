using Assets.Scripts.Model;

using NUnit.Framework;

using UnityEngine;

namespace Assets.Scripts.Scenes.TerrainTest
{
    public class ChunkBehaviour : MonoBehaviour
    {
        public Chunk Chunk { get; private set; }

        public void SetChunk(Chunk chunk)
        {
            this.Chunk = chunk;

            if (chunk != null)
            {
                var terrain = GetComponent<Terrain>();

                var terrainData = terrain.terrainData;

                transform.position = new Vector3(chunk.Position.X * terrainData.size.x, 0, chunk.Position.Y * terrainData.size.z);

                LoadFields();
            }
        }

        private void LoadFields()
        {
            foreach (var field in Chunk.Fields)
            {
                
            }
        }
    }
}
