namespace Serialization
{
    public interface IDataSerializable<T>
    {
        public T GetData();
        public void SetData(T data);
    }
}