﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName, verticalInputName;
    [SerializeField] private float movementSpeed;

    private CharacterController charController;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    private bool isJumping;

    //For the head bob
    [SerializeField] private Transform weaponParent;
    private Vector3 weaponParentOrigin;
    private float movementCounter;
    private float idleCounter;
    private Vector3 targetWeaponBobPosition;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        weaponParentOrigin = weaponParent.localPosition; // dapat ba sa start to

    }

    private void Update()
    {
        PlayerMovement();

    }

    private void PlayerMovement()
    {
        //Don't need time.deltaTime for these because SimpleMove already applies it under the hood
        float horizInput = Input.GetAxis(horizontalInputName);
        float vertInput = Input.GetAxis(verticalInputName);

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        //ClampMagnitude is like Mathf.Clamp but for Vector3 and you can only specify a maximum value
        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);
        JumpInput();

        //try headbob here, idle version
        if((horizInput == 0) && (vertInput == 0))
        {
            HeadBob(idleCounter, 0.025f, 0.025f);
            idleCounter += Time.deltaTime;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 2f);
        }
        else
        {
            HeadBob(movementCounter, 0.038f, 0.038f); //motion version
            movementCounter += Time.deltaTime * 2.7f;
            weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 4f);
        }
        
        
    }

    private void JumpInput()
    {
        if(Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime); //Move does not apply Time.deltaTime unlike in SimpleMove
            timeInAir += Time.deltaTime;
            yield return null;

        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 90.0f;
        isJumping = false;
    }

    private void HeadBob(float z, float xIntensity, float yIntensity)
    {
        targetWeaponBobPosition = weaponParent.localPosition = weaponParentOrigin + new Vector3(Mathf.Cos(z) * xIntensity, Mathf.Sin(z * 2) * yIntensity, 0);
    }
}
