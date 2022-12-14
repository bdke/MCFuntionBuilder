grammar MCFBuilder;


program: line* EOF;

line: assignFunction | statement | ifBlock | executeBlock | whileBlock | forBlock | assignFile | COMMENT | command;

statement: (assignment | functionCall | return | global) SEMI;

ifBlock
    : 'if' ifConditionalBlock+ block ('else' elseIfBlock)? #ifConditionalExpression
    | 'if' '(' expression ')' block ('else' elseIfBlock)? #IfStandardExpression
    ;

ifConditionalBlock
    : 'entity' '(' selector ')' #IfEntityExpression
    | 'score' '(' IDENTIFIER selector compareOp expression ')' #IfScoreMatchesCompareExpression
    | 'score' expression ('to' expression)? '(' IDENTIFIER selector ')' #IfScoreMatchesExpression
    | 'score' '('IDENTIFIER selector compareOp IDENTIFIER selector ')' #IfScoreCompareExpression
    | 'predicate' '(' expression ',' expression ')' #IfPredicateExpression
    | 'block' vector '(' expression ')' #IfBlockExpression
    | 'blocks' vector vector vector '(' IFBLOCKSTYPE ')' #IfBlocksExpression
    | 'data' 'block' vector '(' expression ')' #IfDataBlockExpression
    | 'data' 'entity' selector '(' expression? ')' #IfDataEntityExpression
    | 'data' 'storage' expression '(' expression ')' #IfDataStorageExpression
    ;

executeBlock: 'execute' (executeTypes)+ block ;
executeTypes
            : ('as' selector) #ExecuteAsExpression
            | ('at' selector) #ExecuteAtExpression
            | ('positioned' (('as' selector) | (vector))) #ExeuctePositionedExpression
            ;
vector
    : '~' expression? '~' expression? '~' expression? #ExactVectorExpression
    | '^' expression? '^' expression? '^' expression? #RelativeVectorExpression
    ;

elseIfBlock: block | ifBlock;

whileBlock: WHILE '(' expression ')' block; 
WHILE: 'while';

forBlock: 'for' '(' IDENTIFIER 'in' (INTEGER 'to' INTEGER | dict | list | IDENTIFIER) ')' block;

assignFunction: 'def' IDENTIFIER '(' IDENTIFIER? (',' IDENTIFIER)* ')' block ;




// assignment: ((VARIABLES_TYPE)? IDENTIFIER ( ':' IDENTIFIER ) selector assignOp expression selector? ) 
//             | ((VARIABLES_TYPE)? IDENTIFIER assignOp expression) 
//             | ((VARIABLES_TYPE)? IDENTIFIER selector assignOp expression);

assignment: localScoresAssignment || scoresEqual || localTagsAssignment ||  generalAssignment ;

generalAssignment: localAssignment || operation;

localTagsAssignment: 'tag' 'var' IDENTIFIER (selector '=' BOOL)? ;
localAssignment: 'var' IDENTIFIER ('=' expression)? ;
localScoresAssignment: 'score' 'var' IDENTIFIER selector '=' expression;

global: globalAssignment || globalScoresAssignment || globalTagsAssignment ;

globalTagsAssignment: 'tag' 'global' IDENTIFIER (selector '=' BOOL)? ;
globalAssignment: 'global' IDENTIFIER ('=' expression)? ;
globalScoresAssignment: 'score' 'global' IDENTIFIER selector '=' expression;

operation: expression selector? assignOp expression ;
// scoresOperation: 'score' IDENTIFIER selector assignOp expression ;
// tagsOperation: 'tag' IDENTIFIER selector '=' BOOL ;

scoresEqual: IDENTIFIER selector assignOp IDENTIFIER selector ;


selector: SELECTOR | expression ;

VARIABLES_TYPE: ('var' | 'global') ;


createClass: 'new' IDENTIFIER '(' (expression (',' expression)*)? ')' ;
classVariables: IDENTIFIER DOT IDENTIFIER ;
functionCall: IDENTIFIER (DOT IDENTIFIER)? '(' (expression (',' expression)*)? ')'; 

return: 'return' expression;



expression
    : expression '[' expression ']' #getIdentifierDataExpression
    | constant  #constantExpression
    | IDENTIFIER #identifierExpression
    | createClass #createClassExpression
    | classVariables #classVariablesExpression
    | functionCall #functionCallExpression
    | '(' expression ')' #parenthesizedExpression
    | '!' expression    #notExpression
    | expression multOp expression  #multiplicativeExpression
    | expression addOp expression   #additiveExpression
    | expression compareOp expression #comparisionExpression
    | expression boolOp expression #booleanExpression
    | SELECTOR #selectorExpressionInExpression
;


assignFile : AssignFile;
AssignFile: '#' (IDENTIFIER ('/' IDENTIFIER)*) ~[\r\n]* ;

//operator
multOp: '*' | '/';
addOp: '+' | '-' ;
compareOp: '==' | '!=' | '>' | '<' | '<=' | '>=';
boolOp: BOOL_OPERATOR;

assignOp: '=' | '+=' | '-=' | '%=' | '*=' | '/=' ;

BOOL_OPERATOR: '&&' | '||';


//data type

constant: INTEGER | FLOAT | STRING | BOOL | dict | list | NULL;

list: '[' expression? (',' expression)* ']' ;
dict: '{' (STRING ':' expression)? (',' STRING ':' expression)* '}' ;

number: INTEGER | FLOAT ;
INTEGER: [0-9]+;
FLOAT: [0-9]+ '.' [0-9]+;
STRING: ('"' ~'"'* '"') | ('\'' ~'\''* '\'');
BOOL: 'true' | 'false';
NULL: 'null';

block: '{' line* '}';

DOT: '.';
IFTYPES: 'entity' | 'score' ;
IFBLOCKSTYPE: 'all' | 'masked' ;
SELECTOR: ('@' ('s'|'a'|'r'|'e'|'p'));
command: COMMAND ;
COMMAND: '/' ~[\r\n]*;
COMMENT: '//' ~[\r\n]* -> skip;
WS: [ \t\r\n]+ -> skip;
IDENTIFIER: [a-zA-Z0-9_][a-zA-Z0-9_]*;
SEMI: ';';

