using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaptor {
    class Program {
        
        static void Main(string[] args) {
            
            string toParse = "1 2";

            Console.Write("\n");

            foreach (Token item in Tokenizer.Tokenize(toParse, 0, toParse.Length)) {
                Console.WriteLine("[" + item.type + "] " + item.lexema);
            }
            
            Console.ReadLine();
        }
        static object HelloWorld(SymbolTable s) {
            Console.WriteLine(s.GetVariable("WhatToSayFirst"));
            return "World!";
        }
    }
    
}
