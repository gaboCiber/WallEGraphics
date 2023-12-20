

  public class Context  {

   public Dictionary<string, object> variables;
   public Dictionary<string, KeyValuePair<int, Def_Func>> functions;
   public Dictionary<string, Expression> heap;
   public Dictionary<string, List<int> > predeterm_functions;
   public Stack<string> colors;
   public List<Figure> output;

   public Context Parent ;

   public Context() {
    
    variables= new Dictionary<string, object>();
    functions= new Dictionary<string, KeyValuePair<int, Def_Func>>();
    heap= new Dictionary<string, Expression>();
    predeterm_functions= new Dictionary<string, List<int>>();
    output= new List<Figure>();
    colors= new Stack<string>();
    colors.Push("black");
    
   }

   public Context Create_Chield() {

     var result = new Context() ;
     result.Parent= this ;
     return result ;

  }
   
   public bool Is_Defined( string variable ) {  return variables.ContainsKey( variable) || heap.ContainsKey(variable) || ( Parent!= null &&  Parent.Is_Defined( variable) )  ;  }

   public bool Is_Defined( string function, int args ) {  return ( functions.ContainsKey( function) && functions[ function ].Key== args )  ||  ( Parent!= null &&  Parent.Is_Defined( function, args )  )  ;    }


   public bool Define_By_Value( string variable ) {

    if( variables.ContainsKey( variable) || heap.ContainsKey( variable) ) return false ;
    variables.Add( variable, null );
    return true ;

   }

   public bool Define( string function, int args, Def_Func node ) {

   if( functions.ContainsKey( function) ) return false ;
   functions[ function ]= new KeyValuePair<int, Def_Func>( args, node ) ;
   Console.WriteLine( "Guardando la funcion {0} en el context", function);
   return true ;

   }

   public bool Define_By_Value( string variable, Object value ) {

    if( variables.ContainsKey( variable ) || heap.ContainsKey( variable ) ) return false;
    variables[variable]= value ;
    return true;

   }

   
    public bool Define_In_Heap( string variable, Expression value ) {

    if( variables.ContainsKey( variable ) || heap.ContainsKey( variable ) ) return false;
    heap[variable]= value;
    return true;

   }

   public bool Define( string variable, object value) {

    if( variables.ContainsKey( variable ) || heap.ContainsKey( variable ) ) return false;
    
    if( value is Secuence ) heap[variable]= (Secuence)value;
    else variables[variable]= value;
    return true;
   }

   public bool Is_In_Heap( string variable) { return heap.ContainsKey( variable); }
   
   public object Obtain_Value( string variable ) { 
    
    if( variables.ContainsKey( variable )) return variables[variable] ;  
    if( heap.ContainsKey( variable) ) return heap[variable];
     if( Parent== null ) return null ;
    else return Parent.Obtain_Value( variable );
    
    }

   public Def_Func Obtain_Node( string function, int args ) {  

    if( functions.ContainsKey( function) && functions[function].Key== args ) return functions[function].Value ;
    if( Parent== null ) return null ;
    else return Parent.Obtain_Node( function, args );

    }

    public bool Is_Predeterm( string function, int args) {  return predeterm_functions.ContainsKey(function) &&  predeterm_functions[function].Contains(args);   }
    
    public void Define_As_Predeterm( string function, int args) {

      if( predeterm_functions.ContainsKey(function)) predeterm_functions[function].Add(args);
      var temp= new List<int>();
      temp.Add(args);
      predeterm_functions[function]= temp;
    }

    public void Introduce_Functions() {

     string[] names= { "sin", "cos", "samples", "randoms", "points" } ;
     int[]args= { 1, 1, 0, 0, 1 } ;
     for( int i=0; i< names.Length; i++) 
      Define_As_Predeterm( names[i], args[i] ) ;
      
    }

    public void Introduce_Color(string color) { colors.Push(color);  }
    public void Remove_Top() { if(colors.Count>1) colors.Pop(); }
    public string Get_Color() { return colors.Peek(); }
    public List<Figure> Get_Figures() { return output;  }
    public void Add_Figure( Figure fig) { output.Add( fig);  }

  }

  
  public class List_Node<T> : Expression {

    public List<T> list ;
    public List_Node( T expr )  { 

      var result= new List<T>() ;
      result.Add( expr );
      list= result ;

      }

    public List_Node() { list= new List<T>();  }

    public List_Node( T expr, List<T> list_expr )  {

     var result= new List<T>() ;
     result.Add( expr );
     for( int i=0; i< list_expr.Count; i++ )
      result.Add( list_expr[i] ); 

    }

   public List<T> Descompress() { return list ;  } 

   public void Add( T item ) {  list.Insert( 0, item ) ;  }

   public override Bool_Object Evaluate( Context context ) { return new Bool_Object( true, null ) ;  }

  }


  public class Bool_Object {

   public bool Bool ;
   public object Object ;

   public Bool_Object( bool truth, object obj ) {

    Bool= truth ;
    Object= obj ;
   }

  }

  
 
    

  

