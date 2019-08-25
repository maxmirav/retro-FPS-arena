using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMove : MonoBehaviour
{
    //Variables for ScaleToSize
    private Vector3 startSize = Vector3.zero;
    private Vector3 endSize = new Vector3(2, 2, 2);
    private float lerpTime = 0;
    [SerializeField] float scaleSpeed;

    private float rotationSpeed = 270.0f;

    private void Start()
    {
        StartCoroutine(RotateForSeconds());
    }

    private void Update()
    {
        ScaleToSize();
    //https://answers.unity.com/questions/885972/how-to-stop-rotation-after-a-certain-limit.html

    }

    
    private void ScaleToSize()
    {
        transform.localScale = Vector3.Lerp(startSize, endSize, lerpTime);
        lerpTime += Time.deltaTime * scaleSpeed;
    }

    private IEnumerator RotateForSeconds() //Call this method with StartCoroutine(RotateForSeconds());
    {
        float time = 2.1f;     
        while(time > 0)     
        {
            transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);   
            time -= Time.deltaTime;    
            yield return null;     
        }
}



}
