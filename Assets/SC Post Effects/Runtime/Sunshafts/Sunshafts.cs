﻿using System;
using UnityEngine;
using UnityEngine.Rendering;

using UnityEngine.Rendering.PostProcessing;
using TextureParameter = UnityEngine.Rendering.PostProcessing.TextureParameter;
using BoolParameter = UnityEngine.Rendering.PostProcessing.BoolParameter;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;
using IntParameter = UnityEngine.Rendering.PostProcessing.IntParameter;
using ColorParameter = UnityEngine.Rendering.PostProcessing.ColorParameter;
using MinAttribute = UnityEngine.Rendering.PostProcessing.MinAttribute;

namespace SCPE
{
    [PostProcess(typeof(SunshaftsRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Environment/Sun Shafts", true)]
    [Serializable]
    public sealed class Sunshafts : PostProcessEffectSettings
    {
        public enum BlendMode
        {
            Additive,
            Screen
        }

        public enum SunShaftsResolution
        {
            Full = 1,
            Half = 2,
            Third = 3,
            Quarter = 4,
        }
        
        [Tooltip("Use the color of the Directional Light that's set as the caster")]
        public BoolParameter useCasterColor = new BoolParameter { value = true };
        [Tooltip("Use the intensity of the Directional Light that's set as the caster")]
        public BoolParameter useCasterIntensity = new BoolParameter { value = false };

        [DisplayName("Intensity")]
        [Min(0f)]
        public FloatParameter sunShaftIntensity = new FloatParameter { value = 0f };

        [Serializable]
        public sealed class SunShaftsSourceParameter : ParameterOverride<BlendMode> { }
        [Tooltip("Additive mode adds the sunshaft color to the image, while Screen mode perserves color values")]
        public SunShaftsSourceParameter blendMode = new SunShaftsSourceParameter { value = BlendMode.Screen };

        [Serializable]
        public sealed class SunShaftsResolutionParameter : ParameterOverride<SunShaftsResolution> { }
        [DisplayName("Resolution"), Tooltip("Low, quater resolution\n\nNormal, half resolution\n\nHigh, full resolution\n\nLower resolutions may induce jittering")]
        public SunShaftsResolutionParameter resolution = new SunShaftsResolutionParameter { value = SunShaftsResolution.Half };

        [Tooltip("Enabling this option doubles the amount of blurring performed. Resulting in smoother sunshafts at a higher performance cost.")]
        public BoolParameter highQuality = new BoolParameter { value = false };

        [Tooltip("Any color values over this threshold will contribute to the sunshafts effect")]
        [DisplayName("Sky color threshold")]
        [ColorUsage(true, true)]
        public ColorParameter sunThreshold = new ColorParameter { value = Color.black };

        public ColorParameter sunColor = new ColorParameter { value = new Color(1f, 1f, 1f) };
        [Range(0.1f, 1f)]
        [Tooltip("The degree to which the shafts’ brightness diminishes with distance from the caster")]
        public FloatParameter falloff = new FloatParameter { value = 0.5f };

        [Tooltip("The length of the sunrays from the caster's position to the camera")]
        [Min(0f)]
        public FloatParameter length = new FloatParameter { value = 10f };

        [Range(0f, 1f)]
        public FloatParameter noiseStrength = new FloatParameter { value = 0f };

        //Deprecated as of v2.2.0, the old "SunshaftCaster" class still calls on this.
        public static Vector3 sunPosition;
        
        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (sunShaftIntensity == 0 || length == 0 || !RenderSettings.sun) { return false; }
                return true;
            }

            return false;
        }
        
        [SerializeField]
        public Shader shader;

        private void Reset()
        {
            SerializeShader();
        }
        
        private bool SerializeShader()
        {
            bool wasSerialized = !shader;
            shader = Shader.Find(ShaderNames.Sunshafts);

            return wasSerialized;
        }
    }
}