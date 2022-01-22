﻿using Microsoft.Xna.Framework;
using System;

namespace TSMapEditor.GameMath
{
    public struct Point2D
    {
        public int X;
        public int Y;

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point2D operator +(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point2D operator -(Point2D p1, Point2D p2)
        {
            return new Point2D(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point2D FromXNAPoint(Point point)
            => new Point2D(point.X, point.Y);

        public static Point2D Zero => new Point2D(0, 0);

        public Vector2 ToXNAVector() => new Vector2(X, Y);

        public override int GetHashCode()
        {
            return Y * 1000 + X;
        }

        public override string ToString()
        {
            return X + ", " + Y;
        }

        public static bool operator !=(Point2D p1, Point2D p2)
        {
            return !(p1 == p2);
        }

        public static bool operator ==(Point2D p1, Point2D p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public float Angle()
        {
            return (float)Math.Atan2(Y, X);
        }
    }
}
