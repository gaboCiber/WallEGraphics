
  
  public class Internal_Calculus {
  
  public static double Calculate_Parameter( Parametric_Line line1, Cartesian_Line line2 ) {  
                                                                                    
    if( Parallels( line1, line2 ) ) return 0 ;

    double term= line1.P.X*line2.X + line1.P.Y*line2.Y+ line2.Term ;
    double coeficient= line1.V.X*line2.X + line1.V.Y*line2.Y ;

    return (term/coeficient)*(-1) ;

   }

   public static List<double> Calculate_Parameter( Parametric_Line line, Cartesian_Circle circle ) {

    double term= Math.Pow(line.P.X, 2) + Math.Pow(line.P.Y, 2) + circle.x*line.P.X + circle.y*line.P.Y + circle.term; 
    double lineal= 2*line.P.X*line.V.X + 2*line.P.Y*line.V.Y + circle.x*line.V.X + circle.y*line.V.Y;
    double cuadratic= Math.Pow( line.V.X, 2) + Math.Pow( line.V.Y, 2) ;

    Polinomial2 p= new Polinomial2( term, lineal, cuadratic) ;
    return p.Solve();
    
   }


   public static bool Parallels( Parametric_Line line1, Cartesian_Line line2 ) {  

    return line1.V.X*line2.X + line1.V.Y*line2.Y == 0 ;
   }


   public static Point Calculate_Point( Parametric_Line line, double t ) {

    return new Point( line.P.X+ line.V.X*t, line.P.Y+ line.V.Y*t ) ;
   }


   public static Point Calculate_Intersection( Parametric_Line line1, Cartesian_Line line2) {
    
    if( Parallels( line1, line2)) return null;
     return Calculate_Point( line1, Calculate_Parameter( line1, line2 ) );

   }

   public static List<Point> Calculate_Intersection( Parametric_Line line, Cartesian_Circle circle ) {

    if( Distance(line, circle.center)> circle.radio ) return null;
    return Calculate_Points( line, Calculate_Parameter( line, circle ) );

   }

   public static List<Point> Calculate_Points( Parametric_Line line, List<double> parameters ) {

     var result= new List<Point>();
     for( int i=0; i< parameters.Count; i++) 
      result.Add( Calculate_Point( line, parameters[i] ) );

      return result;

   }

   public static double Distance( Parametric_Line line, Point p) {
     
     if( line.Is_Satisficed_By(p) ) return 0;
     var aux= new Parametric_Line( line.V.Get_Ortogonal(), p );
     var ortogonal= Transform( aux);
     var intersection_point= Calculate_Intersection( line, ortogonal);
     return p.Distance( intersection_point) ;

   } 

   public static Cartesian_Line Transform( Parametric_Line line ) {

     var p= new Point( line.P.X + line.V.X, line.P.Y + line.V.Y ) ;
     return new Cartesian_Line( line.P, p);

   }

  
  }


 public abstract class Ecuation {

  public abstract bool Is_Satisficed_By( Point p);
  public abstract List<double> Obtain_Y_Value( double arg);

 }
 
 public class Cartesian_Line: Ecuation {   

  public double X ; 
  public double Y ;
  public double Term ;  
  public Point P1 ;  
  public Point P2 ;

  public Cartesian_Line( Point p1, Point p2 ) {
   
   if( p1.X==p2.X ) {  
   X= 1 ;
   Y= 0 ;
   Term= p1.X*(-1) ;

   }
   else {              
   X= ( p1.Y- p2.Y )/( p1.X - p2.X) ;    
   Y= -1 ;                               
   Term= p1.Y- X*p1.X ;            

   }
   P1= p1 ;
   P2= p2 ;
   }

   public override bool Is_Satisficed_By( Point p ) { return ( X*p.X + Y*p.Y + Term ) == 0;  }

   public override List<double> Obtain_Y_Value( double arg) {  
    
    var result= new List<double>();
    result.Add( (-1)*(arg*X + Term ) );
     return result;
     
   }

  }


 public class Parametric_Line: Ecuation {   

  public Vector V ;
  public Point P ;

  public Parametric_Line( Vector director, Point p ) {

    V= director ;
    P= p ;
  }

  public Parametric_Line( Point p1, Point p2 ) {

    V= new Vector( p1, p2);
    P= p1;

  }

  public override bool Is_Satisficed_By( Point p ) {

    double parameter= ( p.X - P.X)/ V.X;
      return P.Y + V.Y*parameter == p.Y;

   }

   public override List<double> Obtain_Y_Value( double arg) {  

    var result= new List<double>();
    double t= (arg - P.X)/ V.X;
    result.Add( P.Y + V.Y* t) ;
    return result;

   }


 }

 public class Vector {   

  public double X ;
  public double Y ;
  public Vector( double x, double y ) {

    X= x ;
    Y= y ;
  }

  public Vector( Point p1, Point p2 ) {

    X= p2.X - p1.X;
    Y= p2.Y - p1.Y;

  }

  public Vector Get_Ortogonal() { return new Vector( Y, (-1)*X ); }

  public double Angulo( Vector other ) {

   double cos= ( X*other.X + Y*other.Y) / ( Norm() * other.Norm() );
   return Math.Acos( cos);

  }

  public double Norm() { return Math.Sqrt( Math.Pow( X,2) + Math.Pow( Y,2) ); }

 }

 public class Cartesian_Circle: Ecuation {
  
  public double x_2;
  public double y_2;
  public double x ; 
  public double y ;
  public double term;  
  public Point center; 
  public double radio;

  public Cartesian_Circle( Point center, double radio) {

    this.center= center;
    this.radio= radio;
    x_2= 1;
    y_2= 1;
    x= center.Y*(-2);
    y= center.Y*(-2);
    term= Math.Pow( center.X, 2) + Math.Pow( center.Y, 2) - Math.Pow( radio, 2);

  }

  public override bool Is_Satisficed_By( Point p ) { return Math.Pow( p.X, 2) + Math.Pow( p.Y, 2) + x*p.X + y*p.Y + term == 0  ; }
  
  public override List<double> Obtain_Y_Value( double arg) { 
    
    double indep= Math.Pow( arg, 2) + x*arg + term;
    var p= new Polinomial2( indep, y, y_2 );
    return p.Solve();
  }

 }


 public abstract class Polinomial {

   public abstract List<double> Solve();

 } 

 public class Polinomial2: Polinomial {
    
    public double term;
    public double lineal;
    public double cuadratic;

    public Polinomial2( double term, double lineal, double cuadratic ) {

     this.term= term;
     this.lineal= lineal;
     this.cuadratic= cuadratic;

    } 

    public override List<double> Solve() {

     var result= new List<double>();

     double discriminant= Math.Pow( lineal, 2) - 4*cuadratic*term;
     if( discriminant==0)  result.Add(lineal*(-1)/2*cuadratic);

     if( discriminant>0) {

        result.Add( ( lineal*(-1) + Math.Sqrt(discriminant) )/2*cuadratic );
        result.Add( ( lineal*(-1) - Math.Sqrt(discriminant) )/2*cuadratic );
     }

     return result;

    }
 }


 public class Intervale {

   public double Sup;
   public double Inf;
   public bool Is_Singleton { get { return Left_Acotated && Right_Acotated && Sup==Inf; } }
   public bool Left_Acotated { get; private set;}
   public bool Right_Acotated { get; private set;}


   public Intervale() {

    Left_Acotated= false;
    Right_Acotated= false;
   }

   public Intervale( double x1, double x2 ) {

    Inf= Math.Min(x1, x2);
    Sup= Math.Max(x1, x2);

   }

   public Intervale( double x, bool is_supreme ) {

    if(is_supreme) {
      Sup= x;
      Right_Acotated= true;
     }

     else {
      Inf= x;
      Left_Acotated= true;
     }

   }

   public IEnumerator<double> GetEnumerator() {

    if( Is_Singleton ) while(true) yield return Inf;
    else if( !Left_Acotated && !Right_Acotated ) {

      var ra= new Random();
      while(true) yield return ra.Next( -100, 100);
    }
    else {

      var r= new Random();
      double inf= (Left_Acotated) ? Inf : Sup-100;
      double sup= (Right_Acotated) ? Sup : Inf+ 100;
      while(true) yield return r.Next( (int)Inf, (int)Sup );

    }

   }

   public bool Includes( Point p) { 

    var list= new List<Point>();
    list.Add(p);
    Utils.Filter( list, this);
    return list.Count==1;
   }

 }

 

 
 