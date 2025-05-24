using SystemObject = System.Object;

namespace DotEngine.UI
{
    public enum UIWindowMode
    {

    }

    public enum UIWindowState
    {
        None = 0,
    }

    public class UIWindowAgent
    {
        public string identity { get; private set; }
        public string assetPath { get; private set; }
        public string hierarchy { get; private set; }
        public string stage { get; private set; }
        public UIWindowMode mode { get; private set; }
        public SystemObject userdata { get; private set; }

        public UIWindowState state { get; private set; }
    }
}
