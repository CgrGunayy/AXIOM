Shader "Custom/CurvedUI_Plane_Hologram"
{
    Properties {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        // 0.1 Scale için varsayılan değerleri büyüttük
        _HorizontalBend ("Yatay Bukulme", Float) = 0.05 
        _VerticalBend ("Dikey Bukulme", Float) = 0.05
        _GlowIntensity ("Parlama Yogunlugu", Float) = 2.0
        _ScanlineFrequency ("Tarama Cizgisi Sikligi", Float) = 20.0
        [Toggle] _ShowGrid ("Grid Desenini Goster", Float) = 1.0
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
            fixed4 _Color;
            float _HorizontalBend;
            float _VerticalBend;
            float _GlowIntensity;
            float _ScanlineFrequency;
            float _ShowGrid;
            
            v2f vert(appdata_t v) {
                v2f OUT;
                OUT.objectSpacePos = v.vertex.xyz;

                // --- 3D PLANE MATEMATİĞİ ---
                // Plane yüzeyi X ve Z eksenindedir. Y ekseni ise derinlik (Push/Pull) görevi görür.
                float hBend = (v.vertex.x * v.vertex.x) * _HorizontalBend;
                float vBend = (v.vertex.z * v.vertex.z) * _VerticalBend; // Dikey bükülmeyi artık Z okuyor!
                
                v.vertex.y -= (hBend + vBend); // Derinliği Y eksenine uyguluyoruz!
                // ---------------------------

                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color;
                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target {
                fixed4 baseColor = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                // Hologram efektleri (Plane'in dikey ekseni Z olduğu için taramayı Z'ye bağladık)
                float scanline = sin(IN.objectSpacePos.z * _ScanlineFrequency) * 0.5 + 0.5;
                float glow = pow(scanline, 12.0) * _GlowIntensity;
                
                float grid = 0.0;
                if(_ShowGrid > 0.5) {
                    float grid_thickness = 0.05;
                    float grid_spacing = 1.0;
                    float2 grid_uv = IN.objectSpacePos.xz / grid_spacing; // Grid de XZ eksenine uyarlandı
                    grid_uv = abs(grid_uv - round(grid_uv)) / grid_thickness;
                    grid = max(grid_uv.x, grid_uv.y) < 1.0 ? 1.0 : 0.0;
                }

                fixed4 finalColor = baseColor;
                finalColor.rgb *= (1.0 + glow);
                finalColor.rgb = lerp(finalColor.rgb, fixed3(1.0, 1.0, 1.0), grid * 0.2);

                return finalColor;
            }
            ENDCG
        }
    }
}