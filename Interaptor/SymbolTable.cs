using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interaptor {
    class SymbolTable {

        public SymbolTable Father { get; set; }

        private Dictionary<string, object> _symbols;

        public SymbolTable(SymbolTable father) {
            this.Father = father;
            _symbols = new Dictionary<string, object>();
        }

        public void AddVariable(string name, object value) {
           
                _symbols.Add(name, value);
            
        }
        public void AddFunction(string name, IExecutable exe, List<string> parameters) {
            _symbols.Add(name, new FunctionBLock(this, parameters, exe));
        }
        public void AddFunction(string name,  IExecutable exe, params string[] parameters) {
            List<string> pr;
            
            pr = new List<string>();
            foreach (string i in parameters)
                pr.Add(i);

            _symbols.Add(name, new FunctionBLock(this, pr, exe));
        }
        
     
        //calls a function with a spechific id
        
        public object CallFunction(Id id, List<object> variables) {
            FunctionBLock blc = (FunctionBLock)this.GetVariable(id);
            return blc.Operation.ExecuteWithTable(blc.GetCallSymbolTable(variables));
        }
        public int GetFunctionParametersCount(Id id) {
            FunctionBLock blc = (FunctionBLock)this.GetVariable(id);
            return blc.ParametersCount;
        }
        
        public object GetVariable(Id id) {
            try {
                //ncase the path of the id is llarger then 1 you need to search inside the kid symbo table
                if (id.Length > 1) {
                    SymbolTable scope = (SymbolTable) this.GetVariable(new Id(id.Head));
                    id.Path.RemoveFirst();
                    return scope.GetVariable(id);
                }
                return _symbols[id.Head];
            }
            catch {
                if (Father == null)
                    throw new Exception("Variable \""+id+"\" is undefined");
                return this.Father.GetVariable(id);
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
                 if (Father == null)
                     throw new Exception("Variable \"" + id + "\" is undefined");
                 return this.Father.SetVariable(id, value);

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
        class FunctionBLock {
            SymbolTable Father { get; set; }
            
            List<string> parameters;
            public int ParametersCount { get { return this.parameters.Count; } }

            public IExecutable Operation { get; set; }


            public FunctionBLock(SymbolTable father, List<string> parameters, IExecutable exe) {
                this.parameters = parameters;
                this.Operation = exe;
                this.Father = father;
            }

            public SymbolTable GetCallSymbolTable(List<object> variables) {
                SymbolTable fuTable = new SymbolTable(this.Father);
                
                if (variables.Count != this.parameters.Count) {
                    throw new Exception("Interpretation error: parameters length dont match");
                }
                for (int i = 0; i < variables.Count; i++) {
                    fuTable.AddVariable(this.parameters[i], variables[i]);
                }
                return fuTable;
            }
        }
    }

}
