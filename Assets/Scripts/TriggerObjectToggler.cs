using UnityEngine;

public class TriggerObjectToggler : MonoBehaviour
{
    [Header("Objects To Enable")]//objects that should become active when the player enters
    [SerializeField] private GameObject[] objectsToEnable;

    [Header("Objects To Disable")]//objects that should become inactive when the player enters
    [SerializeField] private GameObject[] objectsToDisable;

    private void OnTriggerEnter(Collider other)//toggle objects when the player enters the trigger
    {
        if (!other.CompareTag("Player")) return;

        enableObjects();
        disableObjects();
    }

    private void enableObjects()//turn on all assigned objects
    {
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

    private void disableObjects()//turn off all assigned objects
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
