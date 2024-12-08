using UnityEngine;

public class FieldExpand : MonoBehaviour
{
    public HierarchyField Field;
    public RealtimeHierarchy RealtimeHierarchy;
    public void Expand()
    {
        this.Field.refGameObject.GetComponent<SceneExpand>().isExpanded = true;
        RealtimeHierarchy.ReInitializeLayout();
    }
    public void Contract()
    {
        this.Field.refGameObject.GetComponent<SceneExpand>().isExpanded = false;
        RealtimeHierarchy.ReInitializeLayout();
    }
}
