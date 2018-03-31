// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HealthShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_SecondBar("SecondBar", Range( 0 , 1)) = 0.3559519
		_FirstBar("FirstBar", Range( 0 , 1)) = 0.838034
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
			
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		
		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float _FirstBar;
			uniform float _SecondBar;
			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				float4 _Color0 = float4(0.08855966,0.9264706,0.198355,0);
				float2 uv1 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_9_0 = ( 1.0 - floor( ( uv1.x + _FirstBar ) ) );
				float4 appendResult30 = (float4(_Color0.r , _Color0.g , _Color0.b , temp_output_9_0));
				float blendOpSrc20 = temp_output_9_0;
				float blendOpDest20 = floor( ( uv1.x + _SecondBar ) );
				
				fixed4 c = ( appendResult30 + ( 1.0 - ( saturate( ( 0.5 - 2.0 * ( blendOpSrc20 - 0.5 ) * ( blendOpDest20 - 0.5 ) ) )) ) );
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14001
294;238;1522;788;510.1078;338.8853;1.6;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-669.4999,-123.2901;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;3;-602.5399,222.8294;Float;False;Property;_FirstBar;FirstBar;0;0;0.838034;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;2;-281.8337,-28.76444;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-644.4611,328.2664;Float;False;Property;_SecondBar;SecondBar;0;0;0.3559519;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-244.4386,309.065;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;4;-63.43685,-20.33738;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;9;195.3958,-35.64002;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;19;31.85883,324.4631;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;20;361.7639,161.2051;Float;True;Exclusion;True;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;249.4151,-218.1643;Float;False;Constant;_Color0;Color 0;0;0;0.08855966,0.9264706,0.198355,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;30;765.2555,-92.63105;Float;True;COLOR;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;21;638.1295,171.6664;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;557.9014,-349.3918;Float;True;2;2;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;955.1274,135.1902;Float;True;2;2;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMasterNode;0;1259.577,38.2642;Float;False;True;2;Float;ASEMaterialInspector;0;4;HealthShader;0f8ba0101102bb14ebf021ddadce9b49;Sprites Default;3;One;OneMinusSrcAlpha;0;One;Zero;Off;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;1
WireConnection;2;1;3;0
WireConnection;18;0;1;1
WireConnection;18;1;7;0
WireConnection;4;0;2;0
WireConnection;9;0;4;0
WireConnection;19;0;18;0
WireConnection;20;0;9;0
WireConnection;20;1;19;0
WireConnection;30;0;11;1
WireConnection;30;1;11;2
WireConnection;30;2;11;3
WireConnection;30;3;9;0
WireConnection;21;0;20;0
WireConnection;24;0;11;0
WireConnection;24;1;9;0
WireConnection;25;0;30;0
WireConnection;25;1;21;0
WireConnection;0;0;25;0
ASEEND*/
//CHKSM=FB792FA87C6A3BEAD976273437B9B5A6416201F0