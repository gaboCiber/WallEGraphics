
 public static class Utils {
   
   
   public static Bool_Object Obtain_Value( this Binary_Operation expr, Context context ) {

    var t= Obtain_Values( context, expr);
    if( t.Item1==null) return new Bool_Object( false, null);
    var results= t.Item1;
    var operators= t.Item2;

    foreach( var x in results )
    Console.WriteLine(x);
    foreach( var x in operators )
    Console.WriteLine(x);
    
    
    return Obtain_Result( results[0], results, operators, 1, 0);

   }


    public static Bool_Object Obtain_Result( object acum, List<object> results, List<string> operators, int index_result, int index_op ) {
      
      object actual= results[index_result];

      if( (actual is string) || ( actual is Figure && !(actual is Measure) ) ) {

        Operation_System.Print_in_Console( "La operacion de multilplicacion o cociente solo esta definida para numeros u objectos tipo measure");
        return new Bool_Object( false, null);

      }

     object result= null;

      if( actual is double ) {
     
     switch(operators[index_op]) {
      case "*":
      if( acum is double) result= ((double)acum)*((double)actual) ;
      else result= Obtain_New_Measure( (acum as Measure), (double)actual );
      break;
      case "/":
      if( acum is double ) result= (double)acum/(double)actual;
      else {

        Operation_System.Print_in_Console("No se puede efectuar una operacion de division entre un measure y un number");
        return new Bool_Object( false, null);
      }
      break;

      }

     }

     if( actual is Measure ) {

      switch(operators[index_op]) {
      case "*":
      if( acum is double) result= Obtain_New_Measure( actual as Measure, (double)acum );
      else {
        
        Operation_System.Print_in_Console( "La operacion de producto no puede ser efectuada entre dos measure");
        return new Bool_Object( false, null);
      }
      break;

      case "/":
      if( acum is Measure) result= Obtain_Proportion( (Measure)acum, (Measure)actual );
      else {

        Operation_System.Print_in_Console(" La operacion de division no puede efectuarse entre en measure y un number" );
        return new Bool_Object( false, null);
      }
      break;

     }

   }
     
     if( index_result== results.Count-1 ) return new Bool_Object( true, result);

      return Obtain_Result( result, results, operators, index_result+1, index_op+1 );

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
     //foreach( var x in list)
     //Console.WriteLine( x);
     var t= list[0].GetType();
     //Console.WriteLine(t);
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


   public static bool Interprete( this object obj ) { 
    
    return !( obj== null || ( (obj is Secuence) && ((Secuence)obj).Is_Empty() ) || ( obj.ToString()=="0"  ) );
    
     }

   public static bool Is_Predeterm( this string function, Context context, int args) { return context.Is_Predeterm( function, args);  }
  
    public static bool Same_Type( this object obj, object other ) {

   if( ( obj is string && other is string ) || ( obj is double && other is double ) || ( obj is bool && other is bool ) || ( obj is Secuence && other is Secuence ) || ( obj is Measure && other is Measure ) ) return true;
   else return false ;
   
  }

  public static bool Is_Posible_Secuence( Expression expr) { return (expr is ID) || (expr is Secuence) || (expr is Func_Call) ; }
  
 
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
         case "arc":
         result= new Arc();
         break;
         
        }

        return result;
    }

  
    public static Secuence Combine_Secuences( this Secuence left, Secuence right ) {
     
     if( left is Undefined ) return new Undefined();
     if( right is Undefined ) return left;
     return new Sum_Secuence( left, right);

    }

    public static void Define_Undefined_Variables( Context context, List<string> variables ) {

      for( int i=0; i< variables.Count; i++) 
       context.Define( variables[i], new Undefined() );

    }


   public static Bool_Object Combine_Condition( double value1, double value2, string op ) {
   
     bool result= false;
     switch( op ) {

       case">=": 
       result= value1>=value2 ;
       break;
       case"<=": 
       result= value1<=value2 ;
       break;
       case"<": 
       result= value1<value2 ;
       break;
       case">": 
       result= value1>value2 ;
       break;
       
      }  
      
      if( result ) return new Bool_Object( true, 1 );
      return new Bool_Object( true, 0);
      
      }

      public static double Combine_Boolean( double left, double right, string op) {
        
        double result= 0;
        switch( op) {

         case "or":
         result=  left + right;
         break;
         case "and":
         result= (left + right== 2 ) ? 1 : 0;
         break;

        }

        return result;

      }


      public static double Compare( object obj, object other ) {

       if( !obj.Same_Type( other)) return 0;
      
       if( obj is double || obj is string ) {

        if( obj.ToString() == other.ToString() ) return 1;
        else return 0;
       }


        if( obj is Secuence ) {

          var sec1= (Secuence)obj;
          var sec2= (Secuence)other;
          if( !sec1.Finite || !sec2.Finite ) return 0;
          if( sec1.Count!= sec2.Count) return 0;
          var list1= new List<object>();
          var list2= new List<object>();
          foreach( object x in sec1)
           list1.Add( x);

           foreach( object x in sec2) 
           list2.Add( x);
           
          for( int i=0; i< list1.Count; i++)
           if( Compare( list1[i], list2[i])==0) return 0;

           return 1;

        }

        return 0;

      }

   public static (List<object>, List<string>) Obtain_Values( Context context, IBinary expr ) {

    var results= new List<object>();
    var operators= new List<string>();
    if( Obtain_Values( context, results, operators, expr) ) return (results, operators);
    return (null, null);

   }


   public static bool Obtain_Values( Context context, List<object> list, List<string> operators, IBinary expr ) {

    var left= expr.Left.Evaluate( context ).Object;
    if( left==null ) return false;
    list.Add( left);
    operators.Add( expr.Op);

    if( !expr.Right.Is_Binary_Respect( expr ) ) {
 
    var right= expr.Right.Evaluate( context).Object;
    if( right==null) return false;
    list.Add( right);
    return true;

    }
    return Obtain_Values( context, list, operators, (IBinary)expr.Right);

   }


   public static bool Is_Binary_Respect( this Expression expr, Expression other ) {

    return ( expr.Is_Product() && other.Is_Product() ) || ( expr is Boolean_Operation && other is Boolean_Operation ) ;

   }


   public static Measure Obtain_New_Measure( Measure m, double mult) {

     double component1= m.p2.X - m.p1.X;
     double component2= m.p2.Y - m.p1.Y;
     int factor= (int)mult;
     var p2= new Point( m.p1.X + component1* factor, m.p1.Y + component2* factor);
     return new Measure( m.p1, p2);

   }


   public static double Obtain_Proportion( Measure m1, Measure m2 ) {

     double parcial= m1.Get_Distance()/ m2.Get_Distance();
     //Console.WriteLine( "Parcial: {0} aux_int: {1}  result: {2}", parcial, aux, (double)parcial );
     return Math.Truncate( parcial );

   }




 }

  