using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelArtPostProcessFeature : ScriptableRendererFeature
{
	PixelShaderPostProcessPass _customPass;
	[SerializeField] Shader _PixelShader;
	Material _material;
	[SerializeField] RenderPassEvent _whenToDo;

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(_customPass);
	}

	public override void Create()
	{
		//_material = CoreUtils.CreateEngineMaterial(_PixelShader);
		_customPass = new PixelShaderPostProcessPass(_material,_whenToDo);
	}

	//public override void SetupRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	//{
	//	if(renderingData.cameraData.cameraType == CameraType.Game)
	//	{
	//		_customPass.ConfigureInput(ScriptableRenderPassInput.Depth);
	//		_customPass.ConfigureInput(ScriptableRenderPassInput.Color);
	//		_customPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTarget);
	//	}
	//}

	protected override void Dispose(bool disposing)
	{
		CoreUtils.Destroy(_material);
		base.Dispose(disposing);
	}
}
