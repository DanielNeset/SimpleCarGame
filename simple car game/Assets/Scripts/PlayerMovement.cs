using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    PlayerInputActions playerInputActions;

    public float speed = 10f;
    public float rotationSpeed = 100f;
    public float maxSteeringAngle = 45f;
    public float steeringResetSpeed = 200f;
    public List<GameObject> frontWheels;

    private float currentSteeringAngle = 0f;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void FixedUpdate()
    {
        Vector2 input = playerInputActions.Player.Movement.ReadValue<Vector2>();
        float steeringInput = input.x;
        float desiredSteeringAngle = steeringInput * rotationSpeed * Time.deltaTime;

        if (steeringInput != 0)
        {
            currentSteeringAngle = Mathf.Clamp(currentSteeringAngle + desiredSteeringAngle, -maxSteeringAngle, maxSteeringAngle);
        }
        else
        {
            currentSteeringAngle = Mathf.MoveTowards(currentSteeringAngle, 0, steeringResetSpeed * Time.deltaTime);
        }


        currentSteeringAngle = Mathf.Clamp(currentSteeringAngle + desiredSteeringAngle, -maxSteeringAngle, maxSteeringAngle);



        transform.Translate(Vector3.forward * input.y * speed * Time.deltaTime);


        if(input.y != 0){
            transform.Rotate(Vector3.up, input.x * input.y * Time.deltaTime * rotationSpeed);
            Debug.Log(input.x);
        }

        foreach (GameObject frontWheel in frontWheels)
        {
            Vector3 localRotation = frontWheel.transform.localEulerAngles;
            localRotation.y = currentSteeringAngle; 
            frontWheel.transform.localEulerAngles = localRotation;
        }


    }


}
