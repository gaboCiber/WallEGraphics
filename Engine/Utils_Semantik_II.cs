
 public static class Utils {
   
   
   public static Bool_Object Obtain_Value( this Binary_Operation expr, Context context ) {

    var pair= expr.Left.Evaluate( context );
    if(  !pair.Bool && pair.Object==null ) return new Bool_Object( false, null );
    if( pair.Object is bool || pair.Object is string ) {

      Operation_System.Print_in_Console( "Entre objetos de tipo string o bool no se pueden realizar operaciones de multiplicacion o division ");
      return new Bool_Object( false, null );
    }
    double optr = (double)(pair.Object) ;
    return Obtain_Value( optr, expr.Op, expr.Right, context );

   }


   public static Bool_Object Obtain_Value( double acum, string optr, Expression expr, Context context ) {
    
     var obj= ( expr.Is_Product() ) ? ((Binary_Operation)expr).Left.Evaluate( context ).Object : expr.Evaluate( context).Object ;
     if( obj==null ) return new Bool_Object( false, null);
     if( obj is bool || obj is string ) {

      Operation_System.Print_in_Console( "Las operaciones de producto o division no pueden ser efectuadas entre strings o booleanos" );
      return new Bool_Object( false, null);
     }

     double op= (double)obj;
     double temp= 0;
     switch(optr) {
      case "*":
      temp= acum*op ;
      break;
      case "/":
      temp= acum/op ;
      break;
     }

    if( !expr.Is_Product() ) return new Bool_Object( true, temp );
     
     Binary_Operation aux= (Binary_Operation)expr ;
     return Obtain_Value( temp, aux.Op, aux.Right, context );

   }

   public static bool Is_Product( this Expression expr ) {

    if ( !(expr is Binary_Operation) )  return false;
    string op= ((Binary_Operation)expr).Op ; 
    return op=="*"  || op=="/"  ;

   }

   public static Bool_Object Oposite_Of( this Bool_Object pair ) {

    if( !pair.Bool || pair.Object==null || !(pair.Object is bool) ) {
      
      if( !(pair.Object is bool ) ) Operation_System.Print_in_Console( "Semantik Error: La operacion de negacion solo se aplica a valores booleanos" ) ;
      return new Bool_Object( false, null );
    }
    return new Bool_Object( true, !((bool)pair.Object) ) ;

   }

   public static double Combine_Numbers( double op1, double op2, string op ) {
      
     double result= 0;
     switch( op) {

        case"+": 
        result= op1+op2 ;
        break;
        case"-":
        result= op1-op2 ;
        break;
        case"/":
        result= op1/op2;
        break;
        case"*":
        result= op1*op2;
        break;
        case"^":
        result= Math.Pow( op1, op2) ;
        break;
        case"%":
        result= op1%op2;
        break;

      }

      return result;

   }

    public static string Combine_Strings( this string s1, string s2, string op ) { return s1 + s2; }

    public static double Evaluate_Trigonometrics( double n, string name) {

     double result= 0 ;
      switch( name ) {

       case"sin":
       result= Math.Sin(n) ;
       break ;
       case"cos":
       result= Math.Cos(n) ;
       break ; 
      }

      return result;

    }
    
    public static List<object> Filter( Context context, params Expression[]expr) {
       
       var result= new List<object>();
      for( int i=0; i< expr.Length; i++) {

        var pair= expr[i].Evaluate( context );
        if( !pair.Bool ) return null;
        result.Add( pair.Object);

      }

      return result;
       
    }


    public static List<object> Filter<T>( Context context, T item, bool equals, params Expression[]expr ) {

     var list= Filter( context, expr);
     if( list==null) return null;
     var t= list[0].GetType();
     if(  ( equals && !t.Equals(item.GetType()) )  || ( !equals && t.Equals(item.GetType()) )  ) return null;
     
     for( int i=1; i< list.Count; i++) 
      if( list[i].GetType()!= t ) return null;
     
     return list;

    }

    public static Line Make_Line( Point p1, Point p2, string especification ) {

     Line result= null;
     switch( especification) {

       case "line":
       result= new Rect( p1, p2);
       break;
       case "segment":
       result= new Segment( p1, p2);
       break;
       case "ray":
       result= new Ray( p1, p2);
       break;
       default:
       break;
     }

     return result;

    }


   public static bool Interprete( object obj ) { 
    
    return !( obj== null || ( (obj is Secuence) && ((Secuence)obj).Is_Empty() ) || ( (obj is double) && ((double)obj)==0  ) );
    
     }

   public static bool Is_Predeterm( this string function, Context context, int args) { return context.Is_Predeterm( function, args);  }
  
    public static bool Same_Type( this object obj, object other ) {

   if( ( obj is string && other is string ) || ( obj is double && other is double ) || ( obj is bool && other is bool ) ) return true ;
   else return false ;
   
  }

  public static bool Is_Posible_Secuence( Expression expr) { return (expr is ID) || (expr is Secuence) || (expr is Func_Call) ; }
  
  public static void Filter( List<Point> list, Intervale range ) {
    
    var temp= list;
    if( range.Left_Acotated ) Filter( temp, range.Inf, "inf" );
    if( range.Right_Acotated) Filter( temp, range.Sup, "sup");
    
  }

  public static void Filter( List<Point> list, double x, string s ) {
    
    int cursor= 0;
    if( s=="sup") 
     while( cursor< list.Count) {

      if( list[cursor].X> x)  list.RemoveAt(cursor);
       else cursor++;

    }
    else 
     while( cursor< list.Count) {

      if( list[cursor].X< x)  list.RemoveAt(cursor);
       else cursor++;
    }
    
  }

  public static void Filter( List<Point> list, Vector v1, Vector v2, Point center ) {

    int cursor= 0;
    while( cursor< list.Count ) {

     Vector vector= new Vector(center, list[cursor]);
     if( vector.Angulo( v1) + vector.Angulo( v2)== v1.Angulo( v2) ) cursor++;
     else list.RemoveAt(cursor);

    }
    
  }

  public static List<string> Filter( this List<ID> list ) {
   
    var result= new List<string>();
    for( int i=0; i< list.Count; i++)
     result.Add( list[i].Name);

     return result;
  }

  public static Sub_Secuence Take( Secuence secuence, int ignored ) {

    return new Sub_Secuence( secuence, ignored );
  }

  public static Abstract_Figure Get_Figure( string fig, List<Expression> expr) {
    
    Abstract_Figure result= new Abstract_Point( expr);
    switch( fig) {
    
     case "line": case "segment": case "ray":
     result= new Abstract_Line( expr, fig);
     break;
     case "arc":
     result= new Abstract_Arc( expr);
     break;
     case "point":
     result= new Abstract_Point( expr);
     break;
     case "measure":
     result= new Abstract_Measure( expr);
     break;
     case "circle":
     result= new Abstract_Circle( expr);
     break;

    }

    return result;


  }

  public static List<T> Cast<T,R>( List<R> list ) where T: R {

    var result= new List<T>();
    for( int i=0; i< list.Count; i++) 
     result.Add( (T)list[i] );

     return result;
  }


  public static Figure Generate_Aleatory_Figure( string type) {

    Figure result= new Point();
        switch(type) {
        
         case "circle":
         result= new Circle();
         break;
         case "line":
         result= new Rect();
         break;
         case "segment":
         result= new Segment();
         break;
         case "ray":
         result= new Ray();
         break;
         //case "arc":
         //result= new Arc();
         //break;
         
        }

        return result;
    }

 }

  