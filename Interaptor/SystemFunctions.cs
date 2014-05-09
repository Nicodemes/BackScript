using System;

namespace Interpreter {
    class SystemFunction : IExecutable {
        //the delgate of all of the reserved functions.
        
        RDelegate toDo;
        public SystemFunction(RDelegate toDo) {
            this.toDo = toDo;
        }
        public void ExecuteByhInterpreter(Interpreter machine) {
            //Console.WriteLine("Executing with table " + tble.GetHashCode());
            machine.ProcessStack.Push(toDo(machine));
        }
    }
    public delegate object RDelegate(Interpreter machine);
}
