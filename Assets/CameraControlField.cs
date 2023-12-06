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

    private void OnTriggerEnter(Collider other)
    {
        HitBox hb;
        if(other.TryGetComponent<HitBox>(out hb))
        {
            if (hb.GetComponentInParent<PlayerAbilityCaster>())
            {
                cameraControler.Bind(target);
                play();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        cameraControler.Release();
    }

    private void play()
    {
        if(Boss.Enabled)
        Boss.Value.SetTrigger("Play");
    }
}
