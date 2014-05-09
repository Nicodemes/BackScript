using System;

namespace Interpreter.InteractiveColorMode {
    struct Style {
        public ConsoleColor Forground;
        public ConsoleColor Backgorund;
        public Style(ConsoleColor forground, ConsoleColor backgorund) {
            this.Forground = forground;
            this.Backgorund = backgorund;
        }
        public void WriteString(string s){
            ConsoleColor tmp1=Console.BackgroundColor;
            ConsoleColor tmp2=Console.ForegroundColor;
            Console.BackgroundColor =this.Backgorund;
            Console.ForegroundColor=this.Forground;
            Console.Write(s);
            Console.BackgroundColor =tmp1;
            Console.ForegroundColor=tmp2;
        }
    }
}
