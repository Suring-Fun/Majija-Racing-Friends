// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WaterOverLand"
{
    Properties
    {
    }
    SubShader
    {
        Tags {
            //"Queue" = "Transparent"
            "IgnoreProjector" = "True"
            //"RenderType" = "Transparent"
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
            

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                float2 pixel = i.uv * 128;
                float value1 = sin(pixel.x + _Time.y + cos(pixel.y + _Time.y * 2) * 0.5 * sin(_Time.y * 2)) * 0.5 + 1;
                float value2 = cos(pixel.y + _Time.y + sin(pixel.x + _Time.y * 2) * 0.5 * cos(_Time.y * 2)) * 0.5 + 1;
                float value3 = cos(pixel.y + _Time.y * 2 + sin(pixel.x + _Time.y * 2) * 0.5 * cos(_Time.y * 2)) * 0.5 + 1;
                float value4 = cos(pixel.y + _Time.y * 2 + sin(pixel.x + _Time.y * 2) * 0.5 * cos(_Time.y * 2)) * 0.5 + 1;
                //float value5 = cos(pixel.y + _Time.y * 4 + sin(pixel.x + _Time.y * 2) * 0.5 * cos(_Time.y * 2)) * 0.5 + 1;
                ///float value6 = cos(pixel.y + _Time.y * 4 + sin(pixel.x + _Time.y * 2) * 0.5 * cos(_Time.y * 2)) * 0.5 + 1;
                
                float value = value1 * ( 0.75 * 0.5) + value2 * (0.75 * 0.5) + value3 * 0.125 + value4 * 0.125;
                
                float4 color = float4(14.0/255.0, 65.0/255.0,120.0/255.0,1.0 / 0.75) * 0.75;

                if(value < 1.005) {
                    color = float4(100.0/255.0, 125.0/255.0,200.0/255.0,1.0 / 0.75) * 0.75;
                }
                
                return color;
                //return fixed4(value, value,value,1.0);
                //return fixed4(14.0/255.0, 65.0/255.0,120.0/255.0,1.0);
            }
            ENDCG
        }
    }
}
