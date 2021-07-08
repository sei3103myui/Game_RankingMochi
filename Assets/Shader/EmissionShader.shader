Shader "Unlit/EmissionShader"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _MainColor("Color", Color) = (1,1,1,1)
        _EmissionMap("Emission Map", 2D) = "black" {}               //�ǉ�
        [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)    //�ǉ�
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            float4 _MainColor;

            uniform sampler2D _EmissionMap;    //�ǉ�
            float4 _EmissionColor;             //�ǉ�

            fixed4 frag(v2f_img i) : SV_Target
            {
                fixed4 albedo = tex2D(_MainTex, i.uv) * _MainColor;

            //������̃L���B�ʏ��albedo��Emission���̐F�𑫂��Ă���
            return albedo + tex2D(_EmissionMap, i.uv) * _EmissionColor;
        }
        ENDCG
        }
    }
}
