Shader "Unlit/Sphere"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1,1,1,1)
        _Transparancy("Transparancy", Range(0.0,0.5)) = 0.25
        _CutoutTresh("Cutout Treshhold", Range(0.0,1.0)) = 0.5
        _Distance("Distance", float) = 1
        _Amplitude("Amplitude", float) = 1
        _Speed("Speed", float) = 1
        _Amount("Amount", Range(0.0, 1.0)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"  }
        LOD 100 

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert1
            #pragma fragment frag1
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_
            {
                float2 uv : TEXCOORD0;
                float4 vertex1 : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex1 : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _TintColor;
            float _Transparancy;
            float _CutoutTresh;
            float _Distance;
            float _Amplitude;
            float _Speed;
            float _Amount;

            v2f vert1 (appdata_ v)
            {
                v2f o;
                v.vertex1.x += sin(_Time.y * _Speed + v.vertex1.y * _Amplitude) * _Distance * _Amount;                o.vertex1 = UnityObjectToClipPos(v.vertex1);
                o.vertex1 = UnityObjectToClipPos(v.vertex1);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag1 (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
                col.a = _Transparancy;
                clip(col.r - _CutoutTresh);
                return col;
            }
            ENDCG
        }
    }
}
