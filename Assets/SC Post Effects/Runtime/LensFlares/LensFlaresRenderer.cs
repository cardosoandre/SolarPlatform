﻿using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace SCPE
{
    public sealed class LensFlaresRenderer : PostProcessEffectRenderer<LensFlares>
    {
        Shader shader;
        private int emissionTex;
        private int flaresTex;
        RenderTexture aoRT;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.LensFlares);
            emissionTex = Shader.PropertyToID("_BloomTex");
            flaresTex = Shader.PropertyToID("_FlaresTex");
        }

        public override void Release()
        {
            base.Release();
        }

        enum Pass
        {
            LuminanceDiff,
            Ghosting,
            Blur,
            Blend,
            Debug
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);
            CommandBuffer cmd = context.command;

            sheet.properties.SetFloat("_Intensity", settings.intensity);
            float luminanceThreshold = Mathf.GammaToLinearSpace(settings.luminanceThreshold.value);
            sheet.properties.SetFloat("_Threshold", luminanceThreshold);
            sheet.properties.SetFloat("_Distance", settings.distance);
            sheet.properties.SetFloat("_Falloff", settings.falloff);
            sheet.properties.SetFloat("_Ghosts", settings.iterations);
            sheet.properties.SetFloat("_HaloSize", settings.haloSize);
            sheet.properties.SetFloat("_HaloWidth", settings.haloWidth);
            sheet.properties.SetFloat("_ChromaticAbberation", settings.chromaticAbberation);

            sheet.properties.SetTexture("_ColorTex", settings.colorTex.value ? settings.colorTex : Texture2D.whiteTexture as Texture);

            sheet.properties.SetTexture("_MaskTex", settings.maskTex.value ? settings.maskTex : Texture2D.whiteTexture as Texture);

            cmd.GetTemporaryRT(emissionTex, context.width, context.height, 0, FilterMode.Bilinear, RenderTextureFormat.DefaultHDR);
            cmd.BlitFullscreenTriangle(context.source, emissionTex, sheet, (int)Pass.LuminanceDiff);
            cmd.SetGlobalTexture("_BloomTex", emissionTex);

            cmd.GetTemporaryRT(flaresTex, context.width, context.height, 0, FilterMode.Bilinear, RenderTextureFormat.DefaultHDR);
            cmd.BlitFullscreenTriangle(emissionTex, flaresTex, sheet, (int)Pass.Ghosting);
            cmd.SetGlobalTexture("_FlaresTex", flaresTex);

            // get two smaller RTs
            int blurredID = Shader.PropertyToID("_Temp1");
            int blurredID2 = Shader.PropertyToID("_Temp2");
            cmd.GetTemporaryRT(blurredID, context.width / 2, context.height / 2, 0, FilterMode.Bilinear);
            cmd.GetTemporaryRT(blurredID2, context.width / 2, context.height / 2, 0, FilterMode.Bilinear);

            // downsample screen copy into smaller RT, release screen RT
            cmd.BlitFullscreenTriangle(flaresTex, blurredID);
            cmd.ReleaseTemporaryRT(flaresTex);

            for (int i = 0; i < settings.passes; i++)
            {
                // horizontal blur
                cmd.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(settings.blur / context.screenWidth, 0, 0, 0));
                cmd.BlitFullscreenTriangle(blurredID, blurredID2, sheet, (int)Pass.Blur);  // source -> tempRT

                // vertical blur
                cmd.SetGlobalVector(ShaderParameters.BlurOffsets, new Vector4(0, settings.blur / context.screenHeight, 0, 0));
                cmd.BlitFullscreenTriangle(blurredID2, blurredID, sheet, (int)Pass.Blur);  // source -> tempRT       
            }

            cmd.SetGlobalTexture("_FlaresTex", blurredID);

            //Blend tex with image
            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, (settings.debug) ? (int)Pass.Debug : (int)Pass.Blend);

            // release
            cmd.ReleaseTemporaryRT(emissionTex);
            cmd.ReleaseTemporaryRT(flaresTex);
            cmd.ReleaseTemporaryRT(blurredID);
            cmd.ReleaseTemporaryRT(blurredID2);
        }
    }
}