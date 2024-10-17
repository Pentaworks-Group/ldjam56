using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitons;
using Assets.Scripts.Extensions;
using Assets.Scripts.Model;

using UnityEngine;

using Frame = GameFrame.Core.Math;

namespace Assets.Scripts.Core.Generation
{
    public class NewWorldGenerator : WorldGenerator
    {
        private readonly WorldDefinition worldDefinition;
        private World world;

        public NewWorldGenerator(WorldDefinition worldDefinition) : base(new GeneratorParameters(worldDefinition))
        {
            this.worldDefinition = worldDefinition;
        }

        public World Generate()
        {
            this.world = new World()
            {
                SunAngles = new Frame.Vector3(125,125,0),
                MoonAngles = new Frame.Vector3(180,90,0),
                TerrainScale = parameters.TerrainScale,
                TerrainSeed = parameters.TerrainSeed,
                ChunkSize = parameters.ChunkSize,
                Biomes = parameters.Biomes
            };

            this.world.Chunks = GenerateRootChunks();

            return this.world;
        }

        private List<Chunk> GenerateRootChunks()
        {
            var bottomLeft = GenerateChunk(new Frame.Vector2(-1, -1));
            var bottomCenter = GenerateChunk(new Frame.Vector2(0, -1));
            var bottomRight = GenerateChunk(new Frame.Vector2(1, -1));
            var middleLeft = GenerateChunk(new Frame.Vector2(-1, 0));
            var middleCenter = GenerateChunk(new Frame.Vector2(0, 0), true);
            var middleRight = GenerateChunk(new Frame.Vector2(1, 0));
            var topLeft = GenerateChunk(new Frame.Vector2(-1, 1));
            var topCenter = GenerateChunk(new Frame.Vector2(0, 1));
            var topRight = GenerateChunk(new Frame.Vector2(1, 1));

            var chunks = new List<Chunk>()
            {
                bottomLeft,
                bottomCenter,
                bottomRight,
                middleLeft,
                middleCenter,
                middleRight,
                topLeft,
                topCenter,
                topRight
            };

            //Stitch(bottomLeft, bottomCenter, Direction.Right);
            //Stitch(bottomCenter, bottomRight, Direction.Right);

            //Stitch(middleLeft, middleCenter, Direction.Right);
            //Stitch(middleCenter, middleRight, Direction.Right);

            //Stitch(topLeft, topCenter, Direction.Right);
            //Stitch(topCenter, topRight, Direction.Right);

            //Stitch(bottomLeft, middleLeft, Direction.Top);
            //Stitch(middleLeft, topLeft, Direction.Top);

            //Stitch(bottomCenter, middleCenter, Direction.Top);
            //Stitch(middleCenter, topCenter, Direction.Top);

            //Stitch(bottomRight, middleRight, Direction.Top);
            //Stitch(middleRight, topRight, Direction.Top);

            StitchAll(middleCenter);
            StitchAll(topLeft);
            StitchAll(topRight);
            StitchAll(bottomRight);
            StitchAll(bottomLeft);

            foreach (var chunk in chunks)
            {
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    var neighbour = GetNeighbour(chunk, direction);

                    if (neighbour != default)
                    {
                        var edge = direction.ToEdge();

                        var neighbourFields = neighbour.GetEdgeFields(edge.Opposing());

                        foreach (var field in chunk.GetEdgeFields(direction.ToEdge()))
                        {
                            var opposingField = default(Field);

                            switch (direction)
                            {
                                default:
                                case Direction.Left:
                                    opposingField = neighbourFields.FirstOrDefault(f => f.Position.X == parameters.EdgeIndex && f.Position.Z == field.Position.Z);
                                    break;

                                case Direction.Top:
                                    opposingField = neighbourFields.FirstOrDefault(f => f.Position.X == field.Position.X && f.Position.Z == 0);
                                    break;

                                case Direction.Right:
                                    opposingField = neighbourFields.FirstOrDefault(f => f.Position.X == 0 && f.Position.Z == field.Position.Z);
                                    break;

                                case Direction.Bottom:
                                    opposingField = neighbourFields.FirstOrDefault(f => f.Position.X == field.Position.X && f.Position.Z == parameters.EdgeIndex);
                                    break;
                            }

                            if (field.Position.Y != opposingField.Position.Y)
                            {
                                var difference = field.Position.Y - opposingField.Position.Y;

                                Debug.LogFormat("[{0} / {1}] ==> {2} - {3} => {4}", chunk.Position, neighbour.Position, field.Edges, opposingField.Edges, difference);
                            }
                        }
                    }
                }
            }

            return chunks;
        }
    }
}
