Shader "Unlit/Celshade"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "White"
        _Color("Colour", Color) = (1,1,1,1)
    }

        SubShader
    {
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertext vertexFunc
            #pragma fragment fragmentFunc
            #include "UnityCG.cginc"

            sampler2D _MainTex
            struct v2f
            {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
            };

            v2f vertexFunc(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texCoord
                    return o;
            }
            fixed4 _Color;
            float4 _MainTex_TexelSize;


            ENDCD
        }
    }
}

