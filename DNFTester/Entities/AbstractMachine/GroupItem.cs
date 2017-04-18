namespace DNFTester.Entities.AbstractMachine
{
    public class GroupItem
    {
        public GroupItem(int? first, int? second)
        {
            if (first > second)
            {
                var temp = first;
                first = second;
                second = temp;
            }
            First = first ?? 0;
            Second = second ?? 0;
        }

        public int First { get; set; }

        public int Second { get; set; }

        public override string ToString()
        {
            return $"{First}-{Second}";
        }
    }
}