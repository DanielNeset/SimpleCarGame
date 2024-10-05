using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControll : MonoBehaviour
{

    public List<Camera> cameras = new List<Camera>();
    public float fovChangeSpeed = 5f;  
    public float minFOV = 20f;         
    public float maxFOV = 90f;
    public GameObject player;
    private Vector3 offset;

    PlayerInputActions playerInputActions;

    private Camera cameraComponent;

    private bool isIncreasingFOV = false;  
    private bool isDecreasingFOV = false;

    private int cameraIndex = 0;
    private List<Vector3> offsets = new List<Vector3>();
    private List<Quaternion> rotationOffsets = new List<Quaternion>();


    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void Start()
    {
        //Need offsett for each camera
        offset = transform.position - player.transform.position;

        if (cameras.Count == 0)
        {
            Debug.LogError("Need to set Main camera in the list!");
        }
        foreach (Camera camera in cameras)
        {
            camera.enabled = false;
            camera.GetComponent<AudioListener>().enabled = false;

            Vector3 offset = camera.transform.position - player.transform.position;
            offsets.Add(offset);

            Quaternion rotationOffset = camera.transform.rotation;
            rotationOffsets.Add(rotationOffset);
        }
        cameras[0].enabled = true;
        cameras[0].GetComponent<AudioListener>().enabled = true;
        cameraComponent = cameras[0];
    }

    private void OnEnable()
    {
        playerInputActions.Camera.Enable();
        playerInputActions.Camera.IncreaseFov.started += OnIncreaseFOVStarted;
        playerInputActions.Camera.IncreaseFov.canceled += OnIncreaseFOVCanceled;
        playerInputActions.Camera.DecreaseFov.started += OnDecreaseFOVStarted;
        playerInputActions.Camera.DecreaseFov.canceled += OnDecreaseFOVCanceled;
        playerInputActions.Camera.NextCamera.performed += NextCamera;
        playerInputActions.Camera.PreviousCamera.performed += PreviousCamera;

    }

    private void OnDisable()
    {
        playerInputActions.Camera.IncreaseFov.started -= OnIncreaseFOVStarted;
        playerInputActions.Camera.IncreaseFov.canceled -= OnIncreaseFOVCanceled;
        playerInputActions.Camera.DecreaseFov.started -= OnDecreaseFOVStarted;
        playerInputActions.Camera.DecreaseFov.canceled -= OnDecreaseFOVCanceled;
        playerInputActions.Camera.NextCamera.performed -= NextCamera;
        playerInputActions.Camera.PreviousCamera.performed -= PreviousCamera;

        playerInputActions.Camera.Disable();
    }


    private void FixedUpdate()
    {
        if (isIncreasingFOV)
        {
            IncreaseFOV();
        }else if (isDecreasingFOV)
        {
            DecreaseFOV();
        }
    }

    private void LateUpdate()
    {
        Camera activeCamera = cameras[cameraIndex];

        // Rotate and position only the active camera
        if (!activeCamera.CompareTag("StaticCamera"))
        {
            // Rotate only on the Y-axis based on the player's rotation
            Quaternion playerRotationY = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);

            // Use the stored offset for the position
            Vector3 rotatedOffset = playerRotationY * offsets[cameraIndex];

            // Update the camera's position based on the player's position and the rotated offset
            activeCamera.transform.position = player.transform.position + rotatedOffset;

            // Apply the Y rotation of the player, but preserve the initial X and Z rotation of the camera
            Vector3 cameraRotation = rotationOffsets[cameraIndex].eulerAngles;  // Get initial rotation
            activeCamera.transform.rotation = Quaternion.Euler(cameraRotation.x, player.transform.eulerAngles.y, cameraRotation.z);

        }else
        {
            activeCamera.transform.position = player.transform.position + offsets[cameraIndex];
        }

    }

    private void OnIncreaseFOVStarted(InputAction.CallbackContext context)
    {
        isIncreasingFOV = true;
    }
    private void OnIncreaseFOVCanceled(InputAction.CallbackContext context)
    {
        isIncreasingFOV = false;
    }
    private void OnDecreaseFOVStarted(InputAction.CallbackContext context)
    {
        isDecreasingFOV = true;
    }
    private void OnDecreaseFOVCanceled(InputAction.CallbackContext context)
    {
        isDecreasingFOV = false;
    }
    private void NextCamera(InputAction.CallbackContext context)
    {
        int index = cameraIndex;
        int newIndex = index + 1;

        if (newIndex <= cameras.Count - 1)
        {
            cameras[index].enabled = false;
            cameras[newIndex].enabled = true;
            cameraIndex++;
        }else
        {
            cameraIndex = 0;
            cameras[index].enabled = false;
            cameras[cameraIndex].enabled = true;
        }
        cameraComponent = cameras[cameraIndex];
    }

    private void PreviousCamera(InputAction.CallbackContext context)
    {
        int index = cameraIndex;
        int newIndex = index - 1;

        if (newIndex >= 0)
        {
            cameras[index].enabled = false;
            cameras[newIndex].enabled = true;
            cameraIndex--;
        }
        else
        {
            cameraIndex = cameras.Count - 1;
            cameras[index].enabled = false;
            cameras[cameraIndex].enabled = true;
        }
        cameraComponent = cameras[cameraIndex];
    }


    // Method to increase FOV (Zoom out)
    public void IncreaseFOV()
    {
        if (cameraComponent != null)
        {
            cameraComponent.fieldOfView += fovChangeSpeed * Time.deltaTime;
            cameraComponent.fieldOfView = Mathf.Clamp(cameraComponent.fieldOfView, minFOV, maxFOV);
        }
    }

    // Method to decrease FOV (Zoom in)
    public void DecreaseFOV()
    {
        if (cameraComponent != null)
        {
            cameraComponent.fieldOfView -= fovChangeSpeed * Time.deltaTime;
            cameraComponent.fieldOfView = Mathf.Clamp(cameraComponent.fieldOfView, minFOV, maxFOV);
        }
    }

}
