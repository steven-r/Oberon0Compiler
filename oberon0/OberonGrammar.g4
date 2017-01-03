grammar OberonGrammar;

@header {
using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Statements;
}

@members {
	private bool isVar(string id, Block block) {
		return block.LookupVar(id, true) != null;
	}
}

module returns [Module modres]: 
		MODULE n=ID ';'
		{ 
			$modres = new Module(); 
			$modres.Name = $n.text; 
		}
		declarations[$modres.Block]
		rId=block[$modres.Block]
		{
			if ($rId.ret != $n.text) {
				NotifyErrorListeners(_localctx.n, "The name of the module does not match the end node", null);
			}
		}
		'.'
		;

declarations[Block bParam]:
		( procedureDeclaration[bParam] | localDeclaration[bParam] ) *
		;

procedureDeclaration[Block bParam]:
		p=procedureHeader[bParam]
		localDeclaration[$p.proc.Block]*
		block[$p.proc.Block]
		';'
		;

procedureHeader[Block bParam] returns[FunctionDeclaration proc]:
		PROCEDURE name=ID (r=procedureParameters[bParam])? ';'
		;

procedureParameters[Block bParam] returns [ProcedureParameter[] params]:
		'(' (p+=procedureParameter[bParam] ';') * p+=procedureParameter[bParam] ')'
		;

procedureParameter[Block bParam] returns[ProcedureParameter param] locals[bool isVar]
@init{
	$isVar = false;
}
		:
		( VAR {$isVar = true;} )? name=ID ':' t=typeName[bParam]
		;

typeName[Block bParam] 
	returns [TypeDefinition returnType]
	locals [RecordTypeDefinition recordType = new RecordTypeDefinition()]
		: ID															# simpleTypeName
		| ARRAY e=simpleExpression[bParam] OF t=typeName[bParam]		# arrayType
		| RECORD r=recordTypeNameElements[bParam] END						# recordTypeName
		;

recordTypeNameElements[Block bParam]
	returns [TypeDefinition returnType]
	locals [RecordTypeDefinition record = new RecordTypeDefinition()]
		: recordElement[bParam, $record] (';' recordElement[bParam, $record])*
		;

recordElement[Block bParam, RecordTypeDefinition record]
		: (ids+=ID ',')* ids+=ID ':' t=typeName[bParam]
		;

localDeclaration[Block bParam]
		: variableDeclaration[bParam]
		| constDeclaration[bParam]
		| typeDeclaration[bParam]
		;

typeDeclaration[Block bParam]:
		TYPE
          singleTypeDeclaration[bParam]+
		  ;

singleTypeDeclaration[Block bParam]:
		  id=ID '=' t=typeName[bParam] ';'
		  ;

variableDeclaration[Block bParam]:
		VAR
          singleVariableDeclaration[bParam]+
		  ;

singleVariableDeclaration[Block bParam]:
		  (v+=ID ',')* v+=ID ':' t=typeName[bParam] ';'
		  ;

constDeclaration[Block bParam]:
		CONST
		  constDeclarationElement[bParam]+
		  ;

constDeclarationElement[Block bParam]:
		c=ID '=' e=relationalExpression[bParam] ';'
		;

block[Block bParam] returns [string ret]
		: (BEGIN statements[bParam, bParam])? END ID
		{ $ret = $ID.text; }
		;

statements[Block bParam, Block container]:
		statement[bParam, container]
		( ';' statement[bParam, container] )*
		;

statement[Block bParam, Block container]
		: {isVar(CurrentToken.Text, $bParam)}? assign_statement[bParam, container]
		| {!isVar(CurrentToken.Text, $bParam)}? procCall_statement[bParam, container]
		| while_statement[bParam, container]
		| repeat_statement[bParam, container]
		| if_statement[bParam, container]
		| { NotifyErrorListeners("Statement expected"); }
		;

procCall_statement[Block bParam, Block container]
		: id=ID ('(' cp=callParameters[bParam] ')')?
		;

assign_statement[Block bParam, Block container]
		: id=ID s=selector[bParam, $bParam.LookupVar($id.text)] ':=' r=relationalExpression[bParam]
		;

while_statement[Block bParam, Block container]
	locals[WhileStatement ws]
	@init{
		$ws = new WhileStatement(bParam);
	}
		: WHILE r=relationalExpression[bParam] DO
		  statements[bParam, $ws.Block]
		  END
		;

repeat_statement[Block bParam, Block container]
	locals[RepeatStatement rs]
	@init{
		$rs = new RepeatStatement(bParam);
	}
		: REPEAT
		  statements[bParam, $rs.Block]
		  UNTIL r=relationalExpression[bParam]
		;

if_statement[Block bParam, Block container]
	locals[IfStatement ifs, Block thenBlock]
	@init{
		$ifs = new IfStatement(bParam);
	}
		: IF c+=relationalExpression[bParam] THEN
			  { $thenBlock = new Block(bParam); }
			  statements[bParam, $thenBlock]
			  { $ifs.ThenParts.Add($thenBlock); }
		  ( ELSIF c+=relationalExpression[bParam] THEN
			  { $thenBlock = new Block(bParam); }
			  statements[bParam, $thenBlock]
			  { $ifs.ThenParts.Add($thenBlock); }
		  )* 
		  (ELSE 
			  { $thenBlock = new Block(bParam); }
			  statements[bParam, $thenBlock]
			  { $ifs.ElsePart = $thenBlock; }
		  )? 
		  END
		;

// Expressions
relationalExpression[Block bParam]
	returns[Expression expReturn]
	: s+=simpleExpression[bParam] ( op+=('<' | '<=' | '>' | '>=' | '=' | '#') s+=simpleExpression[bParam] ) *
	;

simpleExpression[Block bParam]
	returns[Expression expReturn]
	: f+=factorExpression[bParam] ( op+=('+' | '-' | OR)  f+=factorExpression[bParam] )*
	;

factorExpression[Block bParam]
	returns[Expression expReturn]
	: s+=expressionPrefix[bParam] ( op+=('*' | DIV | MOD | '&') s+=expressionPrefix[bParam] )*
	;

expressionPrefix[Block bParam]
	returns[Expression expReturn]
	: '-' t=expressionTerm[bParam]		#unaryExpressionPrefix
	| t=expressionTerm[bParam]			#nonUnaryExpressionPrefix
	;

expressionTerm[Block bParam]
	returns[Expression expReturn]
	: {isVar(CurrentToken.Text, $bParam)}? id=ID s=selector[bParam, $bParam.LookupVar($id.text)]				#termSingleId
	| {!isVar(CurrentToken.Text, $bParam)}? 
		id=ID '(' cp=callParameters[bParam]? ')'	#termFuncCall
	| c=Constant								#termConstant
	| s=STRING_LITERAL							#termStringLiteral
	| '(' e=relationalExpression[bParam] ')'	#termEmbeddedExpression
	| '~' e=relationalExpression[bParam]		#termNotExpression
	;

callParameters[Block bParam]
		: p+=relationalExpression[bParam] (',' p+=relationalExpression[bParam])*
		;

selector[Block bParam, Declaration type] returns [VariableSelector vsRet]
	: i+=arrayOrRecordSelector[bParam]*
	;

arrayOrRecordSelector[Block bParam] returns [BaseSelectorElement selRet]
	: '[' e=simpleExpression[bParam] ']'		# arraySelector
	| '.' ID									# recordSelector
	;

// lexer tokens
STRING_LITERAL
   : '\'' ('\'\'' | ~ ('\''))* '\''
   ;

Constant
    :   IntegerConstant
    |   FloatingConstant
	;

IntegerConstant: DigitSequence;

fragment
FloatingConstant
    :   FractionalConstant ExponentPart? 
    |   DigitSequence ExponentPart 
    ;

fragment
FractionalConstant
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
MULT:		'*';
NOT:		'~';
LT:			'<';
LE:			'<=';
GT:			'>';
GE:			'>=';
Assign:		':=';

/* keywords */
MODULE:		'MODULE';
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

