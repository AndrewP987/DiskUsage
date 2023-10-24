

// Namespace "Configuration" contains classes related to command-line configuration.
namespace Configuration
{
    // Class to hold command-line configuration and arguments
    public class CLI_Configuration
    {
        // Indicates if a mode has been selected
        private bool modeSelected = false;

        // Stores the selected mode ("-s", "-d", or "-b")
        private string modeStr = string.Empty;

        // Indicates if a path has been selected
        private bool pathSelected = false;

        // Stores the selected directory path
        private string pathStr = string.Empty;

        // Stores the index of the mode argument in the command line
        private int modeArgIndex = 0;

        // Property to get or set the mode selection status
        public bool ModeSelected
        {
            get { return modeSelected; }
            set { modeSelected = value; }
        }

        // Property to get or set the selected mode string ("-s", "-d", or "-b")
        public string ModeStr
        {
            get { return modeStr; }
            set { modeStr = value; }
        }

        // Property to get or set the path selection status
        public bool PathSelected
        {
            get { return pathSelected; }
            set { pathSelected = value; }
        }

        // Property to get or set the selected directory path
        public string PathStr
        {
            get { return pathStr; }
            set { pathStr = value; }
        }

        // Property to get or set the index of the mode argument in the command line
        public int ModeArgIndex
        {
            get { return modeArgIndex; }
            set { modeArgIndex = value; }
        }
    }
}

