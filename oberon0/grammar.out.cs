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
namespace Oberon0.Compiler
{
	using TT = TokenType;
	public enum TokenType
	{
		EOF = 0, Or, Mod, Module, If, Then, IfElse, Else, Begin, End, While, Do, Repeat, Until, Type, Var, Const, Array, Of, Record, Procedure, Id, Num, Shr, GE, GT, Shl, LE, LT, Dot, Exp, Mul, Div, Add, Not, Sub, Semicolon, Assign, Equals, Colon, Comma, NotEquals, LParen, LBracket, RBracket, RParen, Unary, Unknown
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
			// Line 138: ([\t\n\r ])*
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
			// Line 141: ( (Num | [.] [n] [a] [n] | [.] [i] [n] [f]) | ([>] [>] / [>] [=] / [>] / [<] [<] / [<] [=] / [<] / [.] / [\^] / [*] / [/] / [+] / [~] / [\-] / [;] / [:] [=] / [=] / [:] / [,] / [#] / [(] / [[] / [\]] / [)]) | IdOrKeyword )
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
					#line 141 "grammar.ecs"
					_tok.Type = TT.Num;
					#line default
					Num();
				}
				break;
			case '.':
				{
					la1 = Src.LA(1);
					if (la1 == 'n') {
						#line 142 "grammar.ecs"
						_tok.Type = TT.Num;
						#line default
						Src.Skip();
						Src.Skip();
						Src.Match('a');
						Src.Match('n');
						#line 142 "grammar.ecs"
						_tok.Value = double.NaN;
						#line default
					} else if (la1 == 'i') {
						#line 143 "grammar.ecs"
						_tok.Type = TT.Num;
						#line default
						Src.Skip();
						Src.Skip();
						Src.Match('n');
						Src.Match('f');
						#line 143 "grammar.ecs"
						_tok.Value = double.PositiveInfinity;
						#line default
					} else {
						Src.Skip();
						#line 199 "grammar.ecs"
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
						#line 199 "grammar.ecs"
						_tok.Type = TT.Shr;
						#line default
					} else if (la1 == '=') {
						Src.Skip();
						Src.Skip();
						#line 199 "grammar.ecs"
						_tok.Type = TT.GE;
						#line default
					} else {
						Src.Skip();
						#line 199 "grammar.ecs"
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
						#line 199 "grammar.ecs"
						_tok.Type = TT.Shl;
						#line default
					} else if (la1 == '=') {
						Src.Skip();
						Src.Skip();
						#line 199 "grammar.ecs"
						_tok.Type = TT.LE;
						#line default
					} else {
						Src.Skip();
						#line 199 "grammar.ecs"
						_tok.Type = TT.LT;
						#line default
					}
				}
				break;
			case '^':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.Exp;
					#line default
				}
				break;
			case '*':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.Mul;
					#line default
				}
				break;
			case '/':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.Div;
					#line default
				}
				break;
			case '+':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.Add;
					#line default
				}
				break;
			case '~':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.Not;
					#line default
				}
				break;
			case '-':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.Sub;
					#line default
				}
				break;
			case ';':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
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
						#line 199 "grammar.ecs"
						_tok.Type = TT.Assign;
						#line default
					} else {
						Src.Skip();
						#line 199 "grammar.ecs"
						_tok.Type = TT.Colon;
						#line default
					}
				}
				break;
			case '=':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.Equals;
					#line default
				}
				break;
			case ',':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.Comma;
					#line default
				}
				break;
			case '#':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.NotEquals;
					#line default
				}
				break;
			case '(':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.LParen;
					#line default
				}
				break;
			case '[':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.LBracket;
					#line default
				}
				break;
			case ']':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.RBracket;
					#line default
				}
				break;
			case ')':
				{
					Src.Skip();
					#line 199 "grammar.ecs"
					_tok.Type = TT.RParen;
					#line default
				}
				break;
			default:
				if (la0 >= 'A' && la0 <= 'Z' || la0 == '_' || la0 >= 'a' && la0 <= 'z')
					IdOrKeyword();
				else {
					#line 147 "grammar.ecs"
					_tok.Type = TT.EOF;
					#line default
					// Line 147: ([^\$])?
					la0 = Src.LA0;
					if (la0 != -1) {
						Src.Skip();
						#line 147 "grammar.ecs"
						_tok.Type = TT.Unknown;
						#line default
					}
				}
				break;
			}
			#line 149 "grammar.ecs"
			return _tok.Type != TT.EOF;
			#line default
		}
		static readonly HashSet<int> IdOrKeyword_set0 = LexerSource.NewSetOfRanges('0', '9', 'A', 'Z', '_', '_', 'a', 'z');
		void IdOrKeyword()
		{
			int la1, la2, la3, la4, la5, la6, la7, la8;
			// Line 154: ( [A] [R] [R] [A] [Y] [^0-9A-Z_a-z] =>  / [B] [E] [G] [I] [N] [^0-9A-Z_a-z] =>  / [C] [O] [N] [S] [T] [^0-9A-Z_a-z] =>  / [D] [O] [^0-9A-Z_a-z] =>  / [I] [F] [^0-9A-Z_a-z] =>  / [D] [I] [V] [^0-9A-Z_a-z] =>  / [E] [N] [D] [^0-9A-Z_a-z] =>  / [E] [L] [S] [E] [^0-9A-Z_a-z] =>  / [E] [L] [S] [I] [F] [^0-9A-Z_a-z] =>  / [M] [O] [D] [^0-9A-Z_a-z] =>  / [M] [O] [D] [U] [L] [E] [^0-9A-Z_a-z] =>  / [O] [F] [^0-9A-Z_a-z] =>  / [R] [E] [C] [O] [R] [D] [^0-9A-Z_a-z] =>  / [R] [E] [P] [E] [A] [T] [^0-9A-Z_a-z] =>  / [P] [R] [O] [C] [E] [D] [U] [R] [E] [^0-9A-Z_a-z] =>  / [T] [H] [E] [N] [^0-9A-Z_a-z] =>  / [T] [Y] [P] [E] [^0-9A-Z_a-z] =>  / [U] [N] [T] [I] [L] [^0-9A-Z_a-z] =>  / [V] [A] [R] [^0-9A-Z_a-z] =>  / [W] [H] [I] [L] [E] [^0-9A-Z_a-z] =>  / Id )
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
											#line 154 "grammar.ecs"
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
											#line 155 "grammar.ecs"
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
											#line 156 "grammar.ecs"
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
				case 'D':
					{
						la1 = Src.LA(1);
						if (la1 == 'O') {
							la2 = Src.LA(2);
							if (!IdOrKeyword_set0.Contains(la2)) {
								Src.Skip();
								Src.Skip();
								#line 157 "grammar.ecs"
								_tok.Type = TT.Do;
								#line default
							} else
								goto matchId;
						} else if (la1 == 'I') {
							la2 = Src.LA(2);
							if (la2 == 'V') {
								la3 = Src.LA(3);
								if (!IdOrKeyword_set0.Contains(la3)) {
									Src.Skip();
									Src.Skip();
									Src.Skip();
									#line 159 "grammar.ecs"
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
				case 'I':
					{
						la1 = Src.LA(1);
						if (la1 == 'F') {
							la2 = Src.LA(2);
							if (!IdOrKeyword_set0.Contains(la2)) {
								Src.Skip();
								Src.Skip();
								#line 158 "grammar.ecs"
								_tok.Type = TT.If;
								#line default
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
									#line 160 "grammar.ecs"
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
										#line 161 "grammar.ecs"
										_tok.Type = TT.Else;
										#line default
									} else
										goto matchId;
								} else if (la3 == 'I') {
									la4 = Src.LA(4);
									if (la4 == 'F') {
										la5 = Src.LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											#line 162 "grammar.ecs"
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
									#line 163 "grammar.ecs"
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
												#line 164 "grammar.ecs"
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
								#line 165 "grammar.ecs"
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
												#line 166 "grammar.ecs"
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
							} else if (la2 == 'P') {
								la3 = Src.LA(3);
								if (la3 == 'E') {
									la4 = Src.LA(4);
									if (la4 == 'A') {
										la5 = Src.LA(5);
										if (la5 == 'T') {
											la6 = Src.LA(6);
											if (!IdOrKeyword_set0.Contains(la6)) {
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												Src.Skip();
												#line 167 "grammar.ecs"
												_tok.Type = TT.Repeat;
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
														#line 168 "grammar.ecs"
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
										#line 169 "grammar.ecs"
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
										#line 170 "grammar.ecs"
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
				case 'U':
					{
						la1 = Src.LA(1);
						if (la1 == 'N') {
							la2 = Src.LA(2);
							if (la2 == 'T') {
								la3 = Src.LA(3);
								if (la3 == 'I') {
									la4 = Src.LA(4);
									if (la4 == 'L') {
										la5 = Src.LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											#line 171 "grammar.ecs"
											_tok.Type = TT.Until;
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
									#line 172 "grammar.ecs"
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
				case 'W':
					{
						la1 = Src.LA(1);
						if (la1 == 'H') {
							la2 = Src.LA(2);
							if (la2 == 'I') {
								la3 = Src.LA(3);
								if (la3 == 'L') {
									la4 = Src.LA(4);
									if (la4 == 'E') {
										la5 = Src.LA(5);
										if (!IdOrKeyword_set0.Contains(la5)) {
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											Src.Skip();
											#line 173 "grammar.ecs"
											_tok.Type = TT.While;
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
				default:
					goto matchId;
				}
				break;
			matchId:
				{
					Id();
					#line 174 "grammar.ecs"
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
			// Line 186: ([0-9A-Z_a-z])*
			for (;;) {
				la0 = Src.LA0;
				if (Id_set1.Contains(la0))
					Src.Skip();
				else
					break;
			}
			#line 187 "grammar.ecs"
			_tok.Value = Src.CharSource.Slice(_tok.StartIndex, Src.InputPosition - _tok.StartIndex).ToString();
			#line default
		}
		void Num()
		{
			int la0, la1;
			Src.Skip();
			// Line 191: ([0-9])*
			for (;;) {
				la0 = Src.LA0;
				if (la0 >= '0' && la0 <= '9')
					Src.Skip();
				else
					break;
			}
			// Line 192: ([.] [0-9] ([0-9])*)?
			la0 = Src.LA0;
			if (la0 == '.') {
				la1 = Src.LA(1);
				if (la1 >= '0' && la1 <= '9') {
					Src.Skip();
					Src.Skip();
					// Line 192: ([0-9])*
					for (;;) {
						la0 = Src.LA0;
						if (la0 >= '0' && la0 <= '9')
							Src.Skip();
						else
							break;
					}
				}
			}
			#line 193 "grammar.ecs"
			_tok.Value = Src.CharSource.Slice(_tok.StartIndex, Src.InputPosition - _tok.StartIndex).ToString();
			#line default
		}
	}
	public partial class CompilerParser
	{
		public ParserSource<Token> Src { get; set; }
		public Module module { get; set; }
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
		void RecordElement(RecordTypeDefinition typeDefinition, Block block)
		{
			var i = Src.Match((int) TT.Id);
			Src.Match((int) TT.Colon);
			var v = VarTypeReference(block);
			#line 246 "grammar.ecs"
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
			// Line 258: ( TT.Id | TT.Array SimpleExpression TT.Of VarTypeReference | TT.Record RecordElement TT.Semicolon (RecordElement TT.Semicolon)* TT.End )
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Id) {
				var i = Src.MatchAny();
				#line 259 "grammar.ecs"
				result = block.LookupType(i.Value.ToString());
				if (result == null)
					Src.Error(0, "Type '{0}' not found", i.Value.ToString());
				#line default
			} else if (la0 == TT.Array) {
				Src.Skip();
				var e = SimpleExpression(block);
				Src.Match((int) TT.Of);
				var t = VarTypeReference(block);
				#line 263 "grammar.ecs"
				var constex = ConstantSolver.Solve(e, block)as ConstantIntExpression;
				if (constex == null) {
					Src.Error(0, "The array size must resolve as INTEGER");
				} else {
					result = new ArrayTypeDefinition(constex.ToInt32(), t);
				}
				#line default
			} else {
				Src.Match((int) TT.Record);
				#line 271 "grammar.ecs"
				var rtd = new RecordTypeDefinition();
				#line default
				RecordElement(rtd, block);
				Src.Match((int) TT.Semicolon);
				// Line 272: (RecordElement TT.Semicolon)*
				for (;;) {
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.Id) {
						RecordElement(rtd, block);
						Src.Match((int) TT.Semicolon);
					} else
						break;
				}
				Src.Match((int) TT.End);
				#line 273 "grammar.ecs"
				result = rtd;
				#line default
			}
			return result;
		}
		void IdListElement(Block block, List<string> idList)
		{
			var n = Src.Match((int) TT.Id);
			#line 278 "grammar.ecs"
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
			// Line 290: (IdListElement (TT.Comma IdListElement)*)
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Id) {
				#line 290 "grammar.ecs"
				List<string> ids = new List<string>();
				#line default
				IdListElement(block, ids);
				// Line 293: (TT.Comma IdListElement)*
				for (;;) {
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.Comma) {
						Src.Skip();
						IdListElement(block, ids);
					} else
						break;
				}
				#line 295 "grammar.ecs"
				return ids;
				#line default
			} else {
				#line 296 "grammar.ecs"
				Src.Error(0, "An identifier is expected here");
				#line 296 "grammar.ecs"
				return null;
				#line default
			}
		}
		void SingleVarDeclaration(Block block)
		{
			var ids = IdList(block);
			Src.Match((int) TT.Colon);
			var t = VarTypeReference(block);
			#line 302 "grammar.ecs"
			foreach (string v in ids)
				block.Declarations.Add(new Declaration(v, t, block));
			#line default
		}
		void SimpleTypeDeclaration(Block block)
		{
			var i = Src.Match((int) TT.Id);
			Src.Match((int) TT.Equals);
			var v = VarTypeReference(block);
			#line 309 "grammar.ecs"
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
			var e = SimpleExpression(block);
			#line 323 "grammar.ecs"
			string name = i.Value.ToString();
			if (block.Declarations.Any(x => x.Name == name)) {
				Src.Error(0, "The identifier '{0}' has been defined already", name);
			} else {
				var constex = ConstantSolver.Solve(e, block)as ConstantExpression;
				block.Declarations.Add(new ConstDeclaration(name, new SimpleTypeDefinition(constex.TargetType, name), constex, block));
			}
			#line default
		}
		void ProcedureBody(FunctionDeclaration proc)
		{
			TokenType la0;
			Declarations(proc.Block);
			// Line 337: (TT.Begin StatementSequence)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Begin) {
				Src.Skip();
				StatementSequence(proc.Block);
			}
			Src.Match((int) TT.End);
			var n = Src.Match((int) TT.Id);
			#line 339 "grammar.ecs"
			string name = n.Value.ToString();
			if (name != proc.Name) {
				Src.Error(0, "END name does not match procedure declaration");
			}
			#line default
		}
		void FPSection(FunctionDeclaration proc)
		{
			TokenType la0;
			#line 347 "grammar.ecs"
			bool isVar = false;
			#line default
			// Line 348: (TT.Var)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Var) {
				Src.Skip();
				#line 348 "grammar.ecs"
				isVar = true;
				#line default
			}
			var n = Src.Match((int) TT.Id);
			Src.Match((int) TT.Colon);
			var v = VarTypeReference(proc.Block);
			#line 351 "grammar.ecs"
			string name = n.Value.ToString();
			proc.Parameters.Add(new ProcedureParameter(name, v, isVar));
			#line default
		}
		void FormalParameters(FunctionDeclaration proc)
		{
			TokenType la0;
			FPSection(proc);
			// Line 358: (TT.Semicolon FPSection)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Semicolon) {
					Src.Skip();
					FPSection(proc);
				} else
					break;
			}
		}
		FunctionDeclaration ProcedureHeading(Block block)
		{
			TokenType la0;
			FunctionDeclaration result = default(FunctionDeclaration);
			Src.Skip();
			var n = Src.Match((int) TT.Id);
			#line 364 "grammar.ecs"
			string name = n.Value.ToString();
			if (block.Procedures.Any(x => x.Name == name)) {
				Src.Error(0, "This procedure has been defined before");
			}
			#line 369 "grammar.ecs"
			result = new FunctionDeclaration(name, block);
			#line default
			// Line 371: (TT.LParen FormalParameters TT.RParen)?
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
			#line 378 "grammar.ecs"
			block.Procedures.Add(p);
			#line default
		}
		void Declarations(Block block)
		{
			TokenType la0;
			// Line 383: (TT.Const SingleConstDeclaration TT.Semicolon (SingleConstDeclaration TT.Semicolon)*)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Const) {
				Src.Skip();
				SingleConstDeclaration(block);
				Src.Match((int) TT.Semicolon);
				// Line 383: (SingleConstDeclaration TT.Semicolon)*
				for (;;) {
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.Id) {
						SingleConstDeclaration(block);
						Src.Match((int) TT.Semicolon);
					} else
						break;
				}
			}
			// Line 384: (TT.Type SimpleTypeDeclaration TT.Semicolon (SimpleTypeDeclaration TT.Semicolon)*)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Type) {
				Src.Skip();
				SimpleTypeDeclaration(block);
				Src.Match((int) TT.Semicolon);
				// Line 384: (SimpleTypeDeclaration TT.Semicolon)*
				for (;;) {
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.Id) {
						SimpleTypeDeclaration(block);
						Src.Match((int) TT.Semicolon);
					} else
						break;
				}
			}
			// Line 385: (TT.Var SingleVarDeclaration TT.Semicolon (SingleVarDeclaration TT.Semicolon)*)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Var) {
				Src.Skip();
				SingleVarDeclaration(block);
				Src.Match((int) TT.Semicolon);
				// Line 385: (SingleVarDeclaration TT.Semicolon)*
				for (;;) {
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.Id) {
						SingleVarDeclaration(block);
						Src.Match((int) TT.Semicolon);
					} else
						break;
				}
			}
			// Line 386: (ProcedureDeclaration TT.Semicolon)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Procedure) {
					ProcedureDeclaration(block);
					Src.Match((int) TT.Semicolon);
				} else
					break;
			}
		}
		List<Expression> ParameterList(Block block)
		{
			TokenType la0;
			Expression r1 = default(Expression);
			List<Expression> result = default(List<Expression>);
			result = new List<Expression>();
			// Line 391: (RelationalExpression (TT.Comma RelationalExpression)*)?
			switch ((TokenType) Src.LA0) {
			case TT.Id:
			case TT.LParen:
			case TT.Not:
			case TT.Num:
			case TT.Sub:
				{
					var r = RelationalExpression(block);
					#line 391 "grammar.ecs"
					result.Add(r);
					#line default
					// Line 392: (TT.Comma RelationalExpression)*
					for (;;) {
						la0 = (TokenType) Src.LA0;
						if (la0 == TT.Comma) {
							Src.Skip();
							r1 = RelationalExpression(block);
							#line 392 "grammar.ecs"
							result.Add(r1);
							#line default
						} else
							break;
					}
				}
				break;
			}
			return result;
		}
		Expression Term(Block block)
		{
			TokenType la0;
			List<Expression> pl1 = default(List<Expression>);
			Expression result = default(Expression);
			#line 397 "grammar.ecs"
			string name = string.Empty;
			#line 397 "grammar.ecs"
			var pl = new List<Expression>();
			#line default
			// Line 400: ( TT.Id Selector (TT.LParen ParameterList ()? TT.RParen)? | TT.Num | TT.LParen SimpleExpression TT.RParen | TT.Not Term )
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Id) {
				var t = Src.MatchAny();
				#line 400 "grammar.ecs"
				name = t.Value.ToString();
				#line default
				var s = Selector(block);
				// Line 402: (TT.LParen ParameterList ()? TT.RParen)?
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.LParen) {
					Src.Skip();
					pl1 = ParameterList(block);
					// Line 402: ()?
					la0 = (TokenType) Src.LA0;
					if (la0 == TT.RParen) {
						#line 402 "grammar.ecs"
						if (pl1 != null)
							pl.AddRange(pl1);
						#line default
					}
					Src.Match((int) TT.RParen);
					#line 403 "grammar.ecs"
					FunctionDeclaration f = block.LookupFunction(name);
					if (f == null) {
						Src.Error(0, "There's no function named '{0}'", name);
					}
					if (f.ReturnType == BaseType.VoidType) {
						Src.Error(0, "You cannot call procedure '{0}' as a function", name);
					}
					#line 408 "grammar.ecs"
					if (f.Parameters.Count != pl.Count) {
						Src.Error(0, "The number of parameters doesn't match (Expected {0}, found {1})", f.Parameters.Count, pl.Count);
					}
					#line 411 "grammar.ecs"
					if (s.Count > 0) {
						Src.Error(0, "Using selectors for functions is not allowed");
					}
					result = new FunctionCallExpression(f, block, pl.ToArray());
					#line default
				}
				#line 416 "grammar.ecs"
				if (result == null) {
					result = VariableReferenceExpression.Create(block, name, s);
					if (result == null) {
						Src.Error(0, "The variable {0} is not known", name);
					}
				}
				#line default
			} else if (la0 == TT.Num) {
				var t = Src.MatchAny();
				#line 422 "grammar.ecs"
				result = ConstantExpression.Create(t.Value);
				#line default
			} else if (la0 == TT.LParen) {
				Src.Skip();
				result = SimpleExpression(block);
				Src.Match((int) TT.RParen);
			} else if (la0 == TT.Not) {
				Src.Skip();
				var n = Term(block);
				#line 424 "grammar.ecs"
				result = BinaryExpression.Create(TT.Not, n, new ConstantBoolExpression(false));
				#line default
			} else {
				#line 425 "grammar.ecs"
				result = null;
				#line 425 "grammar.ecs"
				Src.Error(0, "Expected identifer, number, or (parens)");
				#line default
			}
			// Line 428: greedy(TT.Exp Term)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Exp) {
					switch ((TokenType) Src.LA(1)) {
					case TT.Id:
					case TT.LParen:
					case TT.Not:
					case TT.Num:
						{
							Src.Skip();
							var e = Term(block);
							#line 428 "grammar.ecs"
							result = BinaryExpression.Create(TT.Exp, result, e);
							#line default
						}
						break;
					default:
						goto stop;
					}
				} else
					goto stop;
			}
		stop:;
			return result;
		}
		Expression PrefixExpr(Block block)
		{
			TokenType la0;
			Expression result = default(Expression);
			// Line 432: (TT.Sub Term | Term)
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Sub) {
				Src.Skip();
				var t = Term(block);
				#line 432 "grammar.ecs"
				result = BinaryExpression.Create(TT.Sub, ConstantExpression.Create(0), t);
				#line default
			} else {
				var t = Term(block);
				#line 433 "grammar.ecs"
				result = t;
				#line default
			}
			return result;
		}
		Expression MulExpr(Block block)
		{
			TokenType la0;
			Expression result = default(Expression);
			result = PrefixExpr(block);
			// Line 438: ((TT.Div|TT.Mod|TT.Mul) PrefixExpr)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Div || la0 == TT.Mod || la0 == TT.Mul) {
					var op = Src.MatchAny();
					var rhs = PrefixExpr(block);
					#line 438 "grammar.ecs"
					result = BinaryExpression.Create(op.Type, result, rhs);
					#line default
				} else
					break;
			}
			return result;
		}
		Expression SimpleExpression(Block block)
		{
			TokenType la0;
			Expression result = default(Expression);
			result = MulExpr(block);
			// Line 443: ((TT.Add|TT.Sub) MulExpr)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Add || la0 == TT.Sub) {
					var op = Src.MatchAny();
					var rhs = MulExpr(block);
					#line 443 "grammar.ecs"
					result = BinaryExpression.Create(op.Type, result, rhs);
					#line default
				} else
					break;
			}
			return result;
		}
		Expression RelationalExpression(Block block)
		{
			Expression result = default(Expression);
			result = SimpleExpression(block);
			// Line 448: ((TT.Equals|TT.GE|TT.GT|TT.LE|TT.LT|TT.NotEquals) SimpleExpression)?
			switch ((TokenType) Src.LA0) {
			case TT.Equals:
			case TT.GE:
			case TT.GT:
			case TT.LE:
			case TT.LT:
			case TT.NotEquals:
				{
					var t = Src.MatchAny();
					var rhs = SimpleExpression(block);
					#line 449 "grammar.ecs"
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
			SingleStatement(block);
			// Line 455: (TT.Semicolon SingleStatement)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Semicolon) {
					Src.Skip();
					SingleStatement(block);
				} else
					break;
			}
		}
		void AssignmentStmt(string varName, VariableSelector selector, Block block)
		{
			#line 460 "grammar.ecs"
			var variable = block.LookupVar(varName);
			if (variable == null) {
				Src.Error(0, "Variable '{0}' not found", varName);
			}
			#line default
			var e = RelationalExpression(block);
			#line 470 "grammar.ecs"
			block.Statements.Add(new AssignmentStatement { 
				Variable = variable, Expression = e
			});
			#line default
		}
		VariableSelector Selector(Block block)
		{
			TokenType la0;
			VariableSelector result = default(VariableSelector);
			result = new VariableSelector();
			// Line 476: (TT.Dot TT.Id | TT.LBracket SimpleExpression TT.RBracket)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.Dot) {
					Src.Skip();
					var i = Src.Match((int) TT.Id);
					#line 476 "grammar.ecs"
					result.Add(new IdentifierSelector(i.Value.ToString()));
					#line default
				} else if (la0 == TT.LBracket) {
					Src.Skip();
					var e = SimpleExpression(block);
					Src.Match((int) TT.RBracket);
					#line 479 "grammar.ecs"
					if (e.TargetType != BaseType.IntType) {
						Src.Error(0, "Error offset needs to be integer type");
					}
					#line 482 "grammar.ecs"
					result.Add(new IndexSelector(e));
					#line default
				} else
					break;
			}
			return result;
		}
		void ProcedureCall(string name, Block block)
		{
			TokenType la0;
			List<Expression> pl1 = default(List<Expression>);
			FunctionDeclaration p = block.LookupFunction(name);
			if (p == null) {
				Src.Error(0, "Procedure {0} not found", name);
			}
			List<Expression> pl = new List<Expression>();
			// Line 493: (TT.LParen ParameterList TT.RParen)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.LParen) {
				Src.Skip();
				pl1 = ParameterList(block);
				Src.Match((int) TT.RParen);
				#line 493 "grammar.ecs"
				pl.AddRange(pl1);
				#line default
			}
			#line 495 "grammar.ecs"
			if (pl.Count != p.Parameters.Count) {
				Src.Error(0, "Wrong number of parameters to call {0}. Expected {1}, found {2}", name, p.Parameters.Count, pl.Count);
			}
			#line 498 "grammar.ecs"
			block.Statements.Add(new ProcedureCallStatement(p, block, pl));
			#line default
		}
		void AssignmentOrCall(Block block)
		{
			TokenType la0;
			var v = Src.MatchAny();
			var s = Selector(block);
			// Line 505: (TT.Assign AssignmentStmt | ProcedureCall)
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Assign) {
				Src.Skip();
				AssignmentStmt(v.Value.ToString(), s, block);
			} else {
				ProcedureCall(v.Value.ToString(), block);
				#line 507 "grammar.ecs"
				if (s.Count > 0) {
					Src.Error(0, "Procedure calls do not expect a selector");
				}
				#line default
			}
		}
		void IfStmt(Block block)
		{
			TokenType la0;
			#line 513 "grammar.ecs"
			IfStatement ifs = new IfStatement(block);
			#line default
			var c = RelationalExpression(block);
			Src.Match((int) TT.Then);
			#line 515 "grammar.ecs"
			Block thenBlock = new Block();
			#line 515 "grammar.ecs"
			thenBlock.Parent = block;
			#line default
			StatementSequence(thenBlock);
			#line 516 "grammar.ecs"
			ifs.Conditions.Add(c);
			#line 516 "grammar.ecs"
			ifs.ThenParts.Add(thenBlock);
			#line default
			// Line 517: (TT.IfElse RelationalExpression TT.Then StatementSequence)*
			for (;;) {
				la0 = (TokenType) Src.LA0;
				if (la0 == TT.IfElse) {
					Src.Skip();
					var c1 = RelationalExpression(block);
					Src.Match((int) TT.Then);
					#line 518 "grammar.ecs"
					thenBlock = new Block();
					#line 518 "grammar.ecs"
					thenBlock.Parent = block;
					#line default
					StatementSequence(thenBlock);
					#line 520 "grammar.ecs"
					ifs.Conditions.Add(c1);
					#line 520 "grammar.ecs"
					ifs.ThenParts.Add(thenBlock);
					#line default
				} else
					break;
			}
			// Line 522: (TT.Else StatementSequence)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Else) {
				Src.Skip();
				#line 523 "grammar.ecs"
				Block elseBlock = new Block();
				#line 523 "grammar.ecs"
				elseBlock.Parent = block;
				#line default
				StatementSequence(elseBlock);
				#line 525 "grammar.ecs"
				ifs.ElsePart = elseBlock;
				#line default
			}
			Src.Match((int) TT.End);
			#line 529 "grammar.ecs"
			block.Statements.Add(ifs);
			#line default
		}
		void WhileStmt(Block block)
		{
			#line 533 "grammar.ecs"
			WhileStatement w = new WhileStatement(block);
			#line default
			var e = RelationalExpression(block);
			Src.Match((int) TT.Do);
			#line 534 "grammar.ecs"
			w.Condition = e;
			#line default
			StatementSequence(w.Block);
			Src.Match((int) TT.End);
		}
		void RepeatStmt(Block block)
		{
			#line 540 "grammar.ecs"
			RepeatStatement r = new RepeatStatement(block);
			#line default
			StatementSequence(r.Block);
			Src.Match((int) TT.Until);
			var e = RelationalExpression(block);
			#line 542 "grammar.ecs"
			r.Condition = e;
			#line default
			Src.Match((int) TT.End);
		}
		void SingleStatement(Block block)
		{
			TokenType la0;
			// Line 547: ( AssignmentOrCall | TT.If IfStmt | TT.While WhileStmt | TT.Repeat RepeatStmt |  )
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Id)
				AssignmentOrCall(block);
			else if (la0 == TT.If) {
				Src.Skip();
				IfStmt(block);
			} else if (la0 == TT.While) {
				Src.Skip();
				WhileStmt(block);
			} else if (la0 == TT.Repeat) {
				Src.Skip();
				RepeatStmt(block);
			} else {
				#line 551 "grammar.ecs"
				Src.Error(0, "Statement expected");
				#line default
			}
		}
		Module Module()
		{
			TokenType la0;
			Module result = default(Module);
			#line 556 "grammar.ecs"
			result = module = new Module();
			#line default
			Src.Match((int) TT.Module);
			var m = Src.Match((int) TT.Id);
			Src.Match((int) TT.Semicolon);
			Declarations(result.Block);
			// Line 558: (TT.Begin StatementSequence)?
			la0 = (TokenType) Src.LA0;
			if (la0 == TT.Begin) {
				Src.Skip();
				StatementSequence(module.Block);
			}
			Src.Match((int) TT.End);
			var e = Src.Match((int) TT.Id);
			#line 562 "grammar.ecs"
			string endName = e.Value.ToString();
			result.Name = m.Value.ToString();
			if (endName != result.Name) {
				Src.Error(0, "The module name {0} does not match {1}", result.Name, endName);
			}
			#line default
			Src.Match((int) TT.Dot);
			return result;
		}
	}
}
