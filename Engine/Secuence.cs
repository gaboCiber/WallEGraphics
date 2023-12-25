 
 public abstract class Secuence: Expression {
  
  public Context context;
  public virtual bool Finite{ get; set; }
  public virtual int Count{ get; set; }
  public abstract IEnumerator<object> GetEnumerator(); 
  public bool Is_Empty() { return Finite && Count==0; }
  public void Print() {
     
     int cursor= 0;
    foreach( var item in this ) {

      if( cursor>100) break;
      if( item is Figure ) ((Figure)item).Print();
      else Console.WriteLine( item);
     cursor++;
    }

  }

  public void Put_In_Context( Context context ) {  this.context= context; }

}


public class Collection: Secuence {

  public List<Expression> elements;

  public Collection( List<Expression> elements) {

    this.elements= elements;
    Count= elements.Count;
    Finite= true;
    context= Semantik_Analysis.Context;

  }

  public Collection( Expression expr) {

    this.elements= new List<Expression>();
    elements.Add(expr);
    Count=1;
    Finite= true;
    context= Semantik_Analysis.Context;

  }

  public override IEnumerator<object> GetEnumerator() {
     
    foreach( var element in elements ) 
     yield return element.Evaluate( context).Object ;

  }

  public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, this) ;  }
  
}


 public class Sub_Set: Secuence {

  public Expression Min;

  public Sub_Set( Expression min) {

    Min= min;
    Finite= false;
    context= Semantik_Analysis.Context;
    
  }

  public virtual Sub_Z GetEnumerable( Context context) {
    
    var pair= Min.Evaluate( context);
    if( pair.Object==null || !(pair.Object is double) ) return null;
    double aux= (double)pair.Object;
    return new Sub_Z( (int)aux );

  }

  public override IEnumerator<object> GetEnumerator() {
   
    Sub_Z set= GetEnumerable( context);
    if( set==null) yield return null;
    foreach( var item in set )
     yield return item;
    
  }

  public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, this);  }
  
 }



 public class Restricted_Sub_Set: Sub_Set {

  public Expression Max;

  public Restricted_Sub_Set( Expression min, Expression max ) : base( min) {  
    
    Max= max; 
    Finite= true;
    context= Semantik_Analysis.Context;

    }

  public override Sub_Z GetEnumerable( Context context) {
    
    double aux= 5;
    var list= Utils.Filter<double>( context, aux, true, Min, Max );

    if( list== null) return null;
    var cast_list= Utils.Cast<double, object>( list);
    return new Sub_Z( (int)cast_list[0], (int)cast_list[1] );

  }

  public override int Count { 

    get {
       
      double aux= 5;
      var list= Utils.Filter<double>( Semantik_Analysis.Context, aux, true, Min, Max );
      if( list==null) return 0;
      var cast_list= Utils.Cast<double, object>( list);
      return (int)cast_list[1]- (int)cast_list[0] + 1;

    }

  }

 } 


    public class Figure_Secuence: Secuence {

     public string type;

     public Figure_Secuence( string type) { 
      
      this.type= type; 
      Finite= false;
      context= Semantik_Analysis.Context;

      }

     public override Bool_Object Evaluate( Context context) {  return new Bool_Object(true, this); } 
     
     public override IEnumerator<object> GetEnumerator() {

       while(true) yield return Utils.Generate_Aleatory_Figure(type);

     }

    }


    public class Sub_Secuence: Secuence {

     Secuence source;
     int ignored;
     
     public Sub_Secuence( Secuence secuence, int count) {

      source= secuence;
      ignored= count;
      Finite= source.Finite; 
      context= Semantik_Analysis.Context;

     }

     public override Bool_Object Evaluate( Context context) { return new Bool_Object( true, this );  }
     
     public override IEnumerator<object> GetEnumerator() {
       
       int count= 0;
       foreach( var obj in source) {

         if( count>=ignored ) yield return obj;
         count++;
         
       }

     }

     public override int Count { get { return source.Count - ignored; } }
      
    }


     public class Samples: Secuence {
      
      public Samples() { Finite= false; }

      public override IEnumerator<object> GetEnumerator() {   while(true) yield return new Point(); }
      
      public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, this) ;  }
    }



    public class Randoms: Secuence {
      
      public Randoms() { Finite= false; }

      public override IEnumerator<object> GetEnumerator() { 

        var r= new Random();
        while(true) yield return r.Next();

      }

      public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, this) ;  }

    }



    public class Points: Secuence {
     
     public Expression Fig;

     public Points( Expression fig) { 
      
      Fig= fig;  
      Finite= false;
      context= Semantik_Analysis.Context;

      }

     public override IEnumerator<object> GetEnumerator() { yield break; }

     public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, this) ;  }

    }

    public class Undefined: Secuence {

     public Undefined() {

      Finite= true;
      Count= 0;

     }

     public override IEnumerator<object> GetEnumerator() { yield break; }
     public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, this) ;  }

    }


    public class Sum_Secuence: Secuence {

      public Secuence left;
      public Secuence right;
      public Sum_Secuence( Secuence left, Secuence right) {

        this.left= left;
        this.right= right;
        Finite= left.Finite && right.Finite;

      }

      public override int Count {    get { return left.Count + right.Count ; }   }
       
      public override IEnumerator<object> GetEnumerator() {

        foreach( var x in left )
         yield return x;

        foreach( var x in right )
         yield return x;

      } 

      public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, this); }

    }

    public class Intersection : Secuence {

      public Expression Fig1;
      public Expression Fig2;

      public Intersection( Expression fig1, Expression fig2 ) {

        Fig1= fig1;
        Fig2= fig2;
        context= Semantik_Analysis.Context;

      }

      public override Bool_Object Evaluate( Context context ) { return new Bool_Object( true, this); }

      public override IEnumerator<object> GetEnumerator() {

       var figures= Utils.Filter( context, Fig1, Fig2);
       if( !(figures[0] is Figure) || !(figures[1] is Figure ) )  {

       Operation_System.Print_in_Console( "A la funcion Interseccion solo pueden asignarsele como parametros objectos de tipo \"Figure\" ");
       yield return null;

      }

      var points= ((Figure)figures[0]).Get_Intersection( (Figure)figures[1]);
      if( points==null) yield break;
      foreach( var p in points) 
       yield return p;

      }

    }

     


