Shader "Custom/GradientWithUpperHalfStarsShader"
{
    Properties
    {
        // Gradient Properties
        _Color1 ("Color 1 (Top)", Color) = (1, 0.949, 0.541, 1)
        _Color2 ("Color 2", Color) = (0.827, 0.416, 0, 1)
        _Color3 ("Color 3", Color) = (0.514, 0.235, 0.259, 1)
        _Color4 ("Color 4", Color) = (0.157, 0.086, 0.329, 1)
        _Color5 ("Color 5 (Bottom)", Color) = (0.110, 0.078, 0.157, 1)

        _GradientCenter ("Gradient Center", Vector) = (0.5, 0.0, 0, 0)
        _GradientScale ("Gradient Scale", Float) = 1.0
        _Position1 ("Position for Color 1", Float) = 0.0
        _Position2 ("Position for Color 2", Float) = 0.25
        _Position3 ("Position for Color 3", Float) = 0.5
        _Position4 ("Position for Color 4", Float) = 0.75
        _Position5 ("Position for Color 5", Float) = 1.0

        // Star Properties
        _StarIntensity ("Star Intensity", Float) = 1.0
        _StarDensity ("Star Density", Float) = 100.0
        _StarSize ("Star Size", Float) = 0.001
        _RandomSeed ("Random Seed", Float) = 0.0
    }
    SubShader
    {
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // Gradient Properties
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;
            float4 _Color5;

            float4 _GradientCenter;
            float _GradientScale;
            float _Position1;
            float _Position2;
            float _Position3;
            float _Position4;
            float _Position5;

            // Star Properties
            float _StarIntensity;
            float _StarDensity;
            float _StarSize;
            float _RandomSeed;

            // Pseudo-random number generator based on UV coordinates
            float rand(float2 co)
            {
                return frac(sin(dot(co.xy + _RandomSeed, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Gradient Code (same as before)
                float2 uv = i.uv;
                float2 gradientCenter = _GradientCenter.xy;
                float2 direction = uv - gradientCenter;
                float distance = length(direction) * _GradientScale;

                fixed4 color;

                if (distance < _Position2)
                {
                    color = lerp(_Color1, _Color2, (distance - _Position1) / (_Position2 - _Position1));
                }
                else if (distance < _Position3)
                {
                    color = lerp(_Color2, _Color3, (distance - _Position2) / (_Position3 - _Position2));
                }
                else if (distance < _Position4)
                {
                    color = lerp(_Color3, _Color4, (distance - _Position3) / (_Position4 - _Position3));
                }
                else
                {
                    color = lerp(_Color4, _Color5, (distance - _Position4) / (_Position5 - _Position4));
                }

                // Star Generation in the upper half of the UV space
                float starFactor = 0.0;

                // Iterate and randomly place stars based on bias towards upper half
                float densityFactor = _StarDensity * _StarSize;

                // Weight stars to appear more in the upper half
                float starProbability = smoothstep(0.5, 1.0, i.uv.y);

                float randVal = rand(i.uv * densityFactor); 

                // Add a threshold for star generation based on density and bias
                if (randVal < _StarSize * starProbability)
                {
                    // Stars get brighter as the distance to the star center decreases
                    starFactor = (1.0 - randVal / _StarSize) * _StarIntensity;
                }

                // Combine stars with the gradient background
                fixed4 starColor = fixed4(1.0, 1.0, 1.0, 1.0) * starFactor;
                return color + starColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
