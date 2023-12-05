using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControlField : MonoBehaviour
{
    CameraControler cameraControler;
    [SerializeField] Transform target;
    public Optional<Animator> Boss;    
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
        play();
    }
    private void OnTriggerExit(Collider other)
    {
        cameraControler.Release();
    }

    private void play()
    {
        Boss.Value.SetTrigger("Play");
    }
}
