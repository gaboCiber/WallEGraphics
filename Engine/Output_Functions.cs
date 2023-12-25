
 using System;
 using System.Collections.Generic;
using System.Linq;

public abstract class Figure {

    public abstract List<Point> Get_Intersection( Figure other);
    public abstract void Print();
    public string color;
    public void Assign_Color( string color) {  this.color= color; }

   }


    public class Point: Figure
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point( double x, double y) {

          X= x;
          Y= y;

        }

        public Point() {

         var r= new Random();
         X= r.Next(-100, 100) ;
         Y= r.Next(-100, 100) ;
    
        }


       public override void Print() {

       Console.WriteLine( "Point : X={0}, Y={1}", X, Y);
       Console.WriteLine( "color: {0}", color);

      }

     public double Distance( Point other ) {  
      
      return Math.Sqrt( Math.Pow( X-other.X, 2 ) + Math.Pow( Y- other.Y, 2 ) ) ;

    }

    public override List<Point> Get_Intersection( Figure other) { return null; }

   }

    public class Line: Figure
    {
        public double Slope { get; set; }
        public double Intercept { get; set; }
        public Point P1 { get; set; }
        public Point P2 { get; set; }


        public Line(double slope, double intercept)
        {
            Slope = slope;
            Intercept = intercept;
        }

        public Line( Point p1, Point p2) {

         double slope; 
         double intercept;
         FindSlopeAndIntercept( p1, p2, out slope, out intercept );
         Slope= slope;
         Intercept= intercept;
         P1= p1;
         P2= p2;

        }

        public Line() {

          P1= new Point();
          P2= new Point();
          double slope; 
          double intercept;
          FindSlopeAndIntercept( P1, P2, out slope, out intercept );
          Slope= slope;
          Intercept= intercept;

        }

        public override void Print() { 
    
         Console.WriteLine( "Line : ");
         P1.Print();
         P2.Print();
         Console.WriteLine( "color: {0}", color);
         Console.Write("\n");

       }


      public static void FindSlopeAndIntercept(Point point1, Point point2, out double slope, out double intercept)
      {
         if (point1.X == point2.X)
         {
             throw new ArgumentException("Los puntos no pueden tener la misma coordenada x");
         }

         slope = (point2.Y - point1.Y) / (point2.X - point1.X);
         intercept = point1.Y - slope * point1.X;
     }

      public override List<Point> Get_Intersection( Figure other ) {
    
        if(other is Line) return Internal_Calculus.FindLineLineIntersections( this, (Line)other ); 
        if( other is Circle) return Internal_Calculus.FindCircleLineIntersections( (Circle)other, this ); 
  
        return null;

     }

   }


 public class Circle: Figure
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }
        public Point center;
        public Measure radio; 

        public Circle( Point center, Measure measure ) {

          X= center.X;
          Y= center.Y;
          Radius= measure.Get_Distance();
          this.center= center; 
          radio= measure;

        }

         public Circle() {

         center= new Point();
         radio= new Measure();
         X= center.X;
         Y= center.Y;
         Radius= radio.Get_Distance();
    
          }

        public override List<Point> Get_Intersection( Figure other ) {
    
         if(other is Line) return Internal_Calculus.FindCircleLineIntersections( this, (Line)other ); 
         if( other is Circle) return Internal_Calculus.FindCircleCircleIntersections( (Circle)other, this ); 
  
        return null;

       }

       public override void Print() {

       Console.WriteLine( "Circle :");
       center.Print();
       Console.WriteLine( "radio : {0}", radio.Get_Distance() );
       Console.WriteLine( "color: {0}", color);

      }

    }


  public class Measure: Figure {

  public Point p1;
  public Point p2;

  public Measure( Point p1, Point p2) {

    this.p1= p1;
    this.p2= p2;
    
  }

  public Measure() {

    p1= new Point();
    p2= new Point();

  }

  public double Get_Distance() { return p1.Distance(p2); }

  public override List<Point> Get_Intersection( Figure fig ) { return null; }
  public override void Print() { Console.WriteLine( Get_Distance()); }

 }


  public class Arc : Figure {

  public Point center;
  public Point initial;
  public Point final;
  public Measure radio;
  public string color;

  public Arc( Point center, Point initial, Point final, Measure radio ) {

    this.center= center;
    this.initial= initial;
    this.final= final;
    this.radio= radio;

  }

  public Arc() {

    center= new Point();
    initial= new Point();
    final= new Point();
    radio= new Measure();
    
   }

   public override void Print() { 
    
         Console.WriteLine( "Arc : ");
         center.Print();
         initial.Print();
         final.Print();
         Console.WriteLine( "radio : {0}", radio.Get_Distance() );
         Console.WriteLine( "color: {0}", color);
         Console.Write("\n");

        }

    public override List<Point> Get_Intersection( Figure other ) {  return null; } 

  }

   

   public class Rect: Line {

   public Rect( Point p1, Point p2) : base(p1, p2 ) {}
   public Rect() : base() {}

 }

 public class Segment: Line {

   public Segment( Point p1, Point p2) : base(p1, p2 ) {}
   public Segment() : base() {}

   public override List<Point> Get_Intersection( Figure other ) {

     var points= base.Get_Intersection( other);
     double max= ( P1.X>= P2.X ) ? P1.X : P2.X; 
     double min= ( P1.X<= P2.X ) ? P1.X : P2.X;
     int cursor= 0;

     while( cursor< points.Count ) {

      if( points[cursor].X > max || points[cursor].X < min ) points.RemoveAt(cursor);
      else cursor++;

     }

     return points;

    }

  }


 public class Ray: Line {

   public Ray( Point p1, Point p2) : base(p1, p2 ) {}
   public Ray() : base() {}

   public override List<Point> Get_Intersection( Figure other ) {

    var points= base.Get_Intersection( other);
    bool min= false;
    double cota= P1.X;
    if( P1.X <= P2.X ) min= true;
    var result= new List<Point>();
    IEnumerable<Point> ienumerable= ( min) ? points.Where( x=> x.X>= cota) : points.Where( x=> x.X<= cota );

    foreach( var p in ienumerable) 
     result.Add(p);

     return result;

   }

 }

 





 
  
    
 