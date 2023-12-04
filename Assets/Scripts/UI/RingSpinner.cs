using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSpinner : MonoBehaviour
{
    public RectTransform ring1,ring2,ring3;
    public float rotationSpeed;
    public float ringSpeed1,ringSpeed2,ringSpeed3;
    


    // Update is called once per frame
    void Update()
    {
        RotateRing(ring1, ringSpeed1);
        RotateRing(ring2, ringSpeed2);
        RotateRing(ring3, ringSpeed3);
    }
    void RotateRing(RectTransform rectTransform,float speed)
    {
        rectTransform.localEulerAngles += new Vector3(0,0,speed * rotationSpeed * Time.unscaledDeltaTime);
    }
}
