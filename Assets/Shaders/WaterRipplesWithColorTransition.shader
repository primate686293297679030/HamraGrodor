Shader "Custom/WaterRipplesWithColorTransition"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Speed ("Speed", Range(0, 10)) = 1
        _Frequency ("Frequency", Range(0, 10)) = 1
        _Amplitude ("Amplitude", Range(0, 1)) = 0.1
        _Color1 ("Color 1", Color) = (0, 0.47, 0.85, 1)
        _Color2 ("Color 2", Color) = (0.2, 0.49, 0.81, 1)
        _Color3 ("Color 3", Color) = (0.15, 0.52, 0.93, 1)
        _Color4 ("Color 4", Color) = (0, 0.64, 0.99, 1)
        _Color5 ("Color 5", Color) = (0.29, 0.72, 1, 1)
        _Blend ("Blend", Range(0, 1)) = 0
    }
 
    SubShader
    {
        Tags {"Queue"="Overlay" }
        LOD 100

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
                float4 vertex : POSITION;
            };

            float _Speed;
            float _Frequency;
            float _Amplitude;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;
            float4 _Color5;
            float _Blend;
            sampler2D _MainTex;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.uv = v.uv + float2(sin(v.vertex.x * _Frequency + _Time.y * _Speed) * _Amplitude, 0);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = _Blend;
                fixed4 color = lerp(lerp(lerp(_Color1, _Color2, t), _Color3, t), lerp(_Color4, _Color5, t), t);
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return texColor * color;
            }
            ENDCG
        }
    }
}
