grammar OberonGrammar;

@header {
using System.Collections.Generic;
using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Statements;
}

@members {
	internal Stack<Block> blockStack = new Stack<Block>();
	internal Block currentBlock;

	public Module module = new Module(null);

	private bool isVar(string id, Block block) {
		return block.LookupVar(id, true) != null;
	}

	internal Block PushBlock() {
		var block = new Block(currentBlock, module);
		blockStack.Push(currentBlock);
		currentBlock = block;
		return block;
	}

	internal Block PushBlock(Block block) {
		blockStack.Push(currentBlock);
		currentBlock = block;
		return block;
	}

	internal void PopBlock() {
		currentBlock = blockStack.Pop();
	}
}

moduleDefinition: 
		MODULE n=ID ';'
		{ 
			module.Name = $n.text; 
			currentBlock = module.Block;
		}
		declarations
		rId=block
		'.'
		;

declarations:
		( procedureDeclaration | localDeclaration | importDefinition ) *
		;

importDefinition:
		IMPORT id=ID ';'
		;

procedureDeclaration:
		p=procedureHeader 
		(procedureDeclaration|localDeclaration)*
		endname=block
		';'
		;

procedureHeader 
	returns[FunctionDeclaration proc]
	locals[Block procBlock]
	: { $procBlock = PushBlock(); }
		PROCEDURE name=ID (pps=procedureParameters)? (export=STAR)? ';'
		;

procedureParameters returns [ProcedureParameterDeclaration[] params]:
		'(' (p+=procedureParameter ';') * p+=procedureParameter ')'
		;

procedureParameter returns[ProcedureParameterDeclaration param] locals[bool isVar]
@init{
	$isVar = false;
}
		:
		( VAR {$isVar = true;} )? name=ID ':' t=typeName
		;

typeName
	returns [TypeDefinition returnType]
	locals [RecordTypeDefinition recordType = new RecordTypeDefinition()]
		: ID															# simpleTypeName
		| ARRAY e=expression OF t=typeName		# arrayType
		| RECORD r=recordTypeNameElements END						# recordTypeName
		;

recordTypeNameElements
	returns [TypeDefinition returnType]
	locals [RecordTypeDefinition record = new RecordTypeDefinition()]
		: recordElement[$record] (';' recordElement[$record])*
		;

recordElement[RecordTypeDefinition record]
		: (ids+=ID ',')* ids+=ID ':' t=typeName
		;

localDeclaration
		: variableDeclaration
		| constDeclaration
		| typeDeclaration
		;

typeDeclaration:
		TYPE
		  singleTypeDeclaration+
		  ;

singleTypeDeclaration:
		  id=ID export=STAR? '=' t=typeName ';'
		  ;

variableDeclaration:
		VAR
		  singleVariableDeclaration+
		  ;

singleVariableDeclaration:
			(v+=exportableID ',')* v+=exportableID  ':' t=typeName ';'
			;

exportableID:
			ID (export=STAR)?
			;

constDeclaration:
		CONST
		  constDeclarationElement+
		  ;

constDeclarationElement:
		c=ID export=STAR? '=' e=expression ';'
		;

block returns [IToken ret]
		: (BEGIN statements)? END ID
		{ $ret = $ID; }
		;

statements:
		statement
		( ';' statement )*
		;

statement
		: assign_statement
		| procCall_statement
		| while_statement
		| repeat_statement
		| if_statement
		| 
		;

procCall_statement
		: id=ID ('(' cp=callParameters ')')?
		;

assign_statement
		: id=ID s=selector[currentBlock.LookupVar($id.text)] ':=' r=expression
		;

while_statement
	locals[WhileStatement ws]
	@init{
		$ws = new WhileStatement(currentBlock);
		PushBlock($ws.Block);
	}
		: WHILE r=expression DO
		  statements
		  END
		  { PopBlock(); }
		;

repeat_statement
	locals[RepeatStatement rs]
	@init{
		$rs = new RepeatStatement(currentBlock);
		PushBlock($rs.Block);
	}
		: REPEAT
		  statements
		  UNTIL r=expression 
		  { PopBlock(); }
		;

if_statement
	locals[IfStatement ifs, Block thenBlock]
	@init{
		$ifs = new IfStatement();
	}
		: IF c+=expression THEN
			  { $thenBlock = PushBlock(); }
			  statements
			  { $ifs.ThenParts.Add($thenBlock); PopBlock(); }
		  ( ELSIF c+=expression THEN
			  { $thenBlock = PushBlock(); }
			  statements
			  { $ifs.ThenParts.Add($thenBlock); PopBlock(); }
		  )* 
		  (ELSE 
			  { $thenBlock = PushBlock(); }
			  statements
			  { $ifs.ElsePart = $thenBlock; PopBlock(); }
		  )? 
		  END
		;

// Expressions
expression
	returns[Expression expReturn]
	: op=(NOT | MINUS) e=expression											#exprNotExpression
	| l=expression op=(STAR | DIV | MOD | AND) r=expression					#exprMultPrecedence
	| l=expression op=('+' | '-' | OR)  r=expression						#exprFactPrecedence
	| l=expression op=('<' | '<=' | '>' | '>=' | '=' | '#') r=expression	#exprRelPrecedence
	| id=ID s=selector[currentBlock.LookupVar($id.text)]					#exprSingleId
	| id=ID '(' cp=callParameters? ')'										#exprFuncCall
	| '(' e=expression ')'													#exprEmbeddedExpression
	| int_n=IntegerNumber													#exprIntegerNumber
	| int_r=FloatingNumber													#exprFloatingNumber
	| s=STRING_LITERAL														#exprStringLiteral
	;

callParameters
		: p+=expression (',' p+=expression)*
		;

selector[Declaration referenceId] returns [VariableSelector vsRet]
	: i+=arrayOrRecordSelector*
	;

arrayOrRecordSelector returns [BaseSelectorElement selRet]
	: '[' e=expression ']'		# arraySelector
	| '.' ID									# recordSelector
	;

// lexer tokens
STRING_LITERAL
   : '\'' ('\'\'' | ~ ('\''))* '\''
   ;

IntegerNumber: DigitSequence;

FloatingNumber
	:   FractionalNumber ExponentPart? 
	|   DigitSequence ExponentPart 
	;

fragment
FractionalNumber
	:   DigitSequence? '.' DigitSequence
	|   DigitSequence '.'
	;

fragment
ExponentPart
	:   'e' Sign? DigitSequence
	|   'E' Sign? DigitSequence
	;

fragment
Sign
	:   '+' | '-'
	;

fragment
DigitSequence
	:   Digit+
	;

fragment
Digit: [0-9];

Whitespace
		:   [ \t]+
		-> skip
		;

Newline
		:   (   '\r' '\n'?
		|   '\n'
		)
		-> skip
		;

BlockComment
		:   '(*' .*? '*)'
		-> skip
		;


SEMI:		';';
COLON:		':';
DOT:		'.';
LPAREN:		'(';
RPAREN:		')';
COMMA:		',';
PLUS:		'+';
AND:		'&';
MINUS:		'-';
NOTEQUAL:	'#';
EQUAL:		'=';
STAR:		'*';
NOT:		'~';
LT:			'<';
LE:			'<=';
GT:			'>';
GE:			'>=';
Assign:		':=';

/* keywords */
MODULE:		'MODULE';
IMPORT:		'IMPORT';
VAR:		'VAR';
BEGIN:		'BEGIN';
CONST:		'CONST';
END:		'END';
PROCEDURE:	'PROCEDURE';
TYPE:		'TYPE';
ARRAY:		'ARRAY';
OF:			'OF';
OR:			'OR';
RECORD:		'RECORD';
WHILE:		'WHILE';
DO:			'DO';
IF:			'IF';
THEN:		'THEN';
ELSE:		'ELSE';
ELSIF:		'ELSIF';
REPEAT:		'REPEAT';
UNTIL:		'UNTIL';
DIV:		'DIV';
MOD:		'MOD';

ID:         [a-zA-Z_] [_a-zA-Z0-9]*;

/* move all lexer errors to the parser */
ErrorChar : . ;
