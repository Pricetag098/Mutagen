using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class TransparencyScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<GameObject> Pieces;
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
            foreach (var piece in Pieces)
            {

                alpha -= Time.deltaTime * timeToFade;
                var render = piece.GetComponent<Renderer>();
                render.material.SetFloat("_Tweak_transparency", alpha);
                if (alpha <= -1)
                {
                    alpha = -1;
                    render.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                    fade = false;
                    Debug.Log("stopped");
                    render.material.SetFloat("_Tweak_transparency", alpha);
                }
            }
        }
        if (reset)
        {
            foreach (var piece in Pieces)
            {
                var render = piece.GetComponent<Renderer>();
                render.shadowCastingMode = ShadowCastingMode.On;
                alpha += Time.deltaTime * timeToFade;
                render.material.SetFloat("_Tweak_transparency", alpha);
                if (alpha >= 0)
                {
                    alpha = 0;
                    reset = false;
                    Debug.Log("stopped");
                    render.material.SetFloat("_Tweak_transparency", alpha);

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
