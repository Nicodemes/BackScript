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
            
            raptor.ActiveScope.AddFunction("~indaxer", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Indexer_Fu)), "index", "enums");
            raptor.ActiveScope.AddFunction("list", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(List_Fu)), "items");
            raptor.ActiveScope.AddFunction("Set", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Be_Fu)), "value", "id");
            raptor.ActiveScope.AddFunction("Def", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Def_Fu)), "value", "id");
            raptor.ActiveScope.AddFunction("Add", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Add_Fu)), "a", "b");
            raptor.ActiveScope.AddFunction("Clear", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Clear_Fu)));
            raptor.ActiveScope.AddFunction("Pop", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Pop_Fu)));
            raptor.ActiveScope.AddFunction("Print", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Print_Fu)), "value");
            raptor.ActiveScope.AddFunction("PrintL", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(PrintL_Fu)), "value");
            raptor.ActiveScope.AddFunction("ReadFile", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(ReadFile_Fu)), "path");
            raptor.ActiveScope.AddFunction("Eval", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(Eval_Fu)), "string");
            raptor.ActiveScope.AddFunction("ReadLine", new ReservedFunction(new ReservedFunction.ReservedFuncDelegate(ReadLine_Fu)));
            
            if (args.Length == 0) {
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
                            //foreach (Token item in tokens) 
                            //   Console.WriteLine("[" + item.type + "] " + item.lexema);

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

                }
            }
            else {
                System.IO.StreamReader myFile = new System.IO.StreamReader(args[0]);
                string myString = myFile.ReadToEnd();
                myFile.Close();
                string row = myString;
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
                    Console.WriteLine("  " + e.Message);
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
            Console.Write(s.GetValue(new Id("value")));
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
        static object List_Fu(SymbolTable s) {
            Opcodes p = s.GetValue(new Id("items")) as Opcodes;
            List<object> toReturn= new List<object>();
            foreach( object  a in p.ops){
                switch ((a as Token).type) {
                  
                    case Token.Type.String:
                        toReturn.Add((a as Token).lexema);
                        break;
                    case Token.Type.Double:
                        toReturn.Add(Double.Parse((a as Token).lexema));
                        break;
                    case Token.Type.Integer:
                        toReturn.Add(Double.Parse((a as Token).lexema));
                        break;
                }
            }
            return toReturn;            
        }

        static object Indexer_Fu(SymbolTable s) {
            int p = (int)s.GetValue(new Id("index"));
            IEnumerable<object> ls =( IEnumerable<object>) s.GetValue(new Id("enums"));
            return ls.ElementAt<object>(p);
        }
        static object ReadLine_Fu(SymbolTable s) {
            return Console.ReadLine();
        }
    }
}
