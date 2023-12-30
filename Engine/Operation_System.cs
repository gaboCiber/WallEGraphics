


public class Operation_System  {
   
   public static bool error;
   public static bool let_context;
  
   public static (List<Figure>, List<string> ) Validate_Program( string s ) {
     
      Semantik_Analysis.AST= new Program_Node() ;
      Semantik_Analysis.Context= new Context()  ;
      Semantik_Analysis.Context.Introduce_Functions() ;
      error= false;
      let_context= false;

     int index= 0;
     Instruction instruction= new String("");
     int count= 0;

     for( int i=0; i< s.Length; i++) {
      
      if( i== s.Length-1 && s[i]!= ';') Print_in_Console( "Mising \";\" token at the end of the last instruction");
      
      if(s[i]=='l' || s[i]=='i' ) Modify_Sintax( s[i], s, i);

      if( s[i]==';' && !let_context ) {
        
        //Console.WriteLine(s.Substring(index, i-index));
      instruction= Obtain_AST( s.Substring(index, i-index)).Item1 ;
      if( instruction== null) {
        
        error= true;
       Console.WriteLine( "Problem with AST in line {0}", count);
      }
      if( instruction!=null && !error ) Semantik_Analysis.AST.lines.Add( instruction);
      index= i+1;

      }

     }
     
     var context= Semantik_Analysis.Context;
     var boolean= Semantik_Analysis.AST.Evaluate( context ).Bool;
     if( boolean )  return ( context.Get_Figures(), null ) ;
     
     return ( null, context.errors );

   }

    

  public static Tuple<Instruction, bool> Obtain_AST( string s ) {

   Node node= Parser.Parsing( Lexer.Tokenization( s ) );
    if( node== null ) return Tuple.Create<Instruction, bool>( null, false );
   // Console.WriteLine("Sintactic_tree_Completed");
    Instruction sub_tree= Semantik_Analysis.To_AST( node );
    if( sub_tree== null )  return Tuple.Create<Instruction, bool>( null, false );
     
    return Tuple.Create<Instruction, bool>( sub_tree, true );
    
  } 


  public static void Interface() {
    
    string s= ""; 
    do {

    s= Console.ReadLine() ;
    var figures= Validate_Program( s );
    
     Print( figures.Item1);
     Print( figures.Item2 );

    }
    while( s!= "finish");
    
  } 

   public static void Print_in_Console( object obj ) { 
    
    if( obj is string)  Semantik_Analysis.Context.Introduce_Error( (string)obj );
    if( obj is double || obj is int ) Console.WriteLine( obj );
    if( obj is Figure ) ((Figure)obj).Print();
    if( obj is Secuence && !(obj is Undefined) ) { 

      ((Secuence)obj).Print();
      Console.WriteLine("Secuence");
      
    }
    if( obj is Undefined ) (( Undefined )obj).Print();

    
  
   }

     public static void Print( List<Figure> figures ) {
       
       if( figures==null) return;
       
      for( int i=0; i< figures.Count; i++) 
       Print_in_Console( figures[i]);

     }

     public static void Print( List<string> figures ) {
       
       if( figures== null) return ;
       
      for( int i=0; i< figures.Count; i++) 
        Console.WriteLine( figures[i]);

     }

     public static void Modify_Sintax( char c, string s, int index ) {

       if( c=='l' && ( s.Length-index<= 3 || ( index-1>0 && s[index-1]!=' ' ) || s[index+3]!=' ') ) return;
       if( c=='i' && ( s.Length-index<= 2 || index-1<=0 || s[index-1]!=' ' || s[index+2]!=' ' )) return;

       if( c=='l' && s[index+1]=='e' && s[index+2]=='t' ) let_context= true;
       if( let_context && c=='i' && s[index+1]=='n' ) let_context= false;
      
       }

     }





