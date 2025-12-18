using UnityEngine;

public class InputTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse click detected at: " + Input.mousePosition);
        }
    }
}
