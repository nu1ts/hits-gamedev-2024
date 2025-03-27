Shader "Custom/FishEyeShaderSmooth"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Strength ("Strength", Range(0, 1)) = 0.5
        _Zoom ("Zoom", Range(0.5, 1.0)) = 0.75
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Strength;
            float _Zoom;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * 2.0 - 1.0; // Преобразование координат из [0,1] в [-1,1]
                float r = length(uv);
                float theta = atan2(uv.y, uv.x);
                
                // Изменение радиуса для эффекта "рыбий глаз"
                float newRadius = sqrt(1.0 / (1.0 + _Strength * r)) * _Zoom;
                
                uv = newRadius * float2(cos(theta), sin(theta));
                uv = (uv + 1.0) * 0.5; // Преобразование координат обратно в [0,1]

                // Проверка на выход за границы текстуры
                if (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0)
                {
                    discard; // Отбрасываем пиксели за пределами текстуры
                }

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
