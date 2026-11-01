using UnityEngine;
using UnityEngine.UI;

public class SuspicionOverlayController : MonoBehaviour
{
    [Header("References")]//reference to the object containing the game manager
    [SerializeField] private GameObject gameManagerObject;

    [Header("Overlay Settings")]//controls how visible the red warning becomes
    [SerializeField] private float maxAlpha = 0.75f;
    [SerializeField] private float fadeSpeed = 5f;

    [Header("Pulse Settings")]//controls how the warning pulses at higher suspicion levels
    [SerializeField] private float pulseSpeed = 6f;
    [SerializeField] private float pulseStrength = 0.1f;

    private GameManager gameManager;
    private Image overlayImage;

    private void Awake()//grab required references before gameplay starts
    {
        overlayImage = GetComponent<Image>();

        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
    }

    private void Update()
    {
        updateOverlay();
    }

    private void updateOverlay()//make the warning effect stronger as suspicion rises
    {
        if (gameManager == null) return;

        float suspicionPercent =
            gameManager.getCurrentSuspicion() /
            gameManager.getMaxSuspicion();

        //pulse only becomes noticeable as suspicion increases
        float pulse =
            Mathf.Sin(Time.time * pulseSpeed) *
            pulseStrength *
            suspicionPercent;

        float targetAlpha =
            (suspicionPercent * maxAlpha) + pulse;

        //keep the value legal
        targetAlpha = Mathf.Clamp(targetAlpha, 0f, maxAlpha);

        Color currentColor = overlayImage.color;

        currentColor.a = Mathf.Lerp(
            currentColor.a,
            targetAlpha,
            fadeSpeed * Time.deltaTime);

        overlayImage.color = currentColor;
    }
}