using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sharpAHK
{
    public partial class _AHK
    {
        #region === User32 DLL Calls ===


        //       User32.dll (used in AHK Functions)

        [DllImport("User32")]
        private static extern int _ShowWindow(int hwnd, int nCmdShow);  // user32.dll call

        private const int SW_HIDE = 0;  // user32.dll var
        private const int SW_SHOW = 5;   // user32.dll var

        [DllImport("user32.dll")]
        private static extern bool _SetForegroundWindow(IntPtr hWnd);  // user32.dll call -  win activate

        [DllImport("user32.dll")]
        private static extern bool _ShowWindowAsync(IntPtr hWnd, int nCmdShow);  // user32.dll call - win restore

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();  // user32.dll call - Active Window Title
        //static extern IntPtr _GetForegroundWindow();  // user32.dll call - Active Window Title

        [DllImport("user32.dll")]
        static extern int _GetWindowText(IntPtr hWnd, StringBuilder text, int count);  // user32.dll call - Active Window Title

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool _GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);  // user32.dll call - is WinMaximized / WinMinimized

        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        //=== Set Window Always On Top ============
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);  // user32.dll call
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);  // user32.dll call
        private const UInt32 SWP_NOSIZE = 0x0001;  // user32.dll call
        private const UInt32 SWP_NOMOVE = 0x0002;  // user32.dll call
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;  // user32.dll call

        static readonly IntPtr HWND_TOP = new IntPtr(0);  // user32.dll call
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);  // user32.dll call

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)] // changed to private - confirm working 
        private static extern bool _SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);  // user32.dll call

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]  // changed to private - confirm working 
        private static extern IntPtr _SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);  // user32.dll call - Win Move


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr _SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);  // user32.dll call

        const UInt32 WM_CLOSE = 0x0010;  //win close

        //===== Get Window Position =======
        [DllImport("user32.dll", CharSet = CharSet.Auto)] // changed to private - confirm working 
        private static extern IntPtr _FindWindow(string strClassName, string strWindowName);  // user32.dll call

        [DllImport("user32.dll")] // changed to private - confirm working 
        private static extern bool _GetWindowRect(IntPtr hwnd, ref Rect rectangle);  // user32.dll call

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //private static extern int GetWindowText(int hWnd, StringBuilder title, int size);  // user32.dll call - Get Window Title

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        static extern long GetWindowText(IntPtr hwnd, StringBuilder lpString, int cch);


        private delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);  // user32.dll call

        [DllImport("USER32.DLL")]
        private static extern bool _EnumWindows(EnumWindowsProc enumFunc, int lParam);  // user32.dll call

        [DllImport("USER32.DLL")]
        private static extern int _GetWindowTextLength(IntPtr hWnd);  // user32.dll call

        [DllImport("USER32.DLL")]
        private static extern bool _IsWindowVisible(IntPtr hWnd);  // user32.dll call

        [DllImport("USER32.DLL")]
        private static extern IntPtr _GetShellWindow();  // user32.dll call

        //WARN: Only for "Any CPU":
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int _GetWindowThreadProcessId(IntPtr handle, out uint processId);  // user32.dll call


        //=== post message ==== 
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool _PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);  // user32.dll call


        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        const uint WM_CHAR = 0x0102;
        const int VK_TAB = 0x09;
        const int VK_ENTER = 0x0D;
        const int VK_UP = 0x26;
        const int VK_DOWN = 0x28;
        const int VK_RIGHT = 0x27;

        const uint NpadMsg = 0x111;
        //const IntPtr wParam = 44010;


        [DllImport("user32.dll")]
        static extern int _GetFocus();  // user32.dll call

        [DllImport("user32.dll")]
        static extern bool _AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);  // user32.dll call

        [DllImport("kernel32.dll")]
        static extern uint _GetCurrentThreadId();   // user32.dll call

        [DllImport("user32.dll")]
        static extern uint _GetWindowThreadProcessId(int hWnd, int ProcessId);  // user32.dll call

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern int _SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);  // user32.dll call

        const int WM_SETTEXT = 12;
        //const int WM_GETTEXT = 13;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int _GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);  // user32.dll call


        private const int WM_SYSCOMMAND = 274;
        private const int SC_MAXIMIZE = 61488;

        // changed to private - confirm working 
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int _SetParent(IntPtr hWndChild, IntPtr hWndNewParent);  // user32.dll call

        // changed to private - confirm working 
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        private static extern int _SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);  // user32.dll call


        //======= Mouse Click ======================
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool _SetCursorPos(int x, int y);  // user32.dll call

        // changed to private - confirm working 
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);  // user32.dll call

        private const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        private const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;  // changed to private - confirm working 
        private const int MOUSEEVENTF_LEFTUP = 0x04;    // changed to private - confirm working 


        const int WM_GETTEXT = 0x000D;
        const int WM_GETTEXTLENGTH = 0x000E;

        // changed to private - confirm working 
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int _RegisterWindowMessage(string lpString);  // user32.dll call

        // changed to private - confirm working 
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = System.Runtime.InteropServices.CharSet.Auto)] //
        private static extern bool _SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);  // user32.dll call

        // changed to private - confirm working 
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr _SendMessage(int hWnd, int Msg, int wparam, int lparam);  // user32.dll call

        // changed to private - confirm working 
        [DllImport("User32.dll")]
        private static extern Boolean _EnumChildWindows(int hWndParent, Delegate lpEnumFunc, int lParam);  // user32.dll call

        // changed to private - confirm working 
        private void _PostMsg(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)  // send postmessage to another application/form
        {
            _PostMessage(hWnd, msg, wParam, lParam);
        }


        #endregion
    }
}
