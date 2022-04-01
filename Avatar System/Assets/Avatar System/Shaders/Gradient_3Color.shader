Shader "Custom/Gradient_3Color" {
     Properties {
         [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
         _ColorTop ("Top Color", Color) = (1,1,1,1)
         _ColorMid ("Mid Color", Color) = (1,1,1,1)
         _ColorBot ("Bot Color", Color) = (1,1,1,1)
         _Middle ("Middle", Range(0.001, 0.999)) = 1
     }
 
     SubShader {
         Tags {"Queue"="Background"  "IgnoreProjector"="True" "RenderType"="Transparent"}
         LOD 100
 
         ZWrite On
 
         Pass {
         CGPROGRAM
         #pragma vertex vert  
         #pragma fragment frag
         #include "UnityCG.cginc"

         sampler2D _MainTex;
         fixed4 _ColorTop;
         fixed4 _ColorMid;
         fixed4 _ColorBot;
         float  _Middle;
 
         struct v2f {
             float4 pos : SV_POSITION;
             float4 texcoord : TEXCOORD0;
         };
 
         v2f vert (appdata_full v) {
             v2f o;
             o.pos = UnityObjectToClipPos (v.vertex);
             o.texcoord = v.texcoord;
             return o;
         }
 
         fixed4 frag (v2f i) : COLOR {
            // fixed4 c = lerp(_ColorBot, _ColorMid, i.texcoord.x / _Middle) * step(i.texcoord.x, _Middle);
            // c += lerp(_ColorMid, _ColorTop, (i.texcoord.x - _Middle) / (1 - _Middle)) * step(_Middle, i.texcoord.x);
             fixed4 c = tex2D(_MainTex, i.texcoord);
             clip(c.a - 1);
             c *= lerp(_ColorBot, _ColorMid, i.texcoord.x / _Middle) * step(i.texcoord.x, _Middle);
             c += lerp(_ColorMid, _ColorTop, (i.texcoord.x - _Middle) / (1 - _Middle)) * step(_Middle, i.texcoord.x);

             return c;
         }
         ENDCG
         }
     }
 }