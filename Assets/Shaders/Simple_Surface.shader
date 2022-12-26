Shader "Custom/Simple_Surface"
{
    Properties
    {
        _MainText ("Texture", 2D) = "white" {}
        _BumpMap ("BumpMap", 2D) = "Bump" {}

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Lambert

        struct Input{
            float2 uv_MainText;
            float2 uv_BitMap;
            };

        sampler2D _MainText;
        sampler2D _BumpMap;

        void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainText, IN.uv_MainText).rgb;
          o.Normal = UnpackNormal (tex2D(_BumpMap, IN.uv_BitMap));
            }
       
        ENDCG
    }

    FallBack "Diffuse"
}
