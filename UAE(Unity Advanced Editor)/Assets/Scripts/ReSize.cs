using System.Windows.Forms;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;
using Screen = UnityEngine.Screen;

public class ReSize : MonoBehaviour
{
    public Texture2D HorizontalCursor;
    public Texture2D VerticalCursor;
    public Texture2D DiagonalRightCursor;
    public Texture2D DiagonalLeftCursor;
    public int CursorID;

    public bool allowedSizeChange = false;

    public void OnGUI()
    {
        if(allowedSizeChange == true)
        {
            if (new Rect(0f, 30f, 15f, Screen.height - 45).Contains(Event.current.mousePosition))
            {
                Cursor.SetCursor(HorizontalCursor, new Vector2(15f, 15f), CursorMode.Auto);
                CursorID = 1;
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    Window.ResizeLeftStart();
                }
            }
            else if (CursorID == 1)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            if ((Event.current.type == EventType.MouseUp && Event.current.button == 0) || (Window.MoveWindow && !Input.GetKey(KeyCode.Mouse0)))
            {
                Window.ResizeLeftEnd();
            }
            if (new Rect(15f, Screen.height - 15, Screen.width - 30, 15f).Contains(Event.current.mousePosition))
            {
                Cursor.SetCursor(VerticalCursor, new Vector2(15f, 15f), CursorMode.Auto);
                CursorID = 3;
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    Window.ResizeDownStart();
                }
            }
            else if (CursorID == 3)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            if ((Event.current.type == EventType.MouseUp && Event.current.button == 0) || (Window.MoveWindow && !Input.GetKey(KeyCode.Mouse0)))
            {
                Window.ResizeDownEnd();
            }
            if (new Rect(Screen.width - 15, 30f, 15f, Screen.height - 45).Contains(Event.current.mousePosition))
            {
                Cursor.SetCursor(HorizontalCursor, new Vector2(15f, 15f), CursorMode.Auto);
                CursorID = 5;
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    Window.ResizeRightStart();
                }
            }
            else if (CursorID == 5)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            if ((Event.current.type == EventType.MouseUp && Event.current.button == 0) || (Window.MoveWindow && !Input.GetKey(KeyCode.Mouse0)))
            {
                Window.ResizeRightEnd();
            }
            if (new Rect(Screen.width - 15, Screen.height - 15, 15f, 15f).Contains(Event.current.mousePosition))
            {
                Cursor.SetCursor(DiagonalRightCursor, new Vector2(15f, 15f), CursorMode.Auto);
                CursorID = 4;
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    Window.ResizeDownRightStart();
                }
            }
            else if (CursorID == 4)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            if ((Event.current.type == EventType.MouseUp && Event.current.button == 0) || (Window.MoveWindow && !Input.GetKey(KeyCode.Mouse0)))
            {
                Window.ResizeDownRightEnd();
            }
            if (new Rect(0f, Screen.height - 15, 15f, 15f).Contains(Event.current.mousePosition))
            {
                Cursor.SetCursor(DiagonalLeftCursor, new Vector2(15f, 15f), CursorMode.Auto);
                CursorID = 2;
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    Window.ResizeDownLeftStart();
                }
            }
            else if (CursorID == 2)
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            if ((Event.current.type == EventType.MouseUp && Event.current.button == 0) || (Window.MoveWindow && !Input.GetKey(KeyCode.Mouse0)))
            {
                Window.ResizeDownLeftEnd();
            }
        }
    }
}