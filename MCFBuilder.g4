grammar MCFBuilder;

program: line* EOF;

line: statement | ifBlock | whileBlock | forBlock;

statement: (assignment | functionCall) SEMI;

ifBlock: 'if' '(' expression ')' block ('else' elseIfBlock)?;
elseIfBlock: block | ifBlock;

whileBlock: WHILE '(' expression ')' block; 
WHILE: 'while';

forBlock: 'for' '(' IDENTIFIER 'in' (INTEGER 'to' INTEGER | dict | list | IDENTIFIER) ')' block;

assignFcuntion: 'def' IDENTIFIER '(' IDENTIFIER* ')' block ;

assignment: IDENTIFIER '=' expression;

functionCall: IDENTIFIER '(' (expression (',' expression)*)? ')'; 

expression
    : constant  #constantExpression
    | IDENTIFIER #identifierExpression
    | functionCall #functionCallExpression
    | '(' expression ')' #parenthesizedExpression
    | '!' expression    #notExpression
    | expression multOp expression  #multiplicativeExpression
    | expression addOp expression   #additiveExpression
    | expression compareOp expression #comparisionExpression
    | expression boolOp expression #booleanExpression
;

//operator
multOp: '*' | '/' | '%';
addOp: '+' | '-';
compareOp: '==' | '!=' | '>' | '<' | '<=' | '>=';
boolOp: BOOL_OPERATOR;

BOOL_OPERATOR: '&&' | '||';


//data type
list: '[' constant? (',' constant)* ']' ;
dict: '{' (STRING ':' constant)? (',' STRING ':' constant)* '}' ;
constant: INTEGER | FLOAT | STRING | BOOL | dict | list | NULL;

INTEGER: [0-9]+;
FLOAT: [0-9]+ '.' [0-9]+;
STRING: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
BOOL: 'true' | 'false';
NULL: 'null';

block: '{' line* '}';

WS: [ \t\r\n]+ -> skip;
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
SEMI: ';';