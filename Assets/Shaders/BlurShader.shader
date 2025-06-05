Shader "Custom/BlurShader_ScreenParams"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 10)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlurSize;
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = 0;
                float2 offsets[9] = {
                    float2(-1,  1), float2(0,  1), float2(1,  1),
                    float2(-1,  0), float2(0,  0), float2(1,  0),
                    float2(-1, -1), float2(0, -1), float2(1, -1)
                };
                
                // Beräkna texelstorlek baserat på skärmupplösningen
                float2 texelSize = 1.0 / _ScreenParams.xy;
                
                // Suddar genom att samla in kringliggande texlar med offset
                for (int j = 0; j < 9; j++)
                {
                    col += tex2D(_MainTex, i.uv + offsets[j] * _BlurSize * texelSize);
                }
                col /= 9;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
