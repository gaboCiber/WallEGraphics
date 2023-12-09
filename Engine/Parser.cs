
 public static class Parser {
  

  public static SymbolSet Firsts ;
  public static SymbolSet Follow;
  public static Symbol epsilon;
  public static Table table;
  public static bool processing;
  public static Node Tree;
  public static int index;

  public static Node Parsing( List<Token> tokens ) {
   
   if( !processing ) {
     Pre_Processing() ;
     processing= true ;
   }
   
   return ConstructTree( tokens );
   
  }

  public static void Pre_Processing() {

   Data.Create_Gramatik();
   Calculate_Firsts();
   Calculate_Follow();
   BuildTable();
   //Program.Aux_Table();
   
  }


  public static void Calculate_Firsts() {
  
  var firsts= new SymbolSet();
  epsilon= Data.Epsilon;
   foreach( var t in Data.gramatik.Terminals ) {
  
   var list= new List<Symbol>();
   list.Add( t);
   firsts.Add( t, list );
   }

   foreach( var n in Data.gramatik.No_Terminals ) {

   firsts.Add( n, new List<Symbol>() );
   }

   bool change;
   do {
    
    change= false;
   foreach( var p in Data.gramatik.Productions ) {
   
   Symbol left= p.Left ;
   var right= p.Right;

   if(p.IsEpsilon ) { 
    if( change) firsts[left].Add_Bool( epsilon );
    else change= firsts[left].Add_Bool( epsilon );
    continue;
   }

   bool all_epsilon= true;
   foreach( var s in right ) {
    
    if( change) firsts[left].Add_All( firsts[s] );
     else change= firsts[left].Add_All( firsts[s] );

    if( !firsts[s].Contains(epsilon) ) {
     all_epsilon= false;
     break;
    }

     if( all_epsilon) {
      if( change) firsts[left].Add_Bool( epsilon );
       else change= firsts[left].Add_Bool( epsilon );
     }

    
    }

    }

   }
  while( change );

  Firsts= firsts ;
  }


  public static void BuildTable() {

  var table_aux= new Table();

  foreach( var p in Data.gramatik.Productions ) 
   foreach( var t in Data.gramatik.Terminals ) {
    
   // if( t.Class.Length== 0) continue;
    if( !p.IsEpsilon && Calculate_Firsts_Sufix( p.Right, 0 ).Contains_Bool( t) ) table_aux.Add( p.Left, t, p );
    if( p.IsEpsilon && Follow[ p.Left].Contains_Bool( t) ) table_aux.Add( p.Left, t, p ); 
   
   }

   table= table_aux ;
  
  }


  public static Node ConstructTree( List<Token> tokens ) {
   
   index= 0;
   Node tree= new Node( Data.gramatik.Initial.Class, null) ;
   if( Construct( tree, tokens ) && index==tokens.Count-1 ) return tree ;
   return null;

  }

  public static bool Construct( Node node, List<Token> tokens ) {
    
    //Console.WriteLine( node.Symbol ) ;
    if( node.Is_Terminal() ) {
      
      if( node.Is_Epsilon() ) return true ;
      if( node.Symbol!= tokens[index].Class ) return false ;
      node.Chain=tokens[index].Chain ;
      index++;
      return true ;
    }

    int temp= index ;
    var list= table.Search( new Symbol( node.Symbol ), new Symbol( tokens[index].Class ) ) ;
    if( list== null ) {

     Operation_System.Print_in_Console("Sintactic Error") ;
     return false;
    }

    for( int i=0; i< list.Count; i++) {

     var right= list[i].Right ;
     bool valid_production= false ;
     for( int j=0; j< right.Count; j++) {

      node.Children.Add( new Node(right[j].Class, node ) ) ;
      if( !Construct( node.Children[j], tokens ) ) break ;
      if( j==right.Count-1) valid_production= true ;
     }

     if( (valid_production && node.Parent!=null) || ( valid_production && node.Parent==null && index==tokens.Count-1 ) ) return true ;
     node.Children= new List<Node>() ;
     index= temp ;

    }

    return false ;
     
  }
   

   public static void Calculate_Follow() {
   
   var follow= new SymbolSet();
   foreach( var n in Data.gramatik.No_Terminals ) 
    follow.Add( n, new List<Symbol>() ) ;
   
   follow[Data.gramatik.Initial].Add( Data.EOF );
   bool change;
   do {
    change= false; 
   foreach( var p in Data.gramatik.Productions ) {

    var left= p.Left ;
    var right= p.Right ;
    
    for( int i= 0; i< right.Count; i++ ) {
      
      if( right[i].IsTerminal ) continue;

      var firsts= Calculate_Firsts_Sufix( right, i+1 );

      if( change) follow[right[i]].Add_All( firsts);
       else change= follow[right[i]].Add_All( firsts);


      if( firsts.Contains_Epsilon() || i== (right.Count-1) ) {
        
        if( change) follow[ right[i] ].Add_All( follow[left]);
         else change= follow[ right[i] ].Add_All( follow[left]);
       
      }

    }

   }
      
   }
    while( change);

    Follow=  follow ;

   }


   public static List<Symbol> Calculate_Firsts_Sufix( List<Symbol> symbols, int ini ) {

    var result= new List<Symbol>() ;
    if( ini>= symbols.Count ) return result;
    bool all_epsilon= true;

    for( int i= ini; i< symbols.Count; i++ ) {

      result.Add_All( Firsts[ symbols[i] ] );
      if( !Firsts[symbols[i]].Contains(epsilon) ) {
        all_epsilon= false;
        break;
      }
    }

    if( all_epsilon ) result.Add( epsilon);

    return result;

   }

   public static string Transform( string s, Stack<Symbol_Node> stack) {

    if( s=="expr" || s=="term" || s=="factor" ) return "Expression" ;
    if( s=="atom" || s=="mol") return "ID, Function_Call, Number or String" ;
    if( s=="condition") return "Condition" ;
    if( s=="list_expr") return "Expression List";
    if( s=="list_arg") return "Argument List" ;
    if( s=="line") return "Expression or Statement";
    if( s=="statement") return "Statement";
    if( s=="list_assignments") return "Assignment List" ;
    if( s=="op") return "Comparison Operator";
    if( s=="aux_expr" || s=="aux_term" || s=="aux_factor" ) {
    var aux1= stack.Pop() ;
    string aux2=aux1.Symbol.Class ;
    return Transform( aux2, stack );
    }
    if(s=="aux_list_arg") return ") or Argument List";
    if(s=="aux_list_expr") return ") or Expression List";
    if(s=="aux_list_assignments") return "token \"in\" or more assignments" ;
    //Modificar estos 3 ultimos para que pidan al usuario agregar comas "
    
    return s;
  
   }


 } 

 public static class Node_Extensions {

  public static bool Check_Errors( this Node node ) {
   
   if( ( node.Symbol=="list_arg" || node.Symbol=="list_expr" ) && node.Children.Count==1 && node.Parent!=null && node.Parent.Children[0].Symbol==",") {
   Operation_System.Print_in_Console( "Syntax Error!! : Expression List or Argument List expected after \",\"") ;
   return true ;
   }
   for(int i= 0; i< node.Children.Count; i++ )
    if( node.Children[i].Check_Errors()) return true ;

    return false ;

  }


 }

