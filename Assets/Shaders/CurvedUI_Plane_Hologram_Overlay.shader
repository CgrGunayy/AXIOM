Shader "Custom/CurvedUI_Plane_Hologram_Overlay"
{
    Properties {
        [Header(Ana Zemin Ayarlari)]
        [PerRendererData] _MainTex ("Zemin Texture (Opsiyonel)", 2D) = "white" {}
        _Color ("Zemin Rengi (Tint)", Color) = (1,1,1,0.5)
        
        [Header(Bukulme Ayarlari)]
        _HorizontalBend ("Yatay Bukulme", Float) = 0.05 
        _VerticalBend ("Dikey Bukulme", Float) = 0.05
        
        [Header(Hologram Efektleri)]
        _GlowIntensity ("Parlama Yogunlugu", Float) = 2.0
        _ScanlineFrequency ("Tarama Cizgisi Sikligi", Float) = 20.0
        [Toggle] _ShowGrid ("Grid Desenini Goster", Float) = 1.0

        [Header(Ikon ve Gorsel Katmani)]
        _OverlayTex ("Ikon/Gorsel (Alpha Destekli)", 2D) = "black" {}
        _OverlayPos ("Ikon Pozisyonu (X, Y)", Vector) = (0.5, 0.5, 0, 0)
        _OverlayScale ("Ikon Boyutu (Genislik, Yukseklik)", Vector) = (0.2, 0.2, 0, 0)
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off

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
                float3 objectSpacePos : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            sampler2D _OverlayTex;
            
            fixed4 _Color;
            float _HorizontalBend;
            float _VerticalBend;
            float _GlowIntensity;
            float _ScanlineFrequency;
            float _ShowGrid;
            
            float4 _OverlayPos;
            float4 _OverlayScale;
            
            v2f vert(appdata_t v) {
                v2f OUT;
                OUT.objectSpacePos = v.vertex.xyz;

                // 3D PLANE MATEMATİĞİ (X ve Z eksenine göre Y'yi bük)
                float hBend = (v.vertex.x * v.vertex.x) * _HorizontalBend;
                float vBend = (v.vertex.z * v.vertex.z) * _VerticalBend; 
                v.vertex.y -= (hBend + vBend); 

                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color;
                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target {
                // --- 1. ZEMİN VE HOLOGRAM ---
                fixed4 baseColor = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                float scanline = sin(IN.objectSpacePos.z * _ScanlineFrequency) * 0.5 + 0.5;
                float glow = pow(scanline, 12.0) * _GlowIntensity;
                
                float grid = 0.0;
                if(_ShowGrid > 0.5) {
                    float grid_thickness = 0.05;
                    float grid_spacing = 1.0;
                    float2 grid_uv = IN.objectSpacePos.xz / grid_spacing;
                    grid_uv = abs(grid_uv - round(grid_uv)) / grid_thickness;
                    grid = max(grid_uv.x, grid_uv.y) < 1.0 ? 1.0 : 0.0;
                }

                fixed4 finalColor = baseColor;
                finalColor.rgb *= (1.0 + glow);
                finalColor.rgb = lerp(finalColor.rgb, fixed3(1.0, 1.0, 1.0), grid * 0.2);

                // --- 2. İKON / GÖRSEL KATMANI (OVERLAY) ---
                // İkonun merkezini ayarlamak için UV koordinatlarını matematiksel olarak kaydırıyoruz
                float2 overlayUV = (IN.texcoord - _OverlayPos.xy) / _OverlayScale.xy + float2(0.5, 0.5);
                
                // Görselin ekranın dışına taşıp tekrar etmesini engellemek için sınırlar çiziyoruz
                float boundsCheck = step(0.0, overlayUV.x) * step(overlayUV.x, 1.0) * step(0.0, overlayUV.y) * step(overlayUV.y, 1.0);
                
                // İkonu oku (Sınırların dışındaysa görünmez yap)
                fixed4 overlayColor = tex2D(_OverlayTex, overlayUV) * boundsCheck;
                
                // Alpha Blending (İkonun transparanlık değerine göre zeminin üzerine bindir)
                finalColor.rgb = lerp(finalColor.rgb, overlayColor.rgb, overlayColor.a);
                finalColor.a = max(finalColor.a, overlayColor.a); // İkon olan yerler tamamen görünür kalsın

                return finalColor;
            }
            ENDCG
        }
    }
}