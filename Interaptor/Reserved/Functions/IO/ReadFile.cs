using System;
namespace Interpreter.Reserved {
    partial class Functions {
        public static object ReadFile_Fu(SymbolTable s) {
            System.IO.StreamReader myFile =
            new System.IO.StreamReader(s.GetValue(new Id("~path")) as string);
            string myString = myFile.ReadToEnd();
            myFile.Close();
            return myString;
        }
    }
}