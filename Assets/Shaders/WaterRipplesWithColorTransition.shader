Shader "Custom/WaterShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {} // Normal map for surface details
        _Speed ("Speed", Range(0, 10)) = 1
        _Frequency ("Frequency", Range(0, 10)) = 1
        _Amplitude ("Amplitude", Range(0, 1)) = 0.1
        _Shininess ("Shininess", Range(0, 1)) = 0.5 // Shininess for specular reflection
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
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
            float _Shininess;
            sampler2D _MainTex;
            sampler2D _NormalMap;

            v2f vert(appdata_t v)
            {
                v2f o;
                 o.uv = v.uv + float2(0, sin(v.vertex.x * _Frequency + _Time.y * _Speed) * _Amplitude);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                float3 normalMap = UnpackNormal(tex2D(_NormalMap, i.uv));

                // Apply normal mapping for surface details
                float3 normal = normalize(normalize(i.vertex.xyz) + normalMap * _Shininess);

                // Apply lighting model (e.g., Lambertian)
                float3 lightDir = normalize(float3(1, 1, -1));
                float diffuse = max(0, dot(normal, lightDir));

                // Add specular reflection (Phong reflection model)
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.vertex.xyz);
                float3 reflectDir = reflect(-lightDir, normal);
                float specular = pow(max(0, dot(viewDir, reflectDir)), 16); // Shininess factor

                // Water color with diffuse and specular components
                fixed3 waterColor = texColor.rgb * (0.5 + 0.5 * diffuse) + float3(0.5, 0.7, 1) * specular;

                // Apply alpha from the texture
                float alpha = texColor.a;

                return fixed4(waterColor, alpha);
            }
            ENDCG
        }
    }
}
