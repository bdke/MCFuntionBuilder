// Generated from c:\Users\William\source\realthings\MCFBuilder\ArithmeticLexer.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.Lexer;
import org.antlr.v4.runtime.CharStream;
import org.antlr.v4.runtime.Token;
import org.antlr.v4.runtime.TokenStream;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.misc.*;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class ArithmeticLexer extends Lexer {
	static { RuntimeMetaData.checkVersion("4.9.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		INT=1, BOOL=2, STRING=3, TRUE=4, FALSE=5, VARIABLE=6, SCIENTIFIC_NUMBER=7, 
		VALIDSTRING=8, LPAREN=9, RPAREN=10, PLUS=11, MINUS=12, TIMES=13, DIV=14, 
		GT=15, LT=16, EQ=17, POINT=18, POW=19, SEMI=20, WS=21;
	public static String[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static String[] modeNames = {
		"DEFAULT_MODE"
	};

	private static String[] makeRuleNames() {
		return new String[] {
			"INT", "BOOL", "STRING", "TRUE", "FALSE", "VARIABLE", "SCIENTIFIC_NUMBER", 
			"VALIDSTRING", "LPAREN", "RPAREN", "PLUS", "MINUS", "TIMES", "DIV", "GT", 
			"LT", "EQ", "POINT", "POW", "SEMI", "WS", "VALID_ID_START", "VALID_ID_CHAR", 
			"NUMBER", "UNSIGNED_INTEGER", "E", "SIGN"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'int '", "'bool '", "'string '", "'true'", "'false'", null, null, 
			null, "'('", "')'", "'+'", "'-'", "'*'", "'/'", "'>'", "'<'", "'='", 
			"'.'", "'^'", "';'"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, "INT", "BOOL", "STRING", "TRUE", "FALSE", "VARIABLE", "SCIENTIFIC_NUMBER", 
			"VALIDSTRING", "LPAREN", "RPAREN", "PLUS", "MINUS", "TIMES", "DIV", "GT", 
			"LT", "EQ", "POINT", "POW", "SEMI", "WS"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}


	public ArithmeticLexer(CharStream input) {
		super(input);
		_interp = new LexerATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@Override
	public String getGrammarFileName() { return "ArithmeticLexer.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public String[] getChannelNames() { return channelNames; }

	@Override
	public String[] getModeNames() { return modeNames; }

	@Override
	public ATN getATN() { return _ATN; }

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\2\27\u00ac\b\1\4\2"+
		"\t\2\4\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4"+
		"\13\t\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22"+
		"\t\22\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31"+
		"\t\31\4\32\t\32\4\33\t\33\4\34\t\34\3\2\3\2\3\2\3\2\3\2\3\3\3\3\3\3\3"+
		"\3\3\3\3\3\3\4\3\4\3\4\3\4\3\4\3\4\3\4\3\4\3\5\3\5\3\5\3\5\3\5\3\6\3\6"+
		"\3\6\3\6\3\6\3\6\3\7\3\7\7\7Z\n\7\f\7\16\7]\13\7\3\b\3\b\3\b\5\bb\n\b"+
		"\3\b\3\b\5\bf\n\b\3\t\3\t\7\tj\n\t\f\t\16\tm\13\t\3\t\3\t\3\n\3\n\3\13"+
		"\3\13\3\f\3\f\3\r\3\r\3\16\3\16\3\17\3\17\3\20\3\20\3\21\3\21\3\22\3\22"+
		"\3\23\3\23\3\24\3\24\3\25\3\25\3\26\6\26\u008a\n\26\r\26\16\26\u008b\3"+
		"\26\3\26\3\27\5\27\u0091\n\27\3\30\3\30\5\30\u0095\n\30\3\31\6\31\u0098"+
		"\n\31\r\31\16\31\u0099\3\31\3\31\6\31\u009e\n\31\r\31\16\31\u009f\5\31"+
		"\u00a2\n\31\3\32\6\32\u00a5\n\32\r\32\16\32\u00a6\3\33\3\33\3\34\3\34"+
		"\2\2\35\3\3\5\4\7\5\t\6\13\7\r\b\17\t\21\n\23\13\25\f\27\r\31\16\33\17"+
		"\35\20\37\21!\22#\23%\24\'\25)\26+\27-\2/\2\61\2\63\2\65\2\67\2\3\2\6"+
		"\5\2\13\f\17\17\"\"\5\2C\\aac|\4\2GGgg\4\2--//\2\u00af\2\3\3\2\2\2\2\5"+
		"\3\2\2\2\2\7\3\2\2\2\2\t\3\2\2\2\2\13\3\2\2\2\2\r\3\2\2\2\2\17\3\2\2\2"+
		"\2\21\3\2\2\2\2\23\3\2\2\2\2\25\3\2\2\2\2\27\3\2\2\2\2\31\3\2\2\2\2\33"+
		"\3\2\2\2\2\35\3\2\2\2\2\37\3\2\2\2\2!\3\2\2\2\2#\3\2\2\2\2%\3\2\2\2\2"+
		"\'\3\2\2\2\2)\3\2\2\2\2+\3\2\2\2\39\3\2\2\2\5>\3\2\2\2\7D\3\2\2\2\tL\3"+
		"\2\2\2\13Q\3\2\2\2\rW\3\2\2\2\17^\3\2\2\2\21g\3\2\2\2\23p\3\2\2\2\25r"+
		"\3\2\2\2\27t\3\2\2\2\31v\3\2\2\2\33x\3\2\2\2\35z\3\2\2\2\37|\3\2\2\2!"+
		"~\3\2\2\2#\u0080\3\2\2\2%\u0082\3\2\2\2\'\u0084\3\2\2\2)\u0086\3\2\2\2"+
		"+\u0089\3\2\2\2-\u0090\3\2\2\2/\u0094\3\2\2\2\61\u0097\3\2\2\2\63\u00a4"+
		"\3\2\2\2\65\u00a8\3\2\2\2\67\u00aa\3\2\2\29:\7k\2\2:;\7p\2\2;<\7v\2\2"+
		"<=\7\"\2\2=\4\3\2\2\2>?\7d\2\2?@\7q\2\2@A\7q\2\2AB\7n\2\2BC\7\"\2\2C\6"+
		"\3\2\2\2DE\7u\2\2EF\7v\2\2FG\7t\2\2GH\7k\2\2HI\7p\2\2IJ\7i\2\2JK\7\"\2"+
		"\2K\b\3\2\2\2LM\7v\2\2MN\7t\2\2NO\7w\2\2OP\7g\2\2P\n\3\2\2\2QR\7h\2\2"+
		"RS\7c\2\2ST\7n\2\2TU\7u\2\2UV\7g\2\2V\f\3\2\2\2W[\5-\27\2XZ\5/\30\2YX"+
		"\3\2\2\2Z]\3\2\2\2[Y\3\2\2\2[\\\3\2\2\2\\\16\3\2\2\2][\3\2\2\2^e\5\61"+
		"\31\2_a\5\65\33\2`b\5\67\34\2a`\3\2\2\2ab\3\2\2\2bc\3\2\2\2cd\5\63\32"+
		"\2df\3\2\2\2e_\3\2\2\2ef\3\2\2\2f\20\3\2\2\2gk\7$\2\2hj\5/\30\2ih\3\2"+
		"\2\2jm\3\2\2\2ki\3\2\2\2kl\3\2\2\2ln\3\2\2\2mk\3\2\2\2no\7$\2\2o\22\3"+
		"\2\2\2pq\7*\2\2q\24\3\2\2\2rs\7+\2\2s\26\3\2\2\2tu\7-\2\2u\30\3\2\2\2"+
		"vw\7/\2\2w\32\3\2\2\2xy\7,\2\2y\34\3\2\2\2z{\7\61\2\2{\36\3\2\2\2|}\7"+
		"@\2\2} \3\2\2\2~\177\7>\2\2\177\"\3\2\2\2\u0080\u0081\7?\2\2\u0081$\3"+
		"\2\2\2\u0082\u0083\7\60\2\2\u0083&\3\2\2\2\u0084\u0085\7`\2\2\u0085(\3"+
		"\2\2\2\u0086\u0087\7=\2\2\u0087*\3\2\2\2\u0088\u008a\t\2\2\2\u0089\u0088"+
		"\3\2\2\2\u008a\u008b\3\2\2\2\u008b\u0089\3\2\2\2\u008b\u008c\3\2\2\2\u008c"+
		"\u008d\3\2\2\2\u008d\u008e\b\26\2\2\u008e,\3\2\2\2\u008f\u0091\t\3\2\2"+
		"\u0090\u008f\3\2\2\2\u0091.\3\2\2\2\u0092\u0095\5-\27\2\u0093\u0095\4"+
		"\62;\2\u0094\u0092\3\2\2\2\u0094\u0093\3\2\2\2\u0095\60\3\2\2\2\u0096"+
		"\u0098\4\62;\2\u0097\u0096\3\2\2\2\u0098\u0099\3\2\2\2\u0099\u0097\3\2"+
		"\2\2\u0099\u009a\3\2\2\2\u009a\u00a1\3\2\2\2\u009b\u009d\7\60\2\2\u009c"+
		"\u009e\4\62;\2\u009d\u009c\3\2\2\2\u009e\u009f\3\2\2\2\u009f\u009d\3\2"+
		"\2\2\u009f\u00a0\3\2\2\2\u00a0\u00a2\3\2\2\2\u00a1\u009b\3\2\2\2\u00a1"+
		"\u00a2\3\2\2\2\u00a2\62\3\2\2\2\u00a3\u00a5\4\62;\2\u00a4\u00a3\3\2\2"+
		"\2\u00a5\u00a6\3\2\2\2\u00a6\u00a4\3\2\2\2\u00a6\u00a7\3\2\2\2\u00a7\64"+
		"\3\2\2\2\u00a8\u00a9\t\4\2\2\u00a9\66\3\2\2\2\u00aa\u00ab\t\5\2\2\u00ab"+
		"8\3\2\2\2\16\2[aek\u008b\u0090\u0094\u0099\u009f\u00a1\u00a6\3\2\3\2";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}