Shader "Custom/CartoonShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _Color("Main Tex Color", Color) = (1,1,1,1)
        _BumpMap("NormalMap", 2D) = "bump" {}

        _Cutout("Alpha Cutout", Float) = 0.5

        _CellCount("Cell Count", Range(0,10)) = 3
        _ToonValue("ToonValue", Range(0,5)) = 0.35
        _SpecularPower("Specular Power", Range(0.0, 1.0)) = 0.2

        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType" = "TransparentCutout" "Queue" = "Transparent"}

        Cull Front
        Pass
        {
            CGPROGRAM
            #pragma vertex _VertexFuc
            #pragma fragment _FragmentFuc
            #include "UnityCG.cginc"

            struct ST_VertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct ST_VertexOutput
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _OutlineWidth;
            fixed4 _OutlineColor;

            uniform float _Cutout = 0.5;

            ST_VertexOutput _VertexFuc(ST_VertexInput stInput)
            {
                ST_VertexOutput stOutput;

                float3 fNormalized_Normal = normalize(stInput.normal);
                float3 fOutline_Position = stInput.vertex + fNormalized_Normal * (_OutlineWidth * 0.1f);

                stOutput.vertex = UnityObjectToClipPos(fOutline_Position);
                stOutput.uv = TRANSFORM_TEX(stInput.uv, _MainTex);
                return stOutput;
            }


            float4 _FragmentFuc(ST_VertexOutput i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = _OutlineColor.rgb;
                clip(col.a - _Cutout);
                return col;
            }

            ENDCG
        }

        Cull Off
        CGPROGRAM

        #pragma surface surf _BandedLighting keepalpha addshadow fullforwardshadows
        #include "AutoLight.cginc"
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Band_Tex;
            float2 uv_BumpMap;
        };

        struct SurfaceOutputCustom     
        {
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 Emission;
            half Specular;
            fixed Gloss;
            fixed Alpha;

        };

        sampler2D _MainTex;
        sampler2D _Band_Tex;
        sampler2D _BumpMap;

        float4 _Color;

        uniform float _Cutout = 0.5;

        float _CellCount;
        float _ToonValue;
        float _SpecularPower;

        void surf(Input IN, inout SurfaceOutputCustom o)
        {
            float4 fMainTex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = fMainTex.rgb;
            o.Alpha = fMainTex.a;

            float3 fNormalTex = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Normal = fNormalTex;

            clip(fMainTex.a - _Cutout);
        }

        float4 Lighting_BandedLighting(SurfaceOutputCustom s, float3 lightDir, float3 viewDir, float atten)
        {
            float3 fBandedDiffuse;
            float fNDotL = dot(s.Normal, lightDir) * 0.5f + 0.5f;
            fBandedDiffuse = ceil(fNDotL * _CellCount) / _CellCount * _ToonValue;

            float3 fSpecularColor;
            float3 fHalfVector = normalize(lightDir + viewDir);
            float fHDotN = saturate(dot(fHalfVector, s.Normal));
            float fPowedHDotN = pow(fHDotN, 1000.0f);

            float fSpecularSmooth = smoothstep(0.005, 0.01f, fPowedHDotN);
            fSpecularColor = fSpecularSmooth * _SpecularPower;

            float4 fFinalColor;
            fFinalColor.rgb = ((s.Albedo * _Color) + fSpecularColor) *
                                 fBandedDiffuse * _LightColor0.rgb * atten;
            fFinalColor.a = s.Alpha;

            return fFinalColor;
        }

        ENDCG

    }
    FallBack "Diffuse"
}