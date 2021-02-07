namespace Veilleur
{
    internal static class WinExit
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        internal const uint EWX_LOGOFF = 0;  //Log off the network
        internal const uint EWX_FORCE = 4;   //Force any applications to quit
    }
}
