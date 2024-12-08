using UnityEngine;
using UnityEngine.UI;

public class GoBack : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }
}
