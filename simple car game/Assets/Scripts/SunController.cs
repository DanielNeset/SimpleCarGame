using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SunController : MonoBehaviour
{
    public Light theSun;
    public float rotationSpeed = 20f;
    public float intensityChangeSpeed = 2f;
    public float minIntensity = 0f;
    public float maxIntensity = 10f;



    PlayerInputActions playerInputActions;

    private bool rotateRight = false;
    private bool rotateLeft = false;
    private bool increaseIntensity = false;
    private bool decreaseIntensity = false;
    private float intensity = 1f;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnEnable()
    {
        playerInputActions.Sun.Enable();
        playerInputActions.Sun.RotateRight.started += RotateRightStart;
        playerInputActions.Sun.RotateRight.canceled += RotateRightCanceled;
        playerInputActions.Sun.RotateLeft.started += RotateLeftStart;
        playerInputActions.Sun.RotateLeft.canceled += RotateLeftCanceled;
        playerInputActions.Sun.IncreaseIntensity.started += IncreaseIntensityStart;
        playerInputActions.Sun.IncreaseIntensity.canceled += IncreaseIntensityCanceled;
        playerInputActions.Sun.DecreaseIntensity.started += DencreaseIntensityStart;
        playerInputActions.Sun.DecreaseIntensity.canceled += DencreaseIntensityCanceled;
    }

    private void OnDisable()
    {
        playerInputActions.Sun.RotateRight.started -= RotateRightStart;
        playerInputActions.Sun.RotateRight.canceled -= RotateRightCanceled;
        playerInputActions.Sun.RotateLeft.started -= RotateLeftStart;
        playerInputActions.Sun.RotateLeft.canceled -= RotateLeftCanceled;
        playerInputActions.Sun.IncreaseIntensity.started -= IncreaseIntensityStart;
        playerInputActions.Sun.IncreaseIntensity.canceled -= IncreaseIntensityCanceled;
        playerInputActions.Sun.DecreaseIntensity.started -= DencreaseIntensityStart;
        playerInputActions.Sun.DecreaseIntensity.canceled -= DencreaseIntensityCanceled;
        playerInputActions.Sun.Disable();
    }

    public void RotateRightStart (InputAction.CallbackContext context)
    {
        rotateRight = true;
    }
    public void RotateRightCanceled(InputAction.CallbackContext context)
    {
        rotateRight = false;
    }
    public void RotateLeftStart(InputAction.CallbackContext context)
    {
        rotateLeft = true;
    }
    public void RotateLeftCanceled(InputAction.CallbackContext context)
    {
        rotateLeft = false;
    }

    public void IncreaseIntensityStart(InputAction.CallbackContext context)
    {
        increaseIntensity = true;
    }
    public void IncreaseIntensityCanceled(InputAction.CallbackContext context)
    {
        increaseIntensity = false;
    }
    public void DencreaseIntensityStart(InputAction.CallbackContext context)
    {
        decreaseIntensity = true;
    }
    public void DencreaseIntensityCanceled(InputAction.CallbackContext context)
    {
        decreaseIntensity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotateRight)
        {
            theSun.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }
        if (rotateLeft)
        {
            theSun.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime * (-1));
        }
        if (increaseIntensity)
        {
            ChangeIntensity(intensityChangeSpeed * Time.deltaTime);
        }
        if(decreaseIntensity)
        {
            ChangeIntensity(-intensityChangeSpeed * Time.deltaTime);
        }
    }

    private void ChangeIntensity(float amount)
    {
        theSun.intensity = Mathf.Clamp(theSun.intensity + amount, minIntensity, maxIntensity);
    }
}
