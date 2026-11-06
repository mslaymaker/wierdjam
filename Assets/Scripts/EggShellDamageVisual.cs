using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EggShellDamageVisual : MonoBehaviour
{
    [Header("References")]// reference to the game manager object
    [SerializeField] private GameObject gameManagerObject;

    [Header("Materials")]//materials representing different cracks
    [SerializeField] private Material baseMat;
    [SerializeField] private Material crack1;
    [SerializeField] private Material crack2;
    [SerializeField] private Material crack3;

    private GameManager gameManager;
    private Renderer shellRenderer;

    //remember the last health value so we don't keep reassigning materials every frame
    private int lastHealth = -1;

    private void Awake()//grab required references before gameplay starts
    {
        shellRenderer = GetComponent<Renderer>();

        if (gameManagerObject != null)//check to see if manager is assigned, if not then we can't grab it and will crash
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
    }

    private void Start()//set the initial shell appearance
    {
        matChange();
    }

    private void Update()
    {
        if (gameManager == null) return;

        int currentHealth = gameManager.getCurrentHealth();

        
        if (currentHealth != lastHealth)//only update when health changes...saves us needing to change materials each frame if not neccessary
        {
            matChange();
        }
    }

    private void matChange()//change shell appearance based on remaining health
    {
        int currentHealth = gameManager.getCurrentHealth();//get the health from the game manager

        lastHealth = currentHealth;

        //healthy egg...freaking pristine!
        if (currentHealth <= 2)
        {
            shellRenderer.material = baseMat;
        }

        //slightly cracked
        else if (currentHealth <= 4)
        {
            shellRenderer.material = crack1;
            Debug.Log("crack1");
        }

        //moderately cracked
        if (currentHealth <= 6)
        {
            shellRenderer.material = crack2;
            Debug.Log("crack2");
        }

        //hold together egg..you are breaking!!!
        else if (currentHealth <= 8)
        {
            shellRenderer.material = crack3;
            Debug.Log("crack3");
        }

    }
}