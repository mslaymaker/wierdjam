using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]//inspector properties to be modified for gameplay
    [SerializeField] private float moveForce = 15f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float torqueAmount = 10f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Ground Check")]//settings used to determine WHETHER the egg is touching the floor
    [SerializeField] private float groundCheckDistance = 1.5f;
    [SerializeField] private LayerMask floorLayer;

    [Header("Collision Damage")]//settings for how much crashing into things hurts
    [SerializeField] private float lowDamageSpeed = 10f;
    [SerializeField] private float mediumDamageSpeed = 20f;
    [SerializeField] private float highDamageSpeed = 35f;

    [SerializeField] private int lowDamageAmount = 1;
    [SerializeField] private int mediumDamageAmount = 2;
    [SerializeField] private int highDamageAmount = 3;

    [SerializeField] private float damageCooldown = 1f;

    [Header("Spotting Settings")]//how fast the egg must be moving before customers get suspicious
    [SerializeField] private float spotSpeed = 2f;

    [Header("Audio")]//sounds related to egg movement and damage
    [SerializeField] private AudioClip crackSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip rollingSound;

    [SerializeField] private float crackVolume = .75f;
    [SerializeField] private float jumpVolume = 1f;
    [SerializeField] private float rollingMaxVolume = .75f;

    [SerializeField] private float rollingFadeSpeed = 5f;
    [SerializeField] private float rollingMinPitch = 0.7f;
    [SerializeField] private float rollingMaxPitch = 1.5f;

    [Header("References")]//required reference to the camera so we can find and have access
    [SerializeField] private Camera mainCamera;

    //values recorded from the input system to pass into our callback methods
    private Vector2 moveValue;
    private bool jumpValue;

    //tracks whether the player is standing on valid ground to jump or not, mark jumpable ground as floor
    [SerializeField] private bool isGrounded;

    //tracks whether at least one customer currently sees the player
    [SerializeField] private bool isSpotted;

    //all customer vision triggers currently overlapping the player
    private HashSet<Collider> activeCustomerTriggers = new HashSet<Collider>();

    private float lastDamageTime;//prevents several damage events from occurring constantly, this is the delay iframes

    private Rigidbody rb;

    //used for jump and crack sounds
    private AudioSource effectsSource;
    private AudioSource rollingSource;//used for looping rolling audio

    private void Awake()//grab required components before gameplay starts
    {
        rb = GetComponent<Rigidbody>();
        effectsSource = GetComponent<AudioSource>();
//
        rollingSource = gameObject.AddComponent<AudioSource>();

        //set up the rolling audio source
        rollingSource.loop = true;
        rollingSource.playOnAwake = false;
        rollingSource.volume = 0f;

        if (rollingSound != null)//maker sure one is applied so we don't crash the game
        {
            rollingSource.clip = rollingSound;
            rollingSource.Play();
        }

        //allow damage immediately at the start of the game
        lastDamageTime = -damageCooldown;
    }

    private void FixedUpdate()
    {
        checkGroundedStatus();//see if we're standing on something that counts as floor

        move();//roll around the store

        if (jumpValue && isGrounded)//only jump if our egg is sitting on the floor
        {
            jump();

            //reset the jump request after using it
            jumpValue = false;
        }
    }

    private void Update()
    {
        updateSpottedStatus();//check if customers should currently be suspicious of us
        updateRollingAudio();//adjust rolling sound based on movement speed
    }

    //record movement values from the input system
    public void recordMove(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
    }

    //record jump button presses from the input system
    public void recordJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpValue = true;
        }
    }

    private void checkGroundedStatus()//shoot a ray downward and see if it hits a floor layer
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, floorLayer);
    }

    private void updateSpottedStatus()//decide if customers currently consider the egg suspicious
    {
        isSpotted = activeCustomerTriggers.Count > 0 && getCurrentSpeed() >= spotSpeed;
    }

    private void updateRollingAudio()//rolling gets louder and higher pitched as the egg moves faster
    {
        if (rollingSource == null) return;

        float speedPercent = Mathf.Clamp01(getCurrentSpeed() / maxSpeed);

        float targetVolume = 0f;

        //only produce rolling sounds while grounded
        if (isGrounded && speedPercent > 0.05f)
        {
            targetVolume = speedPercent * rollingMaxVolume;
        }
        //higher speed means a higher pitch
        rollingSource.pitch = Mathf.Lerp(rollingMinPitch, rollingMaxPitch, speedPercent);
        //smoothly fade volume instead of cutting off instantly during jumps
        rollingSource.volume = Mathf.Lerp(rollingSource.volume, targetVolume, rollingFadeSpeed * Time.deltaTime);
    }

    private void move()//apply rolling force relative to the camera direction
    {
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * moveValue.y + right * moveValue.x;
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            //push the egg in the desired direction
            rb.AddForce(moveDirection.normalized * moveForce, ForceMode.Acceleration);
            //spin the egg so it actually looks like it's rolling and not sliding
            Vector3 rollAxis = Vector3.Cross(Vector3.up, moveDirection.normalized);
            rb.AddTorque(rollAxis * torqueAmount, ForceMode.Acceleration);
        }

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;

            //keep the player from becoming an olympic egg sprinter...
            rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        }
    }

    private void jump()//launch the egg upward with extreme enthusiasm!!!
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        
        if (jumpSound != null)//play jump sound
        {
            effectsSource.PlayOneShot(jumpSound, jumpVolume);
        }
    }

    private void OnCollisionEnter(Collision collision)//figure out how badly we bonked into something
    {
        //ignore damage during the cooldown period
        if (Time.time < lastDamageTime + damageCooldown) return;

        takeDamage(collision.relativeVelocity.magnitude);
    }

    private void takeDamage(float collisionSpeed)//damage depends on how hard we slammed into something
    {
        int damageAmount = 0;
//check each threshold value for appropriate damage that should be assigned when hit hard
        if (collisionSpeed >= highDamageSpeed)
        {
            damageAmount = highDamageAmount;
        }
        else if (collisionSpeed >= mediumDamageSpeed)
        {
            damageAmount = mediumDamageAmount;
        }
        else if (collisionSpeed >= lowDamageSpeed)
        {
            damageAmount = lowDamageAmount;
        }

        if (damageAmount > 0)
        {
            lastDamageTime = Time.time;

            //play the egg cracking sound effect
            if (crackSound != null)
            {
                effectsSource.PlayOneShot(crackSound, crackVolume);
            }

            //tell the game manager that the egg made a poor decision
            GameManager.instance.takeDamage(damageAmount);

            Debug.Log("Crash Speed: " + collisionSpeed);
            Debug.Log("Damage Taken: " + damageAmount);
        }
    }

    private void OnTriggerEnter(Collider other)//keep track of customer vision cones we're currently inside
    {
        if (other.CompareTag("customer"))
        {
            activeCustomerTriggers.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)//remove customer vision cones we are no longer inside
    {
        if (other.CompareTag("customer"))
        {
            activeCustomerTriggers.Remove(other);
        }
    }

    public bool getIsSpotted()//let other systems check if the player is currently visible
    {
        return isSpotted;
    }

    public float getCurrentSpeed()//let other systems know how fast the egg is moving
    {
        return rb.linearVelocity.magnitude;
    }

    private void OnDrawGizmosSelected()//draw the ground check ray so we can see what it's up to
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}