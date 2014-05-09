using System;
namespace Interpreter.Reserved{
    partial class Functions {
        //assigens value to variable
        public static object Be_Fu(SymbolTable s) {
            if (!(s.GetVariable(new Id("~id")) is Id))
                throw new Exception("invalid ID");
            Id name = (s.GetVariable(new Id("~id")) as Id);
            object value = s.GetValue(new Id("~value"));
            
              
            //Console.WriteLine("variable \"" + name + "\" has been assigend with the value \"" + value.ToString() + "\"");
            return s.SetVariable(name, value);
        }
        
    }
}
