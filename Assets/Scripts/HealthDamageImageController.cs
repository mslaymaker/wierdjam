using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HealthDamageImageController : MonoBehaviour
{
    [Header("References")]//reference to the object containing the game manager
    [SerializeField] private GameObject gameManagerObject;

    [Header("Damage Sprites")]//sprites shown as the egg becomes more damaged
    [SerializeField] private Sprite damageLevel1Sprite;
    [SerializeField] private Sprite damageLevel2Sprite;
    [SerializeField] private Sprite damageLevel3Sprite;
    [SerializeField] private Sprite damageLevel4Sprite;

    private GameManager gameManager;
    private Image imageObject;

    //remember the previous health so we only update when needed
    private int lastHealth = -1;

    private void Awake()//grab required references before gameplay starts
    {
        imageObject = GetComponent<Image>();

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
    }

    private void Update()
    {
        if (gameManager == null) return;

        int currentHealth = gameManager.getCurrentHealth();

        //only update the image if health changed
        if (currentHealth != lastHealth)
        {
            updateDamageSprite();

            lastHealth = currentHealth;
        }
    }

    private void updateDamageSprite()//show the appropriate sprite based on current health
    {
        int currentHealth = gameManager.getCurrentHealth();

        if (currentHealth <= 2)
        {
            imageObject.sprite = damageLevel4Sprite;
        }
        else if (currentHealth <= 5)
        {
            imageObject.sprite = damageLevel3Sprite;
        }
        else if (currentHealth <= 8)
        {
            imageObject.sprite = damageLevel2Sprite;
        }
    }
}