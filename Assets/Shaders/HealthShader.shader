// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HealthShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_Color0("Color 0", Color) = (0.08855966,0.9264706,0.198355,0)
		_Texture("Texture", 2D) = "white" {}
		_FirstBar("FirstBar", Range( 0 , 1)) = 0.838034
		_Alpha("Alpha", Range( 0 , 1)) = 0
		_SecondBar("SecondBar", Range( 0 , 1)) = 0.3559519
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
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
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]


		Pass
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			

			
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
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform float4 _Color0;
			uniform float _FirstBar;
			uniform float _SecondBar;
			uniform sampler2D _Texture;
			uniform float4 _Texture_ST;
			uniform float _Alpha;
			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				OUT.worldPosition = IN.vertex;
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				float2 uv1 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_9_0 = ( 1.0 - floor( ( uv1.x + _FirstBar ) ) );
				float4 appendResult30 = (float4(_Color0.r , _Color0.g , _Color0.b , temp_output_9_0));
				float blendOpSrc20 = temp_output_9_0;
				float blendOpDest20 = floor( ( uv1.x + _SecondBar ) );
				float2 uv_Texture = IN.texcoord.xy * _Texture_ST.xy + _Texture_ST.zw;
				float4 tex2DNode32 = tex2D( _Texture, uv_Texture );
				float4 appendResult39 = (float4(tex2DNode32.r , tex2DNode32.g , tex2DNode32.b , ( tex2DNode32.a * _Alpha )));
				float4 blendOpSrc37 = ( appendResult30 + ( 1.0 - ( saturate( ( 0.5 - 2.0 * ( blendOpSrc20 - 0.5 ) * ( blendOpDest20 - 0.5 ) ) )) ) );
				float4 blendOpDest37 = appendResult39;
				
				half4 color = ( saturate( ( blendOpSrc37 * blendOpDest37 ) ));
				
				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14001
7;29;1522;788;1357.918;233.4127;1.204879;True;True
Node;AmplifyShaderEditor.RangedFloatNode;3;-602.5399,222.8294;Float;False;Property;_FirstBar;FirstBar;1;0;0.838034;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1123.74,-132.9846;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-644.4611,328.2664;Float;False;Property;_SecondBar;SecondBar;2;0;0.3559519;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;2;-281.8337,-28.76444;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-244.4386,309.065;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;4;-63.43685,-20.33738;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;9;195.3958,-35.64002;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;19;31.85883,324.4631;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;32;560.6106,-413.9661;Float;True;Property;_Texture;Texture;1;0;Assets/Sprites/HPHUD_mask.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;588.6614,-194.1617;Float;False;Property;_Alpha;Alpha;2;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;20;361.7639,161.2051;Float;True;Exclusion;True;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;11;302.2151,-290.1643;Float;False;Property;_Color0;Color 0;0;0;0.08855966,0.9264706,0.198355,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;951.188,-242.7354;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;21;638.1295,171.6664;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;765.2555,-92.63105;Float;True;COLOR;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;972.3001,127.6773;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;39;1139.56,-371.8702;Float;False;COLOR;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;37;1165.425,-170.3521;Float;True;Multiply;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMasterNode;31;1358.318,32.89783;Float;False;True;2;Float;ASEMaterialInspector;0;3;HealthShader;5056123faa0c79b47ab6ad7e8bf059a4;ASETemplateShaders/UIDefault;2;SrcAlpha;OneMinusSrcAlpha;0;SrcAlpha;OneMinusSrcAlpha;Off;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;1
WireConnection;2;1;3;0
WireConnection;18;0;1;1
WireConnection;18;1;7;0
WireConnection;4;0;2;0
WireConnection;9;0;4;0
WireConnection;19;0;18;0
WireConnection;20;0;9;0
WireConnection;20;1;19;0
WireConnection;40;0;32;4
WireConnection;40;1;38;0
WireConnection;21;0;20;0
WireConnection;30;0;11;1
WireConnection;30;1;11;2
WireConnection;30;2;11;3
WireConnection;30;3;9;0
WireConnection;25;0;30;0
WireConnection;25;1;21;0
WireConnection;39;0;32;1
WireConnection;39;1;32;2
WireConnection;39;2;32;3
WireConnection;39;3;40;0
WireConnection;37;0;25;0
WireConnection;37;1;39;0
WireConnection;31;0;37;0
ASEEND*/
//CHKSM=EC7B59E555B7BA11CC707BB7834649125ECA5F42