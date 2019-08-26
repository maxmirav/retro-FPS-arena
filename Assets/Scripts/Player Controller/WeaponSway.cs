using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private float swayAmount;
    [SerializeField] private float maxAmount;
    [SerializeField] private float smoothAmount;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        float movementX = Input.GetAxis("Mouse X") * swayAmount;
        float movementY = Input.GetAxis("Mouse Y") * swayAmount;

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
    }
}
