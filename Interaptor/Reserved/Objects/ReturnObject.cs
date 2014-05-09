using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter.Reserved {
    public class ReturnObject {
        public ReturnObject(object toReturn) {
            this.toReturn = toReturn;
        }
        protected ReturnObject() {
        }
        public object toReturn;
    }
    public class Void:ReturnObject{
        public Void() { 
        //this is a void object.
        }
    }
}
