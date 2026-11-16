using System.Collections.Generic;
using UnityEngine;

public class CameraObstructionFade : MonoBehaviour
{
    [Header("References")]//player reference used for visibility checks
    [SerializeField] private Transform player;

    [Header("Fade Settings")]//controls how transparent obstructions become
    [SerializeField] private float fadedAlpha = 0.25f;
    [SerializeField] private float fadeSpeed = 5f;

    [Header("Detection Settings")]//layers that can be faded
    [SerializeField] private LayerMask fadeLayers;

    private Dictionary<Renderer, Material[]> obstructedRenderers =
        new Dictionary<Renderer, Material[]>();

    private void Update()
    {
        updateObstructions();
    }

    private void updateObstructions()//find objects between camera and player
    {
        if (player == null) return;

        Vector3 direction =
            player.position - transform.position;

        float distance =
            direction.magnitude;

        RaycastHit[] hits =
            Physics.RaycastAll(
                transform.position,
                direction.normalized,
                distance,
                fadeLayers);

        HashSet<Renderer> currentObstructions =
            new HashSet<Renderer>();

        foreach (RaycastHit hit in hits)
        {
            Renderer renderer =
                hit.collider.GetComponent<Renderer>();

            if (renderer == null) continue;

            currentObstructions.Add(renderer);

            if (!obstructedRenderers.ContainsKey(renderer))
            {
                obstructedRenderers.Add(
                    renderer,
                    renderer.materials);
            }

            fadeRenderer(renderer, fadedAlpha);
        }

        List<Renderer> toRestore =
            new List<Renderer>();

        foreach (Renderer renderer in obstructedRenderers.Keys)
        {
            if (!currentObstructions.Contains(renderer))
            {
                restoreRenderer(renderer);
                toRestore.Add(renderer);
            }
        }

        foreach (Renderer renderer in toRestore)
        {
            obstructedRenderers.Remove(renderer);
        }
    }

    private void fadeRenderer(Renderer renderer, float targetAlpha)//fade obstruction toward transparency
    {
        foreach (Material material in renderer.materials)
        {
            Color color = material.color;

            color.a = Mathf.Lerp(
                color.a,
                targetAlpha,
                fadeSpeed * Time.deltaTime);

            material.color = color;
        }
    }

    private void restoreRenderer(Renderer renderer)//restore original opacity
    {
        foreach (Material material in renderer.materials)
        {
            Color color = material.color;

            color.a = Mathf.Lerp(
                color.a,
                1f,
                fadeSpeed * Time.deltaTime);

            material.color = color;
        }
    }
}