Shader "UI/CircularHole"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,1)          // 遮罩颜色（通常黑）
        _Radius("Radius", Range(0,2)) = 1.5          // 圆洞半径（以屏幕归一化 0-1 计，给点冗余所以上限设 2）
        _Feather("Feather", Range(0,0.5)) = 0.15     // 边缘过渡柔和度
        _Center("Center (Viewport XY)", Vector) = (0.5, 0.5, 0, 0) // 圆心（视口坐标）
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            float4 _Color;
            float  _Radius;
            float  _Feather;
            float4 _Center; // xy 用到

            v2f vert(appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv; // 全屏 UI Image 拉满时 uv 即为 [0,1] 屏幕空间
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 p = i.uv;                 // [0,1] 坐标
                float2 c = _Center.xy;           // 圆心
                float  d = distance(p, c);       // 到圆心距离

                // 计算“遮罩强度”：圆内为 0（透明），圆外为 1（不透明），边缘柔化
                float edge0 = _Radius - _Feather;
                float edge1 = _Radius + _Feather;
                float mask  = smoothstep(edge0, edge1, d); // d < edge0 -> 0, d > edge1 -> 1

                fixed4 col = _Color;
                col.a *= saturate(mask); // 圆内透明（看得到游戏），圆外为遮罩色
                return col;
            }
            ENDHLSL
        }
    }
}
