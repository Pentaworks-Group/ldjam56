using System;

using Assets.Scripts.Model;

namespace Assets.Scripts.Extensions
{
    public static class EdgeSideExtensions
    {
        public static Direction ToDirection(this EdgeSide edgeSide)
        {
            switch (edgeSide)
            {
                case EdgeSide.Left: return Direction.Left;
                case EdgeSide.Top: return Direction.Top;
                case EdgeSide.Right: return Direction.Right;
                case EdgeSide.Bottom: return Direction.Bottom;

                case EdgeSide.TopLeft:
                case EdgeSide.TopRight:
                case EdgeSide.BottomRight:
                case EdgeSide.BottomLeft:
                    throw new NotSupportedException("Cant convert Edge to direction due to being a corner!");

                case EdgeSide.None:
                default:
                    throw new NotSupportedException("Unsupported Edges");
            }
        }

        public static EdgeSide Opposing(this EdgeSide edgeSide)
        {
            switch (edgeSide)
            {
                case EdgeSide.Left: return EdgeSide.Right;
                case EdgeSide.Top: return EdgeSide.Bottom;
                case EdgeSide.Right: return EdgeSide.Left;
                case EdgeSide.Bottom: return EdgeSide.Top;

                case EdgeSide.TopLeft:
                case EdgeSide.TopRight:
                case EdgeSide.BottomRight:
                case EdgeSide.BottomLeft:
                    throw new NotSupportedException("Cant get the Opposing side without a direction!");
            }

            throw new Exception("What.The.Hell");
        }

        public static EdgeSide Opposing(this EdgeSide edgeSide, Direction direction)
        {
            switch (edgeSide)
            {
                case EdgeSide.TopLeft:
                    switch (direction)
                    {
                        case Direction.Left: return EdgeSide.TopRight;
                        case Direction.Top: return EdgeSide.BottomLeft;
                        default: throw new NotSupportedException("Cant get the opposing side of a non-adjacent side");
                    }

                case EdgeSide.TopRight:
                    switch (direction)
                    {
                        case Direction.Right: return EdgeSide.TopLeft;
                        case Direction.Top: return EdgeSide.BottomRight;
                        default: throw new NotSupportedException("Cant get the opposing side of a non-adjacent side");
                    }

                case EdgeSide.BottomRight:
                    switch (direction)
                    {
                        case Direction.Right: return EdgeSide.BottomLeft;
                        case Direction.Bottom: return EdgeSide.TopRight;
                        default: throw new NotSupportedException("Cant get the opposing side of a non-adjacent side");
                    }

                case EdgeSide.BottomLeft:
                    switch (direction)
                    {
                        case Direction.Left: return EdgeSide.BottomRight;
                        case Direction.Bottom: return EdgeSide.TopLeft;
                        default: throw new NotSupportedException("Cant get the opposing side of a non-adjacent side");
                    }

                default:
                    return edgeSide.Opposing();
            }

            throw new Exception("What.The.Hell");
        }
    }
}
