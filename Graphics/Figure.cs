using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using WallE.Graphics;

namespace WallE.FigureGraphics
{
    public class FigureBase
    {
        public FigureBase()
        {
            Tag = "";
            Color = GraphicColors.black;
        }

        public FigureBase(string tag, GraphicColors color)
        {
            Tag = tag;
            Color = color;
        }

        public virtual int Dimension()
        {
            return 0;
        }

        public string Tag {get;set;}

        public GraphicColors Color { get; set;}
    }

    public class Point : FigureBase, IEquatable<Point>
    {
        public float X {private set; get; }
        
        public float Y {private set; get; }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override int Dimension() => 0;

        public bool Equals(Point other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public Point Inverse() => new Point(-X,-Y);

        public static Point operator + (Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator - (Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

    }

    public class Line : FigureBase
    {
        public Point Point1 {private set; get;}
        public Point Point2 {private set; get;}

        public Line(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public Line( float x1, float y1, float x2, float y2)
        {
            Point1 = new Point(x1, y1);
            Point2 = new Point(x2, y2);
        }

        public override int Dimension() => 1;
    }

    public class Ray : Line
    {
        public enum Extends { Point1, Point2}

        public Ray(Point point1, Point point2) : base(point1, point2)
        {
        }  

        public Ray(float x1, float y1, float x2, float y2, Extends extends) : base(x1, y1, x2, y2)
        {
        }
    }

    public class Segment : Line
    {
        public Segment(Point point1, Point point2) : base(point1, point2)
        {
        }

        public Segment(float x1, float y1, float x2, float y2) : base(x1, y1, x2, y2)
        {
        }

        private float distance;

        public float Distance 
        {
            private set
            { 
                distance = MathF.Sqrt( MathF.Pow(Point1.X - Point2.X, 2) + MathF.Pow(Point1.Y - Point2.Y, 2) );    
            } 

            get {return distance; }
        }

    }

    public class Circle : FigureBase
    {
        public Point Center {private set; get;}

        public float Radio {private set; get;}

        public Circle(Point center, float radio)
        {
            Center = center;
            Radio = radio;
        }
        
        public Circle(Point center, Point pointOfTheCircumference)
        {
            Center = center;
            Radio = new Segment(center, pointOfTheCircumference).Distance;
        }

        public Circle(Segment diameter)
        {
            Center = new Point( (diameter.Point1.X + diameter.Point2.X)/2, (diameter.Point1.Y + diameter.Point2.Y)/2 );
            Radio = new Segment(Center, diameter.Point1).Distance;
        }

        public override int Dimension() => 2;
    }

    public class Arc: FigureBase
    {
        public Point Center {private set; get;}

        public float Radio {private set; get;}

        public float StarAngle {private set; get;}
    
        public float EndAngle {private set; get;}

        public Arc(Point center, float radio, float startAngle, float endAngle)
        {
            Center = center;
            StarAngle = startAngle;
            EndAngle = endAngle;
            Radio = radio;
        }
        
        public Arc(Point center, Point point1, Point point2, float radio)
        {
            Center = center;
            Radio = radio;

            StarAngle = GetAngle(point1);
            EndAngle =  GetAngle(point2);

            Point traslade1 = point1 - center;
            Point traslade2 = point2 - center;

            if(Cuadrante(traslade1) == 2 || Cuadrante(traslade1) == 3)
                StarAngle += MathF.PI;
            
            if(Cuadrante(traslade2) == 2 || Cuadrante(traslade2) == 3)
                EndAngle += MathF.PI;

            if( EndAngle < StarAngle)
                EndAngle += 2*MathF.PI;

            //----------------------------------//
            float GetAngle(Point point)
            {
                if(center.X == point.X)
                {
                    if(center.Y < point.Y)
                        return MathF.PI/2;
                    else if (center.Y > point.Y)
                        return 3*MathF.PI/2;
                    else
                        return 0;
                }

                if(center.Y == point.Y)
                    return MathF.PI;  

                return MathF.Atan( (center.Y - point.Y)/(center.X - point.X));
            }

            int Cuadrante(Point point)
            {
                if(point.X > 0 && point.Y > 0)
                    return 1;
                
                if(point.X < 0 && point.Y > 0)
                    return 2;
                
                if(point.X < 0 && point.Y < 0)
                    return 3;

                if(point.X > 0 && point.Y < 0)
                    return 4;
                
                return 0;
            }
        }

    }
}