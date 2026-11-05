using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextFadeController : MonoBehaviour
{
    [Header("Timing Settings")]//controls when the text appears and how long it remains visible
    [SerializeField] private float startDelay = 0f;
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float visibleTime = 2f;
    [SerializeField] private float fadeOutTime = 1f;

    private TMP_Text textObject;

    private void Awake()//grab required references before gameplay starts
    {
        textObject = GetComponent<TMP_Text>();

        //start fully transparent
        Color textColor = textObject.color;
        textColor.a = 0f;
        textObject.color = textColor;
    }

    private void Start()
    {
        StartCoroutine(fadeText());
    }

    private IEnumerator fadeText()//wait if needed, fade in, remain visible, then fade out
    {
        if (startDelay > 0f)
        {
            yield return new WaitForSeconds(startDelay);
        }

        Color textColor = textObject.color;

        float timer = 0f;

        //fade in
        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;

            textColor.a = Mathf.Lerp(0f, 1f, timer / fadeInTime);

            textObject.color = textColor;

            yield return null;
        }

        textColor.a = 1f;
        textObject.color = textColor;

        //leave the text visible for a while
        yield return new WaitForSeconds(visibleTime);

        timer = 0f;

        //fade out
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;

            textColor.a = Mathf.Lerp(1f, 0f, timer / fadeOutTime);

            textObject.color = textColor;

            yield return null;
        }

        textColor.a = 0f;
        textObject.color = textColor;
    }
}