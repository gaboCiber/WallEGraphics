


public class Operation_System  {
   
   public static bool error;
  
   public static (List<Figure>, List<string> ) Validate_Program( string s ) {
     
      Semantik_Analysis.AST= new Program_Node() ;
      Semantik_Analysis.Context= new Context()  ;
      Semantik_Analysis.Context.Introduce_Functions() ;
      error= false;

     int index= 0;
     Instruction instruction= new String("");
     int count= 0;

     for( int i=0; i< s.Length; i++) {
      
      if( i== s.Length-1 && s[i]!= ';') Print_in_Console( "Mising \";\" token at the end of the last instruction");
     
      if( s[i]==';') {
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
    Console.WriteLine("Sintactic_tree_Completed");
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
    
    if( obj is string) Semantik_Analysis.Context.Introduce_Error( (string)obj );
    if( obj is Figure ) ((Figure)obj).Print();
    if( obj is Secuence ) ((Secuence)obj).Print();
    Console.WriteLine( obj) ; 

     }

     public static void Print( List<Figure> figures ) {
       
       if( figures==null) return;
       if( figures.Count==0) Console.WriteLine( "vacia");
      for( int i=0; i< figures.Count; i++) 
       Print_in_Console( figures[i]);

     }

     public static void Print( List<string> figures ) {
       
       if( figures== null) return ;
       if( figures.Count==0) Console.WriteLine( "vacia");
      for( int i=0; i< figures.Count; i++) 
        Console.WriteLine( figures[i]);

     }



}

