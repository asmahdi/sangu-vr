// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "OlikShader/SkyBox"
{
    Properties
    {
        _MainTex("Main Texture",2D) = "white" {}
        _SkyColor ("Sky Color", Color) =  (1,1,1,1)
        _HorizonColor ("Horizon Color", Color) =  (1,1,1,1)
        _HorizonOffset ("Horizon Offset", Float) = 0
    }
    SubShader
    {
        Tags { "QUEUE"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        LOD 200
        Cull Off
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
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _SkyColor;
            float4 _HorizonColor;
            float _HorizonOffset;
            sampler2D _MainTex;



            v2f vert (appdata v)
            {

                v2f o;
                o.vertex = UnityObjectToClipPos (v.vertex);
                o.texcoord = v.texcoord;

                return o;

            }
            fixed4 frag (v2f i) : COLOR {
            fixed4 tex = tex2D(_MainTex,i.texcoord);
             fixed4 col = lerp(_HorizonColor, _SkyColor,(i.texcoord.y + _HorizonOffset) );
             col = col*tex;
             col.a = 1;
             return col;
            }

            ENDCG
        }
    }
}
