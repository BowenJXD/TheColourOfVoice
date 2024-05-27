using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomRenderPassFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        public Material crtMaterial;
        public float curvature;
        public float vignetteWidth;

        private RenderTargetHandle temporaryColorTexture;

        public CustomRenderPass(Material material, float curvature, float vignetteWidth)
        {
            this.crtMaterial = material;
            this.curvature = curvature;
            this.vignetteWidth = vignetteWidth;
            temporaryColorTexture.Init("_TemporaryColorTexture");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            cameraTargetDescriptor.depthBufferBits = 0;
            cmd.GetTemporaryRT(temporaryColorTexture.id, cameraTargetDescriptor, FilterMode.Bilinear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (crtMaterial == null)
            {
                Debug.LogError("CRT Material is missing!");
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get("CRTEffect");

            RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;
            cmd.SetGlobalFloat("_Curvature", curvature);
            cmd.SetGlobalFloat("_VignetteWidth", vignetteWidth);

            Blit(cmd, source, temporaryColorTexture.Identifier(), crtMaterial);
            Blit(cmd, temporaryColorTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(temporaryColorTexture.id);
        }
    }

    public Material crtMaterial;
    public float curvature = 1.0f;
    public float vignetteWidth = 30.0f;

    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(crtMaterial, curvature, vignetteWidth)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (crtMaterial != null)
        {
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }
}


