Shader "Custom/Spotlight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _ColorStrength("Color Strength", Range(1,6)) = 1
        _EmissionColor("Emission Color", Color) = (1,1,1,1)
        _EmissionMainTex("Emission Albedo (RGB)", 2D) = "white" {}
        _EmissionColorStrength("Emission Color Strength", Range(0,4)) = 1
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
           
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex, _EmissionMainTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EmissionMainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color, _EmissionColor;
        half _ColorStrength, _EmissionColorStrength;

        // sphere mask properties
        uniform float4 GlobalSpotlight_Position;
        uniform half GlobalSpotlight_Radius;
        uniform half GlobalSpotlight_Softness;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;


            half gScale = (c.r + c.g + c.g) * 0.333;
            fixed3 grey = fixed3(gScale,gScale,gScale);
            half d = distance(GlobalSpotlight_Position, IN.worldPos);
            half sum = saturate((d - GlobalSpotlight_Radius) / -GlobalSpotlight_Softness);
            fixed4 lerpColor = lerp(fixed4(grey, 1), c * _ColorStrength, sum);

            //emission
            fixed4 e = tex2D(_EmissionMainTex, IN.uv_EmissionMainTex) * _EmissionColor * _EmissionColorStrength;
            fixed4 lerpEmission = lerp(fixed4(0,0,0,0),e,sum);

            o.Albedo = lerpColor.rgb;

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Emission = lerpEmission.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
