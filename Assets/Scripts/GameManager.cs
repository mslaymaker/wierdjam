using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player Settings")]//settings related to the egg's health
    [SerializeField] private int maxHealth = 3;

    [Header("Suspicion Settings")]//settings for how suspicious the customers are
    [SerializeField] private float maxSuspicion = 100f;
    [SerializeField] private float susIncreaseRate = 5f;
    [SerializeField] private float susDecreaseRate = 3f;

    [Header("Suspicion Delays")]//time before suspicion starts changing
    [SerializeField] private float spottedDelay = 1f;
    [SerializeField] private float hiddenDelay = 2f;

    [Header("Debug")]//settings for debug messages
    [SerializeField] private float suspicionLogThreshold = 1f;

    [Header("References")]//required references for gameplay systems
    [SerializeField] private PlayerController player;

    //current health of the egg
    [SerializeField] private int currentHealth;

    //current suspicion level in the store
    [SerializeField] private float currentSuspicion;

    //tracks whether the game has already ended
    private bool gameOver;

    //timers used to control suspicion delays
    private float spottedTimer;
    private float hiddenTimer;

    //used to avoid flooding the console with messages every frame
    private float lastLoggedSuspicion;

    private void Awake()//there should only ever be one game manager
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()//get everything ready for a new game
    {
        currentHealth = maxHealth;
        currentSuspicion = 0f;

        spottedTimer = 0f;
        hiddenTimer = 0f;

        lastLoggedSuspicion = currentSuspicion;

        gameOver = false;
    }

    private void Update()
    {
        if (gameOver) return;

        checkSuspicion();
        reportSuspicion();
    }

    public void takeDamage(int damageAmount)//remove health from the egg
    {
        if (gameOver) return;

        currentHealth -= damageAmount;

        //don't let health go below zero
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Current Health: " + currentHealth);

        //check if the egg has survived its final accident
        if (currentHealth <= 0)
        {
            loseGame();
        }
    }

    private void checkSuspicion()//watch the player and adjust suspicion accordingly
    {
        if (player == null) return;

        if (player.getIsSpotted())
        {
            //we can stop counting hidden time because the player has been seen
            hiddenTimer = 0f;

            spottedTimer += Time.deltaTime;

            //give the player a grace period before suspicion starts climbing
            if (spottedTimer >= spottedDelay)
            {
                currentSuspicion += susIncreaseRate * Time.deltaTime;
            }
        }
        else
        {
            //we can stop counting spotted time because the player is hidden
            spottedTimer = 0f;

            hiddenTimer += Time.deltaTime;

            //wait a bit before customers calm down again
            if (hiddenTimer >= hiddenDelay)
            {
                currentSuspicion -= susDecreaseRate * Time.deltaTime;
            }
        }

        //snap to clean values when we're close to the floor or ceiling
        if (currentSuspicion < 0.01f)
        {
            currentSuspicion = 0f;
        }

        if (currentSuspicion > maxSuspicion - 0.01f)
        {
            currentSuspicion = maxSuspicion;
        }

        //keep suspicion inside legal bounds
        currentSuspicion = Mathf.Clamp(currentSuspicion, 0f, maxSuspicion);

        //if suspicion maxes out the customers have figured out something is wrong
        if (currentSuspicion >= maxSuspicion)
        {
            loseGame();
        }
    }

    private void reportSuspicion()//only report meaningful suspicion changes
    {
        if (Mathf.Abs(currentSuspicion - lastLoggedSuspicion) >= suspicionLogThreshold)
        {
            Debug.Log("Suspicion Level: " + currentSuspicion);

            lastLoggedSuspicion = currentSuspicion;
        }
    }

    public void winGame()//the egg escaped the store
    {
        if (gameOver) return;

        gameOver = true;

        Debug.Log("YOU WIN!!! The egg has escaped!");
    }

    public void loseGame()//the customers were not fooled
    {
        if (gameOver) return;

        gameOver = true;

        Debug.Log("GAME OVER!!! Better luck next hatch.");
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public float getCurrentSuspicion()
    {
        return currentSuspicion;
    }

    public float getMaxSuspicion()
    {
        return maxSuspicion;
    }

    public float getSuspicionPercent()
    {
        return currentSuspicion / maxSuspicion;
    }

    public bool getGameOver()
    {
        return gameOver;
    }
}