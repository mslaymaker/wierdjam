using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("References")]//player object for the camera to follow around
    [SerializeField] private GameObject player;

    [Header("Follow Settings")]//camera movement settings
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float followDistance = 8f;
    [SerializeField] private float heightOffset = 3f;

    [Header("Zoom Settings")]//settings for mouse wheel zooming
    [SerializeField] private float zoomSpeed = 0.05f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 15f;

    [Header("Rotation Settings")]//settings for rotating around the player
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float minPitch = 15f;
    [SerializeField] private float maxPitch = 70f;

    //current camera orbit around the player
    private float orbitYaw;
    private float orbitPitch;

    private void Start()//grab the camera's starting orientation
    {
        Vector3 angles = transform.eulerAngles;

        orbitYaw = angles.y;
        orbitPitch = angles.x;
    }

    private void Update()
    {
        handleZoom();//scroll wheel controls zoom distance
        handleRotation();//click and drag to orbit around the player
    }

    private void LateUpdate()//follow after the player has finished bouncing all over the place
    {
        followPlayer();
    }

    private void handleZoom()//use mouse wheel to move the camera closer or farther away
    {
        if (Mouse.current == null) return;

        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll != 0)
        {
            //adjust how far behind the player the camera wants to sit
            followDistance -= scroll * zoomSpeed;

            //keep things reasonable so the camera doesn't end up in another zip code
            followDistance = Mathf.Clamp(followDistance, minZoom, maxZoom);
        }
    }

    private void handleRotation()//hold left click and drag to orbit around the player
    {
        if (Mouse.current == null) return;

        //lock the cursor while rotating so it doesn't wander off the game window
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //give the cursor its freedom back when we're done rotating
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            //left and right movement swings around the player
            orbitYaw += mouseDelta.x * rotationSpeed * Time.deltaTime;

            //up and down movement tilts the camera
            orbitPitch -= mouseDelta.y * rotationSpeed * Time.deltaTime;

            //don't let the player flip the camera upside down like a maniac
            orbitPitch = Mathf.Clamp(orbitPitch, minPitch, maxPitch);
        }
    }

    private void followPlayer()//figure out where the camera should be and smoothly chase it
    {
        if (player == null) return;

        //aim slightly above the player so we're not staring at their feet all day
        Vector3 targetPosition = player.transform.position + Vector3.up * heightOffset;

        //build a rotation from the current orbit angles
        Quaternion rotation = Quaternion.Euler(orbitPitch, orbitYaw, 0f);

        //park the camera behind the player using the desired zoom distance
        Vector3 desiredPosition = targetPosition - (rotation * Vector3.forward * followDistance);

        //smoothly chase the target position instead of snapping to it
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        //keep the player in view at all times
        transform.LookAt(targetPosition);
    }
}