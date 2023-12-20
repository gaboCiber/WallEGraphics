
 public abstract class No_Computable: Instruction {}

 public class Assignment : No_Computable {

    public string variable ;
    public Expression expr ;

    public Assignment( string variable, Expression expr ) {

      this.variable= variable ;
      this.expr= expr ;

    }

    public override Bool_Object Evaluate( Context context )  {  
       
       var pair= expr.Evaluate( context ); 
       if( !pair.Bool) return new Bool_Object( false, null );

       bool boolean= context.Define( variable, pair.Object ) ;
       if( !boolean )  return new Bool_Object( false, null);
      
       return new Bool_Object( true, null );
 
     }


   } 

 public class Def_Func : No_Computable {

    public string Name ;
    public List<string> Args ;
    public Expression Body ;

    public Def_Func( ID name, List<ID> args, Expression body ) { 

     Name= name.Name ;
     Args= args.Filter();
     Body= body ; 
     //Console.WriteLine("Creating_function");

    }

    public override Bool_Object Evaluate( Context context ) { 

     if( !context.Define( Name, Args.Count, this ) ) {
      Operation_System.Print_in_Console( "Semantik Error!! :  Ya existe una funcion con el nombre " + Name );
      return new Bool_Object( false, null ) ;
     }
     else return new Bool_Object( true, null );

    }
    

    public Bool_Object Evaluation( Context context, List<Expression> list ) {
     
     if( Args.Count!= list.Count ) return new Bool_Object( false, null ) ;

     Context chield= context.Create_Chield() ;

     for( int i=0; i< list.Count; i++ ) {

      var pair= list[i].Evaluate( context ) ;
      if( !pair.Bool ) return new Bool_Object( false, null ) ;
      else {

       if( pair.Object is Secuence ) chield.Define_In_Heap( Args[i], (Secuence)pair.Object );
        else chield.Define_By_Value( Args[i], pair.Object );
      }

     }

     return Body.Evaluate( chield );

    }


  }



 public class Match : No_Computable {
 
   public List<string> args;
   public Expression Right;

   public Match( List<string> args, Expression right) {

    this.args= args;
    Right= right;
    //Console.WriteLine("Creating_match_expression...");

   }

   public Match( string variable, Expression right) {

     args= new List<string>();
     args.Add( variable);
     Right= right;

   }

   public override Bool_Object Evaluate( Context context) {
     
     if( !Utils.Is_Posible_Secuence( Right) ) {

      Operation_System.Print_in_Console( "Semantik Error!! : Solamente secuencias de valores pueden utilizarse en la parte derecha de una asignacion de tipo match");
      return new Bool_Object( false, null);

     }
     if( (Right is ID) && !((ID)Right).Is_By_Reference( context) ) return new Bool_Object( false, null);
      
      object temp= Right.Evaluate( context).Object;
      if( !(temp is Secuence) )   {

         Operation_System.Print_in_Console( "Semantik Error!! : Solamente secuencias de valores pueden utilizarse en la parte derecha de una asignacion de tipo match");
        return new Bool_Object( false, null);
      }

      if( temp is Undefined ) Utils.Define_Undefined_Variables( context, args );

      Secuence set= (Secuence)temp;
      
     int index= 0;
     foreach( var item in set ) {

      if( item==null) return new Bool_Object(false, null);
     
      if( index==args.Count-1) {

       if( set.Finite && set.Count-1==index ) context.Define( args[index], item);
       else context.Define_In_Heap( args[index], Utils.Take( set, index ) );
       index++;
       break;

      } 

      if(args[index]!="_") context.Define( args[index], item);
      index++;

     }

     if(index< args.Count) 
      for( int i=index; i< args.Count; i++) 
       context.Define(args[i], new Undefined() );

     return new Bool_Object(true, null);

   }

    public void Add_ID( ID variable ) { args.Insert(0, variable.Name); }

  }



    public class Color: No_Computable {

     public string color;

     public Color( String color)  { this.color= color.Value;  }

     public override Bool_Object Evaluate( Context context) {

       context.Introduce_Color(color);
       return new Bool_Object(true, null);
     }
     
    }



     public class Restore: No_Computable {

     public Restore() {}

     public override Bool_Object Evaluate( Context context) {

       context.Remove_Top();
       return new Bool_Object(true, null);
     }
     
    }

   

    public class Import: No_Computable {
      
      string file;

      public Import( String file ) {  this.file= file.Value; }
      
      public override Bool_Object Evaluate( Context context) {
        
         return new Bool_Object( true, null) ;
      }

    }



    public class Figure_Declaration: No_Computable {

      public string fig; 
      public string variable;

      public Figure_Declaration( string fig, ID variable) {

       this.fig= fig;
       this.variable= variable.Name;
      }

      public override Bool_Object Evaluate( Context context) {
        
        context.Define( variable, Utils.Generate_Aleatory_Figure(fig) );
        //((Figure)context.Obtain_Value( variable)).Print();
        return new Bool_Object(true, null);
      }

    }



    public class Figure_Secuence_Declaration: No_Computable {

      string secuence_type;
      string variable;

      public Figure_Secuence_Declaration( string type, ID variable) {

        this.variable= variable.Name;
        secuence_type= type;

      }

      public override Bool_Object Evaluate( Context context ) {

        context.Define( variable, new Figure_Secuence( secuence_type)); 
        //((Secuence)context.Obtain_Value( variable)).Print();
         return new Bool_Object( true, null);
      }

    }

   
  public class Draw: No_Computable {

    Expression expr;
    string coment;

    public Draw( Expression expr ) {  this.expr= expr;  }
    public Draw( Expression expr, String s ) {

      this.expr= expr;
      coment= s.Value;

    }

    public override Bool_Object Evaluate( Context context ) {

      var obj= expr.Evaluate(context).Object;
      if( obj==null ) return new Bool_Object(false, null);

      if( !(obj is Secuence)) {

       if( !( obj is Figure ) ) {

        Operation_System.Print_in_Console( "Semantik Error :  En el cuerpo de una instruccion draw solamente pueden aparecer expresiones que computen figuras geometricas");
        return new Bool_Object(false, null);

       }
       Semantik_Analysis.Context.Add_Figure( (Figure)obj);
       return new Bool_Object( true, null );

      }   
     
       int count= 0;
       ((Secuence)obj).Put_In_Context( context);
      foreach( var x in (Secuence)obj ) {
         
        if( count> 100) break; 
        if( x== null || !( x is Figure) ) {

          if( x!= null ) Operation_System.Print_in_Console( "Semantik Error :  En el cuerpo de una instruccion draw solamente pueden aparecer expresiones que computen figuras geometricas");
          return new Bool_Object( false, null);
        }
        Semantik_Analysis.Context.Add_Figure( (Figure)x );
        count++;

      }
     
     return new Bool_Object( true, null);

    }

  }


   

    


