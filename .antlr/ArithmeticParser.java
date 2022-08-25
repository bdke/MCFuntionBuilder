// Generated from c:\Users\William\source\realthings\MCFBuilder\ArithmeticParser.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class ArithmeticParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.9.2", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		INT=1, BOOL=2, STRING=3, TRUE=4, FALSE=5, VARIABLE=6, SCIENTIFIC_NUMBER=7, 
		VALIDSTRING=8, LPAREN=9, RPAREN=10, PLUS=11, MINUS=12, TIMES=13, DIV=14, 
		GT=15, LT=16, EQ=17, POINT=18, POW=19, SEMI=20, WS=21;
	public static final int
		RULE_file = 0, RULE_expression = 1, RULE_boolvar = 2, RULE_intvar = 3, 
		RULE_strvar = 4, RULE_atom = 5, RULE_valid_bool = 6, RULE_scientific = 7, 
		RULE_valid_string = 8, RULE_variable = 9;
	private static String[] makeRuleNames() {
		return new String[] {
			"file", "expression", "boolvar", "intvar", "strvar", "atom", "valid_bool", 
			"scientific", "valid_string", "variable"
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

	@Override
	public String getGrammarFileName() { return "ArithmeticParser.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public ArithmeticParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	public static class FileContext extends ParserRuleContext {
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public TerminalNode EOF() { return getToken(ArithmeticParser.EOF, 0); }
		public List<TerminalNode> SEMI() { return getTokens(ArithmeticParser.SEMI); }
		public TerminalNode SEMI(int i) {
			return getToken(ArithmeticParser.SEMI, i);
		}
		public FileContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_file; }
	}

	public final FileContext file() throws RecognitionException {
		FileContext _localctx = new FileContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_file);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(20);
			expression(0);
			setState(25);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,0,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(21);
					match(SEMI);
					setState(22);
					expression(0);
					}
					} 
				}
				setState(27);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,0,_ctx);
			}
			setState(28);
			_la = _input.LA(1);
			if ( !(_la==EOF || _la==SEMI) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExpressionContext extends ParserRuleContext {
		public TerminalNode LPAREN() { return getToken(ArithmeticParser.LPAREN, 0); }
		public List<ExpressionContext> expression() {
			return getRuleContexts(ExpressionContext.class);
		}
		public ExpressionContext expression(int i) {
			return getRuleContext(ExpressionContext.class,i);
		}
		public TerminalNode RPAREN() { return getToken(ArithmeticParser.RPAREN, 0); }
		public AtomContext atom() {
			return getRuleContext(AtomContext.class,0);
		}
		public List<TerminalNode> PLUS() { return getTokens(ArithmeticParser.PLUS); }
		public TerminalNode PLUS(int i) {
			return getToken(ArithmeticParser.PLUS, i);
		}
		public List<TerminalNode> MINUS() { return getTokens(ArithmeticParser.MINUS); }
		public TerminalNode MINUS(int i) {
			return getToken(ArithmeticParser.MINUS, i);
		}
		public BoolvarContext boolvar() {
			return getRuleContext(BoolvarContext.class,0);
		}
		public IntvarContext intvar() {
			return getRuleContext(IntvarContext.class,0);
		}
		public StrvarContext strvar() {
			return getRuleContext(StrvarContext.class,0);
		}
		public TerminalNode POW() { return getToken(ArithmeticParser.POW, 0); }
		public TerminalNode TIMES() { return getToken(ArithmeticParser.TIMES, 0); }
		public TerminalNode DIV() { return getToken(ArithmeticParser.DIV, 0); }
		public ExpressionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_expression; }
	}

	public final ExpressionContext expression() throws RecognitionException {
		return expression(0);
	}

	private ExpressionContext expression(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		ExpressionContext _localctx = new ExpressionContext(_ctx, _parentState);
		ExpressionContext _prevctx = _localctx;
		int _startState = 2;
		enterRecursionRule(_localctx, 2, RULE_expression, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(47);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case LPAREN:
				{
				setState(31);
				match(LPAREN);
				setState(32);
				expression(0);
				setState(33);
				match(RPAREN);
				}
				break;
			case VARIABLE:
			case SCIENTIFIC_NUMBER:
			case PLUS:
			case MINUS:
				{
				setState(38);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==PLUS || _la==MINUS) {
					{
					{
					setState(35);
					_la = _input.LA(1);
					if ( !(_la==PLUS || _la==MINUS) ) {
					_errHandler.recoverInline(this);
					}
					else {
						if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
						_errHandler.reportMatch(this);
						consume();
					}
					}
					}
					setState(40);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(41);
				atom();
				}
				break;
			case INT:
			case BOOL:
			case STRING:
				{
				setState(45);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case BOOL:
					{
					setState(42);
					boolvar();
					}
					break;
				case INT:
					{
					setState(43);
					intvar();
					}
					break;
				case STRING:
					{
					setState(44);
					strvar();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.LT(-1);
			setState(60);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,5,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(58);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,4,_ctx) ) {
					case 1:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(49);
						if (!(precpred(_ctx, 6))) throw new FailedPredicateException(this, "precpred(_ctx, 6)");
						setState(50);
						match(POW);
						setState(51);
						expression(7);
						}
						break;
					case 2:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(52);
						if (!(precpred(_ctx, 5))) throw new FailedPredicateException(this, "precpred(_ctx, 5)");
						setState(53);
						_la = _input.LA(1);
						if ( !(_la==TIMES || _la==DIV) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(54);
						expression(6);
						}
						break;
					case 3:
						{
						_localctx = new ExpressionContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_expression);
						setState(55);
						if (!(precpred(_ctx, 4))) throw new FailedPredicateException(this, "precpred(_ctx, 4)");
						setState(56);
						_la = _input.LA(1);
						if ( !(_la==PLUS || _la==MINUS) ) {
						_errHandler.recoverInline(this);
						}
						else {
							if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
							_errHandler.reportMatch(this);
							consume();
						}
						setState(57);
						expression(5);
						}
						break;
					}
					} 
				}
				setState(62);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,5,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public static class BoolvarContext extends ParserRuleContext {
		public TerminalNode BOOL() { return getToken(ArithmeticParser.BOOL, 0); }
		public VariableContext variable() {
			return getRuleContext(VariableContext.class,0);
		}
		public TerminalNode EQ() { return getToken(ArithmeticParser.EQ, 0); }
		public Valid_boolContext valid_bool() {
			return getRuleContext(Valid_boolContext.class,0);
		}
		public BoolvarContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_boolvar; }
	}

	public final BoolvarContext boolvar() throws RecognitionException {
		BoolvarContext _localctx = new BoolvarContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_boolvar);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(63);
			match(BOOL);
			setState(64);
			variable();
			setState(67);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,6,_ctx) ) {
			case 1:
				{
				setState(65);
				match(EQ);
				setState(66);
				valid_bool();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class IntvarContext extends ParserRuleContext {
		public TerminalNode INT() { return getToken(ArithmeticParser.INT, 0); }
		public VariableContext variable() {
			return getRuleContext(VariableContext.class,0);
		}
		public TerminalNode EQ() { return getToken(ArithmeticParser.EQ, 0); }
		public ScientificContext scientific() {
			return getRuleContext(ScientificContext.class,0);
		}
		public IntvarContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_intvar; }
	}

	public final IntvarContext intvar() throws RecognitionException {
		IntvarContext _localctx = new IntvarContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_intvar);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(69);
			match(INT);
			setState(70);
			variable();
			setState(73);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,7,_ctx) ) {
			case 1:
				{
				setState(71);
				match(EQ);
				setState(72);
				scientific();
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class StrvarContext extends ParserRuleContext {
		public TerminalNode STRING() { return getToken(ArithmeticParser.STRING, 0); }
		public VariableContext variable() {
			return getRuleContext(VariableContext.class,0);
		}
		public TerminalNode EQ() { return getToken(ArithmeticParser.EQ, 0); }
		public List<Valid_stringContext> valid_string() {
			return getRuleContexts(Valid_stringContext.class);
		}
		public Valid_stringContext valid_string(int i) {
			return getRuleContext(Valid_stringContext.class,i);
		}
		public List<TerminalNode> PLUS() { return getTokens(ArithmeticParser.PLUS); }
		public TerminalNode PLUS(int i) {
			return getToken(ArithmeticParser.PLUS, i);
		}
		public StrvarContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_strvar; }
	}

	public final StrvarContext strvar() throws RecognitionException {
		StrvarContext _localctx = new StrvarContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_strvar);
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(75);
			match(STRING);
			setState(76);
			variable();
			setState(86);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,9,_ctx) ) {
			case 1:
				{
				setState(77);
				match(EQ);
				setState(78);
				valid_string();
				setState(83);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,8,_ctx);
				while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
					if ( _alt==1 ) {
						{
						{
						setState(79);
						match(PLUS);
						setState(80);
						valid_string();
						}
						} 
					}
					setState(85);
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,8,_ctx);
				}
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class AtomContext extends ParserRuleContext {
		public ScientificContext scientific() {
			return getRuleContext(ScientificContext.class,0);
		}
		public VariableContext variable() {
			return getRuleContext(VariableContext.class,0);
		}
		public AtomContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_atom; }
	}

	public final AtomContext atom() throws RecognitionException {
		AtomContext _localctx = new AtomContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_atom);
		try {
			setState(90);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case SCIENTIFIC_NUMBER:
				enterOuterAlt(_localctx, 1);
				{
				setState(88);
				scientific();
				}
				break;
			case VARIABLE:
				enterOuterAlt(_localctx, 2);
				{
				setState(89);
				variable();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Valid_boolContext extends ParserRuleContext {
		public TerminalNode TRUE() { return getToken(ArithmeticParser.TRUE, 0); }
		public TerminalNode FALSE() { return getToken(ArithmeticParser.FALSE, 0); }
		public Valid_boolContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_valid_bool; }
	}

	public final Valid_boolContext valid_bool() throws RecognitionException {
		Valid_boolContext _localctx = new Valid_boolContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_valid_bool);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(92);
			_la = _input.LA(1);
			if ( !(_la==TRUE || _la==FALSE) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ScientificContext extends ParserRuleContext {
		public TerminalNode SCIENTIFIC_NUMBER() { return getToken(ArithmeticParser.SCIENTIFIC_NUMBER, 0); }
		public ScientificContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_scientific; }
	}

	public final ScientificContext scientific() throws RecognitionException {
		ScientificContext _localctx = new ScientificContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_scientific);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(94);
			match(SCIENTIFIC_NUMBER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Valid_stringContext extends ParserRuleContext {
		public TerminalNode VALIDSTRING() { return getToken(ArithmeticParser.VALIDSTRING, 0); }
		public VariableContext variable() {
			return getRuleContext(VariableContext.class,0);
		}
		public Valid_stringContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_valid_string; }
	}

	public final Valid_stringContext valid_string() throws RecognitionException {
		Valid_stringContext _localctx = new Valid_stringContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_valid_string);
		try {
			setState(98);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case VALIDSTRING:
				enterOuterAlt(_localctx, 1);
				{
				setState(96);
				match(VALIDSTRING);
				}
				break;
			case VARIABLE:
				enterOuterAlt(_localctx, 2);
				{
				setState(97);
				variable();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class VariableContext extends ParserRuleContext {
		public TerminalNode VARIABLE() { return getToken(ArithmeticParser.VARIABLE, 0); }
		public VariableContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_variable; }
	}

	public final VariableContext variable() throws RecognitionException {
		VariableContext _localctx = new VariableContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_variable);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(100);
			match(VARIABLE);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 1:
			return expression_sempred((ExpressionContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean expression_sempred(ExpressionContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 6);
		case 1:
			return precpred(_ctx, 5);
		case 2:
			return precpred(_ctx, 4);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\3\27i\4\2\t\2\4\3\t"+
		"\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t\13\3"+
		"\2\3\2\3\2\7\2\32\n\2\f\2\16\2\35\13\2\3\2\3\2\3\3\3\3\3\3\3\3\3\3\3\3"+
		"\7\3\'\n\3\f\3\16\3*\13\3\3\3\3\3\3\3\3\3\5\3\60\n\3\5\3\62\n\3\3\3\3"+
		"\3\3\3\3\3\3\3\3\3\3\3\3\3\3\3\7\3=\n\3\f\3\16\3@\13\3\3\4\3\4\3\4\3\4"+
		"\5\4F\n\4\3\5\3\5\3\5\3\5\5\5L\n\5\3\6\3\6\3\6\3\6\3\6\3\6\7\6T\n\6\f"+
		"\6\16\6W\13\6\5\6Y\n\6\3\7\3\7\5\7]\n\7\3\b\3\b\3\t\3\t\3\n\3\n\5\ne\n"+
		"\n\3\13\3\13\3\13\2\3\4\f\2\4\6\b\n\f\16\20\22\24\2\6\3\3\26\26\3\2\r"+
		"\16\3\2\17\20\3\2\6\7\2m\2\26\3\2\2\2\4\61\3\2\2\2\6A\3\2\2\2\bG\3\2\2"+
		"\2\nM\3\2\2\2\f\\\3\2\2\2\16^\3\2\2\2\20`\3\2\2\2\22d\3\2\2\2\24f\3\2"+
		"\2\2\26\33\5\4\3\2\27\30\7\26\2\2\30\32\5\4\3\2\31\27\3\2\2\2\32\35\3"+
		"\2\2\2\33\31\3\2\2\2\33\34\3\2\2\2\34\36\3\2\2\2\35\33\3\2\2\2\36\37\t"+
		"\2\2\2\37\3\3\2\2\2 !\b\3\1\2!\"\7\13\2\2\"#\5\4\3\2#$\7\f\2\2$\62\3\2"+
		"\2\2%\'\t\3\2\2&%\3\2\2\2\'*\3\2\2\2(&\3\2\2\2()\3\2\2\2)+\3\2\2\2*(\3"+
		"\2\2\2+\62\5\f\7\2,\60\5\6\4\2-\60\5\b\5\2.\60\5\n\6\2/,\3\2\2\2/-\3\2"+
		"\2\2/.\3\2\2\2\60\62\3\2\2\2\61 \3\2\2\2\61(\3\2\2\2\61/\3\2\2\2\62>\3"+
		"\2\2\2\63\64\f\b\2\2\64\65\7\25\2\2\65=\5\4\3\t\66\67\f\7\2\2\678\t\4"+
		"\2\28=\5\4\3\b9:\f\6\2\2:;\t\3\2\2;=\5\4\3\7<\63\3\2\2\2<\66\3\2\2\2<"+
		"9\3\2\2\2=@\3\2\2\2><\3\2\2\2>?\3\2\2\2?\5\3\2\2\2@>\3\2\2\2AB\7\4\2\2"+
		"BE\5\24\13\2CD\7\23\2\2DF\5\16\b\2EC\3\2\2\2EF\3\2\2\2F\7\3\2\2\2GH\7"+
		"\3\2\2HK\5\24\13\2IJ\7\23\2\2JL\5\20\t\2KI\3\2\2\2KL\3\2\2\2L\t\3\2\2"+
		"\2MN\7\5\2\2NX\5\24\13\2OP\7\23\2\2PU\5\22\n\2QR\7\r\2\2RT\5\22\n\2SQ"+
		"\3\2\2\2TW\3\2\2\2US\3\2\2\2UV\3\2\2\2VY\3\2\2\2WU\3\2\2\2XO\3\2\2\2X"+
		"Y\3\2\2\2Y\13\3\2\2\2Z]\5\20\t\2[]\5\24\13\2\\Z\3\2\2\2\\[\3\2\2\2]\r"+
		"\3\2\2\2^_\t\5\2\2_\17\3\2\2\2`a\7\t\2\2a\21\3\2\2\2be\7\n\2\2ce\5\24"+
		"\13\2db\3\2\2\2dc\3\2\2\2e\23\3\2\2\2fg\7\b\2\2g\25\3\2\2\2\16\33(/\61"+
		"<>EKUX\\d";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}