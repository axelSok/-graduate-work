using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNFTester.Entities
{
    public class BooleanValue
    {
        private bool _value;

        public bool Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                    _value = value;
            }
        }

        public BooleanValue Disjunction(BooleanValue conValue)
        {
            if(_value || conValue.Value)
                return new BooleanValue(true);
            return new BooleanValue(false);
        }

        public BooleanValue Conjunction(BooleanValue conValue)
        {
            if (_value && conValue.Value)
                return new BooleanValue(true);
            return new BooleanValue(false);
        }

        public BooleanValue(bool value)
        {
            Value = value;
        }
    }
}
