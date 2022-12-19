Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert1
            #pragma fragment frag1
            // make fog work
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

            v2f vert1 (appdata_ v)
            {
                v2f o;
                o.vertex1 = UnityObjectToClipPos(v.vertex1);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag1 (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
