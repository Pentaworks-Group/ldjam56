using System;

using GameFrame.Core.Definitions;

public class WorldDefinition : BaseDefinition
{
    public float Scale { get; set; }
    public Int32 ChunkSize { get; set; }
    public GameFrame.Core.Math.Range SeedRange { get; set; }
}
