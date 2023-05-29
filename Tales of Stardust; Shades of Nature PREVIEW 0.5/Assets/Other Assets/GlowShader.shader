Shader "Custom/GlowShader" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 1
    }
    SubShader {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha One

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Color;
            float4 _GlowColor;
            float _GlowIntensity;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = _Color;
                fixed4 glow = _GlowColor * _GlowIntensity;
                fixed4 finalColor = col + glow;
                return finalColor;
            }
            ENDCG
        }
    }
}
