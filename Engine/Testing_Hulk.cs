
 public class Program {

  public static void Main() {
  
  Operation_System.Interface() ;
  
  }

  public static void Aux() {

  foreach( var pair in Parser.Follow.dicc )  {
   Console.WriteLine( "Key" + "  " + pair.Key );
   foreach( var s in pair.Value )
    if( s.IsEpsilon ) Console.WriteLine( "epsilon" + "  ");
    else Console.Write( s.Class + "  " );

    Console.WriteLine( "\n");
  }

  }


   public static void Aux_Table() {

     foreach( var n in Data.gramatik.No_Terminals ) 
      foreach( var t in Data.gramatik.Terminals ) {
       Console.Write( "Si tenemos "+ n.Class + " y "+ t.Class + " entonces aplicamos :    ");
       var list= Parser.table.Search( n, t);
       if( list== null ) Console.Write( "null");
       else {

      for( int i=0; i<list.Count; i++) {

       Production p= list[i] ;
       Console.Write( p.Left.Class + " => ");
       foreach( var s in p.Right )
       Console.Write( s.Class + "");  
       
       Console.Write( "\n") ;

       }    
      }

      Console.Write( "\n");

      }
   }


   public static void Print( Node node, int height ) {

    Console.WriteLine( node.Symbol + "   " + height );
    foreach( var tree in node.Children )
    Print( tree, height+1 );

   }

   public static void Aux1() {

   Binary_Operation aux= new Binary_Operation( new Binary_Operation( new Number("5"),"+", new Number("5")), "/", new Binary_Operation( new Number("3"), "/", new Number("4")));
   Binary_Operation expr= new Binary_Operation( new Number("3"), "*", new Binary_Operation( new Number("12"), "/", aux ));
   Context context= new Context();
   Console.WriteLine( expr.Obtain_Value( context ).Object );

   }

  

 }


 //instructions ..
 // let x= 2<3 & ( let x1=4 in (x1+2)< 5 | 3<5) in (x==true) ;    answer: true
 // let x=1, y=2 in ( if((Sum(x,y)>6 & 5>x) | "lala"=="land" ) (let x1=3 in (x1/x)) + (print(2))*2 else true ) ;   aunswer: true 
 //let x=1, y=2 in ( if((Sum(x,y)>6 & 5>x) | "lala"=="lala" ) let x1=3 in (x1/x) + print(2)*2 else true ) ;   answer: 7
 // ( let x="lala" in ( x+"land")) / (print("_movie")) ;
 // if( (Fib(3)<Fib(4) & Fib(5)>Fib(6) ) | "movie"=="movi" ) 5 else 2<3 & 4>5 ;
 //print(let x=2>3 | 2<3 in (x) & 2<2 ) ;  answer: false
 //print( if(2<3) 2==2 else 2>3 | 2==2 ) ;
 //print( ( if(2<3) 2==2 else 2>3 ) | 2==2 ) ;
 //print( 2>4 | ( true & 2>1 )) ;
 //print(true & true ) ;
 //10-( 3*2-6/6-( 10+5-14) )+2 ;  answer: 8
 //10-Sum(10,5)/15;  aunswer: 9
 //let x=(let x1="lala" in ( let x2="_land" in (x1+x2) ) )+ "_dame_tu_cosita" in (x=="lala_land_dame_tu_cosita") ;  answer: true
 //if( 2<3 & ( 2<1 | "audi"=="audi" ) ) (let x=2*3 in (x))/2 else 4;    answer: 3
 //"yo"+Sum("_", "robot");
 //true & 2<Sum(2,1) & ( "lala"=="lala" );
 //let x=2<3 in (x)!= 2<3 & 5>4;
 //"lala"=="lala" & !"bebe"=="b" & ( 2+3>5 | !(2>4 & 3<2)) ;   true
 //let x=2 in (2) + print(2)/if(2<3) 2 else 1;
 //if(2<1 | 2<2 ) 2 else let x1="lets", x2="_get" in ( x1+x2 ) + print("_loud") ;  aunswer: lets_get_loud
 //let x="lala" in ( x+ if(2>3) "_land" else 3 );
 //30*2 + 30* let x=30*2 in (x-60) * let x=2 in (x) + 2 + if(2<3) 3 else 9;
 


 //line( point(2,3), point(3,4));
 //circle( point(2,3), measure( point(4,5), point(1,1)));
 //draw { point(5,2), line( point(2,3), point(3,4)) };
 //x= undefined + { 1, 2, 3}; x;
 //x,y,_,x1,x2,x3= { 1, 2, 3}; x1; x2; x3;
 //x= { 1, 2, 3} + undefined; x;
 //x= { 1, 2, 3} + { point(2, 4), point(5,3) } + { 5...8}; x;
 //make_rect( p1, p2) = line( p1, p2);  point p1; point p2; make_rect( p1,p2);
 //mediatriz(p1, p2)= let l1= line(p1 ,p2), m= measure( p1,p2), c1= circle( p1, m), c2= circle( p2, m), draw { c1, c2} in ( l1); point p1; point p2; mediatriz(p1, p2);
 //2<3 and 3<4 and 4>5;
 //{ 1, 2, 4} and { point(3, 4), 5, point(4,5) } + { 1...3} or {};
 //let x1= 3; x2= 6; in ( x1+x2) == 2+7; 
 //2+5 == ( if({}) then 30 else 8) and 2 and {1...};
 //2>3 or ( 2<3 and 3==3 );
 //p1= point(0,0); p2= point(2,2); p3= point(4,8); m1= measure(p1,p2); m2= measure( p1,p3); m2; m1; m2/m1;
 //point p1; point p2; point p3; l1= line( p1, p2); l2= line(p1, p3); p1; p,_= intersect( l1, l2); p;
 //p1= point(2,3); c1= circle( p1, measure( p1, point(3,6))); l1= segment( p1, point(4,7)); x,y,z= intersection( l1, c1); p1; x; y; z;
 //p1= point(2,3); c1= circle( p1, measure( p1, point(3,6))); point p2; c2= circle( p2, measure(p1, p2) ); x,y,z= intersection( c1, c2); x; y; z;
 //x,y,_= { point(2,3), point(4, 5) }; c1= circle(x, measure(x,y)); c1;
 //p1, p2, p3= { point(2, 3), point(4,5), point(10, 9) }; l1= line( p1, p3); l2= line( p2, point(6,5)); i1,_= intersect( l1, l2); i1;
//p1, p2, p3= { point(2, 3), point(4,5), point(10, 9) }; l1= segment( p1, p3); m= measure( p2, point(100000,50000)); c1= circle( p1, m); i1,i2,_= intersect( l1, c1); i1; i2;
// p1= point(3, 4); p2= point(10, 9); p3= point(11,34); p4= point(20,5); s1= line( p1, p2); s2=line( p3, p4); i1,_= intersect( s1, s2); i1;
//p1= point(-2, 0); p2= point(2, 0); m= measure( point(0,0), point(0,3)); c1= circle( p1, m); c2= circle(p2, m); i1, i2, _= intersect( c1, c2); i1; i2;    
//point p1; point p2; point p3; m= measure(p1, p2); a= arc( p1, p2, p3, m); a;
//circle secuence c1; x, y, z, _= c1; x; y; z;
//let x=30; x1, x2, _= { 10 ...}; draw point(x,3) "point" in point(x1, x2); x=3; x;
//mediatriz(p1, p2) =  let l1 = line(p1, p2);  m = measure (p1, p2);  c1 = circle (p1, m);  c2 = circle (p2, m);  i1,i2,_ = intersect(c1, c2);  l2 = line(i1, i2)  in l2;  point p; point p2; draw {p, p2}; l= mediatriz(p,p2);
//mediatriz(p1, p2) =  let l1 = line(p1, p2); m = measure (p1, p2);  c1 = circle (p1, m); c2 = circle (p2, m); i1,i2,_ = intersect(c1, c2);  l2 = line(i1, i2); in l2; puntoMedio(p1,p2)= let medio,_ = intersect(line(p1,p2), mediatriz(p1,p2)); in medio;  point p1; point p2;  draw { p1, p2, puntoMedio(p1, p2) } ;
//regularHexagon(p,m) =  let point p2;  l1 = line(p,p2);  c1 = circle(p,m);  i1,i2,_ = intersect(l1,c1);  c2 = circle(i1,m);  c3 = circle(i2,m);  i3,i4,_ = intersect(c2,c1);  i5,i6,_ = intersect(c3,c1) in {i1,i3,i5,i2,i6,i4};  mediatrix(p1, p2) =  let  l1 = line(p1, p2); m = measure (p1, p2);  c1 = circle (p1, m);  c2 = circle (p2, m);  i1,i2,_ = intersect(c1, c2)  in line(i1,i2);   hexagonalStar(p,m) = let  v1,v2,v3,v4,v5,v6,_ = regularHexagon(p,m);  l1 = mediatrix(v1,v2); l2 = mediatrix(v2,v3); l3 = mediatrix(v3,v4);  i1,_ = intersect(l1,line(v3,v4)); i2,_ = intersect(l1,line(v3,v2));  i3,_ = intersect(l2,line(v1,v2));  i4,_ = intersect(l2,line(v1,v6));  i5,_ = intersect(l3,line(v2,v3));  i6,_ = intersect(l3,line(v2,v1))  in {v1,i2,v2,i3,v3,i5,v4,i1,v5,i4,v6,i6};   getSpikes(p1,p2,p3,m) = if m / measure(p2,p3) > 80 then {}  else let  l1 = mediatrix(p1,p2);  l2 = mediatrix(p1,p3); i1,_ = intersect(l1,line(p1,p2));  i2,_ = intersect(l2,line(p1,p3));  i3,_ = intersect(l1,l2);  draw {segment(i1,i3), segment(i2,i3),segment(i3,p1)} in {i1,i2,i3} + getSpikes(i1,p2,i3,m) + getSpikes(i2,p3,i3,m);  drawRecursiveSnowFly(p,m) = let p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12,_ = hexagonalStar(p,m); m1 = measure(p1,p2);  s1 = getSpikes(p1,p2,p12,m);  s2 = getSpikes(p3,p2,p4,m);  s3 = getSpikes(p5,p4,p6,m);  s4 = getSpikes(p7,p6,p8,m); s5 = getSpikes(p9,p8,p10,m);  s6 = getSpikes(p11,p10,p12,m);  draw { segment(p1,p2),segment(p2,p3),segment(p3,p4),segment(p4,p5), segment(p5,p6),segment(p6,p7),segment(p7,p8),segment(p8,p9), segment(p9,p10),segment(p10,p11),segment(p11,p12),segment(p12,p1), segment(p1,p),segment(p2,p),segment(p3,p),segment(p4,p),segment(p5,p), segment(p6,p),segment(p7,p),segment(p8,p),segment(p9,p),segment(p10,p), segment(p11,p),segment(p12,p)  }  in 0;  pu1 = point(150,0);  pu2 = point(0,0); m = measure(pu1,pu2); a = drawRecursiveSnowFly(point(450,300),m);
//mediatrix(p1, p2) = let l1 = line(p1, p2);  m = measure (p1, p2); c1 = circle (p1, m); c2 = circle (p2, m);  i1,i2,_ = intersect(c1, c2)  in line(i1,i2);  drawTriangle(p1,p2,p3) =  let draw {segment(p1,p2), segment(p2,p3), segment(p3,p1)}  in 1;   getTriangleCenter(p1,p2,p3) =  let  a = mediatrix(p1,p2);  b = mediatrix(p2,p3);   i1,_ = intersect(a,b)  in i1;   regularTriangle(p,m) = let point p2;  l1 = line(p,p2);  c1 = circle(p,m);   i1,i2,_ = intersect(l1,c1);  c2 = circle(i1,m);  c3 = circle(i2,m);  i3,i4,_ = intersect(c2,c1);  i5,i6,_ = intersect(c3,c1)  in {i1,i5,i6};  getReverseTriangle(p1,p2,p3) = let center = getTriangleCenter(p1,p2,p3);  c = circle(center, measure(center,p1) + measure(point(0,0),point(0,0.01)));  i2,_ = intersect(ray(p1,center),c);  i3,_ = intersect(ray(p2,center),c);  i1,_ = intersect(ray(p3,center),c)  in {i1,i2,i3};  findSubTriangle(pPivo,p2,p3,pl1,pl2) =  let i1,_ = intersect(line(pPivo,p2),line(pl1,pl2));  i2,_ = intersect(line(pPivo,p3),line(pl1,pl2))  in {pPivo,i1,i2};   KorshSnowFly(p1,p2,p3,cant) = if cant > 0 then let  x = drawTriangle(p1,p2,p3); t1,t2,t3,_ = getReverseTriangle(p1,p2,p3); t11,t12,t13,_ = findSubTriangle(p1,p2,p3,t1,t3);  t21,t22,t23,_ = findSubTriangle(t1,t2,t3,p1,p2);  t31,t32,t33,_ = findSubTriangle(p2,p1,p3,t1,t2); t41,t42,t43,_ = findSubTriangle(t2,t3,t1,p2,p3);  t51,t52,t53,_ = findSubTriangle(p3,p1,p2,t2,t3);  t61,t62,t63,_ = findSubTriangle(t3,t1,t2,p3,p1); color "red";  x1 = KorshSnowFly(t11,t12,t13,cant-1); color "blue"; x2 = KorshSnowFly(t21,t22,t23,cant-1);  color "yellow";  x3 = KorshSnowFly(t31,t32,t33,cant-1);  color "green"; x4 = KorshSnowFly(t41,t42,t43,cant-1);  color "magenta"; x5 = KorshSnowFly(t51,t52,t53,cant-1); color "cyan";  x6 = KorshSnowFly(t61,t62,t63,cant-1) in 1 else 1; i1,i2,i3,_ = regularTriangle(point(250,250),measure(point(0,0),point(0,150))); k = KorshSnowFly(i1,i2,i3,4);
//p1= point(6,6); p2= point(5,5); l= ray(p1, p2); m= measure(point(0,0), point(0,1)); c= circle( point(1,1), m) ; i1, i2,_= intersect( l, c ); i1; i2;
//p1= point(1,1); p2= point(0, 0); l= ray(p1, p2); m= measure(point(0,0), point(0,1)); c= circle( point(1,1), m) ; i1, i2,_= intersect( l, c ); i1; i2;
//p1= point( 1,2); p2= point( 3,5); l1= line( p1, p2); l2= line( point(3,2), point(3,5)); i1,_= intersect( l1, l2); i1;
//p1= point( 1,1); p2= point( 2,2); l1= line( p1, p2); l2= line( point(3,2), point(3,5)); i1,_= intersect( l1, l2); i1;
//l= line( point(2,3), point(2,5)); m= measure( point(0,0), point(0,2)); c= circle( point( 2,0), m); i1, i2, _= intersect( l,c); i1; i2;
//m1= measure( point(0,0), point(30, 0)); m2= measure( point(0,0), point(0, -10)); x= m1 + m2; x;
//count( { point(2,3), point(4,5) } );
//l1= line(point(0,1), point(-1,0)); center= point(0,0); initial= point(1,1); final= point(-1, 1); m= measure( center, point(0,2)); a= arc( center, initial, final, m ); i1, i2, i3, _= intersect( a, l1); i1; i2; i3;
//mediatrix(p1, p2) = let l1 = line(p1, p2);   m = measure (p1, p2);   c1 = circle (p1, m);   c2 = circle (p2, m);  i1,i2,_ = intersect(c1, c2);   in line(i1,i2);   regularTriangle(p,m) = let  point p2; l1 = line(p,p2);  c1 = circle(p,m);   i1,i2,_ = intersect(l1,c1);  c2 = circle(i1,m);  c3 = circle(i2,m);  i3,i4,_ = intersect(c2,c1);  i5,i6,_ = intersect(c3,c1); in {i1,i5,i6};    divideTriangle(p1,p2,p3,m1) = if (measure(p1,p2)/m1) < 5 then  {} else   let  draw {segment(p1,p2),segment(p2,p3),segment(p3,p1)};  mid1,_ = intersect(mediatrix(p1,p2),line(p1,p2));  mid2,_ = intersect(mediatrix(p2,p3),line(p2,p3));  mid3,_ = intersect(mediatrix(p1,p3),line(p1,p3));   a = divideTriangle(p2,mid2,mid1,m1);  b = divideTriangle(p1,mid1,mid3,m1);   c = divideTriangle(p3,mid2,mid3,m1);  in {};   sierpinskyTriangle(p,m) =  let  pu1 = point(0,0); pu2 = point(0,1);  p1,p2,p3,_ = regularTriangle(p,m);  in divideTriangle(p1,p2,p3,measure(pu1,pu2));  pu1 = point(300,0);  pu2 = point(0,0); m = measure(pu1,pu2);  a = sierpinskyTriangle(point(450,300),m);
//min2(x, y) = if x <= y then  x  else y;  print min2(1,2) "min2 ";   min(s) = let v, tail = s; in  if tail then min2(v, min(tail))  else  v;  print min({3 ... 5}) "min sec ";


