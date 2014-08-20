using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.InteractiveColorMode {
    static class ColorMode {
        static void WriteWithStyle(string str , Style style) {
            style.WriteString(str);
        }
        static Style[] styles = new Style[]{
            new Style(ConsoleColor.White,ConsoleColor.Black),//default
            new Style(ConsoleColor.Cyan,ConsoleColor.Black),//operators
            new Style(ConsoleColor.Green,ConsoleColor.Black)//string
        };


        public static string ColoredReadLine() {

            LinkedList<char> input = new LinkedList<char>();

            ConsoleKeyInfo ch = new ConsoleKeyInfo();
            
            int x = Console.CursorLeft,
                y = Console.CursorTop;

            while (ch.Key != ConsoleKey.Enter) {
                ch = Console.ReadKey();
                
                if (ch.Key != ConsoleKey.Enter) {
                    if (ch.Key == ConsoleKey.Backspace) {
                        if (input.Count != 0) {
                            input.RemoveLast();
                        }
                    }
                    else 
                        input.AddLast(ch.KeyChar);

                    DrawTokens(Program.Tt(toString(input)), x, y);
                }
                
                
            }
            Console.Write("\n");
            return toString(input);
        }
        static string toString(LinkedList<char> l) {
            string toReturn = "";
            foreach (char item in l) {
                toReturn += item;
            }
            return toReturn;
        }
        static void DrawTokens(LinkedList<object> t, int x, int y){
            
            int a = Console.CursorLeft,
                b = Console.CursorTop;

            Console.SetCursorPosition(x, y);
            for (int i = 0; i < CalcLength(x, y, a, b); i++)
                Console.Write(" ");
            Console.SetCursorPosition(x, y);

            foreach (object token in t) {
                if (token is Token) {
                    //Console.Write(" ");
                    switch((token as Token).type){
                        case Token.Type.IdSingle:
                            WriteWithStyle((token as Token).lexema, styles[0]); break;
                        case Token.Type.Operator:
                            WriteWithStyle( (token as Token).lexema,styles[1]);break;
                        case Token.Type.String:
                            WriteWithStyle("\""+ (token as Token).lexema+"\"",styles[2]); break;
                        default :
                            WriteWithStyle((token as Token).lexema,styles[0]); break;
                    }
                    Console.Write(" ");
                }
                else { 
                //TODO:this
                }
            }
        }
        static int CalcLength(int x, int y, int a, int b) {
            int LineWidth = Console.WindowWidth;
            return ((b - y) * LineWidth)+(a-x);
        }
    }
}
