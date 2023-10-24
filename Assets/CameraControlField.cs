using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlField : MonoBehaviour
{
    CameraControler cameraControler;
    [SerializeField] Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraControler = FindObjectOfType<CameraControler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        cameraControler.Bind(target);
    }
    private void OnTriggerExit(Collider other)
    {
        cameraControler.Release();
    }
}
