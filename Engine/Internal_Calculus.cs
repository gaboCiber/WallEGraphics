
 
    public class Internal_Calculus {

      public static List<Point> FindLineLineIntersections(Line line1, Line line2)
    {
        List<Point> intersectionPoints = new List<Point>();

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

    public IEnumerator<int> GetEnumerator() {

      int cursor= Min;
      while( cursor<=Max) {
   
        yield return cursor;
        cursor++;
      }

    }

  }

 


 

 
 