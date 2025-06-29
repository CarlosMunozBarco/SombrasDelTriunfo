/*
=============================================================
Unity Assets by MAKAKA GAMES: https://makaka.org/unity-assets
=============================================================
*/
 
Shader "AR/ARShadowSurface"
{
    Properties
    { 
        _Cutoff("Cutout", Range(0,1)) = 0.5 
    }
 
    SubShader
    { 
        Pass 
        { 
            Alphatest Greater[_Cutoff]
        }   
 
        Pass
        { 
            Blend DstColor Zero Tags 
            { 
                "LightMode" = "ForwardBase"
            }
 
            CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #include "AutoLight.cginc"
 
            struct v2f 
            { 
                float4 pos : SV_POSITION; 
                LIGHTING_COORDS(0,1) 
            };
 
            v2f vert(appdata_base v) 
            {
                v2f o; 
                o.pos = UnityObjectToClipPos(v.vertex); 
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o; 
            }
 
            fixed4 frag(v2f i) : COLOR
            {
                float attenuation = LIGHT_ATTENUATION(i);
                return attenuation;
            } 
 
            ENDCG 
        } 
    } 
 
    Fallback "Transparent/Cutout/VertexLit"
} 
