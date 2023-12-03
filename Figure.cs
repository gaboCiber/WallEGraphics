using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace WallE
{
    public class IFigure
    {
        public IFigure()
        {
            Tag = "";
            Color = Colors.Black;
        }

        public IFigure(string tag, Godot.Color color)
        {
            Tag = tag;
            Color = color;
        }

        public virtual int Dimension()
        {
            return 0;
        }

        public string Tag {get;set;}

        public Godot.Color Color { get; set;}
    }

    public class Point : IFigure, IEquatable<Point>
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
    }

    public class Line : IFigure
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

        public Extends Extension {private set; get;}

        public Ray(Point point1, Point point2, Extends extends) : base(point1, point2)
        {
            Extension = extends;
        }  

        public Ray(float x1, float y1, float x2, float y2, Extends extends) : base(x1, y1, x2, y2)
        {
            Extension = extends;
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

    public class Circle : IFigure
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

    public class Arc: IFigure
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
        
        // public Arc(Point center, Segment radio1, Segment radio2)
        // {
        //     if(radio1.Distance != radio2.Distance)
        //         throw new Exception("The segments of an arc must have the same length");

        //     Center = center;
        //     Radio1 = radio1;
        //     Radio2 = radio2;
        // }

        // public Arc(Ray ray1, Ray ray2, float length)
        // {
        //     if(ray1.Point1.Equals(ray2.Point1))
        //     {
        //         Center = ray1.Point1;
        //         Radio1 = new Segment(Center, ray1.Point2);
        //         Radio2 = new Segment(Center, ray2.Point2);
        //     }
        //     else if(ray1.Point1.Equals(ray2.Point2))
        //     {
        //         Center = ray1.Point1;
        //         Radio1 = new Segment(Center, ray1.Point2);
        //         Radio2 = new Segment(Center, ray2.Point1);
        //     }
        //     else if(ray2.Point1.Equals(ray1.Point1))
        //     {
        //         Center = ray2.Point2;
        //         Radio1 = new Segment(Center, ray2.Point2);
        //         Radio2 = new Segment(Center, ray1.Point2);
        //     }
        //     else if(ray2.Point1.Equals(ray1.Point2))
        //     {
        //         Center = ray2.Point1;
        //         Radio1 = new Segment(Center, ray2.Point2);
        //         Radio2 = new Segment(Center, ray1.Point1);
        //     }
        //     else
        //         throw new Exception("The ray must have a common point"); 
        

    }
}