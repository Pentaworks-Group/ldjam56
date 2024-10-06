using System;

using GameFrame.Core.Definitions;

public class WorldDefinition : BaseDefinition
{
    public Int32 ChunkSize { get; set; }
    public GameFrame.Core.Math.Range SeedRange { get; set; }
}
