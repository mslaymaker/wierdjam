using UnityEngine;

public class MusicPersist : MonoBehaviour
{

    // make a singleton so I don't get multiple music instances playing
    public static MusicPersist Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);//don't kill this on scene load so music plays through story scene
        // check singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Self-destruct if a duplicate tries to spawn..boom!
            return;
        }

        Instance = this;

       
    }
    

  
}
