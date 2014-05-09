using System;
using System.Collections.Generic;
namespace Interpreter.Reserved {
    partial class Functions {
        static object Eval_Fu(Interpreter m) {

            Interpreter raptor = new Interpreter(new Stack<object>(), s);
            
            string row = s.GetValue(new Id("~string")) as string;
            Tokenizer nizer;
            LinkedList<object> tokens = null;
            try {
                try {
                    nizer = new Tokenizer(row);
                    tokens = nizer.Tokenize();
                }
                catch (Exception e) {
                    throw new Exception("Syntax Error: " + e.Message);
                }
                try {
                    raptor.Process(tokens);
                }
                catch (Exception e) {
                    throw new Exception("Interpritation Error: " + e.Message);

                }
            }
            catch (Exception e) {
                Console.WriteLine("  " + e.Message);
                raptor.Reset();
            }
            raptor.ScopeOut();
            return new Void();
        }
        
    }
}