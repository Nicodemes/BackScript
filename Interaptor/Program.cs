//#define _DEBUG
using System;
using System.Collections.Generic;
using System.IO;

namespace Interpreter {
    class Program {
        public static Interpreter interpreter;
        static Tokenizer lexicalAnaliser;
        
        static void Main(string[] args) {
            
            interpreter = new Interpreter();
            Registry rg = new Registry(interpreter.ActiveScope);
            if (args.Length != 0) 
                ExcecuteFile(args[0]);
            else
                StartInteactiveMode();
        }
        //print "backScript$" on the screen
        static void PrintBackScript() {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("backScript");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("$ ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        static void StartInteactiveMode() {
            //prit desclimer
            Console.WriteLine("BackScript Interapter  Copyright (C) 2014  Nicodemes sasha@paticon.com \nThis program comes with ABSOLUTELY NO WARRANTY; for details type `show w'.\n This is free software, and you are welcome to redistribute it\nunder certain conditions; type `show c' for details.");
            string input;
            while (true) {
                
                PrintBackScript();
                input= Console.ReadLine();
                LinkedList<object> tokens = null;
                try {
                    
                    try {
                        lexicalAnaliser = new Tokenizer(input);
                        tokens = lexicalAnaliser.Tokenize();
                    }
                    catch (Exception e) {throw new Exception("Syntax Error: " + e.Message);}

                    try {interpreter.Process(tokens);}
                    catch (Exception e) {throw new Exception("Interpritation Error: " + e.Message);}
                }
                catch (Exception e) {
                    Console.WriteLine("  " + e.Message);
                    interpreter.Reset();
                }
            }
        }
        static void ExcecuteFile(string file) {
            System.IO.StreamReader myFile = new System.IO.StreamReader(args[0]);
            string myString = myFile.ReadToEnd();
            myFile.Close();
            string row = myString;
            LinkedList<object> tokens = null;
            try {
                try {
                    lexicalAnaliser = new Tokenizer(row);
                    tokens = lexicalAnaliser.Tokenize();
                }
                catch (Exception e) {
                    throw new Exception("Syntax Error: " + e.Message);
                }/*
                    foreach (Token item in tokens) {
                    Console.WriteLine("[" + item.type + "] " + item.lexema);
                    }*/
                try {
                    interpreter.Process(tokens);
                }
                catch (Exception e) {
                    throw new Exception("Interpritation Error: " + e.Message);

                }
            }
            catch (Exception e) {
                Console.WriteLine("  " + e.Message);
                interpreter.Reset();
            }
        }
    }
}
