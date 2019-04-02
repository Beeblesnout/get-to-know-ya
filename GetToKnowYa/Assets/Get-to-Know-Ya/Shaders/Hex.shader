Shader "Beeble/Hex"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _HexTex("Hex Info", 2D) = "black" {}
        _Speed("Cycle Speed", float) = 15
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

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
                float2 uv : TEXCOORD;
                fixed4 col : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD;
                fixed4 col : COLOR;
            };

            sampler2D _MainTex, _HexTex;
            half _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv.yx;
                o.col = v.col;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 info = tex2D(_HexTex, i.uv);

                info.g = atan2(0.5f - i.uv.x, 0.5f - i.uv.y) / ((3.141592653589f / 2.0f) + 0.5f);

                fixed r = info.r;
                fixed g = saturate((info.g + _Time.x * _Speed / 2) % .33) * 3;

                // g = abs((g - .5) * 2);
                fixed b = saturate((info.b + _Time.x * _Speed * 3) % 2);
                b = abs((b - .5) * 2);
                b = pow(b, 1.5) * 4;

                fixed4 end;
                end.rgb = i.col.rgb;
                end.a = r*b * i.col.a;
                return end;
            }
            ENDCG
        }
    }
}
