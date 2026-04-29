using UnityEngine;
using UnityEditor;
using System.IO;

public class ShaderGenerator
{
    [MenuItem("Tools/3D Text Icin Nihai Hologram Shader Uret")]
    public static void CreateShader()
    {
        string path = "Assets/CurvedUI_3DText_Final.shader";

        string shaderCode = @"Shader ""Custom/CurvedUI_3DText_Final""
{
    Properties {
        _MainTex (""Font Atlas Texture (BURAYA SURUKLE)"", 2D) = ""white"" {}
        _Color (""Yazi Rengi (Tint)"", Color) = (0, 1, 0, 1)
        
        [Header(Bukulme Ayarlari)]
        _HorizontalBend (""Yatay Bukulme"", Float) = 0.005 
        _VerticalBend (""Dikey Bukulme"", Float) = 0.005
        
        [Header(Hologram Efektleri)]
        _GlowIntensity (""Parlama Yogunlugu"", Float) = 2.0
        _ScanlineFrequency (""Tarama Cizgisi Sikligi"", Float) = 50.0
    }
    SubShader {
        Tags { 
            ""Queue""=""Transparent"" 
            ""IgnoreProjector""=""True"" 
            ""RenderType""=""Transparent"" 
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off 
        Lighting Off 
        ZWrite Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include ""UnityCG.cginc""
            
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
            
            v2f vert(appdata_t v) {
                v2f OUT;
                OUT.objectSpacePos = v.vertex.xyz;

                // 3D Text (X, Y) eksenine göre derinliði (Z) bükme formülü
                float hBend = (v.vertex.x * v.vertex.x) * _HorizontalBend;
                float vBend = (v.vertex.y * v.vertex.y) * _VerticalBend; 
                v.vertex.z += (hBend + vBend); 

                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = v.texcoord;
                OUT.color = v.color * _Color;
                
                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target {
                fixed4 texColor = tex2D(_MainTex, IN.texcoord);
                
                // Beyaz Kutu Çözümü: Rengi biz veriyoruz, þekli (þeffaflýðý) fonttan alýyoruz
                fixed4 baseColor = IN.color * _Color;
                baseColor.a *= texColor.a; 
                
                // Hologram Efektleri
                float scanline = sin(IN.objectSpacePos.y * _ScanlineFrequency) * 0.5 + 0.5;
                float glow = pow(scanline, 12.0) * _GlowIntensity;

                fixed4 finalColor = baseColor;
                finalColor.rgb *= (1.0 + glow);

                return finalColor;
            }
            ENDCG
        }
    }
}";
        File.WriteAllText(path, shaderCode);
        AssetDatabase.Refresh();
        Debug.Log("Basarili! 3D Text Mesh icin nihai shader uretildi.");
    }
}