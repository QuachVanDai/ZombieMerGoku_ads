Shader "Unlit/Shiny_Tun_Ver_2"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

        _ShinyColor ("Shiny Color", Color) = (1,1,1,1)
        _ShinyIntensity ("Shiny Intensity", Range(0,4)) = 1
        _ShinyWidth ("Shiny Width", Range(0.02,1)) = 0.25
        _ShinySoft ("Shiny Softness", Range(0.01,1)) = 0.15

        _ShinyDir ("Shiny Direction (x,y)", Vector) = (0.707,0.707,0,0)
        _ShinySpeed ("Shiny Speed", Range(0.1,3)) = 1
        _ShinyOffset ("Shiny Offset", Range(-2,2)) = -1

        _ShinyRunTime ("Shiny Run Time", Range(0.1,4)) = 1
        _ShinyDelay ("Shiny Delay", Range(0,6)) = 2

        _SpriteFade ("Sprite Fade", Range(0,1)) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                half2 uv : TEXCOORD0;
                half4 color : COLOR;
            };

            sampler2D _MainTex;
            half4 _ShinyColor;
            half _ShinyIntensity;
            half _ShinyWidth;
            half _ShinySoft;
            half2 _ShinyDir;
            half _ShinySpeed;
            half _ShinyOffset;
            half _ShinyRunTime;
            half _ShinyDelay;
            half _SpriteFade;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);

                // ----- Time mask (no branch) -----
                half cycle = _ShinyRunTime + _ShinyDelay;
                half t = frac(_Time.y / cycle) * cycle;
                half active = step(t, _ShinyRunTime);

                half progress = saturate(t / _ShinyRunTime);

                // ----- Shiny calc -----
                half2 uv = i.uv - 0.5;
                half pos = dot(uv, _ShinyDir)
                         + lerp(_ShinyOffset, -_ShinyOffset, progress)
                         * _ShinySpeed;

                half shiny = smoothstep(_ShinyWidth, _ShinyWidth - _ShinySoft, abs(pos));
                shiny *= active;

                col.rgb += shiny * _ShinyColor.rgb * _ShinyIntensity * col.a;

                col.rgb *= i.color.rgb;
                col.a *= i.color.a * _SpriteFade;

                return col;
            }
            ENDCG
        }
    }
    Fallback Off
}