using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class TerrainDataExtensions
    {
        public static TerrainData Copy(this TerrainData sourceTerrainData)
        {
            if (sourceTerrainData != null)
            {
                var copy = new TerrainData()
                {
                    alphamapResolution = sourceTerrainData.alphamapResolution,
                    baseMapResolution = sourceTerrainData.baseMapResolution,

                    heightmapResolution = sourceTerrainData.heightmapResolution,
                    size = sourceTerrainData.size,
                    terrainLayers = sourceTerrainData.terrainLayers
                };

                copy.SetAlphamaps(0, 0, sourceTerrainData.GetAlphamaps(0, 0, sourceTerrainData.alphamapWidth, sourceTerrainData.alphamapHeight));
                copy.SetHeights(0, 0, sourceTerrainData.GetHeights(0, 0, sourceTerrainData.heightmapResolution, sourceTerrainData.heightmapResolution));

                return copy;
            }

            return null;
        }
    }
}
