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

    PlayerInputActions playerInputActions;

    private Camera cameraComponent;

    private bool isIncreasingFOV = false;  
    private bool isDecreasingFOV = false;

    private int cameraIndex = 0;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void Start()
    {
        if(cameras.Count == 0)
        {
            Debug.LogError("Need to set Main camera in the list!");
        }
        foreach (Camera camera in cameras)
        {
            camera.enabled = false;
            camera.GetComponent<AudioListener>().enabled = false;
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
    }

    private void OnDisable()
    {
        playerInputActions.Camera.IncreaseFov.started -= OnIncreaseFOVStarted;
        playerInputActions.Camera.IncreaseFov.canceled -= OnIncreaseFOVCanceled;
        playerInputActions.Camera.DecreaseFov.started -= OnDecreaseFOVStarted;
        playerInputActions.Camera.DecreaseFov.canceled -= OnDecreaseFOVCanceled;
        playerInputActions.Camera.NextCamera.performed -= NextCamera;
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

        Debug.Log(newIndex + " " + cameras.Count);

        if (newIndex <= cameras.Count - 1)
        {
            cameras[index].enabled = false;
            cameras[newIndex].enabled = true;
            cameraIndex++;
            Debug.Log(newIndex + " " + index);
        }else
        {
            cameraIndex = 0;
            cameras[index].enabled = false;
            cameras[cameraIndex].enabled = true;
        }
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
