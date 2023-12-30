
 public abstract class Semantik_Node {} 
 public class Program_Node: Semantik_Node {
 
  public  List<Instruction> lines;

  public Program_Node() { lines= new List<Instruction>() ;  }

  public Bool_Object Evaluate( Context context ) {      
    
    List<object> results= new List<object>() ;
    for( int i= 0; i< lines.Count; i++ ) {
    
    context.instruction= i;
    var pair= lines[i].Evaluate( context ) ;
    if( !pair.Bool )   {
      Console.WriteLine( "Semantik Problem with line {0}", i);
      return new Bool_Object( false, null ) ;
    }
    else if( pair.Object!= null) results.Add( pair.Object );

    }

    for( int i= 0; i< results.Count; i++ )
     Operation_System.Print_in_Console( results[i]) ;

    return new Bool_Object( true, results ) ;

  }

 }



public abstract class Instruction: Semantik_Node {

 public abstract Bool_Object Evaluate( Context context);

}


public abstract class Expression: Instruction {}

public abstract class IBinary: Expression {

 public Expression Left;
 public Expression Right;
 public string Op;
 
 public IBinary( Expression left, string op, Expression right) {

   Left= left;
   Right= right;
   Op= op;

 }

}


public class Binary_Operation: IBinary {

 public Binary_Operation( Expression left, string op, Expression right ) : base( left, op, right )  {
    
      Op= ( op!="-") ? op : "+"; 

   }

   public override Bool_Object Evaluate( Context context) { 
    
     if( this.Is_Product() ) return this.Obtain_Value( context) ;
       var list= Utils.Filter( context, Left, Right );
       if( list==null ) new Bool_Object( false, null );
       var left= list[0];
       var right= list[1];
       
      if( !left.Same_Type( right ) ) {

        Operation_System.Print_in_Console("Semantik Error!! : Los operandos deben de ser del mismo tipo");
        return new Bool_Object( false, null );
      }

      if( left is bool ) {

      Operation_System.Print_in_Console("Semantik Error!! : Los operandos no pueden ser de tipo boolean") ;
        return new Bool_Object( false, null );
      }

      if ( Op!="+" && (  left is string  || left is Secuence || left is Measure ) ) {

        Operation_System.Print_in_Console("Semantik Error!! : El unico operador aritmetico que puede utilizarse entre strings, secuences o measures es el de adicion") ;
        return new Bool_Object( false, null );
      }
      
      if( left is string )  return new Bool_Object( true, ( left as string).Combine_Strings( right as string, "+"  ) );
      if( left is Secuence ) return new Bool_Object( true, ( left as Secuence).Combine_Secuences( right as Secuence ) );   
      if( left is Measure ) return new Bool_Object( true, new Measure( left as Measure, right as Measure) );

      return new Bool_Object( true, Utils.Combine_Numbers( (double)left, (double)right, Op ) );
     }

}



public class Number: Expression {

 public double Value;

 public Number( string value ) { 

  double temp;
  double.TryParse( value, out temp );
  Value= temp;
 
  }

 public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, Value); }  
 
}



public class String: Expression {

 public string Value;

 public String( string value ) { Value= value; }
 public override Bool_Object Evaluate( Context context ) { return new Bool_Object(true, Value); }

}



public class ID: Expression {

 public string Name ; 
 public bool Is_By_Reference( Context context) { return context.Is_In_Heap( Name) ; }

   public ID( string name ) { Name= name ; }

   public override Bool_Object Evaluate( Context context ) { 
    
    bool truth= context.Is_Defined( Name) ;
    if( !truth ) Operation_System.Print_in_Console( "Semantik Error!! : La variable " + Name + " no se encuentra definida " );
    
    return new Bool_Object( truth, context.Obtain_Value( Name ) );  
    
    }

}




public class Func_Call: Expression {

 public string Name;
 public List<Expression> args;

 public Func_Call( ID name, List<Expression> args ) {

  Name= name.Name;
  this.args= args;

 }


 public override Bool_Object Evaluate( Context context) {
    
   if( Semantik_Analysis.Context.Is_Predeterm( Name, args.Count ) ) return Evaluate_Predeterm( context, Name, args ) ;

  if( !context.Is_Defined( Name, args.Count ) ) {
      
      Operation_System.Print_in_Console( "Semantik Error!! :  La funcion " + Name + " no ha sido definida" ) ;
      return new Bool_Object( false, null ) ;
     }

     Def_Func function= context.Obtain_Node( Name, args.Count );
     return function.Evaluation( context, args ) ;
 }


 public static Bool_Object Evaluate_Predeterm( Context context, string name, List<Expression> list ) {

     if( name=="sin" || name=="cos") {
      var pair= list[0].Evaluate( context);
      if( pair.Object==null || !(pair.Object is double) ) return new Bool_Object(false, null);
       return new Bool_Object(true, Utils.Evaluate_Trigonometrics((double)pair.Object, name) ) ;

     }

     if( name=="samples") return new Bool_Object( true, new Samples());
     if( name=="randoms") return new Bool_Object( true, new Randoms());

     if( name=="intersect")  { 
      
      var intersection= new Intersection( list[0], list[1]); 
      intersection.Put_In_Context( context);
      return new Bool_Object( true, intersection);
      
    }

    if( name=="count") {
     
     if( list.Count==0) return new Bool_Object( false, null);
     var obj= list[0].Evaluate( context ).Object;
     if( obj== null ) return new Bool_Object( false, null);

     if( obj is Secuence ) {

      if( ((Secuence)obj).Finite ) return new Bool_Object(true, ((Secuence)obj).Count);
      else return new Bool_Object(true, new Undefined() );
      
     }
     
      Operation_System.Print_in_Console( "Semantik_Error:  La funcion count solo recibe como parametros a secuencias");
      return new Bool_Object( false, null);

    }

     return new Bool_Object( false, null ) ;

    }

}



public class If_Else: Expression {

 public Expression Condition;
 public Expression if_part;
 public Expression else_part;

 public If_Else( Expression condition, Expression if_part, Expression else_part ) {

    Condition= condition ;
    this.if_part= if_part ;
    this.else_part= else_part ;
    
   }

   public override Bool_Object Evaluate( Context context) {
    
    var pair= Condition.Evaluate( context ) ;
    if( !pair.Bool ) return new Bool_Object( false, null ) ;

    bool value= Utils.Interprete( pair.Object);
    if( value ) return if_part.Evaluate( context);
    else return else_part.Evaluate( context) ;

   }
 

}


public class Let_In: Expression {

 public List<Instruction> Instructions;
 public Expression Body; 

   public Let_In( List<Instruction> instructions, Expression body ) {

   this.Instructions= instructions;
   Body= body;
   
   }

   public override Bool_Object Evaluate( Context context ) { 

    Context chield= context.Create_Chield();
   
    for( int i= 0; i< Instructions.Count; i++)
     if( !Instructions[i].Evaluate( chield).Bool ) {
      
      Console.WriteLine("Se evaluo mal la instrucion {0} del let", i);
      return new Bool_Object( false, null ) ;
     }

     return Body.Evaluate( chield ) ; 
   
    }
   
  }



   public class Condition: Expression {
    
    public Expression Left ;
    public Expression Right ;
    string Op;
    

    public Condition( Expression left, string op, Expression right ) {
     
     Left= left ;
     Right= right ;
     Op= op;

    }

    public Condition( string op, Expression left )  {  
      
      Left= left ;
      Right= null ; 
      Op= op;
     }


    public override Bool_Object Evaluate( Context context ) {  
      
      var list= Utils.Filter<double>( context, 5.6, true, Left, Right );
      if( list== null) return new Bool_Object( false, null);

      return Utils.Combine_Condition( (double)list[0], (double)list[1], Op );
    
   }

   }



   public class Boolean_Operation: IBinary {

    public Boolean_Operation( Expression left, string op, Expression right ) : base( left, op, right) {}

    public override Bool_Object Evaluate( Context context) {

     var t= Utils.Obtain_Values( context, this );
     if( t.Item1==null) return new Bool_Object( false, null);
     var results= t.Item1;
     var operators= t.Item2;

     double result= Obtain_Result_Boolean( context, results, operators, 0, 0);
     return new Bool_Object( true, result);

    }


   public static double Obtain_Result_Boolean( Context context, List<object> results, List<string> operators, int index_result, int index_op ) {
   
    double left= 0;
    double right= 0;
    int actual= index_result;
    int op= index_op; 

    if( op<operators.Count && operators[op]=="==") {

      left= Utils.Compare( results[actual], results[actual+1] );
      actual++;
      op++;

    }
    
   else left= ( results[actual].Interprete() ) ? 1 : 0;
   
   if( actual== results.Count-1 ) return left;

    right= Obtain_Result_Boolean( context, results, operators, actual+1, op+1);
   // Console.WriteLine( "Left : {0}, Right : {1}", left, right );
   return Utils.Combine_Boolean( left, right, operators[op]);

   }

    
   }







  
  









  

 





   


 

 





 
