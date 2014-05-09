using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Reserved.Objects {
    class Arguments {
        public List<string> args;
        public Arguments(List<string> args) {
            this.args = args;
        }
        public override string ToString() {
            string toSay = "";
            foreach (var item in this.args)
                toSay += item+" ,";
            return toSay;
        }
    }
}
