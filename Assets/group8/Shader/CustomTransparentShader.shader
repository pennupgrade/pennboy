Shader "Custom/TransparentNoFlicker"
{
    SubShader
    {
        Tags { "Queue"="Overlay" } // Make sure it's rendered after opaque objects
        
        Pass
        {
            ZWrite Off           // Disable depth writing
            ZTest Always         // Always pass the depth test
            Blend SrcAlpha OneMinusSrcAlpha  // Standard transparency blending

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // Vertex shader
            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // Transform vertex position
                o.color = v.color; // Pass color data to fragment shader
                return o;
            }

            // Fragment shader
            half4 frag(v2f i) : SV_Target
            {
                return i.color; // Output color (you can modify this for your texture)
            }
            ENDCG
        }
    }
}
