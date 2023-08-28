using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class WadeTesting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject _wall;
    [SerializeField] List<Material> _Mats;
    [SerializeField] float alpha ;
    bool fade;
    public float timeToFade;
    public bool reset;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (fade)
        {
            alpha -= Time.deltaTime * timeToFade;
            foreach(Material mat in _Mats)
            {
                mat.SetFloat("_Tweak_transparency", alpha);
                if (alpha <= -1)
                {
                    alpha = -1;
                    _wall.SetActive(false);
                    fade = false;
                    Debug.Log("stopped");

                }
            }
            
        }
        if (reset)
        {
            _wall.SetActive(true);
            alpha += Time.deltaTime * timeToFade;
            foreach(Material mat in _Mats)
            {
                mat.SetFloat("_Tweak_transparency", alpha);
                if (alpha >= 0)
                {
                    alpha = 0;
                    
                    reset = false;
                    Debug.Log("stopped");

                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        reset = false;
        if (other.GetComponent<HitBox>())
        {
            Debug.Log("player");
            if (alpha != -1)
            {
                fade = true;
                Debug.Log("Fading out");

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HitBox>())
        {
            _wall.SetActive(true);
        }
        fade = false;
        if (other.GetComponent<HitBox>())
        {
            Debug.Log("player");
            if (alpha != 0)
            {
                reset = true;
                Debug.Log("Fading in");

            }
        }
    }
}
