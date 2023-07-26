using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PixelShaderPostProcessPass : ScriptableRenderPass
{
	Material _material;
	RenderTextureDescriptor _descriptor;
	RTHandle _cameraColorTarget, _cameraDepthTarget;
	PixelShaderEffectComponent _effectComponent;
	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		VolumeStack stack = VolumeManager.instance.stack;
		_effectComponent = stack.GetComponent<PixelShaderEffectComponent>();

		CommandBuffer cmd = CommandBufferPool.Get();

		using (new ProfilingScope(cmd,new ProfilingSampler("Custom Post Process Effects")))
		{
			Setup(cmd, _cameraColorTarget);

			Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _cameraColorTarget, _material, 0);
		}
		context.ExecuteCommandBuffer(cmd);
		cmd.Clear();

		CommandBufferPool.Release(cmd);

	}

	public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
	{
		_descriptor = renderingData.cameraData.cameraTargetDescriptor;
	}

	public void SetTarget(RTHandle cameraColorHandle, RTHandle cameraDepthTargetHandle)
	{
		_cameraColorTarget = cameraColorHandle;
		_cameraDepthTarget = cameraDepthTargetHandle;
	}

	public PixelShaderPostProcessPass(Material mat,RenderPassEvent passEvent)
	{
		_material = mat;

		renderPassEvent = passEvent;
		
	}

	void Setup(CommandBuffer cmd, RTHandle source)
	{
		//Do later

		//Blitter.BlitCameraTexture(cmd,source,)
	}

}

