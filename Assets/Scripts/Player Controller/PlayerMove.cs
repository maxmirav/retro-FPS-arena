using System.Collections;
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

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        weaponParentOrigin = weaponParent.localPosition; // dapat ba sa start to

    }

    private void Update()
    {
        PlayerMovement();

        //idle version of headbob
        if()
        {
            HeadBob();
        }
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
        weaponParent.localPosition = new Vector3(Mathf.Cos(z) * xIntensity, Mathf.Sin(z) * yIntensity, weaponParentOrigin.z);
    }
}
