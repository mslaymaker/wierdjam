using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EggShellAnimator : MonoBehaviour
{
    [Header("References")]//reference to the player controller
    [SerializeField] private PlayerController player;

    [Header("Animation Settings")]//speed thresholds controlling shell behavior
    [SerializeField] private float openSpeed = 1.5f;
    [SerializeField] private float closeSpeed = 2.5f;

    //tracks the current shell state
    [SerializeField] private bool shellOpen;

    private Animator animator;

    private void Awake()//grab required references before gameplay starts
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        updateShellState();
    }

    private void updateShellState()//open or close the shell based on movement speed
    {
        if (player == null) return;

        float currentSpeed = player.getCurrentSpeed();

        //open when moving slowly
        if (!shellOpen && currentSpeed < openSpeed)
        {
            shellOpen = true;
        }
        //close when moving quickly
        else if (shellOpen && currentSpeed > closeSpeed)
        {
            shellOpen = false;
        }

        animator.SetBool("isOpen", shellOpen);
    }
}