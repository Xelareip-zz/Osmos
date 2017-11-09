Shader "Unlit/FakeZoom"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Main Color", Color) = (1,1,1,1)
		_MaxZoom("Max zoom", Float) = 100
		_CurrentZoom("Current zoom", Float) = 1
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _MaxZoom;
			float _CurrentZoom;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv = (o.uv - 0.5) * _CurrentZoom;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 scaledUv = i.uv / _MaxZoom;
				//i.uv.x = fract(i.uv.x * _MaxZoom);
				//i.uv.y = fract(i.uv.y * _MaxZoom);

				//return _Color;
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 zoomedCol = tex2D(_MainTex, scaledUv);
				float blend = _CurrentZoom / _MaxZoom;
				blend = blend * blend * blend;
				col = zoomedCol * blend + col * (1.0f - blend);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col * _Color;
			}
			ENDCG
		}
	}
}
