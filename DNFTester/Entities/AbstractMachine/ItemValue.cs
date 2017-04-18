namespace DNFTester.Entities.AbstractMachine
{
    public class ItemValue : NotifyObjectChanged
    {
        private int _maxOutput;

        private int _maxState;

        private int? _output;
        private int? _state;

        public ItemValue(int state, int output, int maxState, int maxOutput)
        {
            _maxState = maxState;
            _maxOutput = maxOutput;
            Output = output >= 0 ? output : 0;
            State = state >= 0 ? state : 0;
        }

        public int MaxState
        {
            get { return _maxState; }
            set
            {
                if (_maxState != value && value >= 0)
                {
                    _maxState = value;
                    RaisePropertyChanged("MaxState");
                }
            }
        }

        public int MaxOutput
        {
            get { return _maxOutput; }
            set
            {
                if (_maxOutput != value && value >= 0)
                {
                    _maxOutput = value;
                    RaisePropertyChanged("MaxOutput");
                }
            }
        }

        public int? State
        {
            get { return _state; }
            set
            {
                if (_state != value && value >= 0 && value <= _maxState)
                {
                    _state = value;
                    RaisePropertyChanged("State");
                }
            }
        }

        public int? Output
        {
            get { return _output; }
            set
            {
                if (_output != value && value >= 0 && value <= _maxOutput)
                {
                    _output = value;
                    RaisePropertyChanged("Output");
                }
            }
        }

        public override string ToString()
        {
            return $"({State}/{Output})";
        }
    }
}