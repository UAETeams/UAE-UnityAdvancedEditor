using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public SelectedFieldInfo selectedFieldInfo;

    public void setCurrentFieldAsInfo()
    {
        {
            selectedFieldInfo.cell = this.gameObject.GetComponent<HierarchyField>().refGameObject.GetComponent<TypeFieldCell>();
        }
    }
    public void setCurrentFieldAsInfoPrime()
    {
        selectedFieldInfo.cell = this.gameObject.GetComponent<TypeFieldCell>();
    }
}
