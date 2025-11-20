Shader "Custom/ForceFieldOnlyEdges_Emissive"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture (optional)", 2D) = "gray" {}
        [HDR]_Color ("Glow Color", Color) = (0, 2, 5, 1)
        _FresnelPower ("Fresnel Power", Range(0.1, 10)) = 3
        _ScrollSpeed ("Scroll Speed", Range(0, 5)) = 1
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.3
        _EdgeSharpness ("Edge Sharpness", Range(0.5, 10)) = 4
        _EmissionIntensity ("Emission Intensity", Range(0, 10)) = 2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend One One
        Cull Back
        ZWrite Off
        Lighting Off
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _NoiseTex_ST;

            fixed4 _Color;
            float _FresnelPower;
            float _ScrollSpeed;
            float _NoiseIntensity;
            float _EdgeSharpness;
            float _EmissionIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float rim : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Scroll animado de textura
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) + float2(0, _Time.y * _ScrollSpeed);

                // Cálculo de fresnel (bordes)
                float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                float rim = 1.0 - saturate(dot(viewDir, normalize(v.normal)));
                rim = pow(rim, _FresnelPower);
                o.rim = rim;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Base
                fixed4 tex = tex2D(_MainTex, i.uv);

                // Ruido energético
                float noise = tex2D(_NoiseTex, i.uv * 2 + _Time.y * 0.5).r;
                tex.rgb += noise * _NoiseIntensity;

                // Intensidad del borde
                float edge = smoothstep(0.0, 1.0 / _EdgeSharpness, i.rim);

                // Color final solo en el borde
                fixed4 col = tex * _Color * edge;

                // Intensidad emisiva controlable
                col.rgb *= _EmissionIntensity;

                // Transparencia fuera del borde
                col.a = edge;

                return col;
            }
            ENDCG
        }
    }

    FallBack Off
}
