Shader "OlikShader/Waterfall"
{
    Properties
    {
        _Speed("Flow Speed",float) = 1
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DisplGuide("Displacement Guide", 2D) = "white" {}
        _DisplAmount("Displacement Amount", float) = 0
        [HDR]_ColorBottomDark("Color Bottom Dark", color) = (1,1,1,1)
        [HDR]_ColorTopDark("Color Top Dark", color) = (1,1,1,1)
        [HDR]_ColorBottomLight("Color Bottom Light", color) = (1,1,1,1)
        [HDR]_ColorTopLight("Color Top Light", color) = (1,1,1,1)
        _BottomFoamThreshold("Bottom Foam Threshold", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100
        Cull Off

        Stencil {
              Ref 2
                Comp notequal
            }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
             
            #include "UnityCG.cginc"
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 noiseUV : TEXCOORD1;
                float2 displUV : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
 
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            sampler2D _DisplGuide;
            float4 _DisplGuide_ST;
            fixed4 _ColorBottomDark;
            fixed4 _ColorTopDark;
            fixed4 _ColorBottomLight;
            fixed4 _ColorTopLight;
            half _DisplAmount;
            half _BottomFoamThreshold;
            float _Speed;
             
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.noiseUV = TRANSFORM_TEX(v.uv, _NoiseTex);
                o.displUV = TRANSFORM_TEX(v.uv, _DisplGuide);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
             
            fixed4 frag (v2f i) : SV_Target
            {
                //Displacement
                half2 displ = tex2D(_DisplGuide, i.displUV + _Time.y / 5).xy;
                displ = ((displ * 2) - 1) * _DisplAmount;
                 
                //Noise
                half noise = tex2D(_NoiseTex, float2(i.noiseUV.x, i.noiseUV.y + _Time.y*_Speed / 5) + displ).x;
                noise = round(noise * 5.0) / 5.0;
                 
                fixed4 col = lerp(lerp(_ColorBottomDark, _ColorTopDark, i.uv.y), lerp(_ColorBottomLight, _ColorTopLight, i.uv.y), noise);
                col = lerp(fixed4(1,1,1,1), col, step(_BottomFoamThreshold, i.uv.y + displ.y));
                
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
    Fallback "VertexLit"
}