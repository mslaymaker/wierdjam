using UnityEngine;

public class MouseDebug : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         Debug.Log(Cursor.lockState);
    Debug.Log(Cursor.visible);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
