// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WaterUnderCar"
{
    Properties
    {
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
                fixed noise = (sin((i.uv.y + i.uv.x) * 8 + _Time.y * 3 + cos(i.uv.y * 8 + _Time.y * 3 * 2)) * 0.5 + 1) * 0.05;
                
                fixed4 col = fixed4(0, 0, 0, 0);
                i.uv.x -= 0.5;
                i.uv.y -= 0.5;
                fixed radius = i.uv.x * i.uv.x + i.uv.y * i.uv.y;

                fixed delta = (0.1 + noise - radius);

                if(delta * delta < 0.0025)
                    col = float4(0.8,0.85,0.9,1.0);


                // fixed4 col = tex2D(_MainTex, i.uv);
                // fixed threshold = (sin(_Time.y * 2) * 0.5 + 1) * 0.15 + 0.5;
                // fixed threshold2 = (sin((_Time.y) * 2 - 1.5) * 0.5 + 1) * 0.15 + 0.5;
                // //fixed shadow = (sin((_Time.y) * 2) * 0.5 + 1) * 0.15;
                // fixed offset = (sin(_Time.y * 2 * 1.5) * 0.5 + 1) * 0.1;
                // if(col.a > threshold + offset + noise) {
                //     float2 pixel = i.uv * 128;
                //     float value1 = sin(pixel.x + _Time.y * 0.5 + cos(pixel.y + _Time.y * 2 * 0.5) * 0.5 * sin(_Time.y * 2 * 0.5)) * 0.5 + 1;
                //     float value2 = cos(pixel.y + _Time.y * 0.5 + sin(pixel.x + _Time.y * 2 * 0.5) * 0.5 * cos(_Time.y * 2 * 0.5)) * 0.5 + 1;
                //     float value3 = cos(pixel.y + _Time.y * 2 * 0.5 + sin(pixel.x + _Time.y * 2 * 0.5) * 0.5 * cos(_Time.y * 2 * 0.5)) * 0.5 + 1;
                //     float value4 = cos(pixel.y + _Time.y * 2 * 0.5 + sin(pixel.x + _Time.y * 2 * 0.5) * 0.5 * cos(_Time.y * 2 * 0.5)) * 0.5 + 1;
                //     //float value5 = cos(pixel.y + _Time.y * 4 + sin(pixel.x + _Time.y * 2) * 0.5 * cos(_Time.y * 2)) * 0.5 + 1;
                //     ///float value6 = cos(pixel.y + _Time.y * 4 + sin(pixel.x + _Time.y * 2) * 0.5 * cos(_Time.y * 2)) * 0.5 + 1;
                    
                //     float value = (value1 * ( 0.75 * 0.5) + value2 * (0.75 * 0.5) + value3 * 0.125 + value4 * 0.125) * ((col.a - (0.4 + noise * 0.1)) / (0.6 + noise * 0.1));
                    
                    
                //     if(value < 1.005) {
                //         col = float4(100.0/255.0, 125.0/255.0,200.0/255.0,1.0 / 0.65) * 0.65;
                //     }
                //     else {
                //         col = fixed4(14.0/255.0, 65.0/255.0,120.0/255.0,1.0);
                //     }
                // }
                // else if(col.a > threshold + noise) {
                //     col = float4(0.8,0.85,0.9,1.0);
                // }
                // else if(col.a > threshold2 + noise) {
                //     col = float4(0,0,0,0.1);
                // }
                // else {
                //     col.a = 0.0;
                // }
                return col;
            }
            ENDCG
        }
    }
}
