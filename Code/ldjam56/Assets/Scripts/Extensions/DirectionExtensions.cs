using System;

using Assets.Scripts.Model;

namespace Assets.Scripts.Extensions
{
    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                default: throw new Exception("What.The.Hell");
                case Direction.Left: return Direction.Right;
                case Direction.Top: return Direction.Bottom;
                case Direction.Right: return Direction.Left;
                case Direction.Bottom: return Direction.Top;
            }
        }

        public static EdgeSide ToEdge(this Direction direction)
        {
            switch (direction)
            {
                default: throw new Exception("What.The.Hell");
                case Direction.Left: return EdgeSide.Left;
                case Direction.Top: return EdgeSide.Top;
                case Direction.Right: return EdgeSide.Right;
                case Direction.Bottom: return EdgeSide.Bottom;
            }
        }

        public static Direction Right(this Direction direction)
        {
            switch (direction)
            {
                default: throw new Exception("What.The.Hell");
                case Direction.Left: return Direction.Top;
                case Direction.Top: return Direction.Right;
                case Direction.Right: return Direction.Bottom;
                case Direction.Bottom: return Direction.Left;
            }
        }

        public static Direction Left(this Direction direction)
        {
            switch (direction)
            {
                default: throw new Exception("What.The.Hell");
                case Direction.Left: return Direction.Bottom;
                case Direction.Top: return Direction.Left;
                case Direction.Right: return Direction.Top;
                case Direction.Bottom: return Direction.Right;
            }
        }        
    }
}
