namespace Analytics.Parameters
{
    public abstract class EventParameter<T> : AbstractEventParameter
    {
        public readonly string Name;
        public readonly T Value;

        protected EventParameter(string name, T value)
        {
            Name = name;
            Value = value;
        }
    }
}