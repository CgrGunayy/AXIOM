Shader "Custom/CurvedUI_DualAxis"
{
    Properties {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _HorizontalBend ("Yatay Bukulme", Float) = 0.0001
        _VerticalBend ("Dikey Bukulme", Float) = 0.0001
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            fixed4 _Color;
            float _HorizontalBend;
            float _VerticalBend;
            
            v2f vert(appdata_t v) {
                v2f OUT;
                
                // --- Çift Eksenli Bükülme Matematiği ---
                float hBend = (v.vertex.x * v.vertex.x) * _HorizontalBend;
                float vBend = (v.vertex.y * v.vertex.y) * _VerticalBend;
                
                v.vertex.z -= (hBend + vBend);
                // ----------------------------------------

                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color;
                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target {
                return tex2D(_MainTex, IN.texcoord) * IN.color;
            }
            ENDCG
        }
    }
}