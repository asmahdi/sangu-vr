Shader "OlikShader/ToonWaterVR"
{
    Properties
    {
        _WaterColor("Water Color", Color) = (1,1,1,1)

        _FoamColor("Foam Color", Color) = (1,1,1,1)

        // Noise texture used to generate waves.
        _SurfaceNoise("Surface Noise", 2D) = "white" {}

        // Speed, in UVs per second the noise will scroll. Only the xy components are used.
        _SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)

        // Values in the noise texture above this cutoff are rendered on the surface.
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777

        // Red and green channels of this texture are used to offset the
        // noise texture to create distortion in the waves.
        _SurfaceDistortion("Surface Distortion", 2D) = "white" {}   

        // Multiplies the distortion by this value.
        _SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27

        //Wave of the surface 
        _WaveSteepness ("Wave Steepness", Range(0, 1)) = 0.5
        _Wavelength("Wave Length", Float) = 0.1 
        _Direction ("Wave Direction (2D)", Vector) = (1,0,0,0)
       
    }
    SubShader
    {
       Tags
        {
          "Queue" = "Transparent+1"
        }
        Stencil {
                Ref 1
                Comp notequal
                Pass keep 
            }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #define SMOOTHSTEP_AA 0.01

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                
                float2 noiseUV : TEXCOORD0;
                float2 distortUV : TEXCOORD1;
                float3 viewNormal : NORMAL;
                UNITY_FOG_COORDS(2)
                
            };

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;

            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            float _WaveSteepness, _Wavelength;
            float2 _Direction;


            float4 alphaBlend(float4 top, float4 bottom)
            {
                float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
                float alpha = top.a + bottom.a * (1 - top.a);

                return float4(color, alpha);
            }

            v2f vert (appdata v)
            {
                v2f o;

                // Generating waves
                float3 gridPoint = v.vertex.xyz;
                float k = 2 * UNITY_PI / _Wavelength;
                float c = sqrt(9.8 / k);
                float a = _WaveSteepness / k;
                float2 direction = normalize(_Direction);
                float f = k * (dot(direction, gridPoint.xz) - c * _Time.y);
                gridPoint.x += direction.x * (a * cos(f));
                gridPoint.y = a * sin(f);
                gridPoint.z += direction.y * (a * cos(f));
                v.vertex.xyz = gridPoint;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
                o.viewNormal = COMPUTE_VIEW_NORMAL;

                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            
            float _SurfaceNoiseCutoff;
            float _SurfaceDistortionAmount;

            float2 _SurfaceNoiseScroll;
            float4 _WaterColor;
            float4 _FoamColor;


            float4 frag (v2f i) : SV_Target
            {
               
               


                float surfaceNoiseCutoff = 1.2 * _SurfaceNoiseCutoff;

                float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;

                // Distort the noise UV based off the RG channels (using xy here) of the distortion texture.
                // Also offset it by time, scaled by the scroll speed.
                float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, 
                (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
                float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;

                // Use smoothstep to ensure we get some anti-aliasing in the transition from foam to surface.
                // Uncomment the line below to see how it looks without AA.
                // float surfaceNoise = surfaceNoiseSample > surfaceNoiseCutoff ? 1 : 0;
                float surfaceNoise = smoothstep(surfaceNoiseCutoff - SMOOTHSTEP_AA, surfaceNoiseCutoff + SMOOTHSTEP_AA, surfaceNoiseSample);

                float4 surfaceNoiseColor = _FoamColor;
                surfaceNoiseColor.a *= surfaceNoise;

                UNITY_APPLY_FOG(i.fogCoord, _WaterColor);

                // Use normal alpha blending to combine the foam with the surface.
                return alphaBlend(surfaceNoiseColor, _WaterColor);
            }
           
            ENDCG
        }
    }
}
