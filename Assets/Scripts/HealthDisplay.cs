using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
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

    private void Update()//show current health
    {
        displayText.text = "Health : " + gameManager.getCurrentHealth();
    }
}