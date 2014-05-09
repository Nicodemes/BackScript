using System;
using System.Collections.Generic;
using System.Linq;
namespace Interpreter.Reserved {
    partial class Functions {
        //Decloration
        public static object Def_Fu(SymbolTable s) {
            if (!(s.GetVariable(new Id("~id")) is Id))
                throw new Exception("invalid ID");
            string name = (s.GetVariable(new Id("~id")) as Id).ToString();
            object value = s.GetValue(new Id("~value"));

            s.Perent.AddVariable(name, value);
            //Console.WriteLine("variable \"" + name + "\" has been assigend with the value \"" +value.ToString() +"\"");
            return s.GetVariable(new Id("~id"));
        }
        public static object DefMethod_Fu(SymbolTable s) {
            //body -opCodes
            //paramList -list 
            //id - id
            if (!(s.GetVariable(new Id("~id")) is Id))
                throw new Exception("invalid ID");
            string name = (s.GetVariable(new Id("~id")) as Id).ToString();

            //db
            Opcodes body = s.GetVariable(new Id("~body")) as Opcodes;
            List<object> raw = s.GetValue(new Id("~paramList")) as List<object>;
            List<string> param = raw.Select(k => (string)k).ToList();
#if _DEBUG
            string toSay = "Defining new Function\n  name: " + name+"\n  params: ";
            foreach (var item in param) 
                toSay += item;
            
            Console.WriteLine(toSay);
#endif
            s.Perent.AddFunction(name, body, param);
            return new Void();
        }
    }
}