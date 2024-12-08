using UnityEngine;

public class ReplacerRemover : MonoBehaviour
{
    public SelectedFieldInfo info;

    public ReplacerManager manager;

    public void Remove()
    {
        int index = info.cell._cellIndex;

        manager.Delete(index);

        manager.defameData();
        manager.initializeData();
        manager.displayData();
    }
    public void RemoveAll()
    {
        manager.info.Clear();

        manager.defameData();
        manager.initializeData();
        manager.displayData();
    }
    public void RemoveAt(int index)
    {
        manager.Delete(index);

        manager.defameData();
        manager.initializeData();
        manager.displayData();
    }
}
