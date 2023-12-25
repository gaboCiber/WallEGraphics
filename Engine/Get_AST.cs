
 public static class Semantik_Analysis {

   public static Program_Node AST ;
   public static Context Context ;

   public static Instruction To_AST( Node node ) {
   
   if( node.Symbol=="instruction") return To_AST( node.Children[0]);
   if( node.Symbol=="stat_no_computable") return Analize_No_Computable1( node);
   if( node.Symbol=="boolean_op") return To_Expr( node );
   if( node.Symbol=="match_declaration") return new Match( Utils.Cast<ID, Expression>( ((List_Node<Expression>)Analize_List(node.Children[0])).Descompress() ).Filter(), To_Expr( node.Children[2]) );
   
   return null;
  }


  public static Expression To_Expr( this Node node ) {

   if( node.Symbol=="Number") return new Number( node.Chain );
   if( node.Symbol=="string") return new String( node.Chain );
   if( node.Symbol=="ID" || node.Symbol=="_") return new ID( node.Chain );
   if( node.Symbol=="undefined") return new Undefined();
   if( node.Symbol=="variable") return To_Expr( node.Children[0]);
   if( node.Symbol=="let_in") return new Let_In( Analize_List_Instruction(node.Children[1]).Descompress(), To_Expr(node.Children[4]) );
   if( node.Symbol=="if_else") return new If_Else( To_Expr( node.Children[2]), To_Expr( node.Children[5]), To_Expr( node.Children[7]) );
   if( node.Symbol=="stat_computable") return To_Expr( node.Children[0]); 

   if( node.Symbol== "expr" ||  node.Symbol=="factor" ) {
    
    if( node.Children[1].Children.Count== 1) return To_Expr( node.Children[0] );
    else return new Binary_Operation( To_Expr( node.Children[0]), node.Children[1].Children[0].Symbol, To_Expr( node.Children[1].Children[1])  ) ;
  }

   if( node.Symbol== "term" ) {

    Expression tree; 
   if( node.Children[1].Children.Count== 1) tree= To_Expr( node.Children[0] );
    else tree= new Binary_Operation( To_Expr( node.Children[0]), node.Children[1].Children[0].Symbol, To_Expr( node.Children[1].Children[1])  ) ;

    if( node.Parent.Parent.Children[0].Symbol=="-" ) return new Binary_Operation( tree, "*", new Number("-1" ) ) ;
     else return tree ;
   }

   if( node.Symbol== "boolean_op" ) {
    
    if( node.Children[1].Children.Count== 1) return To_Expr( node.Children[0] );
    else return new Boolean_Operation( To_Expr( node.Children[0]), node.Children[1].Children[0].Symbol, To_Expr( node.Children[1].Children[1])  ) ;
  }

  if( node.Symbol== "condition" ) {
    
    if( node.Children[1].Children.Count== 1) return To_Expr( node.Children[0] );
    else return new Condition( To_Expr( node.Children[0]), node.Children[1].Children[0].Symbol, To_Expr( node.Children[1].Children[1])  ) ;
  }

   if( node.Symbol=="atom" ) return Analize_Atom( node);
   if( node.Symbol=="list_expr") return Analize_List( node);
   if( node.Symbol=="list_arg") return Analize_List( node);
   if( node.Symbol=="list_var") return Analize_List( node);
   if( node.Symbol=="inside_keys") return Analize_Inside_Keys( node);

   return null;

  }


   public static Expression Analize_Atom( this Node node ) {

    if( node.Children.Count==1) return To_Expr( node.Children[0] );
    if( node.Children.Count==2) {

      if(node.Children[1].Children.Count== 1) return To_Expr( node.Children[0] );
      else return new Func_Call( (ID)To_Expr( node.Children[0]), ((List_Node<Expression>)To_Expr( node.Children[1].Children[1])).Descompress()  ); 
  
    }

    if( node.Children.Count==3) return To_Expr( node.Children[1] );
      
    if( node.Children.Count==4) {   

      var list= ((List_Node<Expression>)To_Expr( node.Children[2])).Descompress(); 
      return Utils.Get_Figure( node.Children[0].Children[0].Symbol, list );

    }

    return null;

   }


   public static Expression Analize_Inside_Keys( this Node node ) {

     if( node.Children.Count==1 ) return new Collection( new List<Expression>() );

     var expr= To_Expr( node.Children[0]);
     var aux_node= node.Children[1].Children[0];
     if( aux_node.Symbol.Length==0) return new Collection( expr);
     if( aux_node.Symbol=="aux_list_expr" ) {

       var list= ( aux_node.Children.Count==1) ? new List_Node<Expression>() : (List_Node<Expression>)To_Expr( aux_node.Children[1]); 
       list.Add(expr);
       return new Collection( list.Descompress());

     }

     var another_node= node.Children[1].Children[1].Children[0];
     if( another_node.Symbol=="expr") return new Restricted_Sub_Set( expr, To_Expr( another_node) );
     return new Sub_Set( expr);


   }

   public static List_Node<Expression> Analize_List( this Node node ) {

     if( node.Children.Count==1 ) return new List_Node<Expression>();

     if( node.Children[1].Children.Count== 1) return new List_Node<Expression>( To_Expr( node.Children[0] ) ) ;
     
      List_Node<Expression> aux_list1= (List_Node<Expression>)To_Expr( node.Children[1].Children[1]) ;
      
      aux_list1.Add( To_Expr( node.Children[0]) );
      return aux_list1;

   }

   public static Instruction Analize_No_Computable1( Node node ) {

     if( node.Children.Count==1 ) return new Restore();
     Node primogenit= node.Children[0];
     if( primogenit.Symbol=="import") return new Import( (String)To_Expr( node.Children[1] ) );
     if( primogenit.Symbol=="color") return new Color( (String)To_Expr( node.Children[1] ) );
   
     if( primogenit.Symbol=="draw") {
      
      if( node.Children[2].Children[0].Symbol.Length==0)   return new Draw( To_Expr(node.Children[1]) );
      else return new Draw(To_Expr(node.Children[1]), (String)To_Expr(node.Children[2].Children[0]) );

     }

     if( primogenit.Symbol=="figure") {

       if( node.Children[1].Children.Count==1) return new Figure_Declaration( node.Children[0].Children[0].Symbol, (ID)To_Expr( node.Children[1].Children[0]) );
       else return new Figure_Secuence_Declaration( node.Children[0].Children[0].Symbol, (ID)To_Expr( node.Children[1].Children[1] ));

     } 
     
     return Analize_No_Computable2( node );

   }

   public static Instruction Analize_No_Computable2( Node node ) {

     Node aux_node= node.Children[1];
     ID name= (ID)To_Expr(node.Children[0]);
     if( aux_node.Children[0].Symbol=="=") return new Assignment( name.Name, To_Expr( aux_node.Children[1] ) );
     if( aux_node.Children[0].Symbol=="(") return new Def_Func( name, Utils.Cast<ID, Expression>( ((List_Node<Expression>)Analize_List( aux_node.Children[1] )).Descompress() ), To_Expr( aux_node.Children[4] ) );
     if( aux_node.Children[0].Symbol==",") {

      var match= (Match)To_AST(aux_node.Children[1] );
      if( match==null) Console.WriteLine( "Match_null");
      match.Add_ID( name);
      return match;

     }

     return null;

   }


   public static List_Node<Instruction> Analize_List_Instruction( this Node node ) {


     if( node.Children[1].Children.Count== 1) return new List_Node<Instruction>( To_AST( node.Children[0] ) ) ;
     
      List_Node<Instruction> aux_list1=  Analize_List_Instruction(node.Children[1].Children[1]) ;
      
      aux_list1.Add( To_AST( node.Children[0]) );
      return aux_list1;

   }

 }


 