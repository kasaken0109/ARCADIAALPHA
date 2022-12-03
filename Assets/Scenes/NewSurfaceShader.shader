// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OrderedDithering" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _MatrixWidth("Dither Matrix Width/Height", int) = 4
        _MatrixTex("Dither Matrix", 2D) = "black" {}
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Lambert vertex:vert finalcolor:mycolor  

            sampler2D _MainTex;
            int _MatrixWidth;
            sampler2D _MatrixTex;

            struct Input {
                float2 uv_MainTex;
                float4 scrPos;
            };

            void vert(inout appdata_full v, out Input o) {
                UNITY_INITIALIZE_OUTPUT(Input,o);
                float4 pos = UnityObjectToClipPos(v.vertex);
                o.scrPos = ComputeScreenPos(pos);
            }

            void surf(Input IN, inout SurfaceOutput o) {
                half4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }

            void mycolor(Input IN, SurfaceOutput o, inout fixed4 color) {
                // RGB -> HSV �ϊ�  
                float value = max(color.r, max(color.g, color.b));

                // �X�N���[�����ʂɑ΂��ă}�g���b�N�X�e�N�X�`����~���l�߂�  
                float2 uv_MatrixTex = IN.scrPos.xy / IN.scrPos.w * _ScreenParams.xy / _MatrixWidth;

                float threshold = tex2D(_MatrixTex, uv_MatrixTex).r;
                fixed3 binary = ceil(value - threshold);
                color.rgb = binary;
                color.a = 1.0f;
            }
            ENDCG
    }
        FallBack "Diffuse"
}
