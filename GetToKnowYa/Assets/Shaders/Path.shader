Shader "Beeble/Path"
{
    Properties
    {
        _Icon("Trail Icon", 2D) = "black" {}
        _Tiling("Icon Tiling", float) = 1
        _ScrollSpeed("Icon Scroll Speed", float) = 25
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
                float4 col : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD;
                float4 col : COLOR;
            };

            sampler2D _Icon;
            half _ScrollSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.col = v.col;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // return i.uv.x % 1;
                fixed sideMask = abs((i.uv.y - .5) * 2);
                fixed edge = smoothstep(.75, 1, sideMask) > 0;

                fixed icon = tex2D(_Icon, float2(i.uv.x + _Time.x * _ScrollSpeed, i.uv.y)).a;

                fixed4 end = fixed4(i.col.rgb, 0);
                end.a = lerp(end.a, .25, 1-smoothstep(.35, 1, sideMask-.1));
                end.a = lerp(end.a, .5, icon);
                end.a = lerp(end.a, .65, edge);
                end.a *= i.col.a;
                return saturate(end);
            }
            ENDCG
        }
    }
}
