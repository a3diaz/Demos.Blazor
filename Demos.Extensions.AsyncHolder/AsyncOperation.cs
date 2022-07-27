namespace Demos.Extensions
{
    public class AsyncOperation : IDisposable
    {
        internal Guid Id { get; set; }
        internal string Name { get; set; }
        private Action<Guid> DisposingCallback { get; }
        internal AsyncOperation(Guid id, string name, Action<Guid> disposingCallback)
        {
            Id = id;
            Name = name;
            DisposingCallback = disposingCallback;
        }

        public void Dispose()
        {
            DisposingCallback(Id);
        }
    }
}
