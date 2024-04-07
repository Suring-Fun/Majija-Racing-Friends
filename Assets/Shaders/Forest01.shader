// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Forest01"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                // (multiply with model*view*projection matrix)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // just pass the texture coordinate
                o.uv = v.uv;
                return o;
            }
            
            // texture we will sample
            sampler2D _MainTex;

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                fixed4 col = tex2D(_MainTex, i.uv);

                float offset = (cos(i.uv.x * 300 + _Time.y * 6) + sin(i.uv.y * 300 + _Time.y * 6)) * 0.005;
                float offset2 = (cos(i.uv.x * 300 + _Time.y * 6 + 5) + sin(i.uv.y * 300 + _Time.y * 6 + 5)) * 0.005;


                fixed4 col2 = (0, 0, 0, 0);
                
                if(col.a > 0.5 + offset) {
                    col2 = float4(49 / 255.0, 80 / 255.0, 36 / 255.0, 1);
                }

                if(col.r > 0.5 + offset2) {
                    col2 = float4(64 / 255.0, 115 / 255.0, 43 / 255.0, 1);
                }

                if(col.g > 0.5 + offset) {
                    col2 = float4(71 / 255.0, 132 / 255.0, 46 / 255.0, 1);
                }

                if(col.b > 0.5 + offset2) {
                    col2 = float4(86 / 255.0, 147 / 255.0, 61 / 255.0, 1);
                }

                clip(col2.a - 0.5);
                return col2;
            }
            ENDCG
        }
    }
}
