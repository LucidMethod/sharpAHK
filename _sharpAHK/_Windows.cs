using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        // === Windows Applications ===

        /// <summary>
        /// Launch Windows Disk Manager
        /// </summary>
        public void Windows_DiskMgr()
        {
            Run("diskmgmt.msc");
        }

        /// <summary>
        /// Launch Windows MyComputer Window.
        /// </summary>
        public void MyComputer()
        {
            Run("::{20d04fe0-3aea-1069-a2d8-08002b30309d}");
        }

        /// <summary>
        /// Launch Windows RecycleBin Window
        /// </summary>
        public void RecycleBin()
        {
            Run("::{645ff040-5081-101b-9f08-00aa002f954e}");
        }



    }
}
