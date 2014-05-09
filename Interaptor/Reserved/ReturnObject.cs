using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpreter {
    class ReturnObject {
        public ReturnObject(object toReturn) {
            this.toReturn = toReturn;
        }
        protected ReturnObject() {
        }
        public object toReturn;
    }
    class Void:ReturnObject{
        public Void() { 
        //this is a void object.
        }
    }
}
