using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenuForRenderPipeline("Custom/PixelArt", typeof(UniversalRenderPipeline))]
public class PixelShaderEffectComponent : VolumeComponent, IPostProcessComponent
{
    //public BoolParameter enable = new BoolParameter(false,true);
    public FloatParameter scale = new FloatParameter(0,true);

    

    public bool IsActive() { return scale.value > 0; }
    public bool IsTileCompatible() { return false; }
}
