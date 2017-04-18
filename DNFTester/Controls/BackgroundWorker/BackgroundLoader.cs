#region

using System.ComponentModel;

#endregion

namespace PariSpace.LineDesigner
{
    public class BackgroundLoader: BackgroundWorker
    {
        public bool? Loading { get; set; }

        public BackgroundLoader()
        {
            Loading = false;
        }

    }
}
