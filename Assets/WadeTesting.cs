using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class WadeTesting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject roof;
    [SerializeField] float alpha ;
    bool fade;
    Renderer render;
    public float timeToFade;
    public bool reset;

    void Start()
    {
       render = roof.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fade)
        {  
        alpha -= Time.deltaTime *timeToFade;
        render.material.SetFloat("_Tweak_transparency", alpha);
            if (alpha <= -1)
            {
                alpha = -1;
                fade = false;
                Debug.Log("stopped");

            }
        }
        if (reset)
        {
            alpha += Time.deltaTime * timeToFade;
            render.material.SetFloat("_Tweak_transparency", alpha);
            if (alpha >= 0)
            {
                alpha = 0;
                reset = false;
                Debug.Log("stopped");

            }
        }
        render.material.SetFloat("_Tweak_transparency", alpha);

    }

    private void OnTriggerEnter(Collider other)
    {
        reset = false;
        if (other.GetComponent<HitBox>())
        {
            Debug.Log("player");
            if ( alpha != -1)
            {
                fade = true;
                Debug.Log("Fading out");

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        fade = false;
        if (other.GetComponent<HitBox>())
        {
            Debug.Log("player");
            if (alpha != 0)
            {
                reset= true;
                Debug.Log("Fading in");

            }
        }
    }
}
