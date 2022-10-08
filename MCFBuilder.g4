grammar MCFBuilder;


program: line* EOF;

line: assignFunction | statement | ifBlock | executeBlock | whileBlock | forBlock | assignFile | COMMENT | COMMAND;

statement: (assignment | functionCall | return | global) SEMI;

ifBlock: 'if' IFTYPES '(' expression ')' block ('else' elseIfBlock)?;
executeBlock: 'execute' (executeTypes)+ block ;
executeTypes
            : ('as' selector) #ExecuteAsExpression
            | ('at' selector) #ExecuteAtExpression
            | ('positioned' (('as' selector) | (vector))) #ExeuctePositionedExpression
            ;
vector: '~' number? '~' number? '~' number? ;

elseIfBlock: block | ifBlock;

whileBlock: WHILE '(' expression ')' block; 
WHILE: 'while';

forBlock: 'for' '(' IDENTIFIER 'in' (INTEGER 'to' INTEGER | dict | list | IDENTIFIER) ')' block;

assignFunction: 'def' IDENTIFIER '(' IDENTIFIER? (',' IDENTIFIER)* ')' block ;

assignFile: '#' (IDENTIFIER ('/' IDENTIFIER)*) ':';


// assignment: ((VARIABLES_TYPE)? IDENTIFIER ( ':' IDENTIFIER ) selector assignOp expression selector? ) 
//             | ((VARIABLES_TYPE)? IDENTIFIER assignOp expression) 
//             | ((VARIABLES_TYPE)? IDENTIFIER selector assignOp expression);

assignment: localScoresAssignment || scoresEqual || localTagsAssignment ||  generalAssignment ;

generalAssignment: localAssignment || operation;

localTagsAssignment: 'tag' 'var' IDENTIFIER selector '=' BOOL ;
localAssignment: 'var' IDENTIFIER ('=' expression)? ;
localScoresAssignment: 'score' 'var' IDENTIFIER (selector '=' expression)?;

global: globalAssignment || globalScoresAssignment || globalTagsAssignment ;

globalTagsAssignment: 'tag' 'global' IDENTIFIER selector '=' BOOL ;
globalAssignment: 'global' IDENTIFIER ('=' expression)? ;
globalScoresAssignment: 'score' 'global' IDENTIFIER (selector '=' expression)?;

operation: IDENTIFIER selector? assignOp expression ;
// scoresOperation: 'score' IDENTIFIER selector assignOp expression ;
// tagsOperation: 'tag' IDENTIFIER selector '=' BOOL ;

scoresEqual: IDENTIFIER selector assignOp IDENTIFIER selector ;


selector: SELECTOR || STRING || IDENTIFIER;

VARIABLES_TYPE: ('var' | 'global') ;


createClass: 'new' IDENTIFIER '(' (expression (',' expression)*)? ')' ;
classVariables: IDENTIFIER '.' IDENTIFIER ;
classFunctions: IDENTIFIER '.' IDENTIFIER '(' (expression (',' expression)*)? ')'  ;
functionCall: IDENTIFIER '(' (expression (',' expression)*)? ')'; 

return: 'return' expression;

expression
    : constant  #constantExpression
    | IDENTIFIER #identifierExpression
    | createClass #createClassExpression
    | classVariables #classVariablesExpression
    | classFunctions #classFunctionsExpression
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

number: INTEGER | FLOAT ;
INTEGER: [0-9]+;
FLOAT: [0-9]+ '.' [0-9]+;
STRING: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
BOOL: 'true' | 'false';
NULL: 'null';

block: '{' line* '}';

IFTYPES: 'entity' ;
SELECTOR: ('@' ('s'|'a'|'r'|'e'|'p'));
COMMENT: '//' ~[\r\n]* -> skip;
COMMAND: '/' ~[\r\n]* -> skip;
WS: [ \t\r\n]+ -> skip;
IDENTIFIER: [a-zA-Z_][a-zA-Z0-9_]*;
SEMI: ';';

