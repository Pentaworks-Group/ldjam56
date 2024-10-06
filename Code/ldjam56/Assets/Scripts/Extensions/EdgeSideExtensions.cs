using System;

using Assets.Scripts.Model;

namespace Assets.Scripts.Extensions
{
    public static class EdgeSideExtensions
    {
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
                        case Direction.Left: return EdgeSide.Right;
                        case Direction.Top: return EdgeSide.Bottom;
                        default: throw new NotSupportedException("Cant get the opposing side of a non-adjacent side");
                    }
                    
                case EdgeSide.TopRight:
                    switch (direction)
                    {
                        case Direction.Right: return EdgeSide.Left;
                        case Direction.Top: return EdgeSide.Bottom;
                        default: throw new NotSupportedException("Cant get the opposing side of a non-adjacent side");
                    }

                case EdgeSide.BottomRight:
                    switch (direction)
                    {
                        case Direction.Right: return EdgeSide.Left;
                        case Direction.Bottom: return EdgeSide.Top;
                        default: throw new NotSupportedException("Cant get the opposing side of a non-adjacent side");
                    }

                case EdgeSide.BottomLeft:
                    switch (direction)
                    {
                        case Direction.Left: return EdgeSide.Right;
                        case Direction.Bottom: return EdgeSide.Top;
                        default: throw new NotSupportedException("Cant get the opposing side of a non-adjacent side");
                    }

                default:
                    return edgeSide.Opposing();
            }

            throw new Exception("What.The.Hell");
        }
    }
}
