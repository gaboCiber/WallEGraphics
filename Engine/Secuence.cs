 
 public abstract class Secuence: Expression {
  
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

}


public class Collection: Secuence {

  public List<Expression> elements;

  public Collection( List<Expression> elements) {

    this.elements= elements;
    Count= elements.Count;
    Finite= true;

  }

  public Collection( Expression expr) {

    this.elements= new List<Expression>();
    elements.Add(expr);
    Count=1;
    Finite= true;
  }

  public override IEnumerator<object> GetEnumerator() {
     
    Context context= Semantik_Analysis.Context;
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
    Console.WriteLine("Creating_secuence...");
  }

  public virtual Sub_Z GetEnumerable( Context context) {
    
    var pair= Min.Evaluate( context);
    if( pair.Object==null || !(pair.Object is double) ) return null;
    double aux= (double)pair.Object;
    return new Sub_Z( (int)aux );

  }

  public override IEnumerator<object> GetEnumerator() {
   
    Context context= Semantik_Analysis.Context;
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
      
      }

     public override IEnumerator<object> GetEnumerator() {
       
       Context context= Semantik_Analysis.Context;
       var pair= Fig.Evaluate( context);
       if( !pair.Bool || !(pair.Object is Figure) ) yield return null;
       Figure fig= (Figure)pair.Object;
       var ecuation= fig.Get_Ecuation();
       var range= fig.Get_Dom_Range();
       var r= new Random();
       while( true) {
        
        double n= (range.Left_Acotated )? ( (range.Right_Acotated )? r.Next( (int)range.Inf, (int)range.Sup) : r.Next( (int)range.Inf) ) : ( ( range.Right_Acotated )? r.Next( (int)range.Sup ) : r.Next( -100, 100) );
        yield return new Point( n, ecuation.Obtain_Y_Value(n)[0] );

       }

     }

     public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, this) ;  }

    }

