using TMPro;
using UnityEngine;

public class SuspicionDisplay : MonoBehaviour
{
    [Header("References")]//game manager reference
    [SerializeField] private GameObject gameManagerObject;

    private TMP_Text displayText;
    private GameManager gameManager;

    private void Awake()//grab required references
    {
        displayText = GetComponent<TMP_Text>();
        gameManager = gameManagerObject.GetComponent<GameManager>();
    }

    private void Update()//show current suspicion
    {

        displayText.text = "Suspicion : " + gameManager.getCurrentSuspicion().ToString("F1");
    }
}