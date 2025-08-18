Shader "UI/CircularHole"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,1)          // ������ɫ��ͨ���ڣ�
        _Radius("Radius", Range(0,2)) = 1.5          // Բ���뾶������Ļ��һ�� 0-1 �ƣ������������������� 2��
        _Feather("Feather", Range(0,0.5)) = 0.15     // ��Ե������Ͷ�
        _Center("Center (Viewport XY)", Vector) = (0.5, 0.5, 0, 0) // Բ�ģ��ӿ����꣩
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
            float4 _Center; // xy �õ�

            v2f vert(appdata v){
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv; // ȫ�� UI Image ����ʱ uv ��Ϊ [0,1] ��Ļ�ռ�
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 p = i.uv;                 // [0,1] ����
                float2 c = _Center.xy;           // Բ��
                float  d = distance(p, c);       // ��Բ�ľ���

                // ���㡰����ǿ�ȡ���Բ��Ϊ 0��͸������Բ��Ϊ 1����͸��������Ե�ữ
                float edge0 = _Radius - _Feather;
                float edge1 = _Radius + _Feather;
                float mask  = smoothstep(edge0, edge1, d); // d < edge0 -> 0, d > edge1 -> 1

                fixed4 col = _Color;
                col.a *= saturate(mask); // Բ��͸�������õ���Ϸ����Բ��Ϊ����ɫ
                return col;
            }
            ENDHLSL
        }
    }
}
