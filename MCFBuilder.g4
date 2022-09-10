grammar MCFBuilder;
program: line* EOF;

line: assignFunction | statement | ifBlock | whileBlock | forBlock | assignFile | COMMENT;

statement: (assignment | functionCall | return) SEMI;

ifBlock: 'if' '(' expression ')' block ('else' elseIfBlock)?;
elseIfBlock: block | ifBlock;

whileBlock: WHILE '(' expression ')' block; 
WHILE: 'while';

forBlock: 'for' '(' IDENTIFIER 'in' (INTEGER 'to' INTEGER | dict | list | IDENTIFIER) ')' block;

assignFunction: 'def' IDENTIFIER '(' IDENTIFIER? (',' IDENTIFIER)* ')' block ;

assignFile: '#' (IDENTIFIER ('/' IDENTIFIER)*) ':';


assignment: ((VARIABLES_TYPE)? IDENTIFIER ( ':' IDENTIFIER ) selector assignOp expression selector? ) 
            | ((VARIABLES_TYPE)? IDENTIFIER assignOp expression) 
            | ((VARIABLES_TYPE)? IDENTIFIER selector assignOp expression);
selector: ('@' ('s'|'a'|'r'|'e'|'p') | IDENTIFIER)? ('[' IDENTIFIER? ']')?;

VARIABLES_TYPE: ('var' | 'global') ;

functionCall: IDENTIFIER '(' (expression (',' expression)*)? ')'; 

return: 'return' expression;

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
multOp: '*' | '/';
addOp: '+' | '-' ;
compareOp: '==' | '!=' | '>' | '<' | '<=' | '>=';
boolOp: BOOL_OPERATOR;

assignOp: '=' | '+=' | '-=' | '%=' | '*=' | '/=' ;

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

COMMENT: '//' ~[\r\n]* -> skip ;
WS: [ \t\r\n]+ -> skip;
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
SEMI: ';';