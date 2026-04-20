Shader "Custom/HorrorTint"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Darkness ("Darkness Intensity", Range(0, 1)) = 0.5
        // Changed the default to a more saturated blood red (R=0.8, G=0, B=0)
        _HorrorColor ("Scary Tint", Color) = (0.8, 0, 0, 1) 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Darkness;
            float4 _HorrorColor;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // We mix the red tint based on the darkness intensity
                // Multiplying _HorrorColor by 1.5 gives it a slight "glow" or saturation boost
                col.rgb = lerp(col.rgb, _HorrorColor.rgb * 1.5, _Darkness);
                
                // This darkens the final output so it doesn't look like a bright red room, 
                // but a dark room with a red haze
                col.rgb *= (1.2 - _Darkness); 
                
                return col;
            }
            ENDCG
        }
    }
}