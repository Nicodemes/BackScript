//#define _DEBUG
using System;
using System.Collections.Generic;
using System.IO;

namespace Interpreter {
    class Program {
        public static Environment environment;
        
        //flags
        static bool exefile = false;
        static bool readline = false;
        static bool interactivemode = false;
        static bool debugmode = false;
        static bool icolorMode = false;//colored interactive mode

        static void Main(string[] args) {
            environment = new Environment();

            if (args.Length == 0){
                interactivemode = true;
                StartInteactiveMode();
                return;
            }
            //process arguments
            for (int i = 0; i < args.Length; i++)
                Arguments(args[i]);
            if(exefile){ 
                ExcecuteFile(args[0]);
                if (readline)
                Console.ReadLine();
            }   
        }
        
        //proccess all of the arguments that the program can take.
        static void Arguments(string arg) {
            switch (arg) {
                //show help
                case "-h":
                    Help();
                    break;
                //show help
                case "--help":
                    Help();
                    break;
                //start interactive mode.
                case "-i":
                    StartInteactiveMode();
                    break;
                case "--interactive":
                    StartInteactiveMode();
                    break;
                //set the ineractive color mode on
                case "-c":
                    icolorMode = true;
                    break;
                //set the ineractive color mode on
                case "--color":
                    icolorMode = true;
                    break;

                //don't close window after execution.
                case "-r": readline = true;
                    break;
                
                //start debugmode.
                case "-d": debugmode = true;
                    break;
                default:
                    exefile = true;
                    break;
            }
        }
        //print "backScript$" on the screen
        static void PrintBackScript() {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("backScript");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("$ ");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //start the ineractive mode in which the user inputs his commands.
        static void StartInteactiveMode() {
            LincesInfo();
            string input;
            while (true) {
                PrintBackScript();
                input= Console.ReadLine();
                if (input == "help") {
                    Help();
                    continue;
                }
                Execute(input);
            }
        }
        //excecute a file .
        static void ExcecuteFile(string file) {
            System.IO.StreamReader myFile = new System.IO.StreamReader(file);
            string myString = myFile.ReadToEnd();
            myFile.Close();
            Execute(myString);
        }
        
        static void Execute(string data) {
            LinkedList<object> tokens = null;
            try {
                try {
                    environment.lexicalAnaliser = new Tokenizer(data);
                    tokens = environment.lexicalAnaliser.Tokenize();
                }
                catch (Exception e) {
                    throw new Exception("Syntax Error: " + e.Message);
                }
                try {
                    environment.interpreter.Process(tokens);
                }
                catch (Exception e) {
                    throw new Exception("Interpritation Error: " + e.Message);

                }
            }
            catch (Exception e) {
                Console.WriteLine("  " + e.Message);
                environment.interpreter.Reset();
            }
        }
        //prints the help message
        static void Help() {
            LincesInfo();
        }
        //prints the linecs information.
        static void LincesInfo() {
            //prit desclimer
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(
                "BackScript Interapter  Copyright (C) 2014  Nicodemes sasha@paticon.com \n" +
                "This program comes with "
            );
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("ABSOLUTELY NO WARRANTY");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(
                "; for details type `help'.\n" +
                "This is free software, and you are welcome to redistribute it\n" +
                "under certain conditions; type `show c' for details."
            );
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
