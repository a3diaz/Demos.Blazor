using System.Runtime.CompilerServices;

namespace Demos.Extensions.AsyncHolder
{
    public class AsyncHolderService : IAsyncHolderService
    {
        private readonly List<AsyncOperation> _operations;
        public event OperationsChangedEventHandler? OnChange;

        public AsyncHolderService()
        {
            _operations = new();
        }

        public AsyncOperation StartOperation([CallerMemberName] string name = "")
        {
            var operationId = Guid.NewGuid();
            var operation = new AsyncOperation(operationId, name, FinishOperation);
            _operations.Add(operation);
            OnChange?.Invoke();
            return operation;
        }

        public bool IsRunning(string? filter)
        {
            var names = filter?.Split(',', StringSplitOptions.RemoveEmptyEntries) 
                ?? Array.Empty<string>();

            return IsRunning(names);
        }

        public bool IsRunning(params string[] names)
        {
            if (!_operations.Any())
                return false;

            if (names.Any())
                return _operations.Select(o => o.Name)
                    .Intersect(names)
                    .Any();

            return true;
        }

        private void FinishOperation(Guid operationId)
        {
            var operation = _operations.First(o => o.Id == operationId);
            _operations.Remove(operation);
            OnChange?.Invoke();
        }


    }
}