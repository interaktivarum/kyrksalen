Shader "Unlit/PaperShader"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" { }
        _NoiseTex ("Noise texture", 2D) = "white" { }
        _Distort ("Distort", Float) = 0.0
        _Ratio ("Ratio", Float) = 1.0
    }
    SubShader {

        Tags { "Queue"="Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha
        

        Pass {

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag alpha

        #include "UnityCG.cginc"

        sampler2D _MainTex;
        float _Ratio;
        sampler2D _NoiseTex;
        float _Distort;
        

        struct v2f {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        float4 _MainTex_ST;

        float getAlpha(v2f i) {
            const float pi = 3.141592653589793238462;
            float d = distance(i.uv.xy,float2(0.5,0.5));
            return 0.9*smoothstep(1,0.5,abs(_Distort))*smoothstep( 0.5 , 0.35 , d ); 
        }

        float getDistortedPixel(float2 uv) {
            float distort = _Distort * (tex2D(_NoiseTex, float2(_Distort, uv.y * 10)));
            return uv.x/_Ratio + (_Ratio-1 + distort - _Distort/ 2)/2;
        }

        v2f vert (appdata_base v)
        {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX (float2(v.texcoord.x,v.texcoord.y), _MainTex);
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
            fixed4 texcol = tex2D (_MainTex, float2(getDistortedPixel(i.uv),i.uv.y));
            texcol.a = getAlpha(i);
            return texcol;
        }

        ENDCG

        }
    }
}
