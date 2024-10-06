using System;

using GameFrame.Core.Definitions;

public class WorldDefinition : BaseDefinition
{
    public float TerrainScale { get; set; }
    public Int32 ChunkSize { get; set; }
    public GameFrame.Core.Math.Range BiomeSeedRange { get; set; }
    public GameFrame.Core.Math.Range TerrainSeedRange { get; set; }
}
