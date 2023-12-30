
 
    public class Internal_Calculus {

      public static List<Point> FindLineLineIntersections(Line line1, Line line2)
    {
        List<Point> intersectionPoints = new List<Point>();
         
        if( line1.Ortogonal || line2.Ortogonal ) {

         if( line1.Ortogonal && line2.Ortogonal ) return intersectionPoints;

         if( line1.Ortogonal ) intersectionPoints.Add( new Point( line1.P1.X, line2.Slope*line1.P1.X + line2.Intercept ) );
         else intersectionPoints.Add( new Point( line2.P1.X, line1.Slope*line2.P1.X + line1.Intercept ) );
         return intersectionPoints;

        }

        if (line1.Slope == line2.Slope)
        {
            // Las rectas son paralelas, no tienen puntos de intersección
            return intersectionPoints;
        }

        double x = (line2.Intercept - line1.Intercept) / (line1.Slope - line2.Slope);
        double y = line1.Slope * x + line1.Intercept;

        Point intersectionPoint = new Point { X = x, Y = y };
        intersectionPoints.Add(intersectionPoint);

        return intersectionPoints;
    }

  

       public static List<Point> FindCircleCircleIntersections(Circle circle1, Circle circle2)
    {  

       var list= new List<Point>();
        // Calcula la distancia entre los centros de las circunferencias
        double distance = Math.Sqrt(Math.Pow(circle2.X - circle1.X, 2) + Math.Pow(circle2.Y - circle1.Y, 2));

        // Verifica si las circunferencias no se intersectan
        if (distance > circle1.Radius + circle2.Radius || distance < Math.Abs(circle1.Radius - circle2.Radius))
        {
            return null; // No hay intersección
        }

        // Calcula los puntos de intersección
        double a = (Math.Pow(circle1.Radius, 2) - Math.Pow(circle2.Radius, 2) + Math.Pow(distance, 2)) / (2 * distance);
        double h = Math.Sqrt(Math.Pow(circle1.Radius, 2) - Math.Pow(a, 2));
        
        double x2 = circle1.X + a * (circle2.X - circle1.X) / distance;
        double y2 = circle1.Y + a * (circle2.Y - circle1.Y) / distance;

        Point intersection1 = new Point { X = x2 + h * (circle2.Y - circle1.Y) / distance, Y = y2 - h * (circle2.X - circle1.X) / distance };
        Point intersection2 = new Point { X = x2 - h * (circle2.Y - circle1.Y) / distance, Y = y2 + h * (circle2.X - circle1.X) / distance };
         
        list.Add( intersection1);
        list.Add( intersection2);
        return list;
    }


    public static List<Point> FindCircleLineIntersections(Circle circle, Line line)
    {  
        if( line.Ortogonal ) return FindCircleLineOrtogonalIntersections( circle, line );

        List<Point> points= new List<Point>();

        double a = 1 + Math.Pow(line.Slope, 2);
        double b = 2 * (line.Slope * (line.Intercept - circle.Y) - circle.X);
        double c = Math.Pow(circle.X, 2) + Math.Pow(line.Intercept - circle.Y, 2) - Math.Pow(circle.Radius, 2);

        double discriminant = Math.Pow(b, 2) - 4 * a * c;

        if (discriminant < 0)
        {
            return null; // No hay intersección
        }
        
        double x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
        double y1 = line.Slope * x1 + line.Intercept;

        if (discriminant == 0)
        {
            points.Add( new Point { X = x1, Y = y1 } ); // Solo hay un punto de intersección
            return points;
        }

        double x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
        double y2 = line.Slope * x2 + line.Intercept;

        points.Add( new Point { X = x1, Y = y1 } ); 
        points.Add( new Point { X = x2, Y = y2 } );
        return points;
    }


     public static List<Point> FindCircleLineOrtogonalIntersections(Circle circle, Line line) {

       List<Point> points= new List<Point>();
        
        double a = 1;
        double b = 2 * circle.Y * (-1);
        double c = Math.Pow( line.P1.X, 2) - ( 2 * line.P1.X * circle.X ) + Math.Pow( circle.X, 2) + Math.Pow( circle.Y, 2) - Math.Pow(circle.Radius, 2);

        double discriminant = Math.Pow(b, 2) - 4 * a * c;

        if (discriminant < 0)
        {
            return null; // No hay intersección
        }
        
        double y1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
        double x1 = line.P1.X;

        if (discriminant == 0)
        {
            points.Add( new Point { X = x1, Y = y1 } ); // Solo hay un punto de intersección
            return points;
        }

        double y2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
        double x2 = line.P1.X;

        points.Add( new Point { X = x1, Y = y1 } ); 
        points.Add( new Point { X = x2, Y = y2 } );
        return points;

     }


    public static List<Point> FindArcFigIntersections( Arc arc, Figure fig ) {

      var ray1= new  Ray( arc.center, arc.initial);
      var ray2= new Ray( arc.center, arc.final);
      var circle= new Circle( arc.center, arc.radio);

      var i1= ray1.Get_Intersection( fig);
      var i2= ray2.Get_Intersection( fig);
      var i3= circle.Get_Intersection( fig);

      var points= new List<Point>();
      if( i1!= null) {
      foreach( var x in i1 )
      Console.WriteLine(x);}

      if( i2!= null) {
      foreach( var x in i2 )
      Console.WriteLine(x);}

      if( i3!= null) {
      foreach( var x in i3 )
      Console.WriteLine(x);}


      if( i1!= null ) {

      foreach( var x in i1) 
       if(x.Distance( arc.center )<= arc.radio.Get_Distance() ) points.Add(x);

      }

      if( i2!= null ) {

      foreach( var x in i2) 
       if(x.Distance( arc.center )<= arc.radio.Get_Distance() ) points.Add(x);

      }

      if( i3!= null ) {

      foreach( var x in i3) 
       if( x.Is_In_Radar(arc.center, arc.initial, arc.final ) ) points.Add(x);

      }

      if(points.Count!= 0 ) return points;
      return null;


    }

    
  }


 public class Sub_Z {

    public int Min;
    public int Max;
    
    public Sub_Z( int min, int max) {

      Min= min;
      Max= max;

    } 

    public Sub_Z( int min ) {

      Min= min;
      Max= int.MaxValue;
    }

    public IEnumerator<double> GetEnumerator() {

      int cursor= Min;
      while( cursor<=Max) {
   
        yield return (double)cursor;
        cursor++;
      }

    }

  }


  public static class Geometric_Extensions {

   public static bool Is_In_Radar( this Point p, Point center, Point initial, Point final) {

    var v1= new Vector( center, initial );
    var v2= new Vector( center, final );
    var v= new Vector( center, p);

    return v.Angulo( v1 ) + v.Angulo(v2) == v1.Angulo(v2);
     
   }

  }

  public class Vector {   

  public double X ;
  public double Y ;
  
  public Vector( Point p1, Point p2 ) {

    X= p2.X - p1.X;
    Y= p2.Y - p1.Y;

  }

  
  public double Angulo( Vector other ) {

   double cos= ( X*other.X + Y*other.Y) / ( Norm() * other.Norm() );
   return Math.Acos( cos);

  }

  public double Norm() { return Math.Sqrt( Math.Pow( X,2) + Math.Pow( Y,2) ); }

 }

 


 

 
 