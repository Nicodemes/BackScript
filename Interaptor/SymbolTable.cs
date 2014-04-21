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
        public void AddFunction(string name, List<string> parameters, IExecutable exe) {
            _symbols.Add(name, new FunctionBLock(this, parameters, exe));
        }
        //calls a function with a spechific name
        
        public object CallFunction(string name, List<object> variables) {
            FunctionBLock blc;
            try {
                 blc= (FunctionBLock)_symbols[name];
            }
            catch {
                throw new Exception("intrapretation error: no function wit the name" + name + " found in the sybol table");
            }
            return blc.Operation.ExecuteWithTable(blc.GetCallSymbolTable(variables));
        }
        public object GetVariable(string name) {
            try {
                return _symbols[name];
            }
            catch {
                if (Father == null)
                    throw new Exception("Variable is undefined");
                return this.Father.GetVariable(name);
            }
        }
        
            
        public SymbolTable CreateNewScope(string name) {
            _symbols.Add(name, new SymbolTable(this));
            return (SymbolTable)_symbols[name];
        }

        class FunctionBLock {
            SymbolTable Father { get; set; }
            
            List<string> parameters;
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
