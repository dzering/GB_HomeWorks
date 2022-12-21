Shader "Unlit/Plane"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Height ("Height", Range(0,2)) = 0.5
        _PI("PI", float) = 3.1415926
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Height;
            float _PI;

            v2f vert (appdata_ v)
            {
                v2f o;

                v.vertex.xyz += v.normal * _Height * sin(v.uv.x * _PI);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }

        //Pass
        //{
        //    CGPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag
        //    // make fog work
        //    #pragma multi_compile_fog

        //    #include "UnityCG.cginc"

        //    struct appdata_
        //    {
        //        float2 uv : TEXCOORD0;
        //        float4 vertex : POSITION;
        //    };

        //    struct v2f
        //    {
        //        float2 uv : TEXCOORD0;
        //        float4 vertex : SV_POSITION;
        //    };

        //    sampler2D _MainTex;
        //    float4 _MainTex_ST;
        //    float _Height;

        //    v2f vert (appdata_ v)
        //    {
        //        v2f o;
        //        o.vertex = UnityObjectToClipPos(v.vertex);
        //        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        //        return o;
        //    }

        //    fixed4 frag (v2f i) : SV_Target
        //    {
        //        // sample the texture
        //        fixed4 col = tex2D(_MainTex, i.uv);
        //        return col;
        //    }
        //    ENDCG
        //}
    }
}
