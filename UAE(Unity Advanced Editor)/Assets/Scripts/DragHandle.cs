using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Screen = UnityEngine.Screen;

public class DragHandle : MonoBehaviour, IDragHandler
{
    private Vector2 Previous = Vector2.zero;
    public void OnDrag(PointerEventData data)
    {
        if (BorderlessWindow.framed)
            return;

        Previous += data.delta;
        if (data.dragging)
        {
            BorderlessWindow.MoveWindowPos(Previous, Screen.width, Screen.height);
        }
    }
}
