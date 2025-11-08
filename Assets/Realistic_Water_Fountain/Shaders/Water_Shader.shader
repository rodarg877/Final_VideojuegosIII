Shader "benoculus/Reflective Transparent URP Simple"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)
        [MainTexture] _BaseMap("Base Map (RGB) RefStrGloss (A)", 2D) = "white" {}

        _SpecColor("Specular Color", Color) = (0.5,0.5,0.5,1)
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5

        _ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
        _ReflectStrength("Reflection Strength", Range(0,1)) = 1.0
        _ReflectionCube("Reflection Cubemap", Cube) = "" {}

        _Alpha("Alpha", Range(0,1)) = 1.0

        _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Scale", Float) = 1.0

        _Cull("Cull Mode", Float) = 2
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull[_Cull]

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fog
            #pragma shader_feature _NORMALMAP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float3 positionWS : TEXCOORD3;
                float fogCoord : TEXCOORD4;
                float3 tangentWS : TEXCOORD5;
                float3 bitangentWS : TEXCOORD6;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURECUBE(_ReflectionCube);
            SAMPLER(sampler_ReflectionCube);
            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float4 _SpecColor;
                float _Smoothness;
                float4 _ReflectColor;
                float _ReflectStrength;
                float _Alpha;
                float4 _BumpMap_ST;
                float _BumpScale;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.viewDirWS = GetWorldSpaceNormalizeViewDir(vertexInput.positionWS);
                output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                output.tangentWS = normalInput.tangentWS;
                output.bitangentWS = normalInput.bitangentWS;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                half3 albedo = baseMap.rgb * _BaseColor.rgb;
                half alpha = baseMap.a * _BaseColor.a * _Alpha;

                half3 normalWS = normalize(input.normalWS);
                half4 normalSample = SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, input.uv);
                half3 normalTS = UnpackNormalScale(normalSample, _BumpScale);
                normalWS = normalize(mul(normalTS, half3x3(input.tangentWS, input.bitangentWS, input.normalWS)));

                half3 viewDirWS = normalize(input.viewDirWS);
                half3 reflectVector = reflect(-viewDirWS, normalWS);
                half4 reflection = SAMPLE_TEXTURECUBE(_ReflectionCube, sampler_ReflectionCube, reflectVector);
                half3 reflectionColor = reflection.rgb * _ReflectColor.rgb * _ReflectStrength * baseMap.a;

                Light mainLight = GetMainLight(TransformWorldToShadowCoord(input.positionWS));
                half3 lightColor = mainLight.color * mainLight.distanceAttenuation;
                half NdotL = saturate(dot(normalWS, mainLight.direction));
                half3 diffuse = albedo * lightColor * NdotL;

                half3 halfVector = normalize(mainLight.direction + viewDirWS);
                half NdotH = saturate(dot(normalWS, halfVector));
                half specularPower = pow(NdotH, _Smoothness * 128.0) * _Smoothness;
                half3 specular = _SpecColor.rgb * lightColor * specularPower * baseMap.a;

                #ifdef _ADDITIONAL_LIGHTS
                uint pixelLightCount = GetAdditionalLightsCount();
                for (uint i = 0; i < pixelLightCount; ++i)
                {
                    Light light = GetAdditionalLight(i, input.positionWS);
                    half3 addLightColor = light.color * light.distanceAttenuation;
                    half addNdotL = saturate(dot(normalWS, light.direction));
                    diffuse += albedo * addLightColor * addNdotL;

                    half3 addHalfVector = normalize(light.direction + viewDirWS);
                    half addNdotH = saturate(dot(normalWS, addHalfVector));
                    half addSpecularPower = pow(addNdotH, _Smoothness * 128.0) * _Smoothness;
                    specular += _SpecColor.rgb * addLightColor * addSpecularPower * baseMap.a;
                }
                #endif

                half3 finalColor = diffuse + specular + reflectionColor;
                finalColor = MixFog(finalColor, input.fogCoord);

                return half4(finalColor, alpha);
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull[_Cull]

            HLSLPROGRAM
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
    }
}