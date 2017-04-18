using System;

namespace DNFTester.Entities
{
    public class ObjectChangedEventArgs : EventArgs
    {
        public ObjectChangedEventArgs(string property) : this(new string[] { property }) { }

        public ObjectChangedEventArgs(string[] properties)
        {
            Properties = properties;
        }

        public string[] Properties { get; private set; }
    }
}
