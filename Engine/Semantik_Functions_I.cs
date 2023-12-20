
 public abstract class Semantik_Node {} 
 public class Program_Node: Semantik_Node {
 
  public  List<Instruction> lines;

  public Program_Node() { lines= new List<Instruction>() ;  }

  public Bool_Object Evaluate( Context context ) {      
    
    List<object> results= new List<object>() ;
    for( int i= 0; i< lines.Count; i++ ) {

    var pair= lines[i].Evaluate( context ) ;
    if( !pair.Bool )   {
      Console.WriteLine( "Semantik Problem with line {0}", i);
      return new Bool_Object( false, null ) ;
    }
    else if( pair.Object!= null) results.Add( pair.Object );

    }

    //for( int i= 0; i< results.Count; i++ )
     //Operation_System.Print_in_Console( results[i]) ;

    return new Bool_Object( true, results ) ;

  }

 }



public abstract class Instruction: Semantik_Node {

 public abstract Bool_Object Evaluate( Context context);

}


public abstract class Expression: Instruction {}



public class Binary_Operation: Expression {

 public Expression Left;
 public Expression Right;
 public string Op;

 public Binary_Operation( Expression left, string op, Expression right ) {
    
    Left= left ;
    Op= ( op!="-") ? op : "+" ;
    Right= right ;

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

      if ( Op!="+" && ( left is string ) ) {

        Operation_System.Print_in_Console("Semantik Error!! : El unico operador aritmetico que puede utilizarse entre strings es el de suma") ;
        return new Bool_Object( false, null );
      }

      if( left is string )  return new Bool_Object( true, ( left as string).Combine_Strings( right as string, "+"  ) ) ;
      
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
    
   if( context.Is_Predeterm( Name, args.Count ) ) return Evaluate_Predeterm( context, Name, args ) ;

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
   Console.WriteLine("Creating_let_in");
   
   }

   public override Bool_Object Evaluate( Context context ) { 

    Context chield= context.Create_Chield();
   
    for( int i= 0; i< Instructions.Count; i++)
     if( !Instructions[i].Evaluate( chield).Bool ) return new Bool_Object( false, null ) ;

    return Body.Evaluate( chield ) ;  
    
    }
   
  }





  
  









  

 





   


 

 





 
