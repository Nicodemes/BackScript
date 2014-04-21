using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaptor {
    class Program {
        static Interaptor raptor;
       
        static void Main(string[] args) {

            
           
            raptor = new Interaptor();

            List<string> DefParams = new List<string>();
            DefParams.Add("value");
            DefParams.Add("id");
            raptor.ActiveScope.AddFunction("def", DefParams, new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Def_Fu)));
            
            List<string> AddParams = new List<string>();
            AddParams.Add("a");
            AddParams.Add("b");
            raptor.ActiveScope.AddFunction("add", AddParams, new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Add_Fu)));

            raptor.ActiveScope.AddFunction("clear", new List<string>(), new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Clear_Fu)));
            raptor.ActiveScope.AddFunction("pop", new List<string>(), new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Pop_Fu)));

            while (true) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("backScript");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("$ ");
                Console.ForegroundColor = ConsoleColor.White;
                string row = Console.ReadLine();

                LinkedList<Token> tokens = null;
                try {
                    try {
                        tokens = Tokenizer.Tokenize(row, 0, row.Length);
                    }
                    catch (Exception e) {
                        throw new Exception("Syntax Error: " + e.Message);
                    }/*
                foreach (Token item in tokens) {
                    Console.WriteLine("[" + item.type + "] " + item.lexema);
                }*/
                    try {
                        raptor.Process(tokens);
                    }
                    catch (Exception e) {
                        throw new Exception("Interpritation Error: " + e.Message);

                    }
                }
                catch (Exception e) {
                    Console.WriteLine("  "+e.Message);
                    raptor.Reset();
                }

            }

        }
        //defines a new vireavle
        static object Def_Fu(SymbolTable s) {
            if (!(s.GetVariable(new Id("id")) is Id)) 
                throw new Exception("invalid ID");
            string name = (s.GetVariable(new Id("id")) as Id).ToString();
            object value = s.GetVariable(new Id("value"));
            if(value is Id)
                value = s.GetVariable(value as Id);
            s.Father.AddVariable(name , value);
            Console.WriteLine("variable \"" + name + "\" has been assigend with the value \"" +value.ToString() +"\"");
            return s.GetVariable(new Id("id"));
        }
        static object Add_Fu(SymbolTable s) {

            object a = s.GetVariable(new Id("a"));
            object b = s.GetVariable(new Id("b"));
            
            if (a is Id) {
                a = s.GetVariable(a as Id);
            }
            if (b is Id) {
                b = s.GetVariable(b as Id);
            }
            
            try {
                return (double)a + (double)b;
            }
            catch {
                throw new Exception("you cannot add " + a.GetType() + " and " + b.GetType());
            }
        }
        static object Clear_Fu(SymbolTable s) {
            Console.Clear();
            return new Void();
        }
        static object Pop_Fu(SymbolTable s) {
            raptor.ProcessStack.Pop();
            return new Void();
        }
    }
}
