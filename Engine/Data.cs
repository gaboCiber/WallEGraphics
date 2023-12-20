
public static class Data {

   public static Gramatik gramatik;
   public static Symbol EOF;
   public static Symbol Epsilon;
   public static void Create_Gramatik() {
   
   //no_terminals...
   var instruction= new Symbol("instruction");
   var stat_no_computable= new Symbol("stat_no_computable");
   var figure= new Symbol("figure");
   var aux_arg_declaration= new Symbol("aux_arg_declaration");
   var option= new Symbol("option");
   var aux_no_computable= new Symbol("aux_no_computable");
   var match_declaration= new Symbol("match_declaration");
   var list_var= new Symbol("list_var");
   var variable= new Symbol("variable");
   var aux_list_var= new Symbol("aux_list_var");
   var stat_computable= new Symbol("stat_computable");
   var let_in= new Symbol("let_in" );
   var if_else= new Symbol("if_else" );
   var expr= new Symbol("expr" );
   var term= new Symbol("term" );
   var factor= new Symbol("factor" );
   var mol= new Symbol("mol");
   var atom= new Symbol("atom");
   var aux_expr= new Symbol("aux_expr" );
   var aux_factor= new Symbol("aux_factor" );
   var aux_term= new Symbol("aux_term" );
   var aux_id= new Symbol("aux_id");
   var inside_keys= new Symbol("inside_keys");
   var aux_secuence= new Symbol("aux_secuence");
   var after_dots= new Symbol("after_dots");
   var list_expr= new Symbol("list_expr" );
   var aux_list_expr= new Symbol("aux_list_expr");
   var list_arg= new Symbol( "list_arg");
   var aux_list_arg= new Symbol( "aux_list_arg");
   var list_instruction= new Symbol("list_instruction");
   var aux_list_instruction= new Symbol("aux_list_instruction");

   var boolean_op= new Symbol("boolean_op");
   var aux_boolean_op= new Symbol("aux_boolean_op");
   //terminals...
   var id= new Symbol("ID" ); 
   var number= new Symbol("Number" );
   var strings= new Symbol("string" );
   var restore= new Symbol("restore");
   var import= new Symbol("import");
   var draw= new Symbol("draw");
   var color= new Symbol("color");
   var secuence= new Symbol("secuence");
   var s_= new Symbol("_");
   var open_key= new Symbol("{");
   var closed_key= new Symbol("}");
   var undefined= new Symbol("undefined");
   var point= new Symbol("point");
   var arc= new Symbol("arc");
   var line= new Symbol("line");
   var segment= new Symbol("segment");
   var ray= new Symbol("ray");
   var measure= new Symbol("measure");
   var circle= new Symbol("circle");
   var dots= new Symbol("...");
   var let= new Symbol("let" );
   var inn= new Symbol("in" );
   var conditional_if= new Symbol("if" );
   var conditional_then= new Symbol("then");
   var conditional_else= new Symbol("else" );
   var or_logic= new Symbol("or") ;
   var and_logic= new Symbol( "and") ;
   var not_logic= new Symbol("not") ;
   var plus= new Symbol("+" );
   var sub= new Symbol("-" );
   var mult= new Symbol("*" );
   var div= new Symbol("/" ); 
   var pow= new Symbol("^" ); 
   var eq= new Symbol("=" ); 
   var open= new Symbol("(" ); 
   var close= new Symbol(")" ); 
   var M= new Symbol(">" );
   var m= new Symbol("<" ); 
   var Meq= new Symbol(">=" );
   var meq= new Symbol("<=" );
   var eqeq= new Symbol("==" );
   var distint= new Symbol("!=") ;
   var coma= new Symbol(",");
   var dot= new Symbol( ".");
   var epsilon= new Symbol("");
   var eof= new Symbol("$");
   
   Symbol[]no_terminals= { instruction, stat_no_computable, figure, aux_arg_declaration, option, aux_no_computable, match_declaration, list_var, variable, aux_list_var, stat_computable, let_in, if_else, boolean_op, aux_boolean_op, expr, term, factor, mol, atom, aux_expr, aux_factor, aux_term, aux_id, inside_keys, aux_secuence, after_dots, list_expr, list_arg, list_instruction, aux_list_expr, aux_list_arg, aux_list_instruction };
   Symbol[]terminals= { id, number, strings, import, color, restore, draw, secuence, point, line, segment, ray, circle, arc, measure, undefined, s_, let, inn, conditional_if, conditional_then, conditional_else, or_logic, and_logic, not_logic, plus, sub, mult, div, pow, eq, open, close, open_key, closed_key, M, m, Meq, meq, eqeq, distint, coma, dot, dots, epsilon, eof } ;
   

  //no_computables...
   var p_instruction1= new Production( instruction, expr);
   var p_instruction2= new Production( instruction, stat_no_computable);
   var p_stat_no_computable1= new Production( stat_no_computable, color, strings);
   var p_stat_no_computable2= new Production( stat_no_computable, restore);
   var p_stat_no_computable3= new Production( stat_no_computable, import, strings);
   var p_stat_no_computable4= new Production( stat_no_computable, draw, expr, option);
   var p_stat_no_computable5= new Production( stat_no_computable, variable, aux_no_computable );
   var p_stat_no_computable6= new Production( stat_no_computable, figure, aux_arg_declaration );
   var p_option1= new Production( option, strings);
   var p_option2= new Production( option, epsilon);
   var p_aux_arg_declaration1= new Production( aux_arg_declaration, id );
   var p_aux_arg_declaration2= new Production( aux_arg_declaration, secuence, id );
   var p_aux_no_computable1= new Production( aux_no_computable, coma, match_declaration );
   var p_aux_no_computable2= new Production( aux_no_computable, open, list_arg, close, eq, expr );
   var p_aux_no_computable3= new Production( aux_no_computable, eq, expr );
   var p_match_declaration= new Production( match_declaration, list_var, eq, expr);
   var p_list_var= new Production( list_var, variable, aux_list_var);
   var p_var1= new Production( variable, id);
   var p_var2= new Production( variable, s_);
   var p_aux_list_var1= new Production( aux_list_var, coma, list_var);
   var p_aux_list_var2= new Production( aux_list_var, epsilon );
   var p_list_instruction= new Production( list_instruction, instruction, aux_list_instruction );
   var p_aux_list_instruction1= new Production( aux_list_instruction, coma, list_instruction );
   var p_aux_list_instruction2= new Production( aux_list_instruction, epsilon );
   //computables...
   var p_stat_computable1 = new Production( stat_computable, let_in );
   var p_stat_computable2 = new Production( stat_computable, if_else );
   var p_let_in = new Production( let_in, let, list_instruction, inn, open, expr, close );
   var p_if_else = new Production( if_else, conditional_if, open, expr, close, conditional_then, expr, conditional_else, expr );
   var p_expr = new Production( expr, term, aux_expr ); 
   var p_aux_expr1 = new Production( aux_expr, plus, expr );
   var p_aux_expr2 = new Production( aux_expr, sub, expr );
   var p_aux_expr3 = new Production( aux_expr, epsilon );
   var p_term1= new Production( term, factor, aux_term ) ;  
   var p_term2= new Production( term, stat_computable, aux_term ) ;         
   var p_aux_term1= new Production( aux_term, mult, term );
   var p_aux_term2= new Production( aux_term, div, term );
   var p_aux_term3= new Production( aux_term, epsilon);
   var p_factor = new Production( factor, atom, aux_factor );                      
   var p_aux_factor1 = new Production( aux_factor, pow, atom );
   var p_aux_factor2 = new Production( aux_factor, epsilon );
   var p_atom1 = new Production( atom, id, aux_id );
   var p_atom2 = new Production( atom, number );
   var p_atom3 = new Production( atom, strings );
   var p_atom4= new Production( atom, open, expr, close );
   var p_atom5= new Production( atom, open_key, inside_keys, closed_key );
   var p_atom6= new Production( atom, figure, open, list_expr, close);
   var p_atom7= new Production( atom, undefined);
   var p_aux_id1= new Production( aux_id, open, list_expr, close );
   var p_aux_id2= new Production( aux_id, epsilon );
   var p_inside_keys1= new Production( inside_keys, expr, aux_secuence );
   var p_inside_keys2= new Production( inside_keys, epsilon );
   var p_aux_secuence1= new Production( aux_secuence, dots, after_dots );
   var p_aux_secuence2= new Production( aux_secuence, aux_list_expr);
   var p_aux_secuence3= new Production( aux_secuence, epsilon);
   var p_after_dots1= new Production( after_dots, expr );
   var p_after_dots2= new Production( after_dots, epsilon );
   var p_figure1= new Production( figure, line );
   var p_figure2= new Production( figure, circle );
   var p_figure3= new Production( figure, segment );
   var p_figure4= new Production( figure, ray);
   var p_figure5= new Production( figure, arc );
   var p_figure6= new Production( figure, point );
   var p_figure7= new Production( figure, measure );
   var p_list_expr1 = new Production( list_expr, expr, aux_list_expr );
   var p_list_expr2 = new Production( list_expr, epsilon );
   var p_aux_list_expr1 = new Production( aux_list_expr, coma, list_expr );
   var p_aux_list_expr2 = new Production( aux_list_expr, epsilon );
   var p_list_arg1= new Production( list_arg, id, aux_list_arg );
   var p_list_arg2= new Production( list_arg, epsilon );
   var p_aux_list_arg1 = new Production( aux_list_arg, coma, list_arg );
   var p_aux_list_arg2 = new Production( aux_list_arg, epsilon ); 
   

   Production[]productions = { p_instruction1, p_instruction2, p_stat_no_computable1, p_stat_no_computable2, p_stat_no_computable3, p_stat_no_computable4, p_stat_no_computable5, p_stat_no_computable6, p_option1, p_option2, p_aux_arg_declaration1, p_aux_arg_declaration2, p_aux_no_computable1, p_aux_no_computable2, p_aux_no_computable3, p_match_declaration, p_list_var, p_var1, p_var2, p_aux_list_var1, p_aux_list_var2, p_stat_computable1, p_stat_computable2, p_let_in, p_if_else, p_expr, p_term1, p_term2, p_factor, p_atom1, p_atom2, p_atom3, p_atom4, p_atom5, p_atom6, p_atom7, p_aux_id1, p_aux_id2, p_inside_keys1, p_inside_keys2, p_aux_secuence1, p_aux_secuence2, p_aux_secuence3, p_after_dots1, p_after_dots2, p_figure1, p_figure2, p_figure3, p_figure4, p_figure5, p_figure6, p_figure7, p_aux_expr1, p_aux_expr2, p_aux_expr3, p_aux_factor1, p_aux_factor2, p_aux_term1, p_aux_term2, p_aux_term3, p_list_expr1, p_list_expr2, p_list_arg1, p_list_arg2, p_list_instruction, p_aux_list_arg1, p_aux_list_arg2, p_aux_list_expr1, p_aux_list_expr2, p_aux_list_instruction1, p_aux_list_instruction2 };
   var aux_productions= new List<Production>();
   var aux_no_terminals= new List<Symbol>();
   var aux_terminals= new List<Symbol>();

   for( int i= 0; i< productions.Length; i++ ) 
     aux_productions.Add( productions[i]);
   
   for( int i= 0; i< no_terminals.Length; i++ ) 
     aux_no_terminals.Add( no_terminals[i]);
   
   for( int i= 0; i< terminals.Length; i++ ) 
     aux_terminals.Add( terminals[i]);
   
   EOF = eof ;
   Epsilon= epsilon;
   gramatik= new Gramatik { Productions= aux_productions, No_Terminals= aux_no_terminals, Terminals= aux_terminals, Initial= instruction };

   }

  }

