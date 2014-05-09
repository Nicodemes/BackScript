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
            new Style(ConsoleColor.Green,ConsoleColor.Black),//operators
            new Style(ConsoleColor.DarkRed,ConsoleColor.Black)//string
        };


        public static string ColoredReadLine() {
            string input = "";
            ConsoleKeyInfo ch = new ConsoleKeyInfo();
            int count = 0;

            while (ch.Key != ConsoleKey.Enter) {
                
                ch = Console.ReadKey();
                if (ch.Key != ConsoleKey.Enter) {
                    input += ch.KeyChar;
                    try {
                        DrawTokens(Program.Tt(input), count);
                    }
                    catch {
                        //do nothing,
                    }
                }
                
            }
            return input;
        }
        static void DrawTokens(LinkedList<object> t, int count){
            for (int i = 0; i < count; i++)
                Console.Write("\b");
            foreach (LinkedListNode<object> token in t) {
                if (token.Value is Token) {
                    switch((token.Value as Token).type){
                        case Token.Type.Operator:
                            WriteWithStyle( (token.Value as Token).lexema,styles[1]);break;
                        case Token.Type.String:
                            WriteWithStyle("\""+ (token.Value as Token).lexema+"\"",styles[2]); break;
                        default :
                            WriteWithStyle((token.Value as Token).lexema,styles[0]); break;
                    }
                }
                else { 
                //TODO:this
                }
            }
        }
    }
}
