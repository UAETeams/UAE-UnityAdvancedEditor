using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UnityEngine;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;
using Application = UnityEngine.Application;
using Screen = UnityEngine.Screen;
using Cursor = System.Windows.Forms.Cursor;

// Token: 0x02000005 RID: 5
public class Window : MonoBehaviour
{
    // Token: 0x06000016 RID: 22
    [DllImport("USER32.DLL")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    // Token: 0x06000017 RID: 23
    [DllImport("USER32.DLL")]
    public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

    // Token: 0x06000018 RID: 24
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(int hwnd, int nCmdShow);

    // Token: 0x06000019 RID: 25
    [DllImport("user32.dll")]
    private static extern int GetActiveWindow();

    // Token: 0x0600001A RID: 26
    [DllImport("user32.dll")]
    private static extern int SetWindowPos(int hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);

    // Token: 0x0600001B RID: 27
    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(int hWnd, ref Window.RECT NewRect);

    // Token: 0x0600001C RID: 28
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FlashWindowEx(ref Window.Flash pwfi);

    // Token: 0x0600001D RID: 29
    [DllImport("user32.dll")]
    private static extern long GetWindowText(int hWnd, StringBuilder text, int count);

    // Token: 0x0600001E RID: 30
    [DllImport("user32.dll")]
    private static extern bool SetWindowText(int hwnd, string lpString);

    // Token: 0x0600001F RID: 31
    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

    // Token: 0x06000020 RID: 32
    [DllImport("user32.dll")]
    public static extern bool SetFocus(int hWnd);

    // Token: 0x06000021 RID: 33
    [DllImport("user32.dll")]
    public static extern IntPtr GetDesktopWindow();

    // Token: 0x06000022 RID: 34
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    // Token: 0x06000023 RID: 35
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

    // Token: 0x06000024 RID: 36
    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    // Token: 0x06000025 RID: 37
    [DllImport("user32.dll")]
    public static extern int SetParent(int child, int parent);

    // Token: 0x06000026 RID: 38
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    // Token: 0x06000027 RID: 39
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern int GetForegroundWindow();

    // Token: 0x06000028 RID: 40
    [DllImport("user32.dll")]
    public static extern int GetFocus();

    // Token: 0x06000029 RID: 41
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsIconic(IntPtr hWnd);

    // Token: 0x0600002A RID: 42
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetActiveWindow(IntPtr hWnd);

    // Token: 0x0600002B RID: 43
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    // Token: 0x0600002C RID: 44
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetParent(IntPtr hWnd);

    // Token: 0x0600002D RID: 45 RVA: 0x0000462C File Offset: 0x0000282C
    private static Window.Flash FlashWindow(int handle, int flags, int count, int timeout)
    {
        Window.Flash flash = default(Window.Flash);
        flash.a = Convert.ToUInt32(Marshal.SizeOf<Window.Flash>(flash));
        flash.b = (IntPtr)handle;
        flash.c = (uint)flags;
        flash.d = (uint)count;
        flash.e = (uint)timeout;
        Window.FlashWindowEx(ref flash);
        return flash;
    }

    // Token: 0x0600002E RID: 46 RVA: 0x00004684 File Offset: 0x00002884
    private void Awake()
    {
        if (Window.GetActiveWindow() != 0)
        {
            Window.ID = Window.GetActiveWindow();
        }
        else
        {
            Window.ID = Window.ProcessIdByName(Application.productName);
        }
        Window.Local = this;
        Window.PermanentBorderSize = Vector2.zero;
        if (this.CrossSceneSupport)
        {
            Object.DontDestroyOnLoad(base.transform.gameObject);
        }
    }

    // Token: 0x0600002F RID: 47 RVA: 0x000046DC File Offset: 0x000028DC
    private void Start()
    {
        if (!Window.Local)
        {
            Window.Local = this;
        }
        this.Once = true;
        if (Window.ID == 0)
        {
            if (Window.GetActiveWindow() != 0)
            {
                Window.ID = Window.GetActiveWindow();
            }
            else
            {
                Window.ID = Window.ProcessIdByName(Application.productName);
            }
        }
        Screen.fullScreen = false;
        Window.LastResolution = Screen.currentResolution;
    }

    // Token: 0x06000030 RID: 48 RVA: 0x0000473C File Offset: 0x0000293C
    private void Update()
    {
        if (Window.ID == 0)
        {
            if (Window.GetActiveWindow() != 0)
            {
                Window.ID = Window.GetActiveWindow();
            }
            else
            {
                Window.ID = Window.ProcessIdByName(Application.productName);
            }
        }
        if (!Window.Local)
        {
            Window.Local = this;
        }
        if (this.Once && (this.QuickAutoBorderless || this.FullyAutoBorderless) && Window.DoneCalculating && Window.Borders && !Application.isEditor)
        {
            if (this.QuickAutoBorderless)
            {
                if (Window.ID != 0)
                {
                    this.Once = false;
                }
            }
            else
            {
                this.Once = false;
            }
            if (this.QuickAutoBorderless)
            {
                Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
            }
            else
            {
                try
                {
                    Process.Start(Regex.Replace(Application.dataPath.ToString().Remove(Application.dataPath.ToString().Length - 5) + ".exe", "/", "\\"), "-popupwindow");
                    if (Window.ID != Process.GetCurrentProcess().Id)
                    {
                        Debug.LogWarning(string.Concat(new object[]
                        {
                            "Misleaded processes : ",
                            Window.ID,
                            " and ",
                            Process.GetCurrentProcess().Id
                        }));
                    }
                    Process.GetCurrentProcess().Kill();
                }
                catch (Exception ex)
                {
                    Debug.LogError("Could not reboot, reason : " + ex.ToString());
                }
            }
        }
        Window.OnMonitorResolutionChanged();
        if (Window.PermanentBorderSize == Vector2.zero)
        {
            Window.PermanentBorderSize = Window.GetBorderSize();
        }
        if (this.FocusResetOnClick)
        {
            if (Window.IsMinimized())
            {
                this.MinimizeReset = true;
            }
            else if (this.MinimizeReset)
            {
                Window.SetWindowPosition(Window.ID, 0, (int)Window.ResetSize.x, (int)Window.ResetSize.y, (int)Window.ResetSize.width, (int)Window.ResetSize.height, 96);
                this.MinimizeReset = false;
            }
            else
            {
                Window.ResetSize = Window.GetRect();
            }
            if (Window.GetForegroundWindow() == Window.ID)
            {
                if (this.FocusReset)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 96);
                    this.FocusReset = false;
                }
            }
            else
            {
                this.FocusReset = true;
            }
        }
        if (Window.MoveWindow && Window.DoneCalculating)
        {
            Window.SetFocus(Window.ID);
            if (Window.Action == 1)
            {
                Window.SetWindowPosition(Window.ID, 0, (int)((float)Cursor.Position.X + Window.MoveOffSet.x), (int)((float)Cursor.Position.Y + Window.MoveOffSet.y) + 1, (int)Window.MoveOffSet.z, (int)Window.MoveOffSet.w, 96);
            }
            if (Window.Action == 2)
            {
                if ((float)((int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x))) > Window.Limitations.y)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.y - Window.MoveOffSet.z)), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)Window.MoveOffSet.w, 96);
                }
                else if ((float)((int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x))) < Window.Limitations.x)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.x - Window.MoveOffSet.z)), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)Window.MoveOffSet.w, 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)((float)Cursor.Position.X + Window.MoveOffSet.x), (int)Window.MoveOffSet.y + 1, (int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x)), (int)Window.MoveOffSet.w, 96);
                }
            }
            if (Window.Action == 3)
            {
                if ((float)((int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x))) > Window.Limitations.y)
                {
                    if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.y - Window.MoveOffSet.z)), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)Window.Limitations.w, 96);
                    }
                    else if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.y - Window.MoveOffSet.z)), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.y - Window.MoveOffSet.z)), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w), 96);
                    }
                }
                else if ((float)((int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x))) < Window.Limitations.x)
                {
                    if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.x - Window.MoveOffSet.z)), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)Window.Limitations.w, 96);
                    }
                    else if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.x - Window.MoveOffSet.z)), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.x - Window.MoveOffSet.z)), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w), 96);
                    }
                }
                else if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) > Window.Limitations.w)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)((float)Cursor.Position.X + Window.MoveOffSet.x), (int)Window.MoveOffSet.y + 1, (int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x)), (int)Window.Limitations.w, 96);
                }
                else if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) < Window.Limitations.z)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)((float)Cursor.Position.X + Window.MoveOffSet.x), (int)Window.MoveOffSet.y + 1, (int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x)), (int)Window.Limitations.z, 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)((float)Cursor.Position.X + Window.MoveOffSet.x), (int)Window.MoveOffSet.y + 1, (int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x)), (int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w), 96);
                }
            }
            if (Window.Action == 4)
            {
                if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) > Window.Limitations.w)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.MoveOffSet.z, (int)Window.Limitations.w, 96);
                }
                else if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) < Window.Limitations.z)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.MoveOffSet.z, (int)Window.Limitations.z, 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.MoveOffSet.z, (int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w), 96);
                }
            }
            if (Window.Action == 5)
            {
                if ((float)((int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z)) > Window.Limitations.y)
                {
                    if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)Window.Limitations.w, 96);
                    }
                    else if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w), 96);
                    }
                }
                else if ((float)((int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z)) < Window.Limitations.x)
                {
                    if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)Window.Limitations.w, 96);
                    }
                    else if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w), 96);
                    }
                }
                else if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) > Window.Limitations.w)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z), (int)Window.Limitations.w, 96);
                }
                else if ((float)((int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w)) < Window.Limitations.z)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z), (int)Window.Limitations.z, 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z), (int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w), 96);
                }
            }
            if (Window.Action == 6)
            {
                if ((float)((int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z)) > Window.Limitations.y)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)Window.MoveOffSet.w, 96);
                }
                else if ((float)((int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z)) < Window.Limitations.x)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)Window.MoveOffSet.w, 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z), (int)Window.MoveOffSet.w, 96);
                }
            }
            if (Window.Action == 7)
            {
                if ((float)((int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z)) > Window.Limitations.y)
                {
                    if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.w - Window.MoveOffSet.w)) + 1, (int)Window.Limitations.y, (int)Window.Limitations.w, 96);
                    }
                    else if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.z - Window.MoveOffSet.w)) + 1, (int)Window.Limitations.y, (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)((float)Cursor.Position.Y + Window.MoveOffSet.y) + 1, (int)Window.Limitations.y, (int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)), 96);
                    }
                }
                else if ((float)((int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z)) < Window.Limitations.x)
                {
                    if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.w - Window.MoveOffSet.w)) + 1, (int)Window.Limitations.x, (int)Window.Limitations.w, 96);
                    }
                    else if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.z - Window.MoveOffSet.w)) + 1, (int)Window.Limitations.x, (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)((float)Cursor.Position.Y + Window.MoveOffSet.y) + 1, (int)Window.Limitations.x, (int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)), 96);
                    }
                }
                else if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) > Window.Limitations.w)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.w - Window.MoveOffSet.w)) + 1, (int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z), (int)Window.Limitations.w, 96);
                }
                else if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) < Window.Limitations.z)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.z - Window.MoveOffSet.w)) + 1, (int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z), (int)Window.Limitations.z, 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)((float)Cursor.Position.Y + Window.MoveOffSet.y) + 1, (int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z), (int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)), 96);
                }
            }
            if (Window.Action == 8)
            {
                if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) > Window.Limitations.w)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.w - Window.MoveOffSet.w)) + 1, (int)Window.MoveOffSet.z, (int)Window.Limitations.w, 96);
                }
                else if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) < Window.Limitations.z)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.z - Window.MoveOffSet.w)) + 1, (int)Window.MoveOffSet.z, (int)Window.Limitations.z, 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)((float)Cursor.Position.Y + Window.MoveOffSet.y) + 1, (int)Window.MoveOffSet.z, (int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)), 96);
                }
            }
            if (Window.Action == 9)
            {
                if ((float)((int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x))) > Window.Limitations.y)
                {
                    if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.y - Window.MoveOffSet.z)), (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.w - Window.MoveOffSet.w)) + 1, (int)Window.Limitations.y, (int)Window.Limitations.w, 96);
                    }
                    else if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.y - Window.MoveOffSet.z)), (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.z - Window.MoveOffSet.w)) + 1, (int)Window.Limitations.y, (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.y - Window.MoveOffSet.z)), (int)((float)Cursor.Position.Y + Window.MoveOffSet.y) + 1, (int)Window.Limitations.y, (int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)), 96);
                    }
                }
                else if ((float)((int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x))) < Window.Limitations.x)
                {
                    if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.x - Window.MoveOffSet.z)), (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.w - Window.MoveOffSet.w)) + 1, (int)Window.Limitations.x, (int)Window.Limitations.w, 96);
                    }
                    else if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.x - Window.MoveOffSet.z)), (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.z - Window.MoveOffSet.w)) + 1, (int)Window.Limitations.x, (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.x + Window.MoveOffSet.x - (Window.Limitations.x - Window.MoveOffSet.z)), (int)((float)Cursor.Position.Y + Window.MoveOffSet.y) + 1, (int)Window.Limitations.x, (int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)), 96);
                    }
                }
                else if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) > Window.Limitations.w)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)((float)Cursor.Position.X + Window.MoveOffSet.x), (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.w - Window.MoveOffSet.w)) + 1, (int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x)), (int)Window.Limitations.w, 96);
                }
                else if ((float)((int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y))) < Window.Limitations.z)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)((float)Cursor.Position.X + Window.MoveOffSet.x), (int)(Window.OldOffSet.y + Window.MoveOffSet.y - (Window.Limitations.z - Window.MoveOffSet.w)) + 1, (int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x)), (int)Window.Limitations.z, 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)((float)Cursor.Position.X + Window.MoveOffSet.x), (int)((float)Cursor.Position.Y + Window.MoveOffSet.y) + 1, (int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x)), (int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)), 96);
                }
            }
            if (Window.Action == 12)
            {
                int num = (int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x));
                if ((float)num > Window.Limitations.y)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.z + Window.MoveOffSet.z - Window.Limitations.y), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)(Window.Limitations.y / Window.AspectRation), 96);
                }
                else if ((float)num < Window.Limitations.x)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.z + Window.MoveOffSet.z - Window.Limitations.x), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)(Window.Limitations.x / Window.AspectRation), 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.z + Window.MoveOffSet.z - (float)num), (int)Window.MoveOffSet.y + 1, num, (int)((float)num / Window.AspectRation), 96);
                }
            }
            if (Window.Action == 13)
            {
                int num = (int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x));
                if ((float)num / ((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w) >= Window.AspectRation)
                {
                    if ((float)num > Window.Limitations.y)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.z + Window.MoveOffSet.z - Window.Limitations.y), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)(Window.Limitations.y / Window.AspectRation), 96);
                    }
                    else if ((float)num < Window.Limitations.x)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.z + Window.MoveOffSet.z - Window.Limitations.x), (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)(Window.Limitations.x / Window.AspectRation), 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.z + Window.MoveOffSet.z - (float)num), (int)Window.MoveOffSet.y + 1, num, (int)((float)num / Window.AspectRation), 96);
                    }
                }
                if ((float)num / ((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w) < Window.AspectRation)
                {
                    num = (int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w);
                    if ((float)num > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.z + Window.MoveOffSet.z - Window.Limitations.w * Window.AspectRation), (int)Window.MoveOffSet.y + 1, (int)(Window.Limitations.w * Window.AspectRation), (int)Window.Limitations.w, 96);
                    }
                    else if ((float)num < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.z + Window.MoveOffSet.z - Window.Limitations.z * Window.AspectRation), (int)Window.MoveOffSet.y + 1, (int)(Window.Limitations.z * Window.AspectRation), (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.OldOffSet.z + Window.MoveOffSet.z - (float)num * Window.AspectRation), (int)Window.MoveOffSet.y + 1, (int)((float)num * Window.AspectRation), num, 96);
                    }
                }
            }
            if (Window.Action == 14)
            {
                int num = (int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w);
                if ((float)num > Window.Limitations.w)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)(Window.Limitations.w * Window.AspectRation), (int)Window.Limitations.w, 96);
                }
                else if ((float)num < Window.Limitations.z)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)(Window.Limitations.z * Window.AspectRation), (int)Window.Limitations.z, 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)((float)num * Window.AspectRation), num, 96);
                }
            }
            if (Window.Action == 15)
            {
                int num = (int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z);
                if ((float)num / ((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w) >= Window.AspectRation)
                {
                    if ((float)num > Window.Limitations.y)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)(Window.Limitations.y / Window.AspectRation), 96);
                    }
                    else if ((float)num < Window.Limitations.x)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)(Window.Limitations.x / Window.AspectRation), 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, num, (int)((float)num / Window.AspectRation), 96);
                    }
                }
                if ((float)num / ((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w) < Window.AspectRation)
                {
                    num = (int)((float)Cursor.Position.Y - Window.OldOffSet.y + Window.MoveOffSet.w);
                    if ((float)num > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)(Window.Limitations.w * Window.AspectRation), (int)Window.Limitations.w, 96);
                    }
                    else if ((float)num < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)(Window.Limitations.z * Window.AspectRation), (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)((float)num * Window.AspectRation), num, 96);
                    }
                }
            }
            if (Window.Action == 16)
            {
                int num = (int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z);
                if ((float)num > Window.Limitations.y)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.y, (int)(Window.Limitations.y / Window.AspectRation), 96);
                }
                else if ((float)num < Window.Limitations.x)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, (int)Window.Limitations.x, (int)(Window.Limitations.x / Window.AspectRation), 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)Window.MoveOffSet.y + 1, num, (int)((float)num / Window.AspectRation), 96);
                }
            }
            if (Window.Action == 17)
            {
                int num = (int)((float)Cursor.Position.X - Window.OldOffSet.x + Window.MoveOffSet.z);
                if ((float)num / (Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)) >= Window.AspectRation)
                {
                    if ((float)num > Window.Limitations.y)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.z + Window.MoveOffSet.w - Window.Limitations.y / Window.AspectRation), (int)Window.Limitations.y, (int)(Window.Limitations.y / Window.AspectRation), 96);
                    }
                    else if ((float)num < Window.Limitations.x)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.z + Window.MoveOffSet.w - Window.Limitations.x / Window.AspectRation), (int)Window.Limitations.x, (int)(Window.Limitations.x / Window.AspectRation), 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.z + Window.MoveOffSet.w - (float)num / Window.AspectRation), num, (int)((float)num / Window.AspectRation), 96);
                    }
                }
                if ((float)num / (Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)) < Window.AspectRation)
                {
                    num = (int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y));
                    if ((float)num > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.z + Window.MoveOffSet.w - Window.Limitations.w), (int)(Window.Limitations.w * Window.AspectRation), (int)Window.Limitations.w, 96);
                    }
                    else if ((float)num < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.z + Window.MoveOffSet.w - Window.Limitations.z), (int)(Window.Limitations.z * Window.AspectRation), (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)(Window.OldOffSet.z + Window.MoveOffSet.w - (float)num), (int)((float)num * Window.AspectRation), num, 96);
                    }
                }
            }
            if (Window.Action == 18)
            {
                int num = (int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y));
                if ((float)num > Window.Limitations.w)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)((float)Window.Position.y + Window.MoveOffSet.w - Window.Limitations.w), (int)(Window.Limitations.w * Window.AspectRation), (int)Window.Limitations.w, 96);
                }
                else if ((float)num < Window.Limitations.z)
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)((float)Window.Position.y + Window.MoveOffSet.w - Window.Limitations.z), (int)(Window.Limitations.z * Window.AspectRation), (int)Window.Limitations.z, 96);
                }
                else
                {
                    Window.SetWindowPosition(Window.ID, 0, (int)Window.MoveOffSet.x, (int)((float)Window.Position.y + Window.MoveOffSet.w - (float)num), (int)((float)num * Window.AspectRation), num, 96);
                }
            }
            if (Window.Action == 19)
            {
                int num = (int)(Window.MoveOffSet.z - ((float)Cursor.Position.X - Window.OldOffSet.x));
                if ((float)num / (Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)) >= Window.AspectRation)
                {
                    if ((float)num > Window.Limitations.y)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.MoveOffSet.x + Window.MoveOffSet.z - Window.Limitations.y), (int)(Window.OldOffSet.z + Window.MoveOffSet.w - Window.Limitations.y / Window.AspectRation), (int)Window.Limitations.y, (int)(Window.Limitations.y / Window.AspectRation), 96);
                    }
                    else if ((float)num < Window.Limitations.x)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.MoveOffSet.x + Window.MoveOffSet.z - Window.Limitations.x), (int)(Window.OldOffSet.z + Window.MoveOffSet.w - Window.Limitations.x / Window.AspectRation), (int)Window.Limitations.x, (int)(Window.Limitations.x / Window.AspectRation), 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.MoveOffSet.x + Window.MoveOffSet.z - (float)num), (int)(Window.OldOffSet.z + Window.MoveOffSet.w - (float)num / Window.AspectRation), num, (int)((float)num / Window.AspectRation), 96);
                    }
                }
                if ((float)num / (Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y)) < Window.AspectRation)
                {
                    num = (int)(Window.MoveOffSet.w - ((float)Cursor.Position.Y - Window.OldOffSet.y));
                    if ((float)num > Window.Limitations.w)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.MoveOffSet.x + Window.MoveOffSet.z - Window.Limitations.w * Window.AspectRation), (int)(Window.OldOffSet.z + Window.MoveOffSet.w - Window.Limitations.w), (int)(Window.Limitations.w * Window.AspectRation), (int)Window.Limitations.w, 96);
                    }
                    else if ((float)num < Window.Limitations.z)
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.MoveOffSet.x + Window.MoveOffSet.z - Window.Limitations.z * Window.AspectRation), (int)(Window.OldOffSet.z + Window.MoveOffSet.w - Window.Limitations.z), (int)(Window.Limitations.z * Window.AspectRation), (int)Window.Limitations.z, 96);
                    }
                    else
                    {
                        Window.SetWindowPosition(Window.ID, 0, (int)(Window.MoveOffSet.x + Window.MoveOffSet.z - (float)num * Window.AspectRation), (int)(Window.OldOffSet.z + Window.MoveOffSet.w - (float)num), (int)((float)num * Window.AspectRation), num, 96);
                    }
                }
            }
        }
        Window.GetWindowRect(Window.ID, ref Window.Position);
        if (new Vector2(Input.mousePosition.x - Window.CursorUpdate.x, Input.mousePosition.y - Window.CursorUpdate.y) == Vector2.zero)
        {
            Window.ClientPosition = new Vector2((float)Cursor.Position.X - Input.mousePosition.x - 1f, (float)Cursor.Position.Y - ((float)Screen.height - Input.mousePosition.y));
        }
        Window.CursorUpdate = Input.mousePosition;
        if (Screen.height != Window.Position.w - Window.Position.y)
        {
            Window.Borders = true;
        }
        else
        {
            Window.Borders = false;
        }
        if (Window.ID != 0)
        {
            Window.DoneCalculating = true;
        }
    }

    // Token: 0x06000031 RID: 49 RVA: 0x00007734 File Offset: 0x00005934
    public static bool IsDoneLoading()
    {
        return Window.DoneCalculating;
    }

    // Token: 0x06000032 RID: 50 RVA: 0x0000773B File Offset: 0x0000593B
    public static bool IsBorderless()
    {
        return !Window.Borders;
    }

    // Token: 0x06000033 RID: 51 RVA: 0x00007745 File Offset: 0x00005945
    public static void Border()
    {
        Window.Border(!Window.Borders);
    }

    // Token: 0x06000034 RID: 52 RVA: 0x00007754 File Offset: 0x00005954
    public static void Border(bool Active)
    {
        if (!Application.isEditor)
        {
            try
            {
                if (Active)
                {
                    Process.Start(Regex.Replace(Application.dataPath.ToString().Remove(Application.dataPath.ToString().Length - 5) + ".exe", "/", "\\"));
                    if (Window.ID != Process.GetCurrentProcess().Id)
                    {
                        Debug.LogWarning(string.Concat(new object[]
                        {
                            "Misleaded processes : ",
                            Window.ID,
                            " and ",
                            Process.GetCurrentProcess().Id
                        }));
                    }
                    Process.GetCurrentProcess().Kill();
                }
                else
                {
                    Process.Start(Regex.Replace(Application.dataPath.ToString().Remove(Application.dataPath.ToString().Length - 5) + ".exe", "/", "\\"), "-popupwindow");
                    if (Window.ID != Process.GetCurrentProcess().Id)
                    {
                        Debug.LogWarning(string.Concat(new object[]
                        {
                            "Misleaded processes : ",
                            Window.ID,
                            " and ",
                            Process.GetCurrentProcess().Id
                        }));
                    }
                    Process.GetCurrentProcess().Kill();
                }
                return;
            }
            catch (Exception ex)
            {
                Debug.LogError("Could not set borders, reason : " + ex.ToString());
                return;
            }
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("Border function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000035 RID: 53 RVA: 0x000078FC File Offset: 0x00005AFC
    public static void SetRect(Rect Source)
    {
        Window.SetRect(Window.ID, (int)Source.x, (int)Source.y, (int)Source.width, (int)Source.height);
    }

    // Token: 0x06000036 RID: 54 RVA: 0x00007928 File Offset: 0x00005B28
    public static void SetRect(int LeftCorner, int TopCorner, int width, int height)
    {
        Window.SetRect(Window.ID, LeftCorner, TopCorner, width, height);
    }

    // Token: 0x06000037 RID: 55 RVA: 0x00007938 File Offset: 0x00005B38
    public static void SetRect(int windowId, Rect Source)
    {
        Window.SetRect(windowId, (int)Source.x, (int)Source.y, (int)Source.width, (int)Source.height);
    }

    // Token: 0x06000038 RID: 56 RVA: 0x00007960 File Offset: 0x00005B60
    public static void SetRect(int windowId, int LeftCorner, int TopCorner, int width, int height)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow || Window.ID != windowId)
            {
                if ((float)width > Window.Limitations.y && Window.ID == windowId)
                {
                    if ((float)height > Window.Limitations.w && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, LeftCorner, TopCorner + 1, (int)Window.Limitations.y, (int)Window.Limitations.w, 96);
                        return;
                    }
                    if ((float)height < Window.Limitations.z && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, LeftCorner, TopCorner + 1, (int)Window.Limitations.y, (int)Window.Limitations.z, 96);
                        return;
                    }
                    Window.SetWindowPosition(windowId, 0, LeftCorner, TopCorner + 1, (int)Window.Limitations.y, height, 96);
                    return;
                }
                else if ((float)width < Window.Limitations.x && Window.ID == windowId)
                {
                    if ((float)height > Window.Limitations.w && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, LeftCorner, TopCorner + 1, (int)Window.Limitations.x, (int)Window.Limitations.w, 96);
                        return;
                    }
                    if ((float)height < Window.Limitations.z && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, LeftCorner, TopCorner + 1, (int)Window.Limitations.x, (int)Window.Limitations.z, 96);
                        return;
                    }
                    Window.SetWindowPosition(windowId, 0, LeftCorner, TopCorner + 1, (int)Window.Limitations.x, height, 96);
                    return;
                }
                else
                {
                    if ((float)height > Window.Limitations.w && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, LeftCorner, TopCorner + 1, width, (int)Window.Limitations.w, 96);
                        return;
                    }
                    if ((float)height < Window.Limitations.z && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, LeftCorner, TopCorner + 1, width, (int)Window.Limitations.z, 96);
                        return;
                    }
                    Window.SetWindowPosition(windowId, 0, LeftCorner, TopCorner + 1, width, height, 96);
                    return;
                }
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("SetRect function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("SetRect function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000039 RID: 57 RVA: 0x00007B83 File Offset: 0x00005D83
    public static void SetPosition(int LeftCorner, int TopCorner)
    {
        Window.SetPosition(Window.ID, new Vector2((float)LeftCorner, (float)TopCorner));
    }

    // Token: 0x0600003A RID: 58 RVA: 0x00007B98 File Offset: 0x00005D98
    public static void SetPosition(Vector2 Source)
    {
        Window.SetPosition(Window.ID, Source);
    }

    // Token: 0x0600003B RID: 59 RVA: 0x00007BA5 File Offset: 0x00005DA5
    public static void SetPosition(int windowId, int LeftCorner, int TopCorner)
    {
        Window.SetPosition(windowId, new Vector2((float)LeftCorner, (float)TopCorner));
    }

    // Token: 0x0600003C RID: 60 RVA: 0x00007BB8 File Offset: 0x00005DB8
    public static void SetPosition(int windowId, Vector2 Source)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow || Window.ID != windowId)
            {
                int width = Screen.width;
                int height = Screen.height;
                Window.SetWindowPosition(windowId, 0, (int)Source.x, (int)Source.y + 1, width, height, 96);
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("SetPosition function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("SetPosition function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600003D RID: 61 RVA: 0x00007C30 File Offset: 0x00005E30
    public static void SetSize(int Width, int Height)
    {
        Window.SetSize(new Vector2((float)Width, (float)Height));
    }

    // Token: 0x0600003E RID: 62 RVA: 0x00007C40 File Offset: 0x00005E40
    public static void SetSize(Vector2 Source)
    {
        Window.SetSize(Window.ID, Source);
    }

    // Token: 0x0600003F RID: 63 RVA: 0x00007C4D File Offset: 0x00005E4D
    public static void SetSize(int windowId, int Width, int Height)
    {
        Window.SetSize(windowId, new Vector2((float)Width, (float)Height));
    }

    // Token: 0x06000040 RID: 64 RVA: 0x00007C60 File Offset: 0x00005E60
    public static void SetSize(int windowId, Vector2 Source)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow || Window.ID != windowId)
            {
                if ((float)((int)Source.x) > Window.Limitations.y && Window.ID == windowId)
                {
                    if ((float)((int)Source.y) > Window.Limitations.w && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, Window.Position.x, Window.Position.y, (int)Window.Limitations.y, (int)Window.Limitations.w, 96);
                        return;
                    }
                    if ((float)((int)Source.y) < Window.Limitations.z && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, Window.Position.x, Window.Position.y, (int)Window.Limitations.y, (int)Window.Limitations.z, 96);
                        return;
                    }
                    Window.SetWindowPosition(windowId, 0, Window.Position.x, Window.Position.y, (int)Window.Limitations.y, (int)Source.y, 96);
                    return;
                }
                else if ((float)((int)Source.x) < Window.Limitations.x && Window.ID == windowId)
                {
                    if ((float)((int)Source.y) > Window.Limitations.w && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, Window.Position.x, Window.Position.y, (int)Window.Limitations.x, (int)Window.Limitations.w, 96);
                        return;
                    }
                    if ((float)((int)Source.y) < Window.Limitations.z && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, Window.Position.x, Window.Position.y, (int)Window.Limitations.x, (int)Window.Limitations.z, 96);
                        return;
                    }
                    Window.SetWindowPosition(windowId, 0, Window.Position.x, Window.Position.y, (int)Window.Limitations.x, (int)Source.y, 96);
                    return;
                }
                else
                {
                    if ((float)((int)Source.y) > Window.Limitations.w && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, Window.Position.x, Window.Position.y, (int)Source.x, (int)Window.Limitations.w, 96);
                        return;
                    }
                    if ((float)((int)Source.y) < Window.Limitations.z && Window.ID == windowId)
                    {
                        Window.SetWindowPosition(windowId, 0, Window.Position.x, Window.Position.y, (int)Source.x, (int)Window.Limitations.z, 96);
                        return;
                    }
                    Window.SetWindowPosition(windowId, 0, Window.Position.x, Window.Position.y, (int)Source.x, (int)Source.y, 96);
                    return;
                }
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("SetSize function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("SetSize function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000041 RID: 65 RVA: 0x00007F60 File Offset: 0x00006160
    public static Rect GetBorderRect()
    {
        return new Rect((float)Window.Position.x, (float)Window.Position.y, (float)(Window.Position.z - Window.Position.x), (float)(Window.Position.w - Window.Position.y));
    }

    // Token: 0x06000042 RID: 66 RVA: 0x00007FB4 File Offset: 0x000061B4
    public static Vector2 GetBorderPosition()
    {
        return new Vector2((float)Window.Position.x, (float)Window.Position.y);
    }

    // Token: 0x06000043 RID: 67 RVA: 0x00007FD4 File Offset: 0x000061D4
    public static Vector2 GetBorderSize()
    {
        if (Window.Borders)
        {
            return new Vector2((float)((Window.Position.z - Window.Position.x - Screen.width) / 2), (float)((Window.Position.w - Window.Position.y - Screen.height) / 2));
        }
        return Vector2.zero;
    }

    // Token: 0x06000044 RID: 68 RVA: 0x0000802F File Offset: 0x0000622F
    public static Vector2 GetPermnentBorderSize()
    {
        return Window.PermanentBorderSize;
    }

    // Token: 0x06000045 RID: 69 RVA: 0x00008036 File Offset: 0x00006236
    public static Rect GetRect()
    {
        return new Rect((float)Window.Position.x - Window.GetBorderSize().x, (float)Window.Position.y - Window.GetBorderSize().y, (float)Screen.width, (float)Screen.height);
    }

    // Token: 0x06000046 RID: 70 RVA: 0x00008075 File Offset: 0x00006275
    public static Vector2 GetPosition()
    {
        if (Window.Borders)
        {
            return Window.ClientPosition;
        }
        return new Vector2((float)Window.Position.x, (float)Window.Position.y);
    }

    // Token: 0x06000047 RID: 71 RVA: 0x0000809F File Offset: 0x0000629F
    public static Vector2 GetSize()
    {
        return new Vector2((float)Screen.width, (float)Screen.height);
    }

    // Token: 0x06000048 RID: 72 RVA: 0x000080B4 File Offset: 0x000062B4
    public static void GrabStart()
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4(-Input.mousePosition.x, Input.mousePosition.y - (float)Screen.height, (float)Screen.width, (float)Screen.height);
                Window.MoveWindow = true;
                Window.Action = 1;
                if (Window.Local.AutoFixAfterResizing)
                {
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    return;
                }
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a Grab function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("GrabStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000049 RID: 73 RVA: 0x00008214 File Offset: 0x00006414
    public static void GrabEnd()
    {
        if (!Application.isEditor)
        {
            if (Window.MoveWindow && Window.Action == 1)
            {
                Window.MoveOffSet = new Vector4(0f, 0f, 0f, 0f);
                Window.MoveWindow = false;
                Window.Action = 0;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant end a Grab function while you haven't started a Grab function or another Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("GrabEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600004A RID: 74 RVA: 0x00008290 File Offset: 0x00006490
    public static void ResizeLeftStart(float aspectRatio)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4(-Input.mousePosition.x, (float)(Window.Position.y - 1), (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.OldOffSet.z = (float)Window.Position.x;
                Window.MoveWindow = true;
                Window.Action = 12;
                Window.AspectRation = aspectRatio;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeLeft function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600004B RID: 75 RVA: 0x00008370 File Offset: 0x00006570
    public static void ResizeLeftStart()
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4(-Input.mousePosition.x, (float)(Window.Position.y - 1), (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 2;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeLeft function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600004C RID: 76 RVA: 0x00008430 File Offset: 0x00006630
    public static void ResizeLeftEnd()
    {
        if (!Application.isEditor)
        {
            if (Window.MoveWindow && (Window.Action == 2 || Window.Action == 12))
            {
                Window.MoveOffSet = new Vector4(0f, 0f, 0f, 0f);
                Window.MoveWindow = false;
                Window.Action = 0;
                if (Window.Local.AutoFixAfterResizing)
                {
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    return;
                }
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant end a ResizeLeft function while you haven't started a ResizeLeft function or a Grab function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeLeftEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600004D RID: 77 RVA: 0x00008588 File Offset: 0x00006788
    public static void ResizeDownLeftStart(float aspectRatio)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4(-Input.mousePosition.x, (float)(Window.Position.y - 1), (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.OldOffSet.z = (float)Window.Position.x;
                Window.MoveWindow = true;
                Window.Action = 13;
                Window.AspectRation = aspectRatio;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeDownLeft function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeDownLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600004E RID: 78 RVA: 0x00008668 File Offset: 0x00006868
    public static void ResizeDownLeftStart()
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4(-Input.mousePosition.x, (float)(Window.Position.y - 1), (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 3;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeDownLeft function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeDownLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600004F RID: 79 RVA: 0x00008728 File Offset: 0x00006928
    public static void ResizeDownLeftEnd()
    {
        if (!Application.isEditor)
        {
            if (Window.MoveWindow && (Window.Action == 3 || Window.Action == 13))
            {
                Window.MoveOffSet = new Vector4(0f, 0f, 0f, 0f);
                Window.MoveWindow = false;
                Window.Action = 0;
                if (Window.Local.AutoFixAfterResizing)
                {
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    return;
                }
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant end a ResizeDownLeft function while you haven't started a ResizeDownLeft function or a Grab function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeDownLeftEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000050 RID: 80 RVA: 0x00008880 File Offset: 0x00006A80
    public static void ResizeDownStart(float aspectRatio)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, (float)(Window.Position.y - 1), (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 14;
                Window.AspectRation = aspectRatio;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeDown function while another Grab or Resize function hasnt ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeDownStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000051 RID: 81 RVA: 0x00008944 File Offset: 0x00006B44
    public static void ResizeDownStart()
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, (float)(Window.Position.y - 1), (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 4;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeDown function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeDownStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000052 RID: 82 RVA: 0x00008A04 File Offset: 0x00006C04
    public static void ResizeDownEnd()
    {
        if (!Application.isEditor)
        {
            if (Window.MoveWindow && (Window.Action == 4 || Window.Action == 14))
            {
                Window.MoveOffSet = new Vector4(0f, 0f, 0f, 0f);
                Window.MoveWindow = false;
                Window.Action = 0;
                if (Window.Local.AutoFixAfterResizing)
                {
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    return;
                }
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant end a ResizeDown function while you haven't started a ResizeDown function or a Grab function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeDownEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000053 RID: 83 RVA: 0x00008B5C File Offset: 0x00006D5C
    public static void ResizeDownRightStart(float aspectRatio)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, (float)(Window.Position.y - 1), (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 15;
                Window.AspectRation = aspectRatio;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeDownRight function while another Grab or Resize function hasnt ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeDownRightStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000054 RID: 84 RVA: 0x00008C20 File Offset: 0x00006E20
    public static void ResizeDownRightStart()
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, (float)(Window.Position.y - 1), (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 5;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeDownRight function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeDownRightStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000055 RID: 85 RVA: 0x00008CE0 File Offset: 0x00006EE0
    public static void ResizeDownRightEnd()
    {
        if (!Application.isEditor)
        {
            if (Window.MoveWindow && (Window.Action == 5 || Window.Action == 15))
            {
                Window.MoveOffSet = new Vector4(0f, 0f, 0f, 0f);
                Window.MoveWindow = false;
                Window.Action = 0;
                if (Window.Local.AutoFixAfterResizing)
                {
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    return;
                }
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant end a ResizeDownRight function while you haven't started a ResizeDownRight function or a Grab function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeDownRightEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000056 RID: 86 RVA: 0x00008E38 File Offset: 0x00007038
    public static void ResizeRightStart(float aspectRatio)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, (float)(Window.Position.y - 1), (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 16;
                Window.AspectRation = aspectRatio;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeRight function while another Grab or Resize function hasnt ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeRightStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000057 RID: 87 RVA: 0x00008EFC File Offset: 0x000070FC
    public static void ResizeRightStart()
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, (float)(Window.Position.y - 1), (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 6;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeRight function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeRightStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000058 RID: 88 RVA: 0x00008FBC File Offset: 0x000071BC
    public static void ResizeRightEnd()
    {
        if (!Application.isEditor)
        {
            if (Window.MoveWindow && (Window.Action == 6 || Window.Action == 16))
            {
                Window.MoveOffSet = new Vector4(0f, 0f, 0f, 0f);
                Window.MoveWindow = false;
                Window.Action = 0;
                if (Window.Local.AutoFixAfterResizing)
                {
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    return;
                }
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant end a ResizeRight function while you haven't started a ResizeRight function or a Grab function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeRightEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000059 RID: 89 RVA: 0x00009114 File Offset: 0x00007314
    public static void ResizeRightTopStart(float aspectRatio)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, Input.mousePosition.y - (float)Screen.height, (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.OldOffSet.z = (float)(Window.Position.y - 1);
                Window.MoveWindow = true;
                Window.Action = 17;
                Window.AspectRation = aspectRatio;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeRightTop function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeRightTopStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600005A RID: 90 RVA: 0x000091FC File Offset: 0x000073FC
    public static void ResizeRightTopStart()
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, Input.mousePosition.y - (float)Screen.height, (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 7;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeRightTop function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeRightTopStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600005B RID: 91 RVA: 0x000092C0 File Offset: 0x000074C0
    public static void ResizeRightTopEnd()
    {
        if (!Application.isEditor)
        {
            if (Window.MoveWindow && (Window.Action == 7 || Window.Action == 17))
            {
                Window.MoveOffSet = new Vector4(0f, 0f, 0f, 0f);
                Window.MoveWindow = false;
                Window.Action = 0;
                if (Window.Local.AutoFixAfterResizing)
                {
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    return;
                }
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant end a ResizeRightTop function while you haven't started a ResizeRightTop function or a Grab function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeRightTopEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600005C RID: 92 RVA: 0x00009418 File Offset: 0x00007618
    public static void ResizeTopStart(float aspectRatio)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, Input.mousePosition.y - (float)Screen.height, (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.OldOffSet.z = (float)Window.Position.y;
                Window.MoveWindow = true;
                Window.Action = 18;
                Window.AspectRation = aspectRatio;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeTop function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeTopStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600005D RID: 93 RVA: 0x000094FC File Offset: 0x000076FC
    public static void ResizeTopStart()
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, Input.mousePosition.y - (float)Screen.height, (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 8;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeTop function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else
        {
            Debug.LogWarning("ResizeTopStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600005E RID: 94 RVA: 0x000095C0 File Offset: 0x000077C0
    public static void ResizeTopEnd()
    {
        if (!Application.isEditor)
        {
            if (Window.MoveWindow && (Window.Action == 8 || Window.Action == 18))
            {
                Window.MoveOffSet = new Vector4(0f, 0f, 0f, 0f);
                Window.MoveWindow = false;
                Window.Action = 0;
                if (Window.Local.AutoFixAfterResizing)
                {
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    return;
                }
            }
            else
            {
                Debug.LogWarning("You cant end a ResizeTop function while you haven't started a ResizeTop function or a Grab function hasn't ended.");
                return;
            }
        }
        else
        {
            Debug.LogWarning("ResizeTop function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600005F RID: 95 RVA: 0x00009718 File Offset: 0x00007918
    public static void ResizeTopLeftStart(float aspectRatio)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4((float)Window.Position.x, Input.mousePosition.y - (float)Screen.height, (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.OldOffSet.z = (float)Window.Position.y;
                Window.MoveWindow = true;
                Window.Action = 19;
                Window.AspectRation = aspectRatio;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeTopLeft function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeTopLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000060 RID: 96 RVA: 0x000097FC File Offset: 0x000079FC
    public static void ResizeTopLeftStart()
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow && Window.Action == 0)
            {
                Window.MoveOffSet = new Vector4(-Input.mousePosition.x, Input.mousePosition.y - (float)Screen.height, (float)Screen.width, (float)Screen.height);
                Window.OldOffSet.x = (float)Cursor.Position.X;
                Window.OldOffSet.y = (float)Cursor.Position.Y;
                Window.MoveWindow = true;
                Window.Action = 9;
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant start a ResizeTopLeft function while another Grab or Resize function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeTopLeftStart function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000061 RID: 97 RVA: 0x000098C0 File Offset: 0x00007AC0
    public static void ResizeTopLeftEnd()
    {
        if (!Application.isEditor)
        {
            if (Window.MoveWindow && (Window.Action == 9 || Window.Action == 19))
            {
                Window.MoveOffSet = new Vector4(0f, 0f, 0f, 0f);
                Window.MoveWindow = false;
                Window.Action = 0;
                if (Window.Local.AutoFixAfterResizing)
                {
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                    Window.SetWindowPosition(Window.ID, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    return;
                }
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("You cant end a ResizeTopLeft function while you haven't started a ResizeTopLeft function or a Grab function hasn't ended.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ResizeTopLeftEnd function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000062 RID: 98 RVA: 0x00009A17 File Offset: 0x00007C17
    public static void SetMinWidth(int Minimum)
    {
        if (!Application.isEditor)
        {
            Window.Limitations.x = (float)Minimum;
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("SetMinWidth function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000063 RID: 99 RVA: 0x00009A43 File Offset: 0x00007C43
    public static void SetMaxWidth(int Maximum)
    {
        if (!Application.isEditor)
        {
            Window.Limitations.y = (float)Maximum;
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("SetMaxWidth function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000064 RID: 100 RVA: 0x00009A6F File Offset: 0x00007C6F
    public static void SetMinHeight(int Minimum)
    {
        if (!Application.isEditor)
        {
            Window.Limitations.z = (float)Minimum;
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("SetMinHeight function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000065 RID: 101 RVA: 0x00009A9B File Offset: 0x00007C9B
    public static void SetMaxHeight(int Maximum)
    {
        if (!Application.isEditor)
        {
            Window.Limitations.w = (float)Maximum;
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("SetMaxHeight function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000066 RID: 102 RVA: 0x00009AC7 File Offset: 0x00007CC7
    public static int GetMinWidth()
    {
        return (int)Window.Limitations.x;
    }

    // Token: 0x06000067 RID: 103 RVA: 0x00009AD4 File Offset: 0x00007CD4
    public static int GetMaxWidth()
    {
        return (int)Window.Limitations.y;
    }

    // Token: 0x06000068 RID: 104 RVA: 0x00009AE1 File Offset: 0x00007CE1
    public static int GetMinHeight()
    {
        return (int)Window.Limitations.z;
    }

    // Token: 0x06000069 RID: 105 RVA: 0x00009AEE File Offset: 0x00007CEE
    public static int GetMaxHeight()
    {
        return (int)Window.Limitations.w;
    }

    // Token: 0x0600006A RID: 106 RVA: 0x00009AFB File Offset: 0x00007CFB
    public static void QuickDisableBorders()
    {
        Window.QuickDisableBorders(Window.ID);
    }

    // Token: 0x0600006B RID: 107 RVA: 0x00009B08 File Offset: 0x00007D08
    public static void QuickDisableBorders(int windowId)
    {
        if (!Application.isEditor)
        {
            Debug.Log(Window.GetPermnentBorderSize());
            if (!Window.IsBorderless())
            {
                if (Window.Local.StabilizeQuickChanges)
                {
                    Window.SetWindowLong((IntPtr)windowId, -16, 524288U);
                    Window.SetWindowPosition(windowId, -2, (int)Window.GetRect().x + (int)Window.GetPermnentBorderSize().x, (int)Window.GetRect().y + (int)Window.GetPermnentBorderSize().y, (int)Window.GetRect().width - (int)Window.GetPermnentBorderSize().x * 2, (int)Window.GetRect().height - (int)Window.GetPermnentBorderSize().y * 2, 64);
                    Window.SetWindowLong((IntPtr)windowId, -16, 524288U);
                    Window.SetWindowPosition(windowId, -2, (int)Window.GetRect().x + (int)Window.GetPermnentBorderSize().x, (int)Window.GetRect().y + (int)Window.GetPermnentBorderSize().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                    return;
                }
                Window.SetWindowLong((IntPtr)windowId, -16, 524288U);
                Window.SetWindowPosition(windowId, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                Window.SetWindowLong((IntPtr)windowId, -16, 524288U);
                Window.SetWindowPosition(windowId, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("QuickDisableBorders function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600006C RID: 108 RVA: 0x00009CFF File Offset: 0x00007EFF
    public static void QuickEnableBorders()
    {
        Window.QuickEnableBorders(Window.ID);
    }

    // Token: 0x0600006D RID: 109 RVA: 0x00009D0C File Offset: 0x00007F0C
    public static void QuickEnableBorders(int windowId)
    {
        if (!Application.isEditor)
        {
            if (Window.Local.StabilizeQuickChanges)
            {
                Window.SetWindowLong((IntPtr)windowId, -16, 349110272U);
                Window.SetWindowPosition(windowId, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width + (int)Window.GetPermnentBorderSize().x * 4, (int)Window.GetRect().height + (int)Window.GetPermnentBorderSize().y * 4, 64);
            }
            else
            {
                Window.SetWindowLong((IntPtr)windowId, -16, 349110272U);
                Window.SetWindowPosition(windowId, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
            }
            Window.SetWindowLong((IntPtr)windowId, -16, 349110272U);
            Window.SetWindowPosition(windowId, -2, (int)Window.GetRect().x, (int)Window.GetRect().y, (int)Window.GetRect().width, (int)Window.GetRect().height, 64);
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("QuickEnableBorders function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x0600006E RID: 110 RVA: 0x00009E63 File Offset: 0x00008063
    public static void Minimize()
    {
        Window.Minimize(Window.ID, true);
    }

    // Token: 0x0600006F RID: 111 RVA: 0x00009E70 File Offset: 0x00008070
    public static void Minimize(bool TurnAutoBorderlessOff)
    {
        Window.Minimize(Window.ID, TurnAutoBorderlessOff);
    }

    // Token: 0x06000070 RID: 112 RVA: 0x00009E7D File Offset: 0x0000807D
    public static void Minimize(int windowId)
    {
        Window.Minimize(windowId, true);
    }

    // Token: 0x06000071 RID: 113 RVA: 0x00009E88 File Offset: 0x00008088
    public static void Minimize(int windowId, bool TurnAutoBorderlessOff)
    {
        if (!Application.isEditor)
        {
            Window.Local.QuickAutoBorderless = !TurnAutoBorderlessOff;
            if (!Window.MoveWindow || Window.ID != windowId)
            {
                Window.ShowWindow(windowId, 2);
                for (int i = 0; i < Window.ChildId.Length; i++)
                {
                    Window.SetWindowPosition(Window.ChildId[i], -2, 0, 0, 0, 0, 64);
                }
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("Minimize function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("Minimize function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000072 RID: 114 RVA: 0x00009F15 File Offset: 0x00008115
    public static void Fullscreen()
    {
        Window.Fullscreen(Screen.width, Screen.height);
    }

    // Token: 0x06000073 RID: 115 RVA: 0x00009F26 File Offset: 0x00008126
    public static void Fullscreen(Vector2 Quality)
    {
        Window.Fullscreen((int)Quality.x, (int)Quality.y);
    }

    // Token: 0x06000074 RID: 116 RVA: 0x00009F3C File Offset: 0x0000813C
    public static void Fullscreen(int Width, int Height)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow)
            {
                Screen.SetResolution(Width, Height, !Screen.fullScreen);
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("Fullscreen function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("Fullscreen function doesnt work in the editor.");
        }
    }

    // Token: 0x06000075 RID: 117 RVA: 0x00009F94 File Offset: 0x00008194
    public static IEnumerator StoreMaximized(int windowId, bool IgnoreLimitations)
    {
        yield return 0;
        Window.Maximized = Window.GetRect();
        if (!IgnoreLimitations)
        {
            Window.ShowWindow(windowId, 1);
            yield return 0;
            yield return 0;
            Window.SetWindowPos(windowId, 0, (int)(Window.Maximized.x + Window.Maximized.width / 2f - (float)((int)(Window.Limitations.y / 2f))), (int)(Window.Maximized.y + Window.Maximized.height / 2f - (float)((int)(Window.Limitations.w / 2f)) + 1f), (int)Window.Limitations.y, (int)Window.Limitations.w, 96);
            yield return 0;
        }
        yield break;
    }

    // Token: 0x06000076 RID: 118 RVA: 0x00009FAA File Offset: 0x000081AA
    public static void UnMaximize()
    {
        Window.UnMaximize(Window.ID);
    }

    // Token: 0x06000077 RID: 119 RVA: 0x00009FB8 File Offset: 0x000081B8
    public static void UnMaximize(int windowId)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow || Window.ID != windowId)
            {
                Window.ShowWindow(windowId, 1);
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("UnMaximize function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("UnMaximize function is not recommended to be called within the editor, please use it only in the standlone build.");
        }
    }

    // Token: 0x06000078 RID: 120 RVA: 0x0000A011 File Offset: 0x00008211
    public static void Maximize()
    {
        Window.Maximize(Window.ID, true);
    }

    // Token: 0x06000079 RID: 121 RVA: 0x0000A01E File Offset: 0x0000821E
    public static void Maximize(bool IgnoreLimitations)
    {
        Window.Maximize(Window.ID, IgnoreLimitations);
    }

    // Token: 0x0600007A RID: 122 RVA: 0x0000A02B File Offset: 0x0000822B
    public static void Maximize(int windowId)
    {
        Window.Maximize(windowId, true);
    }

    // Token: 0x0600007B RID: 123 RVA: 0x0000A034 File Offset: 0x00008234
    public static void Maximize(int windowId, bool IgnoreLimitations)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow || Window.ID != windowId)
            {
                Window.ShowWindow(windowId, 3);
                Window.Local.StartCoroutine(Window.StoreMaximized(windowId, IgnoreLimitations));
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("Maximize function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("Maximize function is not designed to work in the editor.");
        }
    }

    // Token: 0x0600007C RID: 124 RVA: 0x0000A09F File Offset: 0x0000829F
    public static bool IsMaximized()
    {
        return Window.GetRect() == Window.Maximized;
    }

    // Token: 0x0600007D RID: 125 RVA: 0x0000A0B0 File Offset: 0x000082B0
    public static void Exit()
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow)
            {
                Application.Quit();
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("Exit function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("Exit function is not designed to work in the editor.");
        }
    }

    // Token: 0x0600007E RID: 126 RVA: 0x0000A0FE File Offset: 0x000082FE
    public static void ForceExit()
    {
        Window.ForceExit(Window.ID);
    }

    // Token: 0x0600007F RID: 127 RVA: 0x0000A10C File Offset: 0x0000830C
    public static void ForceExit(int windowId)
    {
        if (!Application.isEditor)
        {
            try
            {
                Process.GetProcessById(Window.PidByHwnd(windowId)).Kill();
                return;
            }
            catch (Exception ex)
            {
                Debug.LogError("Could not kill process, reason : " + ex.ToString());
                return;
            }
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("ForceExit function is not recommended to be called within the editor because it can cause data loss.");
        }
    }

    // Token: 0x06000080 RID: 128 RVA: 0x0000A170 File Offset: 0x00008370
    private void OnApplicationQuit()
    {
        for (int i = 0; i < Window.ChildId.Length; i++)
        {
            Window.ForceExit(Window.ChildId[i]);
        }
        if (this.AllowSizeResettingBeforeExit)
        {
            PlayerPrefs.SetInt("Screenmanager Resolution Width", (int)this.SizeReset.x);
            PlayerPrefs.SetInt("Screenmanager Resolution Height", (int)this.SizeReset.y);
        }
    }

    // Token: 0x06000081 RID: 129 RVA: 0x0000A1CF File Offset: 0x000083CF
    public static void FlashEnd()
    {
        Window.FlashEnd(Window.ID);
    }

    // Token: 0x06000082 RID: 130 RVA: 0x0000A1DC File Offset: 0x000083DC
    public static void FlashEnd(int WindowId)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow)
            {
                Window.FlashWindow(WindowId, 0, 0, 0);
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("FlashEnd function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("FlashEnd function is not designed to work in the editor.");
        }
    }

    // Token: 0x06000083 RID: 131 RVA: 0x0000A22F File Offset: 0x0000842F
    public static void FlashPause()
    {
        Window.FlashPause(Window.ID);
    }

    // Token: 0x06000084 RID: 132 RVA: 0x0000A23C File Offset: 0x0000843C
    public static void FlashPause(int WindowId)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow)
            {
                Window.FlashWindow(WindowId, 0, int.MaxValue, 0);
                return;
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("FlashPause function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("FlashPause function is not designed to work in the editor.");
        }
    }

    // Token: 0x06000085 RID: 133 RVA: 0x0000A293 File Offset: 0x00008493
    public static void FlashStart()
    {
        Window.FlashStart(Window.ID, 0f, int.MaxValue, "");
    }

    // Token: 0x06000086 RID: 134 RVA: 0x0000A2AE File Offset: 0x000084AE
    public static void FlashStart(float MilisecSpeed)
    {
        Window.FlashStart(Window.ID, MilisecSpeed, int.MaxValue, "");
    }

    // Token: 0x06000087 RID: 135 RVA: 0x0000A2C5 File Offset: 0x000084C5
    public static void FlashStart(int FlashTimes)
    {
        Window.FlashStart(Window.ID, 0f, FlashTimes, "");
    }

    // Token: 0x06000088 RID: 136 RVA: 0x0000A2DC File Offset: 0x000084DC
    public static void FlashStart(float MilisecSpeed, int FlashTimes)
    {
        Window.FlashStart(Window.ID, MilisecSpeed, FlashTimes, "");
    }

    // Token: 0x06000089 RID: 137 RVA: 0x0000A2EF File Offset: 0x000084EF
    public static void FlashStart(int WindowId, float MilisecSpeed)
    {
        Window.FlashStart(WindowId, MilisecSpeed, int.MaxValue, "");
    }

    // Token: 0x0600008A RID: 138 RVA: 0x0000A302 File Offset: 0x00008502
    public static void FlashStart(int WindowId, float MilisecSpeed, int FlashTimes)
    {
        Window.FlashStart(WindowId, MilisecSpeed, FlashTimes, "");
    }

    // Token: 0x0600008B RID: 139 RVA: 0x0000A311 File Offset: 0x00008511
    public static void FlashStart(string Mode)
    {
        Window.FlashStart(Window.ID, 0f, int.MaxValue, Mode);
    }

    // Token: 0x0600008C RID: 140 RVA: 0x0000A328 File Offset: 0x00008528
    public static void FlashStart(float MilisecSpeed, string Mode)
    {
        Window.FlashStart(Window.ID, MilisecSpeed, int.MaxValue, Mode);
    }

    // Token: 0x0600008D RID: 141 RVA: 0x0000A33B File Offset: 0x0000853B
    public static void FlashStart(int FlashTimes, string Mode)
    {
        Window.FlashStart(Window.ID, 0f, FlashTimes, Mode);
    }

    // Token: 0x0600008E RID: 142 RVA: 0x0000A34E File Offset: 0x0000854E
    public static void FlashStart(float MilisecSpeed, int FlashTimes, string Mode)
    {
        Window.FlashStart(Window.ID, MilisecSpeed, FlashTimes, Mode);
    }

    // Token: 0x0600008F RID: 143 RVA: 0x0000A35D File Offset: 0x0000855D
    public static void FlashStart(int WindowId, float MilisecSpeed, string Mode)
    {
        Window.FlashStart(WindowId, MilisecSpeed, int.MaxValue, Mode);
    }

    // Token: 0x06000090 RID: 144 RVA: 0x0000A36C File Offset: 0x0000856C
    public static void FlashStart(int WindowId, float MilisecSpeed, int FlashTimes, string Mode)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow)
            {
                if (Mode == "Taskbar")
                {
                    Window.FlashWindow(WindowId, 2, FlashTimes, (int)MilisecSpeed);
                    return;
                }
                if (Mode == "Caption")
                {
                    Window.FlashWindow(WindowId, 1, FlashTimes, (int)MilisecSpeed);
                    return;
                }
                Window.FlashWindow(WindowId, 3, FlashTimes, (int)MilisecSpeed);
                return;
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("FlashStart function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("FlashStart function is not designed to work in the editor.");
        }
    }

    // Token: 0x06000091 RID: 145 RVA: 0x0000A3F2 File Offset: 0x000085F2
    public static void FlashUntilFocus()
    {
        Window.FlashUntilFocus(Window.ID, 0f, int.MaxValue, "");
    }

    // Token: 0x06000092 RID: 146 RVA: 0x0000A40D File Offset: 0x0000860D
    public static void FlashUntilFocus(float MilisecSpeed)
    {
        Window.FlashUntilFocus(Window.ID, MilisecSpeed, int.MaxValue, "");
    }

    // Token: 0x06000093 RID: 147 RVA: 0x0000A424 File Offset: 0x00008624
    public static void FlashUntilFocus(int FlashTimes)
    {
        Window.FlashUntilFocus(Window.ID, 0f, FlashTimes, "");
    }

    // Token: 0x06000094 RID: 148 RVA: 0x0000A43B File Offset: 0x0000863B
    public static void FlashUntilFocus(float MilisecSpeed, int FlashTimes)
    {
        Window.FlashUntilFocus(Window.ID, MilisecSpeed, FlashTimes, "");
    }

    // Token: 0x06000095 RID: 149 RVA: 0x0000A44E File Offset: 0x0000864E
    public static void FlashUntilFocus(int WindowId, float MilisecSpeed)
    {
        Window.FlashUntilFocus(WindowId, MilisecSpeed, int.MaxValue, "");
    }

    // Token: 0x06000096 RID: 150 RVA: 0x0000A461 File Offset: 0x00008661
    public static void FlashUntilFocus(int WindowId, float MilisecSpeed, int FlashTimes)
    {
        Window.FlashUntilFocus(WindowId, MilisecSpeed, FlashTimes, "");
    }

    // Token: 0x06000097 RID: 151 RVA: 0x0000A470 File Offset: 0x00008670
    public static void FlashUntilFocus(string Mode)
    {
        Window.FlashUntilFocus(Window.ID, 0f, int.MaxValue, Mode);
    }

    // Token: 0x06000098 RID: 152 RVA: 0x0000A487 File Offset: 0x00008687
    public static void FlashUntilFocus(float MilisecSpeed, string Mode)
    {
        Window.FlashUntilFocus(Window.ID, MilisecSpeed, int.MaxValue, Mode);
    }

    // Token: 0x06000099 RID: 153 RVA: 0x0000A49A File Offset: 0x0000869A
    public static void FlashUntilFocus(int FlashTimes, string Mode)
    {
        Window.FlashUntilFocus(Window.ID, 0f, FlashTimes, Mode);
    }

    // Token: 0x0600009A RID: 154 RVA: 0x0000A4AD File Offset: 0x000086AD
    public static void FlashUntilFocus(float MilisecSpeed, int FlashTimes, string Mode)
    {
        Window.FlashUntilFocus(Window.ID, MilisecSpeed, FlashTimes, Mode);
    }

    // Token: 0x0600009B RID: 155 RVA: 0x0000A4BC File Offset: 0x000086BC
    public static void FlashUntilFocus(int WindowId, float MilisecSpeed, string Mode)
    {
        Window.FlashUntilFocus(WindowId, MilisecSpeed, int.MaxValue, Mode);
    }

    // Token: 0x0600009C RID: 156 RVA: 0x0000A4CC File Offset: 0x000086CC
    public static void FlashUntilFocus(int WindowId, float MilisecSpeed, int FlashTimes, string Mode)
    {
        if (!Application.isEditor)
        {
            if (!Window.MoveWindow)
            {
                if (Mode == "Taskbar")
                {
                    Window.FlashWindow(WindowId, 15, FlashTimes, (int)MilisecSpeed);
                    return;
                }
                if (Mode == "Caption")
                {
                    Window.FlashWindow(WindowId, 13, FlashTimes, (int)MilisecSpeed);
                    return;
                }
                Window.FlashWindow(WindowId, 15, FlashTimes, (int)MilisecSpeed);
                return;
            }
            else if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("FlashUntilFocus function cant be called while GrabStart has been called and GrabEnd hasn't.");
                return;
            }
        }
        else if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("FlashUntilFocus function is not designed to work in the editor.");
        }
    }

    // Token: 0x0600009D RID: 157 RVA: 0x0000A555 File Offset: 0x00008755
    public static string GetTitle()
    {
        return Window.GetTitle(Window.ID);
    }

    // Token: 0x0600009E RID: 158 RVA: 0x0000A564 File Offset: 0x00008764
    public static string GetTitle(int windowId)
    {
        StringBuilder stringBuilder = new StringBuilder(256);
        if (!Application.isEditor)
        {
            if (Window.GetWindowText(windowId, stringBuilder, 256) > 0L)
            {
                return stringBuilder.ToString();
            }
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("GetTitle function should not be called in the editor.");
            }
        }
        return null;
    }

    // Token: 0x0600009F RID: 159 RVA: 0x0000A5B1 File Offset: 0x000087B1
    public static void SetTitle(string newTitle)
    {
        Window.SetTitle(Window.ID, newTitle);
    }

    // Token: 0x060000A0 RID: 160 RVA: 0x0000A5BE File Offset: 0x000087BE
    public static void SetTitle(int windowId, string newTitle)
    {
        if (!Application.isEditor)
        {
            Window.SetWindowText(Window.ID, newTitle);
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("SetTitle function should not be called in the editor.");
        }
    }

    // Token: 0x060000A1 RID: 161 RVA: 0x0000A5EA File Offset: 0x000087EA
    public static void SetOwner(int ownerId)
    {
        Window.SetOwner(Window.ID, ownerId);
    }

    // Token: 0x060000A2 RID: 162 RVA: 0x0000A5F7 File Offset: 0x000087F7
    public static void SetOwner(int windowId, int ownerId)
    {
        if (!Application.isEditor)
        {
            Window.SetWindowLong((IntPtr)windowId, -8, (uint)ownerId);
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("SetOwner function should not be called in the editor.");
        }
    }

    // Token: 0x060000A3 RID: 163 RVA: 0x0000A626 File Offset: 0x00008826
    public static void SetChild(int childId)
    {
        Window.SetChild(childId, Window.ID);
    }

    // Token: 0x060000A4 RID: 164 RVA: 0x0000A633 File Offset: 0x00008833
    public static void SetChild(int childId, int parentId)
    {
        if (!Application.isEditor)
        {
            Window.SetParent(childId, parentId);
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("SetChild function should not be called in the editor.");
        }
    }

    // Token: 0x060000A5 RID: 165 RVA: 0x0000A65B File Offset: 0x0000885B
    public static void Hide()
    {
        Window.Hide(Window.ID);
    }

    // Token: 0x060000A6 RID: 166 RVA: 0x0000A667 File Offset: 0x00008867
    public static void Hide(int windowId)
    {
        if (!Application.isEditor)
        {
            Window.ShowWindow(windowId, 0);
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("Hide function should not be called in the editor.");
        }
    }

    // Token: 0x060000A7 RID: 167 RVA: 0x0000A68F File Offset: 0x0000888F
    public static void Show()
    {
        Window.Show(Window.ID);
    }

    // Token: 0x060000A8 RID: 168 RVA: 0x0000A69B File Offset: 0x0000889B
    public static void Show(int windowId)
    {
        if (!Application.isEditor)
        {
            Window.ShowWindow(windowId, 5);
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("Show function should not be called in the editor.");
        }
    }

    // Token: 0x060000A9 RID: 169 RVA: 0x0000A6C3 File Offset: 0x000088C3
    public static int FindWindowByName(string windowName)
    {
        return (int)Window.FindWindowByCaption(IntPtr.Zero, windowName);
    }

    // Token: 0x060000AA RID: 170 RVA: 0x0000A6D5 File Offset: 0x000088D5
    public static void AboveRendering()
    {
        Window.AboveRendering(Window.ID, true);
    }

    // Token: 0x060000AB RID: 171 RVA: 0x0000A6E2 File Offset: 0x000088E2
    public static void AboveRendering(bool Active)
    {
        Window.AboveRendering(Window.ID, Active);
    }

    // Token: 0x060000AC RID: 172 RVA: 0x0000A6EF File Offset: 0x000088EF
    public static void AboveRendering(int windowId)
    {
        Window.AboveRendering(windowId, true);
    }

    // Token: 0x060000AD RID: 173 RVA: 0x0000A6F8 File Offset: 0x000088F8
    public static void AboveRendering(int windowId, bool Active)
    {
        if (Application.isEditor)
        {
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("AboveRendering function should not be called in the editor.");
            }
            return;
        }
        if (Active)
        {
            Window.SetWindowPosition(windowId, -1, 0, 0, 0, 0, 3);
            return;
        }
        Window.SetWindowPosition(windowId, -2, 0, 0, 0, 0, 3);
    }

    // Token: 0x060000AE RID: 174 RVA: 0x0000A735 File Offset: 0x00008935
    public static void Clickthrough(bool Active)
    {
        Window.Clickthrough(Window.ID, Active);
    }

    // Token: 0x060000AF RID: 175 RVA: 0x0000A744 File Offset: 0x00008944
    public static void Clickthrough(int windowId, bool Active)
    {
        if (Application.isEditor)
        {
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("Clickthrough function should not be called in the editor.");
            }
            return;
        }
        if (Active)
        {
            Window.SetFocus(windowId);
            Window.SetWindowLong((IntPtr)windowId, -20, 2148007968U);
            return;
        }
        Window.SetWindowLong((IntPtr)windowId, -20, 2415919104U);
    }

    // Token: 0x060000B0 RID: 176 RVA: 0x0000A7A0 File Offset: 0x000089A0
    public static void BootByName(string Location)
    {
        Window.BootByName(Location, "");
    }

    // Token: 0x060000B1 RID: 177 RVA: 0x0000A7B0 File Offset: 0x000089B0
    public static void BootByName(string Location, string Parameters)
    {
        if (!Application.isEditor)
        {
            try
            {
                Process.Start(Regex.Replace(Location, "/", "\\"), Parameters);
                return;
            }
            catch (Exception ex)
            {
                Debug.LogError("Could not boot process, reason : " + ex.ToString());
                return;
            }
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("BootByName function should not be called in the editor.");
        }
    }

    // Token: 0x060000B2 RID: 178 RVA: 0x0000A81C File Offset: 0x00008A1C
    public static int ProcessIdByName(string Name)
    {
        Process[] processesByName = Process.GetProcessesByName(Name);
        if (processesByName.Length > 1)
        {
            Debug.LogWarning("More than one process with name : " + Name + " found.");
        }
        else if (processesByName.Length == 0)
        {
            Debug.LogError("No processes with name : " + Name + " found.");
            return 0;
        }
        return Window.HwndByPid(processesByName[0].Id);
    }

    // Token: 0x060000B3 RID: 179 RVA: 0x0000A878 File Offset: 0x00008A78
    public static string ProcessNameById(int windowId)
    {
        Process[] processes = Process.GetProcesses();
        for (int i = 0; i < processes.Length; i++)
        {
            try
            {
                if (Window.HwndByPid(processes[i].Id) == windowId)
                {
                    return processes[i].ProcessName;
                }
            }
            catch
            {
            }
        }
        return "Error-Not-Found";
    }

    // Token: 0x060000B4 RID: 180 RVA: 0x0000A8D4 File Offset: 0x00008AD4
    internal static int HwndByPid(int processId)
    {
        int value = 0;
        IntPtr intPtr;
        for (; ; )
        {
            IntPtr desktopWindow = Window.GetDesktopWindow();
            if (desktopWindow == IntPtr.Zero)
            {
                return 0;
            }
            intPtr = Window.FindWindowEx(desktopWindow, (IntPtr)value, null, null);
            if ((int)intPtr == 0)
            {
                return 0;
            }
            uint num = 0U;
            Window.GetWindowThreadProcessId(intPtr, out num);
            if ((ulong)num == (ulong)((long)processId) && Window.IsWindowVisible(intPtr) && (int)Window.GetParent(intPtr) == 0)
            {
                break;
            }
            value = (int)intPtr;
        }
        return (int)intPtr;
    }

    // Token: 0x060000B5 RID: 181 RVA: 0x0000A944 File Offset: 0x00008B44
    internal static int PidByHwnd(int windowId)
    {
        uint result = 0U;
        Window.GetWindowThreadProcessId((IntPtr)windowId, out result);
        return (int)result;
    }

    // Token: 0x060000B6 RID: 182 RVA: 0x0000A964 File Offset: 0x00008B64
    public static void SetWindowPosition(int hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags)
    {
        if (!Application.isEditor)
        {
            if (hwnd == Window.ID && Window.ChildId.Length != 0 && Window.DoneCalculating)
            {
                for (int i = 0; i < Window.ChildId.Length; i++)
                {
                    Window.SetWindowPos(Window.ChildId[i], Window.ID, (int)((float)x + Window.ChildOffset[i].x), (int)((float)y + Window.ChildOffset[i].y), (int)((float)cx + Window.ChildOffset[i].width), (int)((float)cy + Window.ChildOffset[i].height), 16 | uFlags);
                }
            }
            if (Window.IsMaximized())
            {
                Window.ShowWindow(hwnd, 1);
            }
            Window.SetWindowPos(hwnd, hwndInsertAfter, x, y, cx, cy, uFlags);
            Window.Maximized = new Rect(0f, 0f, (float)Screen.currentResolution.width, (float)Screen.currentResolution.height);
        }
    }

    // Token: 0x060000B7 RID: 183 RVA: 0x0000AA64 File Offset: 0x00008C64
    public static void AddSyncChild(int windowId, Rect offset)
    {
        if (!Application.isEditor)
        {
            int[] array = new int[Window.ChildId.Length + 1];
            Rect[] array2 = new Rect[Window.ChildId.Length + 1];
            for (int i = 0; i < Window.ChildId.Length; i++)
            {
                array[i] = Window.ChildId[i];
                array2[i] = Window.ChildOffset[i];
            }
            Window.ChildId = new int[Window.ChildId.Length + 1];
            Window.ChildOffset = new Rect[Window.ChildId.Length + 1];
            for (int j = 0; j < Window.ChildId.Length - 1; j++)
            {
                Window.ChildId[j] = array[j];
                Window.ChildOffset[j] = array2[j];
            }
            Window.ChildId[Window.ChildId.Length - 1] = windowId;
            Window.ChildOffset[Window.ChildId.Length - 1] = offset;
            return;
        }
        if (!Window.Local.SilenceWarnings)
        {
            Debug.LogWarning("AddSyncChild function should not be called in the editor.");
        }
    }

    // Token: 0x060000B8 RID: 184 RVA: 0x0000AB58 File Offset: 0x00008D58
    public static void EditSyncChild(int windowId, Rect offset)
    {
        if (Application.isEditor)
        {
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("EditSyncChild function should not be called in the editor.");
            }
            return;
        }
        if (Window.ChildId.Length < windowId)
        {
            Window.ChildOffset[windowId] = offset;
            return;
        }
        Debug.LogError("No sync child with such big id exists.");
    }

    // Token: 0x060000B9 RID: 185 RVA: 0x0000ABA4 File Offset: 0x00008DA4
    public static void RemoveAllSyncChilds()
    {
        Window.ChildId = new int[0];
        Window.ChildOffset = new Rect[0];
    }

    // Token: 0x060000BA RID: 186 RVA: 0x0000ABBC File Offset: 0x00008DBC
    public static void RemoveLastSyncChild()
    {
        if (Application.isEditor)
        {
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("RemoveLastSyncChild function should not be called in the editor.");
            }
            return;
        }
        if (Window.ChildId.Length != 0)
        {
            int[] array = new int[Window.ChildId.Length - 1];
            Rect[] array2 = new Rect[Window.ChildId.Length - 1];
            for (int i = 0; i < Window.ChildId.Length - 1; i++)
            {
                array[i] = Window.ChildId[i];
                array2[i] = Window.ChildOffset[i];
            }
            Window.ChildId = new int[Window.ChildId.Length - 1];
            Window.ChildOffset = new Rect[Window.ChildId.Length - 1];
            for (int j = 0; j < Window.ChildId.Length - 1; j++)
            {
                Window.ChildId[j] = array[j];
                Window.ChildOffset[j] = array2[j];
            }
            return;
        }
        Debug.LogError("RemoveLastSyncChild failed because there are no more childs to remove.");
    }

    // Token: 0x060000BB RID: 187 RVA: 0x0000ACA4 File Offset: 0x00008EA4
    public static void RemoveSyncChild(int windowId)
    {
        if (Application.isEditor)
        {
            if (!Window.Local.SilenceWarnings)
            {
                Debug.LogWarning("RemoveSyncChild function should not be called in the editor.");
            }
            return;
        }
        bool flag = false;
        for (int i = 0; i < Window.ChildId.Length; i++)
        {
            if (Window.ChildId[i] == windowId)
            {
                Window.ChildId[i] = Window.ChildId[Window.ChildId.Length - 1];
                Window.ChildOffset[i] = Window.ChildOffset[Window.ChildId.Length - 1];
                flag = true;
                break;
            }
        }
        if (flag)
        {
            Window.RemoveLastSyncChild();
            return;
        }
        Debug.LogError("RemoveSyncChild failed because no child with id : " + windowId + " exists.");
    }

    // Token: 0x060000BC RID: 188 RVA: 0x0000AD48 File Offset: 0x00008F48
    public static int GetCurrentActive()
    {
        return Window.GetActiveWindow();
    }

    // Token: 0x060000BD RID: 189 RVA: 0x0000AD4F File Offset: 0x00008F4F
    public static void SetActive()
    {
        Window.SetActive(Window.ID);
    }

    // Token: 0x060000BE RID: 190 RVA: 0x0000AD5B File Offset: 0x00008F5B
    public static void SetActive(int windowId)
    {
        Window.SetActiveWindow((IntPtr)windowId);
    }

    // Token: 0x060000BF RID: 191 RVA: 0x0000AD69 File Offset: 0x00008F69
    public static void SetForeground()
    {
        Window.SetForeground(Window.ID);
    }

    // Token: 0x060000C0 RID: 192 RVA: 0x0000AD75 File Offset: 0x00008F75
    public static void SetForeground(int windowId)
    {
        Window.SetForegroundWindow((IntPtr)windowId);
    }

    // Token: 0x060000C1 RID: 193 RVA: 0x0000AD83 File Offset: 0x00008F83
    public static void HideAltTab()
    {
        Window.HideAltTab(Window.ID);
    }

    // Token: 0x060000C2 RID: 194 RVA: 0x0000AD8F File Offset: 0x00008F8F
    public static void HideAltTab(int windowId)
    {
        Window.SetWindowLong((IntPtr)windowId, -20, (uint)(Window.GetWindowLong((IntPtr)windowId, -20) | 128L));
    }

    // Token: 0x060000C3 RID: 195 RVA: 0x0000ADB4 File Offset: 0x00008FB4
    public static bool IsMinimized()
    {
        return Window.IsMinimized(Window.ID);
    }

    // Token: 0x060000C4 RID: 196 RVA: 0x0000ADC0 File Offset: 0x00008FC0
    public static bool IsMinimized(int windowId)
    {
        return Window.IsIconic((IntPtr)windowId);
    }

    // Token: 0x060000C5 RID: 197 RVA: 0x0000ADD0 File Offset: 0x00008FD0
    public static bool OnMonitorResolutionChanged()
    {
        if (!Application.isEditor && (Screen.currentResolution.width != Window.LastResolution.width || Screen.currentResolution.height != Window.LastResolution.height))
        {
            if (Window.Local.AutoFixAfterResizing)
            {
                Window.Loop = 10;
            }
            Window.LastResolution = Screen.currentResolution;
            return true;
        }
        if (Window.Loop <= 0)
        {
            Window.StoredRect = Window.GetRect();
            Window.StoredBorders = Window.Borders;
            Window.StoredBorderSize = Window.GetBorderSize();
        }
        else if (!Window.StoredBorders)
        {
            if (Screen.height != Window.Position.w - Window.Position.y)
            {
                Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                Window.SetWindowPosition(Window.ID, -2, (int)Window.StoredRect.x, (int)Window.StoredRect.y, (int)Window.StoredRect.width, (int)Window.StoredRect.height, 64);
                Window.SetWindowLong((IntPtr)Window.ID, -16, 524288U);
                Window.SetWindowPosition(Window.ID, -2, (int)Window.StoredRect.x, (int)Window.StoredRect.y, (int)Window.StoredRect.width, (int)Window.StoredRect.height, 64);
                Window.Loop--;
            }
            else
            {
                Window.Loop = 0;
            }
        }
        else if (Window.GetRect().width != Window.StoredRect.width || Window.GetRect().height != Window.StoredRect.height)
        {
            Window.SetWindowPosition(Window.ID, 0, (int)Window.StoredRect.x, (int)Window.StoredRect.y, (int)(Window.StoredRect.width + Window.StoredBorderSize.x * 2f), (int)(Window.StoredRect.height + Window.StoredBorderSize.y * 2f), 96);
            Window.Loop--;
        }
        else
        {
            Window.Loop = 0;
        }
        return false;
    }

    // Token: 0x060000C6 RID: 198 RVA: 0x0000AFE9 File Offset: 0x000091E9
    public static int ReturnParent(int ChildId)
    {
        return (int)Window.GetParent((IntPtr)ChildId);
    }

    // Token: 0x060000C7 RID: 199 RVA: 0x0000AFFB File Offset: 0x000091FB
    public static int Find(string ClassName, string WindowName)
    {
        return (int)Window.FindWindow(ClassName, WindowName);
    }

    // Token: 0x060000C8 RID: 200 RVA: 0x0000B009 File Offset: 0x00009209
    public static int FindChild(int ParentId, int NextChild, string ClassName, string WindowName)
    {
        return (int)Window.FindWindowEx((IntPtr)ParentId, (IntPtr)NextChild, ClassName, WindowName);
    }

    // Token: 0x04000012 RID: 18
    public bool QuickAutoBorderless = true;

    // Token: 0x04000013 RID: 19
    public bool FullyAutoBorderless;

    // Token: 0x04000014 RID: 20
    public bool SilenceWarnings;

    // Token: 0x04000015 RID: 21
    public bool AutoFixAfterResizing = true;

    // Token: 0x04000016 RID: 22
    public bool FocusResetOnClick;

    // Token: 0x04000017 RID: 23
    public bool StabilizeQuickChanges = true;

    // Token: 0x04000018 RID: 24
    public bool AllowSizeResettingBeforeExit;

    // Token: 0x04000019 RID: 25
    public bool CrossSceneSupport = true;

    // Token: 0x0400001A RID: 26
    public Vector2 SizeReset = new Vector2(120f, 90f);

    // Token: 0x0400001B RID: 27
    public static bool DoneCalculating;

    // Token: 0x0400001C RID: 28
    public static Window Local;

    // Token: 0x0400001D RID: 29
    public static Vector4 Limitations = new Vector4(0f, 4096f, 0f, 4096f);

    // Token: 0x0400001E RID: 30
    public static Window.RECT Position;

    // Token: 0x0400001F RID: 31
    public static bool MoveWindow;

    // Token: 0x04000020 RID: 32
    public static int ID;

    // Token: 0x04000021 RID: 33
    public static int[] ChildId = new int[0];

    // Token: 0x04000022 RID: 34
    public static Rect[] ChildOffset = new Rect[0];

    // Token: 0x04000023 RID: 35
    public static Rect Maximized;

    // Token: 0x04000024 RID: 36
    private static bool Borders;

    // Token: 0x04000025 RID: 37
    private static Vector4 MoveOffSet;

    // Token: 0x04000026 RID: 38
    private static Vector3 OldOffSet;

    // Token: 0x04000027 RID: 39
    private static int Action;

    // Token: 0x04000028 RID: 40
    private static Vector2 CursorUpdate = Vector3.zero;

    // Token: 0x04000029 RID: 41
    private static Vector2 ClientPosition;

    // Token: 0x0400002A RID: 42
    private static float AspectRation;

    // Token: 0x0400002B RID: 43
    private static Rect ResetSize;

    // Token: 0x0400002C RID: 44
    private static Resolution LastResolution;

    // Token: 0x0400002D RID: 45
    private static int Loop;

    // Token: 0x0400002E RID: 46
    private static Rect StoredRect;

    // Token: 0x0400002F RID: 47
    private static bool StoredBorders;

    // Token: 0x04000030 RID: 48
    private static Vector2 StoredBorderSize;

    // Token: 0x04000031 RID: 49
    private static Vector2 PermanentBorderSize;

    // Token: 0x04000032 RID: 50
    private bool FocusReset = true;

    // Token: 0x04000033 RID: 51
    private bool MinimizeReset = true;

    // Token: 0x04000034 RID: 52
    private bool Once;

    // Token: 0x04000035 RID: 53
    private bool Once1;

    // Token: 0x02000006 RID: 6
    public struct RECT
    {
        // Token: 0x04000036 RID: 54
        public int x;

        // Token: 0x04000037 RID: 55
        public int y;

        // Token: 0x04000038 RID: 56
        public int z;

        // Token: 0x04000039 RID: 57
        public int w;
    }

    // Token: 0x02000007 RID: 7
    private struct Flash
    {
        // Token: 0x0400003A RID: 58
        public uint a;

        // Token: 0x0400003B RID: 59
        public IntPtr b;

        // Token: 0x0400003C RID: 60
        public uint c;

        // Token: 0x0400003D RID: 61
        public uint d;

        // Token: 0x0400003E RID: 62
        public uint e;
    }
}
