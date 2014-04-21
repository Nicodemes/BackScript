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

            raptor.ActiveScope.AddFunction("Set", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Be_Fu)), "value", "id");
            raptor.ActiveScope.AddFunction("Def", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Def_Fu)), "value", "id");
            raptor.ActiveScope.AddFunction("Add",new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Add_Fu)),"a","b");
            raptor.ActiveScope.AddFunction("Clear",new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Clear_Fu)));
            raptor.ActiveScope.AddFunction("Pop",new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Pop_Fu)));
            raptor.ActiveScope.AddFunction("Print", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Print_Fu)),"value");
            raptor.ActiveScope.AddFunction("PrintL", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(PrintL_Fu)), "value");
            raptor.ActiveScope.AddFunction("ReadFile", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(ReadFile_Fu)), "path");
            raptor.ActiveScope.AddFunction("Eval", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Eval_Fu)), "string");
            
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
        
        //assigens value to variable
        static object Be_Fu(SymbolTable s) {
            if (!(s.GetVariable(new Id("id")) is Id))
                throw new Exception("invalid ID");
            Id name = (s.GetVariable(new Id("id")) as Id);
            object value = s.GetVariable(new Id("value"));
            
            while (value is Id)
                value = s.GetVariable(value as Id);
            //Console.WriteLine("variable \"" + name + "\" has been assigend with the value \"" + value.ToString() + "\"");
            return s.SetVariable(name, value);
        }
        //defines a new vireavle
        static object Def_Fu(SymbolTable s) {
            if (!(s.GetVariable(new Id("id")) is Id)) 
                throw new Exception("invalid ID");
            string name = (s.GetVariable(new Id("id")) as Id).ToString();
            object value = s.GetVariable(new Id("value"));
            
            while(value is Id)
                value = s.GetVariable(value as Id);
            s.Father.AddVariable(name , value);
            //Console.WriteLine("variable \"" + name + "\" has been assigend with the value \"" +value.ToString() +"\"");
            return s.GetVariable(new Id("id"));
        }
        //you can add 2 variables
        static object Add_Fu(SymbolTable s) {

            object a = s.GetValue(new Id("a"));
            object b = s.GetValue(new Id("b"));
            try {
                return (double)a + (double)b;
            }
            catch {
                throw new Exception("you cannot add " + a.GetType() + " and " + b.GetType());
            }
        }
        //clears the screan 
        static object Clear_Fu(SymbolTable s) {
            Console.Clear();
            return new Void();
        }
        //pops one item from the pStack
        static object Pop_Fu(SymbolTable s) {
            raptor.ProcessStack.Pop();
            return new Void();
        }
        //prints a variable to the screan
        static object Print_Fu(SymbolTable s) {
            Console.Write(s.GetVariable(new Id("value")));
            return new Void();
        }
        //Console.Writeine()
        static object PrintL_Fu(SymbolTable s) {
            Console.WriteLine(s.GetValue(new Id("value")));
            return new Void();
        }
        static object ReadFile_Fu(SymbolTable s) {
            System.IO.StreamReader myFile =
            new System.IO.StreamReader(s.GetValue(new Id("path"))as string);
            string myString = myFile.ReadToEnd();
            myFile.Close();
            return myString;
        }
        static object Eval_Fu(SymbolTable s) {
           
            raptor.EnterScope(new SymbolTable(raptor.ActiveScope));
            string row = s.GetValue(new Id("string")) as string;
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
            raptor.ScopeOut();
            return new Void();
        }
    }
}
