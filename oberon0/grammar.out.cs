// Generated from grammar.ecs by LeMP custom tool. LeMP version: 1.8.1.0
// Note: you can give command-line arguments to the tool via 'Custom Tool Namespace':
// --no-out-header       Suppress this message
// --verbose             Allow verbose messages (shown by VS as 'warnings')
// --timeout=X           Abort processing thread after X seconds (default: 10)
// --macros=FileName.dll Load macros from FileName.dll, path relative to this file 
// Use #importMacros to use macros in a given namespace, e.g. #importMacros(Loyc.LLPG);
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Loyc;
using Loyc.Collections;
using Loyc.Syntax.Lexing;
using Loyc.Syntax;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Solver;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Expressions.Arithmetic;
namespace Oberon0.Compiler
{
	using TT = TokenType;
	public enum TokenType
	{
		EOF = 0, Or, Mod, Module, If, Then, IfElse, Else, Begin, End, While, Do, Repeat, Until, Type, Var, Const, Array, Of, Record, Procedure, Id, Num, Shr, GE, GT, Shl, LE, LT, Dot, Exp, Mul, Div, Add, Sub, Semicolon, Assign, Equals, Colon, Comma, NotEquals, LParen, RParen, Unary, Unknown
	}
	public struct Token : ISimpleToken<int>
	{
		public TokenType Type { get; set; }
		public object Value { get; set; }
		public int StartIndex { get; set; }
		int ISimpleToken<int>.Type
		{
			get {
				return (int) Type;
			}
		}
		public override string ToString()
		{
			return string.Format("{0:G} -> {1}", Type, Value ?? "NULL");
		}
	}
	partial class CalculatorLexer : IEnumerator<Token>
	{
		public LexerSource Src { get; set; }
		public CalculatorLexer(string text, string fileName = "")
		{
			Src = (LexerSource) text;
		}
		public CalculatorLexer(ICharSource text, string fileName = "")
		{
			Src = new LexerSource(text);
		}
		Token _tok;
		public Token Current
		{
			get {
				return _tok;
			}
		}
		object System.Collections.IEnumerator.Current
		{
			get {
				return Current;
			}
		}
		void System.Collections.IEnumerator.Reset()
		{
			Src.Reset();
		}
		void IDisposable.Dispose()
		{
		}
		public bool MoveNext()
		{
			int la0, la1;
			// Line 137: ([\t\n\r ])*
			for (;;) {
				switch (Src.LA0) {
				case '\t':
				case '\n':
				case '\r':
				case ' ':
					Src.Skip();
					break;
				default:
					goto stop;
				}
			}
		stop:;
			_tok.StartIndex = Src.InputPosition;
			_tok.Value = null;
			// Line 140: ( (Num | [.] [n] [a] [n] | [.] [i] [n] [f]) | ([>] [>] / [>] [=] / [>] / [<] [<] / [<] [=] / [<] / [.] / [\^] / [*] / [/] / [+] / [\-] / [;] / [:] [=] / [=] / [:] / [,] / [#] / [(] / [)]) | IdOrKeyword )
			la0 = Src.LA0;
			switch (la0) {
			case '0':
			case '1':
			case '2':
			case '3':
			case '4':
			case '5':
			case '6':
			case '7':
			case '8':
			case '9':
				{
					#line 140 "grammar.ecs"
					_tok.Type = TT.Num;
					#line default
					Num();
				}
				break;
			case '.':
				{
					la1 = Src.LA(1);
					if (la1 == 'n') {
						#line 141 "grammar.ecs"
						_tok.Type = TT.Num;
						#line default
						Src.Skip();
						Src.Skip();
						Src.Match('a');
						Src.Match('n');
						#line 141 "grammar.ecs"
						_tok.Value = double.NaN;
						#line default
					} else if (la1 == 'i') {
						#line 142 "grammar.ecs"
						_tok.Type = TT.Num;
						#line default
						Src.Skip();
						Src.Skip();
						Src.Match('n');
						Src.Match('f');
						#line 142 "grammar.ecs"
						_tok.Value = double.PositiveInfinity;
						#line default
					} else {
						Src.Skip();
						#line 194 "grammar.ecs"
						_tok.Type = TT.Dot;
						#line default
					}
				}
				break;
			case '>':
				{
					la1 = Src.LA(1);
					if (la1 == '>') {
						Src.Skip();
						Src.Skip();
						#line 194 "grammar.ecs"
						_tok.Type = TT.Shr;
						#line default
					} else if (la1 == '=') {
						Src.Skip();
						Src.Skip();
						#line 194 "grammar.ecs"
						_tok.Type = TT.GE;
						#line default
					} else {
						Src.Skip();
						#line 194 "grammar.ecs"
						_tok.Type = TT.GT;
						#line default
					}
				}
				break;
			case '<':
				{
					la1 = Src.LA(1);
					if (la1 == '<') {
						Src.Skip();
						Src.Skip();
						#line 194 "grammar.ecs"
						_tok.Type = TT.Shl;
						#line default
					} else if (la1 == '=') {
						Src.Skip();
						Src.Skip();
						#line 194 "grammar.ecs"
						_tok.Type = TT.LE;
						#line default
					} else {
						Src.Skip();
						#line 194 "grammar.ecs"
						_tok.Type = TT.LT;
						#line default
					}
				}
				break;
			case '^':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.Exp;
					#line default
				}
				break;
			case '*':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.Mul;
					#line default
				}
				break;
			case '/':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.Div;
					#line default
				}
				break;
			case '+':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.Add;
					#line default
				}
				break;
			case '-':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.Sub;
					#line default
				}
				break;
			case ';':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.Semicolon;
					#line default
				}
				break;
			case ':':
				{
					la1 = Src.LA(1);
					if (la1 == '=') {
						Src.Skip();
						Src.Skip();
						#line 194 "grammar.ecs"
						_tok.Type = TT.Assign;
						#line default
					} else {
						Src.Skip();
						#line 194 "grammar.ecs"
						_tok.Type = TT.Colon;
						#line default
					}
				}
				break;
			case '=':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.Equals;
					#line default
				}
				break;
			case ',':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.Comma;
					#line default
				}
				break;
			case '#':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.NotEquals;
					#line default
				}
				break;
			case '(':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.LParen;
					#line default
				}
				break;
			case ')':
				{
					Src.Skip();
					#line 194 "grammar.ecs"
					_tok.Type = TT.RParen;
					#line default
				}
				break;
			default:
				if (la0 >= 'A' && la0 <= 'Z' || la0 == '_' || la0 >= 'a' && la0 <= 'z')
					IdOrKeyword();
				else {
					#line 146 "grammar.ecs"
					_tok.Type = TT.EOF;
					#line default
					// Line 146: ([^\$])?
					la0 = Src.LA0;
					if (la0 != -1) {
						Src.Skip();
						#line 146 "grammar.ecs"
						_tok.Type = TT.Unknown;
						#line default
					}
				}
				break;
			}
			#line 148 "grammar.ecs"
			return _tok.Type != TT.EOF;
			#line default
		}
		static readonly HashSet<int> IdOrKeyword_set0 = LexerSource.NewSetOfRanges('0', '9', 'A', 'Z', '_', '_', 'a', 'z');
		void IdOrKeyword()
		{
			int la1, la2, la3, la4, la5, la6, la7, la8;
			// Line 153: ( [A] [R] [R] [A] [Y] [^0-9A-Z_a-z] =>  / [B] [E] [G] [I] [N] [^0-9A-Z_a-z] =>  / [C] [O] [N] [S] [T] [^0-9A-Z_a-z] =>  / [I] [F] [^0-9A-Z_a-z] =>  / [I] [F] [E] [L] [S] [E] [^0-9A-Z_a-z] =>  / [D] [I] [V] [^0-9A-Z_a-z] =>  / [E] [N] [D] [^0-9A-Z_a-z] =>  / [E] [L] [S] [E] [^0-9A-Z_a-z] =>  / [M] [O] [D] [^0-9A-Z_a-z] =>  / [M] [O] [D] [U] [L] [E] [^0-9A-Z_a-z] =>  / [O] [F] [^0-9A-Z_a-z] =>  / [R] [E] [C] [O] [R] [D] [^0-9A-Z_a-z] =>  / [P] [R] [O] [C] [E] [D] [U] [R] [E] [^0-9A-Z_a-z] =>  / [T] [H] [E] [N] [^0-9A-Z_a-z] =>  / [T] [Y] [P] [E] [^0-9A-Z_a-z] =>  / [V] [A] [R] [^0-9A-Z_a-z] =>  / Id )
			do {
				switch (Src.LA0) {
				case 'A':
					{
						la1 = Src.LA(1);
						if (la1 == 'R') {
							la2 = Src.LA(2);
							if (la2 == 'R') {
								la3 = Src.LA(3);
								if (la3 == 'A') {
									la4 = Src.LA(4);
									if (la4 == 'Y') {
										la5 = Src.LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											#line 153 "grammar.ecs"
											_tok.Type = TT.Array;
											#line default
										} else
											goto matchId;
									} else
										goto matchId;
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'B':
					{
						la1 = Src.LA(1);
						if (la1 == 'E') {
							la2 = Src.LA(2);
							if (la2 == 'G') {
								la3 = Src.LA(3);
								if (la3 == 'I') {
									la4 = Src.LA(4);
									if (la4 == 'N') {
										la5 = Src.LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											#line 154 "grammar.ecs"
											_tok.Type = TT.Begin;
											#line default
										} else
											goto matchId;
									} else
										goto matchId;
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'C':
					{
						la1 = Src.LA(1);
						if (la1 == 'O') {
							la2 = Src.LA(2);
							if (la2 == 'N') {
								la3 = Src.LA(3);
								if (la3 == 'S') {
									la4 = Src.LA(4);
									if (la4 == 'T') {
										la5 = Src.LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											#line 155 "grammar.ecs"
											_tok.Type = TT.Const;
											#line default
										} else
											goto matchId;
									} else
										goto matchId;
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'I':
					{
						la1 = Src.LA(1);
						if (la1 == 'F') {
							la2 = Src.LA(2);
							if (!IdOrKeyword_set0.Contains(la2)) {
								Src.Skip();
								Src.Skip();
								#line 156 "grammar.ecs"
								_tok.Type = TT.If;
								#line default
							} else if (la2 == 'E') {
								la3 = Src.LA(3);
								if (la3 == 'L') {
									la4 = Src.LA(4);
									if (la4 == 'S') {
										la5 = Src.LA(5);
										if (la5 == 'E') {
											la6 = Src.LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												#line 157 "grammar.ecs"
												_tok.Type = TT.IfElse;
												#line default
											} else
												goto matchId;
										} else
											goto matchId;
									} else
										goto matchId;
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'D':
					{
						la1 = Src.LA(1);
						if (la1 == 'I') {
							la2 = Src.LA(2);
							if (la2 == 'V') {
								la3 = Src.LA(3);
								if (!IdOrKeyword_set0.Contains(la3)) {
									Src.Skip();
									Src.Skip();
									Src.Skip();
									#line 158 "grammar.ecs"
									_tok.Type = TT.Div;
									#line default
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'E':
					{
						la1 = Src.LA(1);
						if (la1 == 'N') {
							la2 = Src.LA(2);
							if (la2 == 'D') {
								la3 = Src.LA(3);
								if (!IdOrKeyword_set0.Contains(la3)) {
									Src.Skip();
									Src.Skip();
									Src.Skip();
									#line 159 "grammar.ecs"
									_tok.Type = TT.End;
									#line default
								} else
									goto matchId;
							} else
								goto matchId;
						} else if (la1 == 'L') {
							la2 = Src.LA(2);
							if (la2 == 'S') {
								la3 = Src.LA(3);
								if (la3 == 'E') {
									la4 = Src.LA(4);
									if (!IdOrKeyword_set0.Contains(la4)) {
										Src.Skip();
										Src.Skip();
										Src.Skip();
										Src.Skip();
										#line 160 "grammar.ecs"
										_tok.Type = TT.Else;
										#line default
									} else
										goto matchId;
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'M':
					{
						la1 = Src.LA(1);
						if (la1 == 'O') {
							la2 = Src.LA(2);
							if (la2 == 'D') {
								la3 = Src.LA(3);
								if (!IdOrKeyword_set0.Contains(la3)) {
									Src.Skip();
									Src.Skip();
									Src.Skip();
									#line 161 "grammar.ecs"
									_tok.Type = TT.Mod;
									#line default
								} else if (la3 == 'U') {
									la4 = Src.LA(4);
									if (la4 == 'L') {
										la5 = Src.LA(5);
										if (la5 == 'E') {
											la6 = Src.LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												#line 162 "grammar.ecs"
												_tok.Type = TT.Module;
												#line default
											} else
												goto matchId;
										} else
											goto matchId;
									} else
										goto matchId;
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'O':
					{
						la1 = Src.LA(1);
						if (la1 == 'F') {
							la2 = Src.LA(2);
							if (!IdOrKeyword_set0.Contains(la2)) {
								Src.Skip();
								Src.Skip();
								#line 163 "grammar.ecs"
								_tok.Type = TT.Of;
								#line default
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'R':
					{
						la1 = Src.LA(1);
						if (la1 == 'E') {
							la2 = Src.LA(2);
							if (la2 == 'C') {
								la3 = Src.LA(3);
								if (la3 == 'O') {
									la4 = Src.LA(4);
									if (la4 == 'R') {
										la5 = Src.LA(5);
										if (la5 == 'D') {
											la6 = Src.LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												#line 164 "grammar.ecs"
												_tok.Type = TT.Record;
												#line default
											} else
												goto matchId;
										} else
											goto matchId;
									} else
										goto matchId;
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'P':
					{
						la1 = Src.LA(1);
						if (la1 == 'R') {
							la2 = Src.LA(2);
							if (la2 == 'O') {
								la3 = Src.LA(3);
								if (la3 == 'C') {
									la4 = Src.LA(4);
									if (la4 == 'E') {
										la5 = Src.LA(5);
										if (la5 == 'D') {
											la6 = Src.LA(6);
											if (la6 == 'U') {
												la7 = Src.LA(7);
												if (la7 == 'R') {
													la8 = Src.LA(8);
													if (la8 == 'E') {
														Src.Skip();
														Src.Skip();
														Src.Skip();
														Src.Skip();
														Src.Skip();
														Src.Skip();
														Src.Skip();
														Src.Skip();
														Src.Skip();
														#line 165 "grammar.ecs"
														_tok.Type = TT.Procedure;
														#line default
													} else
														goto matchId;
												} else
													goto matchId;
											} else
												goto matchId;
										} else
											goto matchId;
									} else
										goto matchId;
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'T':
					{
						la1 = Src.LA(1);
						if (la1 == 'H') {
							la2 = Src.LA(2);
							if (la2 == 'E') {
								la3 = Src.LA(3);
								if (la3 == 'N') {
									la4 = Src.LA(4);
									if (!IdOrKeyword_set0.Contains(la4)) {
										Src.Skip();
										Src.Skip();
										Src.Skip();
										Src.Skip();
										#line 166 "grammar.ecs"
										_tok.Type = TT.Then;
										#line default
									} else
										goto matchId;
								} else
									goto matchId;
							} else
								goto matchId;
						} else if (la1 == 'Y') {
							la2 = Src.LA(2);
							if (la2 == 'P') {
								la3 = Src.LA(3);
								if (la3 == 'E') {
									la4 = Src.LA(4);
									if (!IdOrKeyword_set0.Contains(la4)) {
										Src.Skip();
										Src.Skip();
										Src.Skip();
										Src.Skip();
										#line 167 "grammar.ecs"
										_tok.Type = TT.Type;
										#line default
									} else
										goto matchId;
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				case 'V':
					{
						la1 = Src.LA(1);
						if (la1 == 'A') {
							la2 = Src.LA(2);
							if (la2 == 'R') {
								la3 = Src.LA(3);
								if (!IdOrKeyword_set0.Contains(la3)) {
									Src.Skip();
									Src.Skip();
									Src.Skip();
									#line 168 "grammar.ecs"
									_tok.Type = TT.Var;
									#line default
								} else
									goto matchId;
							} else
								goto matchId;
						} else
							goto matchId;
					}
					break;
				default:
					goto matchId;
				}
				break;
			matchId:
				{
					Id();
					#line 169 "grammar.ecs"
					_tok.Type = TT.Id;
					#line default
				}
			} while (false);
		}
		static readonly HashSet<int> Id_set0 = LexerSource.NewSetOfRanges('A', 'Z', '_', '_', 'a', 'z');
		static readonly HashSet<int> Id_set1 = LexerSource.NewSetOfRanges('0', '9', 'A', 'Z', '_', '_', 'a', 'z');
		void Id()
		{
			int la0;
			Src.Match(Id_set0);
			// Line 181: ([0-9A-Z_a-z])*
			for (;;) {
				la0 = Src.LA0;
				if (Id_set1.Contains(la0))
					Src.Skip();
				else
					break;
			}
			#line 182 "grammar.ecs"
			_tok.Value = Src.CharSource.Slice(_tok.StartIndex, Src.InputPosition - _tok.StartIndex).ToString();
			#line default
		}
		void Num()
		{
			int la0, la1;
			Src.Skip();
			// Line 186: ([0-9])*
			for (;;) {
				la0 = Src.LA0;
				if (la0 >= '0' && la0 <= '9')
					Src.Skip();
				else
					break;
			}
			// Line 187: ([.] [0-9] ([0-9])*)?
			la0 = Src.LA0;
			if (la0 == '.') {
				la1 = Src.LA(1);
				if (la1 >= '0' && la1 <= '9') {
					Src.Skip();
					Src.Skip();
					// Line 187: ([0-9])*
					for (;;) {
						la0 = Src.LA0;
						if (la0 >= '0' && la0 <= '9')
							Src.Skip();
						else
							break;
					}
				}
			}
			#line 188 "grammar.ecs"
			_tok.Value = Src.CharSource.Slice(_tok.StartIndex, Src.InputPosition - _tok.StartIndex).ToString();
			#line default
		}
	}
	public partial class CompilerParser
	{
		public ParserSource<Token> Src { get; set; }
		internal static ArithmeticRepository CalculationRepository = new ArithmeticRepository();
		public Module module { get; set; }
		public Block block { get; set; }
		public Module Calculate(string input)
		{
			Token EofToken = new Token { 
				Type = TT.EOF
			};
			var lexer = new CalculatorLexer(input);
			Src = new ParserSource<Token>(lexer, EofToken, lexer.Src.SourceFile) { 
				TokenTypeToString = tt => ((TT) tt).ToString()
			};
			var result = Module();
			Src.Match((int) TT.EOF);
			return result;
		}
		void RecordElement(RecordTypeDefinition typeDefinition)
		{
			var i = Src.Match((int) TT.Id);
			Src.Match((int) TT.Colon);
			var v = VarTypeReference(block);
			#line 237 "grammar.ecs"
			string name = i.Value.ToString();
			if (typeDefinition.Elements.Any(x => x.Name == name)) {
				Src.Error(0, "The element '{0}' has been defined already", name);
			} else {
				typeDefinition.Elements.Add(new Declaration(i.Value.ToString(), v));
			}
			#line default
		}
		TypeDefinition VarTypeReference(Block block)
		{
			TokenType la0;
			TypeDefinition result = default(TypeDefinition);
			// Line 249: ( TT.Id | TT.Array SimpleExpression TT.Of VarTypeReference | TT.Record RecordElement TT.Semicolon (RecordElement TT.Semicolon)* TT.End )
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Id) {
				var i = Src.MatchAny();
				#line 250 "grammar.ecs"
				result = block.LookupType(i.Value.ToString());
				if (result == null)
					Src.Error(0, "Type '{0}' not found", i.Value.ToString());
				#line default
			} else if (la0 == TT.Array) {
				Src.Skip();
				var e = SimpleExpression();
				Src.Match((int) TT.Of);
				var t = VarTypeReference(block);
				#line 254 "grammar.ecs"
				var constex = ConstantSolver.Solve(e, block)as ConstantIntExpression;
				if (constex == null) {
					Src.Error(0, "The array size must resolve as INTEGER");
				} else {
					result = new ArrayTypeDefinition(constex.ToInt32(), t);
				}
				#line default
			} else {
				Src.Match((int) TT.Record);
				#line 262 "grammar.ecs"
				var rtd = new RecordTypeDefinition();
				#line default
				RecordElement(rtd);
				Src.Match((int) TT.Semicolon);
				// Line 263: (RecordElement TT.Semicolon)*
				for (;;) {
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.Id) {
						RecordElement(rtd);
						Src.Match((int) TT.Semicolon);
					} else
						break;
				}
				Src.Match((int) TT.End);
				#line 264 "grammar.ecs"
				result = rtd;
				#line default
			}
			return result;
		}
		void IdListElement(Block block, List<string> idList)
		{
			var n = Src.Match((int) TT.Id);
			#line 269 "grammar.ecs"
			string name = n.Value.ToString();
			if (block.Declarations.Any(x => x.Name == name) || idList.Any(x => x == name)) {
				Src.Error(0, "The identifier {0} has been defined already", name);
			} else {
				idList.Add(name);
			}
			#line default
		}
		List<string> IdList(Block block)
		{
			TokenType la0;
			// Line 281: (IdListElement (TT.Comma IdListElement)*)
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Id) {
				#line 281 "grammar.ecs"
				List<string> ids = new List<string>();
				#line default
				IdListElement(block, ids);
				// Line 284: (TT.Comma IdListElement)*
				for (;;) {
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.Comma) {
						Src.Skip();
						IdListElement(block, ids);
					} else
						break;
				}
				#line 286 "grammar.ecs"
				return ids;
				#line default
			} else {
				#line 287 "grammar.ecs"
				Src.Error(0, "An identifier is expected here");
				#line 287 "grammar.ecs"
				return null;
				#line default
			}
		}
		void SingleVarDeclaration(Block block)
		{
			var ids = IdList(block);
			Src.Match((int) TT.Colon);
			var t = VarTypeReference(block);
			#line 293 "grammar.ecs"
			foreach (string v in ids)
				block.Declarations.Add(new Declaration(v, t, block));
			#line default
		}
		void SimpleTypeDeclaration(Block block)
		{
			var i = Src.Match((int) TT.Id);
			Src.Match((int) TT.Equals);
			var v = VarTypeReference(block);
			#line 300 "grammar.ecs"
			string name = i.Value.ToString();
			if (block.Types.Any(x => x.Name == name)) {
				Src.Error(0, "The type '{0}' has been defined already", name);
			} else {
				v.Name = i.Value.ToString();
				block.Types.Add(v);
			}
			#line default
		}
		void SingleConstDeclaration(Block block)
		{
			var i = Src.Match((int) TT.Id);
			Src.Match((int) TT.Equals);
			var e = SimpleExpression();
			#line 314 "grammar.ecs"
			string name = i.Value.ToString();
			if (block.Declarations.Any(x => x.Name == name)) {
				Src.Error(0, "The identifier '{0}' has been defined already", name);
			} else {
				var constex = ConstantSolver.Solve(e, block)as ConstantExpression;
				block.Declarations.Add(new ConstDeclaration(name, new SimpleTypeDefinition(constex.TargetType, name), constex, block));
			}
			#line default
		}
		void ProcedureBody(ProcedureDeclaration proc)
		{
			TokenType la0;
			Declarations(proc.Block);
			// Line 328: (TT.Begin StatementSequence)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Begin) {
				Src.Skip();
				StatementSequence(proc.Block);
			}
			Src.Match((int) TT.End);
			var n = Src.Match((int) TT.Id);
			#line 330 "grammar.ecs"
			string name = n.Value.ToString();
			if (name != proc.Name) {
				Src.Error(0, "END name does not match procedure declaration");
			}
			#line default
		}
		void FPSection(ProcedureDeclaration proc)
		{
			TokenType la0;
			#line 338 "grammar.ecs"
			bool isVar = false;
			#line default
			// Line 339: (TT.Var)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Var) {
				Src.Skip();
				#line 339 "grammar.ecs"
				isVar = true;
				#line default
			}
			var n = Src.Match((int) TT.Id);
			Src.Match((int) TT.Colon);
			var v = VarTypeReference(proc.Block);
			#line 342 "grammar.ecs"
			string name = n.Value.ToString();
			proc.Parameters.Add(new ProcedureParameter(name, v, isVar));
			#line default
		}
		void FormalParameters(ProcedureDeclaration proc)
		{
			TokenType la0;
			FPSection(proc);
			// Line 349: (TT.Semicolon FPSection)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Semicolon) {
					Src.Skip();
					FPSection(proc);
				} else
					break;
			}
		}
		ProcedureDeclaration ProcedureHeading(Block block)
		{
			TokenType la0;
			ProcedureDeclaration result = default(ProcedureDeclaration);
			Src.Skip();
			var n = Src.Match((int) TT.Id);
			#line 355 "grammar.ecs"
			string name = n.Value.ToString();
			if (block.Procedures.Any(x => x.Name == name)) {
				Src.Error(0, "This procedure has been defined before");
			}
			#line 360 "grammar.ecs"
			result = new ProcedureDeclaration(name, block);
			block.Procedures.Add(result);
			#line default
			// Line 363: (TT.LParen FormalParameters TT.RParen)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.LParen) {
				Src.Skip();
				FormalParameters(result);
				Src.Match((int) TT.RParen);
			}
			return result;
		}
		void ProcedureDeclaration(Block block)
		{
			var p = ProcedureHeading(block);
			Src.Match((int) TT.Semicolon);
			ProcedureBody(p);
		}
		void Declarations(Block block)
		{
			TokenType la0;
			// Line 372: (TT.Const SingleConstDeclaration TT.Semicolon (SingleConstDeclaration TT.Semicolon)*)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Const) {
				Src.Skip();
				SingleConstDeclaration(block);
				Src.Match((int) TT.Semicolon);
				// Line 372: (SingleConstDeclaration TT.Semicolon)*
				for (;;) {
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.Id) {
						SingleConstDeclaration(block);
						Src.Match((int) TT.Semicolon);
					} else
						break;
				}
			}
			// Line 373: (TT.Type SimpleTypeDeclaration TT.Semicolon (SimpleTypeDeclaration TT.Semicolon)*)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Type) {
				Src.Skip();
				SimpleTypeDeclaration(block);
				Src.Match((int) TT.Semicolon);
				// Line 373: (SimpleTypeDeclaration TT.Semicolon)*
				for (;;) {
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.Id) {
						SimpleTypeDeclaration(block);
						Src.Match((int) TT.Semicolon);
					} else
						break;
				}
			}
			// Line 374: (TT.Var SingleVarDeclaration TT.Semicolon (SingleVarDeclaration TT.Semicolon)*)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Var) {
				Src.Skip();
				SingleVarDeclaration(block);
				Src.Match((int) TT.Semicolon);
				// Line 374: (SingleVarDeclaration TT.Semicolon)*
				for (;;) {
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.Id) {
						SingleVarDeclaration(block);
						Src.Match((int) TT.Semicolon);
					} else
						break;
				}
			}
			// Line 375: (ProcedureDeclaration TT.Semicolon)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Procedure) {
					ProcedureDeclaration(block);
					Src.Match((int) TT.Semicolon);
				} else
					break;
			}
		}
		Expression Term()
		{
			TokenType la0, la1;
			Expression result = default(Expression);
			// Line 380: ( TT.Id | TT.Num | TT.LParen SimpleExpression TT.RParen )
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Id) {
				var t = Src.MatchAny();
				#line 380 "grammar.ecs"
				result = VariableReferenceExpression.Create(block, t.Value.ToString());
				if (result == null) {
					Src.Error(0, "The variable {0} is not known", t.Value);
				}
				#line default
			} else if (la0 == TT.Num) {
				var t = Src.MatchAny();
				#line 383 "grammar.ecs"
				result = ConstantExpression.Create(t);
				#line default
			} else if (la0 == TT.LParen) {
				Src.Skip();
				result = SimpleExpression();
				Src.Match((int) TT.RParen);
			} else {
				#line 385 "grammar.ecs"
				result = null;
				#line 385 "grammar.ecs"
				Src.Error(0, "Expected identifer, number, or (parens)");
				#line default
			}
			// Line 388: greedy(TT.Exp Term)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Exp) {
					la1 = (TokenType) Src.LA(1);
					if (la1 == TT.Id || la1 == TT.LParen || la1 == TT.Num) {
						Src.Skip();
						var t = Term();
						#line 388 "grammar.ecs"
						result = BinaryExpression.Create(TT.Exp, result, t);
						#line default
					} else
						break;
				} else
					break;
			}
			return result;
		}
		Expression PrefixExpr()
		{
			TokenType la0;
			Expression result = default(Expression);
			// Line 392: (TT.Sub Term | Term)
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Sub) {
				Src.Skip();
				var t = Term();
				#line 392 "grammar.ecs"
				result = Expression.CreateUnary(TT.Unary, t);
				#line default
			} else {
				var t = Term();
				#line 393 "grammar.ecs"
				result = t;
				#line default
			}
			return result;
		}
		Expression MulExpr()
		{
			TokenType la0;
			Expression result = default(Expression);
			result = PrefixExpr();
			// Line 398: ((TT.Div|TT.Mod|TT.Mul) PrefixExpr)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Div || la0 == TT.Mod || la0 == TT.Mul) {
					var op = Src.MatchAny();
					var rhs = PrefixExpr();
					#line 398 "grammar.ecs"
					result = BinaryExpression.Create(op.Type, result, rhs);
					#line default
				} else
					break;
			}
			return result;
		}
		Expression SimpleExpression()
		{
			TokenType la0;
			Expression result = default(Expression);
			result = MulExpr();
			// Line 403: ((TT.Add|TT.Sub) MulExpr)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Add || la0 == TT.Sub) {
					var op = Src.MatchAny();
					var rhs = MulExpr();
					#line 403 "grammar.ecs"
					result = BinaryExpression.Create(op.Type, result, rhs);
					#line default
				} else
					break;
			}
			return result;
		}
		Expression RelationalExpression()
		{
			Expression result = default(Expression);
			result = SimpleExpression();
			#line 409 "grammar.ecs"
			if (result.TargetType != BaseType.BoolType) {
				Src.Error(0, "Expression which can evaluate to bool required");
			}
			#line default
			// Line 413: ((TT.Equals|TT.GE|TT.GT|TT.LE|TT.LT|TT.NotEquals) SimpleExpression)?
			switch ((TokenType) Src.LA0) {
			case TT.Equals:
			case TT.GE:
			case TT.GT:
			case TT.LE:
			case TT.LT:
			case TT.NotEquals:
				{
					var t = Src.MatchAny();
					var rhs = SimpleExpression();
					#line 414 "grammar.ecs"
					result = BinaryExpression.Create(t.Type, result, rhs);
					#line default
				}
				break;
			}
			return result;
		}
		void StatementSequence(Block block)
		{
			TokenType la0;
			// Line 423: (SingleStatement TT.Semicolon)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Id || la0 == TT.If) {
					SingleStatement(block);
					Src.Match((int) TT.Semicolon);
				} else
					break;
			}
		}
		void AssignmentStmt(Block block)
		{
			var v = Src.MatchAny();
			#line 429 "grammar.ecs"
			var variable = block.LookupVar(v.Value.ToString());
			if (variable == null) {
				Src.Error(0, "Variable not found");
			}
			#line default
			Src.Match((int) TT.Assign);
			var e = SimpleExpression();
			#line 436 "grammar.ecs"
			block.Statements.Add(new AssignmentStatement { 
				Variable = variable, Expression = e
			});
			#line default
		}
		void IfStmt(Block block)
		{
			TokenType la0;
			#line 441 "grammar.ecs"
			IfStatement ifs = new IfStatement();
			#line default
			var c = RelationalExpression();
			Src.Match((int) TT.Then);
			#line 443 "grammar.ecs"
			Block thenBlock = new Block();
			#line 443 "grammar.ecs"
			thenBlock.Parent = block;
			#line default
			StatementSequence(thenBlock);
			#line 444 "grammar.ecs"
			ifs.Conditions.Add(c);
			#line 444 "grammar.ecs"
			ifs.ThenParts.Add(thenBlock);
			#line default
			// Line 445: (TT.IfElse RelationalExpression TT.Then StatementSequence)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.IfElse) {
					Src.Skip();
					var c1 = RelationalExpression();
					Src.Match((int) TT.Then);
					#line 446 "grammar.ecs"
					thenBlock = new Block();
					#line 446 "grammar.ecs"
					thenBlock.Parent = block;
					#line default
					StatementSequence(thenBlock);
					#line 448 "grammar.ecs"
					ifs.Conditions.Add(c1);
					#line 448 "grammar.ecs"
					ifs.ThenParts.Add(thenBlock);
					#line default
				} else
					break;
			}
			// Line 450: (TT.Else StatementSequence)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Else) {
				Src.Skip();
				#line 451 "grammar.ecs"
				Block elseBlock = new Block();
				#line 451 "grammar.ecs"
				elseBlock.Parent = block;
				#line default
				StatementSequence(elseBlock);
			}
			#line 454 "grammar.ecs"
			block.Statements.Add(ifs);
			#line default
		}
		void SingleStatement(Block block)
		{
			TokenType la0;
			// Line 458: (AssignmentStmt | TT.If IfStmt)
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Id)
				AssignmentStmt(block);
			else {
				Src.Match((int) TT.If);
				IfStmt(block);
			}
		}
		Module Module()
		{
			TokenType la0;
			Module result = default(Module);
			#line 464 "grammar.ecs"
			result = module = new Module();
			#line 464 "grammar.ecs"
			block = module.Block;
			#line default
			Src.Match((int) TT.Module);
			var m = Src.Match((int) TT.Id);
			Src.Match((int) TT.Semicolon);
			Declarations(result.Block);
			// Line 466: (TT.Begin StatementSequence)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Begin) {
				Src.Skip();
				StatementSequence(block);
			}
			Src.Match((int) TT.End);
			Src.Match((int) TT.Dot);
			result.Name = m.Value.ToString();
			return result;
		}
	}
}
