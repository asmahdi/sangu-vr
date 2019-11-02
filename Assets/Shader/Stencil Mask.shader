﻿Shader "Stencil Mask" {
        Properties 
        {
            _Ref("Reference Value", Int) = 1
        }
        

    SubShader {
        Tags 
        { 
            "RenderType"="Transparent" "Queue" = "Geometry+1"
        }

        colormask 0
        ZWrite off

        Pass {
            Stencil {
                Ref [_Ref]
               Pass replace
                
            }
        
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            struct appdata {
                float4 vertex : POSITION;
            };
            struct v2f {
                float4 pos : SV_POSITION;
            };
            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            half4 frag(v2f i) : SV_Target {
                return half4(0,0,1,1);
            }
            ENDCG
        }
    }
}