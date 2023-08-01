namespace Analytics.Parameters
{
    public abstract class EventParameter<T> : AbstractEventParameter
    {
        public string Name { get; protected set; }
        public T Value { get; protected set; }

        protected EventParameter(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}