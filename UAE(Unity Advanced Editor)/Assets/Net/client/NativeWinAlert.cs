using System;
using System.Runtime.InteropServices;

/// <see>https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-messagebox</see>
public static class NativeWinAlert
{
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern System.IntPtr GetActiveWindow();

    public static System.IntPtr GetWindowHandle()
    {
        return GetActiveWindow();
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern int MessageBox(IntPtr hwnd, String lpText, String lpCaption, uint uType);

    /// <summary>
    /// Shows Error alert box with OK button.
    /// </summary>
    /// <param name="text">Main alert text / content.</param>
    /// <param name="caption">Message box title.</param>
    public static void Error(string text, string caption)
    {
        try
        {
            MessageBox(GetWindowHandle(), text, caption, (uint)(0x00000002L | 0x00000010L));
        }
        catch (Exception ex) { }
    }
    public static void Show(string text, string caption, string type)
    {
        {
            string DialogResult = string.Empty;
            uint MB_ABORTRETRYIGNORE = (uint)(0x00000002L | 0x00000010L);
            uint MB_CANCELTRYCONTINUE = (uint)(0x00000006L | 0x00000030L);
            uint MB_HELP = (uint)(0x00004000L | 0x00000040L);
            uint MB_OK = (uint)(0x00000000L | 0x00000040L);
            uint MB_OKCANCEL = (uint)(0x00000001L | 0x00000040L);
            uint MB_RETRYCANCEL = (uint)0x00000005L;
            uint MB_YESNO = (uint)(0x00000004L | 0x00000040L);
            uint MB_YESNOCANCEL = (uint)(0x00000003L | 0x00000040L);
            int intresult = -1;
            string strResult = string.Empty;

            switch (type)
            {
                case "AbortRetryIgnore":
                    intresult = MessageBox(GetWindowHandle(), text, caption, MB_ABORTRETRYIGNORE);
                    break;
                case "CancelTryContinue":
                    intresult = MessageBox(GetWindowHandle(), text, caption, MB_CANCELTRYCONTINUE);
                    break;
                case "Help":
                    intresult = MessageBox(GetWindowHandle(), text, caption, MB_HELP);
                    break;
                case "OK":
                    intresult = MessageBox(GetWindowHandle(), text, caption, MB_OK);
                    break;
                case "OkCancel":
                    intresult = MessageBox(GetWindowHandle(), text, caption, MB_OKCANCEL);
                    break;
                case "RetryCancel":
                    intresult = MessageBox(GetWindowHandle(), text, caption, MB_RETRYCANCEL);
                    break;
                case "YesNo":
                    intresult = MessageBox(GetWindowHandle(), text, caption, MB_YESNO);
                    break;
                case "YesNoCancel":
                    intresult = MessageBox(GetWindowHandle(), text, caption, MB_YESNOCANCEL);
                    break;
                default:
                    intresult = MessageBox(GetWindowHandle(), text, caption, (uint)(0x00000000L | 0x00000010L));
                    break;
            }

            switch (intresult)
            {
                case 1:
                    strResult = "OK";
                    break;
                case 2:
                    strResult = "CANCEL";
                    break;
                case 3:
                    strResult = "ABORT";
                    break;
                case 4:
                    strResult = "RETRY";
                    break;
                case 5:
                    strResult = "IGNORE";
                    break;
                case 6:
                    strResult = "YES";
                    break;
                case 7:
                    strResult = "NO";
                    break;
                case 10:
                    strResult = "TRY AGAIN";
                    break;
                default:
                    strResult = "OK";
                    break;

            }
        }
    }
}