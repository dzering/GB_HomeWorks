Shader "Custom/PlanetShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "black" {}
        _ColorEmission("Color Emission", Color) = (1,1,1,1)
        _Intencity("Emission Intencity", Range(0,2)) = 1.0
    }

    SubShader
    {
        CGPROGRAM

        #pragma surface surf Lambert

        sampler2D _MainTex;
        sampler2D _MaskTex;
        fixed3 _ColorEmission;
        float _Intencity;



        struct Input
        {
            half2 uv_MainTex;
            half2 uv_MaskTex;
        };


        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed3 mask = tex2D(_MaskTex, IN.uv_MaskTex).rgb;
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex) + mask.g;;

            
            o.Emission = tex2D(_MaskTex, IN.uv_MainTex).g * _ColorEmission * _Intencity;
            
        }

        ENDCG
    }

    Fallback "Diffuse"
}
