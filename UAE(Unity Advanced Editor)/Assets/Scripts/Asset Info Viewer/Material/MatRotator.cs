using UnityEngine;

public class MatRotator : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(Vector3.up, 360f * 2000f);
    }
}
