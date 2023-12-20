
 public abstract class Figure {

   public abstract Ecuation Get_Ecuation(); 
   public abstract Intervale Get_Dom_Range();
   public abstract List<Point> Get_Intersection( Figure other);
   public abstract List<double> Get_Y_Value( double x);
   public abstract bool Contains( Point p);
   public abstract void Print();
   public string color;
 }
 
 public class Line: Figure {

  public Point P1;
  public Point P2;

  public Line( Point p1, Point p2 ) {

    P1= p1;
    P2= p2;
    color= Semantik_Analysis.Context.Get_Color();
  }

  public Line() {

    P1= new Point();
    P2= new Point();
    color= Semantik_Analysis.Context.Get_Color();
  }

  public override Ecuation Get_Ecuation() { return new Parametric_Line( P1, P2) ; }

  public Cartesian_Line Get_Cartesian_Ecuation() { return new Cartesian_Line( P1, P2); }

  public override List<Point> Get_Intersection( Figure other ) {
    
    var list= new List<Point>();
    if(other is Line) { list.Add( Internal_Calculus.Calculate_Intersection( (Parametric_Line)other.Get_Ecuation(), this.Get_Cartesian_Ecuation() )); }
   
    list= (other is Line)? list : Internal_Calculus.Calculate_Intersection( (Parametric_Line)Get_Ecuation(), (Cartesian_Circle)other.Get_Ecuation() ); 
    var range= Get_Dom_Range();
    Utils.Filter( list, range );
     return list;

  }

  public override Intervale Get_Dom_Range() { return new Intervale();  }

  public override bool Contains( Point p) { 

    return Get_Ecuation().Is_Satisficed_By( p) && Get_Dom_Range().Includes( p);
   }

  public override List<double> Get_Y_Value( double x) {

    if( Get_Dom_Range().Includes( new Point(x, 1))) return Get_Ecuation().Obtain_Y_Value(x);
    return new List<double>();

  }

  public override void Print() { 
    
    Console.WriteLine( "Line : ");
    P1.Print();
    P2.Print();
    Console.WriteLine( "color: {0}", color);
    Console.Write("\n");

  }


 }

public class Rect: Line {

   public Rect( Point p1, Point p2) : base(p1, p2 ) { Console.WriteLine("creating_rect"); }
   public Rect() : base() {}
 }

 public class Segment: Line {

   public Segment( Point p1, Point p2) : base(p1, p2 ) {}
   
   public override Intervale Get_Dom_Range() { return new Intervale( P1.X, P2.X); }
   public Segment() : base() {}

 }

 public class Ray: Line {

   public Ray( Point p1, Point p2) : base(p1, p2 ) {}

   public override Intervale Get_Dom_Range() { return new Intervale(  P1.X, P1.X> P2.X ); }
   public Ray() : base() {}
 }

 public class Point: Figure {

  public double X;
  public double Y;
  
  public Point() {

    var r= new Random();
    X= r.Next(-100, 100) ;
    Y= r.Next(-100, 100) ;
    color= Semantik_Analysis.Context.Get_Color();
  }

  public Point( double x, double y) {

    X= x;
    Y= y;
    color= Semantik_Analysis.Context.Get_Color();
    Console.WriteLine("creating_point...");
  }

   public override bool Equals( object other ) {
    
    if( !( other is Point ) ) return false ;
    Point p= (Point)other ;
    return this.X==p.X && this.Y==p.Y ; 

  }

  public double Distance( Point other ) {  
      
      return Math.Sqrt( Math.Pow( X-other.X, 2 ) + Math.Pow( Y- other.Y, 2 ) ) ;
  }

  public override Ecuation Get_Ecuation() { return null; }

  public override Intervale Get_Dom_Range() { return new Intervale( X, X );  }

  public override List<Point> Get_Intersection( Figure other ) { 

    var result= new List<Point>();
    if( other.Contains(this) ) result.Add(this); 

    return result;
    
   } 

   public override bool Contains( Point p) { return Equals(p); }

   public override List<double> Get_Y_Value( double x) {
    
    var result= new List<double>();
    if( X== x) result.Add( Y);
    return result;

   }

   public override void Print() {

    Console.WriteLine( "Point : X={0}, Y={1}", X, Y);
    Console.WriteLine( "color: {0}", color);

   }

 }


 public class Circle: Figure {

  public Point center;
  public Measure radio;

  public Circle( Point p1, Measure radio) {

    center= p1;
    this.radio= radio;
    this.color= Semantik_Analysis.Context.Get_Color();
    Console.WriteLine("Creating_circle");
  }

  public Circle() {

    center= new Point();
    radio= new Measure();
    this.color= Semantik_Analysis.Context.Get_Color();
  }

  public override Intervale Get_Dom_Range() { return new Intervale( center.X- radio.Get_Distance(), center.X+ radio.Get_Distance()); }
  public override Ecuation Get_Ecuation() { return new Cartesian_Circle( center, radio.Get_Distance() ) ; }

  public override List<Point> Get_Intersection( Figure other ) {
     
     return other.Get_Intersection( this);

  }

  public override List<double> Get_Y_Value( double x) { 

    if( Get_Dom_Range().Includes( new Point(x,1)) ) return ((Cartesian_Circle)Get_Ecuation()).Obtain_Y_Value(x);
    return new List<double>(); 

   }

   public override bool Contains( Point p) {

     return ((Cartesian_Circle)Get_Ecuation()).Is_Satisficed_By( p);
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
    Console.WriteLine( "Creating_measure...");
  }

  public Measure() {

    p1= new Point();
    p2= new Point();

  }

  public double Get_Distance() { return p1.Distance(p2); }

  public override Ecuation Get_Ecuation() { return null;}
  public override Intervale Get_Dom_Range() { return null; } 
  public override bool Contains(Point p ) { return false; }
  public override List<Point> Get_Intersection( Figure fig ) { return null; }
  public override List<double> Get_Y_Value( double x) { return new List<double>(); }
  public override void Print() { Console.WriteLine( Get_Distance()); }
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

  public class Arc {

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
    color= Semantik_Analysis.Context.Get_Color();

  }

  public Arc() {

    center= new Point();
    initial= new Point();
    final= new Point();
    radio= new Measure();
    color= Semantik_Analysis.Context.Get_Color();
  }

  }