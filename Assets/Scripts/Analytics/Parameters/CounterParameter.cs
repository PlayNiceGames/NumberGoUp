using UnityEngine;

namespace Analytics.Parameters
{
    public class CounterParameter : EventParameter<int>
    {
        public CounterParameter(string name) : base(name, 0)
        {
            Value = GetAndRecordCounter();
        }

        private int GetAndRecordCounter()
        {
            int counter = PlayerPrefs.GetInt(Name, 0);

            PlayerPrefs.SetInt(Name, counter + 1);
            PlayerPrefs.Save();

            return counter;
        }
    }
}