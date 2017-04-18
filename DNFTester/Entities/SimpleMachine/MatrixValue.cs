using System.Linq;

namespace DNFTester.Entities
{
    public class MatrixValue : NotifyObjectChanged
    {
        private static readonly string[] _permittedVariables = { "0", "1", "-" };

        public void Next()
        {
            switch (_value)
            {
                case "0":
                    Value = "1";
                    break;
                case "1":
                    Value = "-";
                    break;
                case "-":
                    Value = "0";
                    break;
            }
        }

        public void Prev()
        {
            switch (_value)
            {
                case "-":
                    Value = "1";
                    break;
                case "1":
                    Value = "0";
                    break;
                case "0":
                    Value = "-";
                    break;
            }
        }

        public string GetInvers()
        {
            switch (_value)
            {
                case "1":
                    return "0";
                case "0":
                    return "1";
            }
            return "-";
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }

            }
        }

        public bool Equals(MatrixValue matrixValue)
        {
            return Value == matrixValue.Value;
        }

        public bool Compare(MatrixValue value)
        {
            return this.Value == "-" || this.Value == "1" || value.Value == "0";
        }

        public MatrixValue(string ch)
        {
            Value = _permittedVariables.Contains(ch) ? ch : "-";
        }

        public MatrixValue() : this("-") { }

        public override string ToString()
        {
            return Value;
        }
    }
}
