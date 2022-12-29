Shader "Custom/PlanetShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _MainTex2("Main Texture", 2D) = "white" {}
        _MaskTex("Mask Texture", 2D) = "black" {}
        _EmissionTex("Emission Texture", 2D) = "black" {}

        _ColorEmission("Color Emission", Color) = (1,1,1,1)
        _Intencity("Emission Intencity", Range(0,10)) = 1.0
        _EmissionAppearence ("Emmision Apearence", Range(0,1)) = 0.5 
    }

    SubShader
    {
        CGPROGRAM

        #pragma surface surf Lambert

        sampler2D _MainTex,
                  _MainTex2, 
                  _MaskTex,
                  _EmissionTex;

        fixed3 _ColorEmission;
        float _Intencity;
        fixed _EmissionAppearence;



        struct Input
        {
            half2 uv_MainTex;
            half2 uv_MaskTex;
            half2 uv_EmissionTex;
        };


        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed3 mask = tex2D(_MaskTex, IN.uv_MaskTex);

            fixed3 result = tex2D(_MainTex, IN.uv_MainTex) * mask.b;

            result += tex2D(_MainTex2, IN.uv_MainTex) * mask.g;

            o.Albedo = result;

            fixed3 emTex = tex2D(_EmissionTex, IN.uv_MaskTex).rgb;
            half appearMask = emTex.b;
            appearMask = smoothstep(_EmissionAppearence - 0.2, _EmissionAppearence, appearMask);

            o.Emission = appearMask * emTex.g * _ColorEmission * _Intencity;  
        }

        ENDCG
    }

    Fallback "Diffuse"
}
