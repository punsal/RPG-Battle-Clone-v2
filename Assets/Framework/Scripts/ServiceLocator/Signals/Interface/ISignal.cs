namespace Framework.ServiceLocator.Signals.Interface
{
    public interface ISignal<in T>
    {
        public void AddListener(T action);
        public void RemoveListener(T action);
    }
}