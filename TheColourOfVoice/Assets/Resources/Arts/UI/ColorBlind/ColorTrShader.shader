Shader "MyShader/ColorTr" {
    Properties {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _UtR ("UtR", Vector) = (1, 0, 0, 0)
        _UtG ("UtG", Vector) = (0, 1, 0, 0)
        _UtB ("UtB", Vector) = (0, 0, 1, 0)
    }
    SubShader {
        Pass {
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _UtR;
            float4 _UtG;
            float4 _UtB;

            struct a2v {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 color = tex2D(_MainTex, i.uv);
                color.r = dot(color.rgb, _UtR.rgb);
                color.g = dot(color.rgb, _UtG.rgb);
                color.b = dot(color.rgb, _UtB.rgb);
                return color;
            }
            ENDCG
        }
    }
    Fallback Off
}
