using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Rig_Controller : MonoBehaviour
{
    RigBuilder rigBuilder;
    public ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        rigBuilder = GetComponent<RigBuilder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleRigBuilder(bool state)
    {
        rigBuilder.layers[0].active = state;
    }

    public void DisableRig()
    {
        ToggleRigBuilder(false);
    }

    public void EnableRig()
    {
        ToggleRigBuilder(true);
    }

    public void playParticle()
    {
        particle.Play();   
    }
}
