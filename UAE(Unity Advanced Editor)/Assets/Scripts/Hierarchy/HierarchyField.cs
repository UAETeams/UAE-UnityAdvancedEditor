using RTG;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyField : MonoBehaviour
{
    public GameObject refGameObject;
    public HierarchyTransforms HT;

    public Toggle NoGizmo;
    public Toggle Move;
    public Toggle Rotate;
    public Toggle Scale;

    private void Start()
    {
        Button button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(SetSelectedGameObject);
        button.onClick.AddListener(SetGizmos);
    }

    private void SetSelectedGameObject()
    {
        HT.targetObject = refGameObject;

        HT.Inspect();
    }
    private void SetGizmos()
    {
        if (!NoGizmo.isOn)
        {
            if (Move.isOn)
            {
                HT.CreateMoveGizmo();
            }
            if (Scale.isOn)
            {
                HT.CreateReSizeGizmo();
            }
            if (Rotate.isOn)
            {
                HT.CreateRotateGizmo();
            }
        }
    }
}
