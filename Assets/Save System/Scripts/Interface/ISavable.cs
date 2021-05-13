namespace Save_System.Scripts.Interface
{
    public interface ISavable
    {
        void Save();
        bool Load(out object result);
    }
}