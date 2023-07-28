namespace Analytics.Parameters
{
    public class IntegerEventParameter : EventParameter<int>
    {
        private IntegerEventParameter(string name, int value) : base(name, value)
        {
        }
    }
}