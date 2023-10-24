using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    bool boundCamera;
    [SerializeField] float time;
    Transform parent;
    [SerializeField] Ease ease;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bind(Transform target)
    {
        transform.parent = null;
        transform.DOMove(target.position, time).SetEase(ease);
        
    }
    public void Release()
    {
        transform.parent = parent;
        transform.DOLocalMove(Vector3.zero, time).SetEase(ease);
    }
}
