using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EggShellDamageVisual : MonoBehaviour
{
    [Header("References")]//reference to the object containing the game manager
    [SerializeField] private GameObject gameManagerObject;

    [Header("Materials")]//materials representing different crack states
    [SerializeField] private Material uncrackedMaterial;
    [SerializeField] private Material crackLevel1Material;
    [SerializeField] private Material crackLevel2Material;
    [SerializeField] private Material crackLevel3Material;

    private GameManager gameManager;
    private Renderer shellRenderer;

    //remember the last health value so we don't keep reassigning materials every frame
    private int lastHealth = -1;

    private void Awake()//grab required references before gameplay starts
    {
        shellRenderer = GetComponent<Renderer>();

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
    }

    private void Start()//set the initial shell appearance
    {
        updateShellMaterial();
    }

    private void Update()
    {
        if (gameManager == null) return;

        int currentHealth = gameManager.getCurrentHealth();

        //only update when health changes
        if (currentHealth != lastHealth)
        {
            updateShellMaterial();
        }
    }

    private void updateShellMaterial()//change shell appearance based on remaining health
    {
        int currentHealth = gameManager.getCurrentHealth();

        lastHealth = currentHealth;

        //healthy egg
        if (currentHealth >= 8)
        {
            shellRenderer.material = uncrackedMaterial;
        }

        //slightly cracked
        else if (currentHealth >= 5)
        {
            shellRenderer.material = crackLevel1Material;
        }

        //moderately cracked
        else if (currentHealth >= 2)
        {
            shellRenderer.material = crackLevel2Material;
        }

        //barely holding together
        else if (currentHealth >= 1)
        {
            shellRenderer.material = crackLevel3Material;
        }
    }
}