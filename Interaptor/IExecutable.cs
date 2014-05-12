
namespace Interpreter {
    public interface IExecutable {
        void ExecuteByhInterpreter(Interpreter machine);
        void ExecuteWithArguments(object[] machine);
    }
}
