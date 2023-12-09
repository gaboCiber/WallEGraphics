
public abstract class Abstract_Figure: Expression {

      public List<Expression> Components;
      public Abstract_Figure( List<Expression> components) { Components= components; }

    }


    public class Abstract_Circle: Abstract_Figure { 

     public Abstract_Circle( List<Expression> components) : base( components) { Console.WriteLine("Creating_Circle"); }  

     public override Bool_Object Evaluate( Context context ) {
      
      if( Components.Count!=2) return new Bool_Object(false, null);
      var list= Utils.Filter( context, Components[0], Components[1] );
      if(list==null) Console.WriteLine( "list_nula");
      if( !(list[0] is Point) || !(list[1] is Measure)) Console.WriteLine( "Incorrect_Paramters");
      if(list==null || !(list[0] is Point) || !(list[1] is Measure) ) return new Bool_Object(false, null);
  
       return new Bool_Object(true, new Circle( (Point)list[0], (Measure)list[1]) );

     }

    }



    public class Abstract_Line: Abstract_Figure {

      public string Especification;

      public Abstract_Line( List<Expression> components, string especification) : base( components) {

        Especification= especification;
        Console.WriteLine("Creating_line");
      }

      public override Bool_Object Evaluate( Context context) {

       if( Components.Count!=2 ) return new Bool_Object( false, null);
       var list= Utils.Filter<Point>( context, new Point(), true, Components[0], Components[1] );
       if( list==null) return new Bool_Object( false, null);
       return new Bool_Object( true, Utils.Make_Line( (Point)list[0], (Point)list[1], Especification) );

      }  

    }


    public class Abstract_Point: Abstract_Figure {
    
    public Abstract_Point( List<Expression> components) : base( components) { }

    public override Bool_Object Evaluate( Context context) {

       if( Components.Count!=2 ) return new Bool_Object( false, null);
       double aux= 5;
       var list= Utils.Filter<double>( context, aux, true, Components[0], Components[1] );
       if( list==null) return new Bool_Object( false, null);
       return new Bool_Object( true, new Point( (double)list[0], (double)list[1] ) );

      }  
 
    }


     public class Abstract_Measure: Abstract_Figure {

      public Abstract_Measure( List<Expression> components) : base( components) { Console.WriteLine("Creating_measure"); }

      public override Bool_Object Evaluate( Context context ) { 

       if( Components.Count!=2 ) return new Bool_Object( false, null);
       var list= Utils.Filter<Point>( context, new Point(), true, Components[0], Components[1] );
       if( list==null) return new Bool_Object( false, null);
       return new Bool_Object( true, new Measure( (Point)list[0], (Point)list[1] ) );

      }

     }



     public class Abstract_Arc: Abstract_Figure {
    
     public Abstract_Arc( List<Expression> components) : base( components) {}

     public override Bool_Object Evaluate( Context context) {

       if( Components.Count!=4 ) return new Bool_Object( false, null);
       var list= Utils.Filter( context, new Point(), true, Components[0], Components[1], Components[2] );
       if( list==null) return new Bool_Object( false, null);
       var pair= Components[4].Evaluate( context);
       if( !pair.Bool || !(pair.Object is Measure) ) new Bool_Object( false, null);

       return new Bool_Object( true, new Arc( (Point)list[0], (Point)list[1], (Point)list[2], (Measure)pair.Object ) );

      }  
 
    }



     public class Intersection: Secuence {

       public Expression Figure1;
       public Expression Figure2;

       public Intersection( Expression fig1, Expression fig2) {

         Figure1= fig1;
         Figure2= fig2;

       }

       public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, this) ;  }

       public override IEnumerator<object> GetEnumerator() {
         
         Context context= Semantik_Analysis.Context;
         var list= Utils.Filter( context, Figure1, Figure2);
         if( list==null || !(list[0] is Figure ) || !(list[1] is Figure) ) yield return null;
         var intersection= ((Figure)list[0]).Get_Intersection( (Figure)list[1] );
         if( intersection== null) yield return null;

         foreach( var point in intersection ) {
            yield return point;
         }

       }

    }
 