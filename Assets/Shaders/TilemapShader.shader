Shader "Custom/WallTilemapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (1, 1, 1, 1)
        _TargetColor1 ("Target Color 1", Color) = (1, 1, 1, 1)
        _Color2 ("Color 2", Color) = (1, 1, 1, 1)
        _TargetColor2 ("Target Color 2", Color) = (1, 1, 1, 1)
        _Color3 ("Color 3", Color) = (1, 1, 1, 1)
        _TargetColor3 ("Target Color 3", Color) = (1, 1, 1, 1)
        _Color4 ("Color 4", Color) = (1, 1, 1, 1)
        _TargetColor4 ("Target Color 4", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
            float4 _Color1;
            float4 _TargetColor1;
            float4 _Color2;
            float4 _TargetColor2;
            float4 _Color3;
            float4 _TargetColor3;
            float4 _Color4;
            float4 _TargetColor4;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);

                if(length(col - _Color1) < 0.01)
                {
                    return half4(_TargetColor1.rgb, col.a);
                }
                if(length(col - _Color2) < 0.01)
                {
                    return half4(_TargetColor2.rgb, col.a);
                }
                if(length(col - _Color3) < 0.01)
                {
                    return half4(_TargetColor3.rgb, col.a);
                }
                if(length(col - _Color4) < 0.01)
                {
                    return half4(_TargetColor4.rgb, col.a);
                }

                return col;
            }
            ENDCG
        }
    }
}