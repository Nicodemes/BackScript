//#define _DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Interpreter {
    class SymbolTable :ITreeNode {

        public SymbolTable Perent { get; set; }

        private Dictionary<string, object> _symbols;

        public SymbolTable(SymbolTable father) {
            this.Perent = father;
            _symbols = new Dictionary<string, object>();
        }

        public void AddVariable(string name, object value) {
#if _DEBUG
            Console.WriteLine("  "+this+ " is adding variable " + name+" with the value "+value);
#endif
                _symbols.Add(name, value);
            
        }
        public void AddFunction(string name, IExecutable exe, List<string> parameters) {
            this.AddVariable(name, new FunctionBLock( parameters, exe));
        }
        public void AddFunction(string name,  IExecutable exe, params string[] parameters) {
            List<string> pr;
            
            pr = new List<string>();
            foreach (string i in parameters)
                pr.Add(i);

            _symbols.Add(name, new FunctionBLock(pr, exe));
        }
        
     
        //calls a function with a spechific id
        
        public object CallFunction(Id id, List<object> variables) {

#if _DEBUG
            Console.WriteLine("  "+this+" calling Function " + id);
#endif 
            FunctionBLock blc = (FunctionBLock)this.GetValue(id);
            return blc.Operation.ExecuteWithTable(blc.GetCallSymbolTable(this, variables));
        }
        public int GetFunctionParametersCount(Id id) {
            FunctionBLock blc = (FunctionBLock)this.GetValue(id);
            return blc.ParametersCount;
        }
        
        public object GetVariable(Id id) {

#if _DEBUG
            Console.Write("  "+this + " getting " + id+ "...");
#endif
            try {
                //ncase the path of the id is llarger then 1 you need to search inside the kid symbo table
                if (id.Length > 1) {
                    SymbolTable scope = (SymbolTable) this.GetVariable(new Id(id.Head));
                    id.Path.RemoveFirst();
                    return scope.GetVariable(id);
                }
#if _DEBUG                
                object toReturn = _symbols[id.Head];
                Console.WriteLine("Sucesess");
                return toReturn;
#else
                return _symbols[id.Head];
#endif
            }
            catch {
#if _DEBUG
                Console.WriteLine("Fail. Searching in perent ↓"+id+"...");
#endif
                if (Perent == null)
                    throw new Exception("Variable \""+id+"\" is undefined");
                return this.Perent.GetVariable(id);
            }
        }
        public object GetValue(Id id) {
            object toGet = GetVariable(id);
            while (toGet is Id)
                toGet = GetVariable(toGet as Id);
            return toGet;
        }
        
        public object SetVariable(Id id, object value) {
             try {
                 if (id.Length > 1) {
                     SymbolTable scope = (SymbolTable)this.GetVariable(new Id(id.Head));
                     id.Path.RemoveFirst();
                     return scope.SetVariable(id, value);
                 }
                 
                _symbols[id.Head] = value;
                return value;
                 
            }
            catch {
                 if (Perent == null)
                     throw new Exception("Variable \"" + id + "\" is undefined");
                 return this.Perent.SetVariable(id, value);

            }
        }
        
            
        public SymbolTable AddNewScope(string name) {
            _symbols.Add(name, new SymbolTable(this));
            return (SymbolTable)_symbols[name];
        }
        /*anonymic scope is a scope without id
           {
         *  {
         *    <-- anonyimic scope inside an anonyimic scope. 
         *  }
         * }
         */
        public SymbolTable AddNewAnonymicScope() {
            return new SymbolTable(this);
        }
        public override string ToString() {
            return "[" + this.GetHashCode() + "]";
        }
        class FunctionBLock {
            
            List<string> parameters;
            public int ParametersCount { get { return this.parameters.Count; } }

            public IExecutable Operation { get; set; }


            public FunctionBLock(List<string> parameters, IExecutable exe) {
                this.parameters = parameters;
                this.Operation = exe;
            }

            public SymbolTable GetCallSymbolTable(SymbolTable s , List<object> variables) {
                SymbolTable fuTable = new SymbolTable(s);
#if _DEBUG
                Console.WriteLine("Building call table "+fuTable+"...");
#endif
                
                
                if (variables.Count != this.parameters.Count) {
                    throw new Exception("Interpretation error: parameters length dont match");
                }
                for (int i = 0; i < variables.Count; i++) {
                    fuTable.AddVariable(this.parameters[i], variables[i]);
                }
#if _DEBUG
                Console.WriteLine("Done building " + fuTable);
#endif
                return fuTable;
            }
           
        }
    }

}
