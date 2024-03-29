﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intepretator
{
    class MyControl
    {

/* **********************************************************
   Action Matrix
Index 1,2,....,14 of course the java matrix is indexed from 0 up to
13, therefor the i-1,j-1 in the indexing below.
Index for the different operators:
AM_Become=1(:=),AM_New=2(new not used),AM_Add=3(+ -),AM_Mul=4(* /),
AM_Cmp=5(= <> < > <= >=),AM_Func=6(user function)
AM_Array=7(array not used),AM_Class=8(class not used),AM_Comma=9(,),
AM_Lpar=10( left-( ),AM_Rpar=11(right-) )
AM_Fpar=12( function left-( ),AM_Dot=13( . not used),AM_Empty=14(end operator
or empty stack)
The actions:
S - stack operator
U - execute operator
A - accept (finish action)
E - error
F - stack user function
C - stack function argument (
P - remove ( from stack and skip )
T - transfer parameter
L - transfer last parameter
M - stack user method (called by dot, not used here)
? - not used (error)

********************************************************** */

    private char[][] ActionMatrix =
	{	new char[]{'E','S','S','S','S','F','S','E','U','S','E','E','S','U'},
		new char[]{'E','E','E','E','E','E','E','F','E','E','E','E','?','E'},
		new char[]{'E','E','U','S','U','F','S','E','U','S','U','E','S','U'},
		new char[]{'E','E','U','U','U','F','S','E','U','S','U','E','S','U'},
		new char[]{'E','E','S','S','E','F','S','E','U','S','U','E','S','U'},
		new char[]{'U','E','U','U','U','?','?','E','U','C','U','E','?','U'},
		new char[]{'U','E','U','U','U','?','?','E','U','S','U','E','U','U'},
		new char[]{'U','E','U','U','U','?','?','E','U','C','U','E','?','U'},
		new char[]{'E','E','E','E','E','E','E','E','E','E','E','E','S','E'},
		new char[]{'S','E','S','S','S','F','S','E','E','S','P','E','S','E'},
		new char[]{'E','E','E','E','E','E','E','E','E','E','E','E','E','E'},
		new char[]{'S','E','S','S','S','F','S','E','T','S','L','E','S','E'},
		new char[]{'?','?','U','U','U','M','D','U','U','U','U','U','U','U'},
		new char[]{'S','S','S','S','S','F','S','E','?','S','E','E','S','A'}
	};

	public char GetAction(int i,int j)
	{	if(i==0){i=14;}
		return ActionMatrix[i-1][j-1];
	}
	
/* **********************************************************
   Follow Matrix
Used for errorhandling!
Used to check an operand/operator directly following an operand/operator
A - OK permitted
1,2... - Error 
********************************************************** */
	private char[][] FollowMatrix=
	{	new char[]{'1','A','9','A','A','A','1','1','?','A','1','A','?','A','A'},
		new char[]{'A','2','A','3','3','3','A','A','?','8','A','4','?','?','2'},
		new char[]{'A','2','9','A','A','A','A','A','A','A','A','A','A','?','A'},
		new char[]{'A','2','9','3','3','3','A','A','?','3','A','3','?','?','3'},
		new char[]{'A','2','9','3','3','3','A','A','?','3','A','3','?','?','3'},
		new char[]{'A','2','9','3','3','3','A','A','?','3','A','3','?','?','3'},
		new char[]{'5','2','9','A','A','A','5','5','?','A','A','A','?','?','A'},
		new char[]{'6','2','9','6','6','6','6','6','?','6','A','6','?','?','6'},
		new char[]{'A','2','9','A','A','A','A','A','?','A','A','A','A','?','A'},
		new char[]{'A','2','9','3','3','3','A','A','?','8','A','3','?','?','3'},
		new char[]{'A','2','9','3','3','3','A','A','?','3','A','3','?','?','7'},
		new char[]{'1','A','A','A','A','A','1','1','?','A','1','A','?','A','A'},
		new char[]{'?','?','?','?','?','?','?','?','?','?','?','?','?','?','?'},
		new char[]{'A','A','A','A','A','A','A','A','A','A','A','A','A','A','A'},
		new char[]{'A','2','A','3','3','3','A','A','?','8','A','4','?','?','A'}
	};
	
	public char FollowOp(int I,int K)
	{	return FollowMatrix[I][K];}
	
	private char[][] TypeMatrix=
	{	new char[]{'A','A','A','A','A','A'},
		new char[]{'A','I','R','E','E','E'},
		new char[]{'A','R','R','E','E','E'},
		new char[]{'A','E','E','T','E','E'},
		new char[]{'A','E','E','E','B','E'},
		new char[]{'A','E','E','E','E','P'}
	};
	
	public char GetResType(int I,int K){return TypeMatrix[I][K];}
	
	private char[][] ModeMatrix=
	{	new char[]{'N','N','B','S','N','?'},
		new char[]{'N','N','B','S','N','?'},
		new char[]{'A','A','D','D','A','?'},
		new char[]{'F','F','R','R','F','?'},
		new char[]{'N','N','B','S','L','?'},
		new char[]{'?','?','?','?','?','?'}
	};
	
	public char GetResMode(int I,int K){return ModeMatrix[I][K];}


    }
}
