// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "OlikShader/Mirror"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        // Red and green channels of this texture are used to offset the
        // noise texture to create distortion in the waves.
        _SurfaceDistortion("Surface Distortion", 2D) = "white" {}   

        // Multiplies the distortion by this value.
        _SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27

        [HideInInspector] _ReflectionTex ("", 2D) = "white" {}
    }
    SubShader
    {
         Tags
        {
          "Queue"="Transparent" 
        }
        LOD 200
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float2 uv : TEXCOORD0;
                float4 refl : TEXCOORD1;
                float4 pos : POSITION;
                float4 col: COLOR;
            };
            struct v2f
            {
                half2 uv : TEXCOORD0;
                float4 refl : TEXCOORD1;
                float2 distortUV : TEXCOORD2;
                float4 pos : SV_POSITION;
                fixed4 col: COLOR;
            };
            float4 _MainTex_ST;
            fixed4 _Color;
            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            v2f vert(appdata_t i)
            {
                v2f o;
                o.pos = UnityObjectToClipPos (i.pos);
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);
                o.col = i.col * _Color;
                o.refl = ComputeScreenPos (o.pos);
                return o;
            }
            sampler2D _MainTex;
            sampler2D _ReflectionTex;
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv) * i.col;
                tex.rgb *= tex.a;
                fixed4 refl = tex2Dproj(_ReflectionTex, UNITY_PROJ_COORD(i.refl));
                return tex * refl;
            }
            ENDCG
        }
    }
}