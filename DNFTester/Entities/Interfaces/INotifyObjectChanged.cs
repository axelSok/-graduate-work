using System;

namespace DNFTester.Entities.Interfaces
{
    public interface INotifyObjectChanged
    {
        void BeginChange();

        void EndChange(bool raiseEvent, bool endAllChanges);

        event EventHandler<ObjectChangedEventArgs> ObjectChanged;
    }
}
