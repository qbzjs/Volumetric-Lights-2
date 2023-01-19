// MADE BY MATTHIEU HOULLIER
// Copyright 2022 BRUTE FORCE, all rights reserved.
// You are authorized to use this work if you have purchased the asset.
// Mail me at bruteforcegamesstudio@gmail.com if you have any questions or improvements you need.
Shader "BruteForce/URP/SnowIceURP" {

	Properties{

		[Header(IIIIIIII          Snow Textures          IIIIIIII)]
		[Space]
		_MainTex("Snow Albedo", 2D) = "white" {}
		_MainTexMult("Snow Albedo Multiplier", Range(0,2)) = 0.11
		[MainColor][HDR]_Color("Snow Tint", Color) = (0.77,0.86,0.91,1)
		_OverallScale("Overall Scale", Float) = 1
		[Space]
		[NoScaleOffset]_BumpMap("Snow Bumpmap", 2D) = "white" {}
		_NormalMultiplier("Snow Bumpmap Multiplier", Range(0,2)) = 0.4
		_SnowNormalScale("Snow Bumpmap Scale", Range(0,2)) = 1

		[Space(20)]
		[NoScaleOffset]_SpecGlossMap("Snow Specular", 2D) = "black" {}
		[NoScaleOffset]_LittleSpec("Snow Little Spec", 2D) = "black" {}
		_LittleSpecForce("Little Spec Multiplier", Float) = 0.5

		[NoScaleOffset]_SnowHeight("Snow Displacement Texture", 2D) = "white" {}
		_HeightScale("Displacement Scale", Float) = 0.33
		_DisplacementStrength("Displacement Strength", Float) = 0.3
		_DisplacementOffset("Displacement Offset", Float) = 0.1
		_DisplacementColorMult("Displacement Color Multiplier", Float) = 0.95
		_DisplacementShadowMult("Displacement Shadow Multiplier",  Range(0,2)) = 0.56
		_UpVector("Up Vector", Float) = 1
		_NormalVector("Normal Vector", Float) = 0

		[Space(20)]
		[NoScaleOffset]_SnowTransition("Snow Transition", 2D) = "black" {}
		_TransitionScale("Transition Scale", Float) = 0.73
		_TransitionPower("Transition Power", Float) = 0.22
		_TransitionColor("Transition Color (additive only)", Color) = (1,1,1,1)


		[Space(10)]
		[Header(IIIIIIII          Snow Values          IIIIIIII)]
		[Space(10)]
		_MountColor("Mount Color", color) = (0.12,0.12,0.121,1)
		_BotColor("Dig Color", color) = (0.71,0.87,0.91,0)
		_NormalRTDepth("Normal Effect Depth", Range(0,3)) = 0.12
		_NormalRTStrength("Normal Effect Strength", Range(0,4)) = 2.2
		_AddSnowStrength("Mount Snow Strength", Range(0,3)) = 0.52
		_RemoveSnowStrength("Dig Snow Strength", Range(0,3)) = 0.5
		_SnowScale("Snow Scale", float) = 1

		[Space(10)]
		[Header(IIIIIIII          Ice Textures          IIIIIIII)]
		[Space(10)]
		[NoScaleOffset]_IceTex("Ice Albedo", 2D) = "white" {}
		_IceTint("Ice Texture Tint", Color) = (0.14,0.35,0.49,1)
		[Space]
		[NoScaleOffset]_ParallaxLayers("Parallax Ice Texture", 2D) = "black" {}
		_ParallaxStrength("Parallax Fade Strength", vector) = (-1.04, 0.125, -0.13, 0.1)
		_OffsetScale("Parallax Offset Scale", float) = 0.14
		[NoScaleOffset]_NormalTex("Ice Normal Texture", 2D) = "black" {}
		_NormalScale("Ice Normal Multiplier", Range(0,1)) = 0.766
		[NoScaleOffset]_Roughness("Ice Roughness Texture", 2D) = "black" {}
		[NoScaleOffset]_UnderWaterFish("Under Water Fish", 2D) = "black" {}
		_FishScale("Fish Scale", float) = 1
		_FishSpeed("Fish Speed", float) = 1
		_FishIntensity("Fish Intensity", float) = 1
		_FishAngle("Fish Angle", float) = 0
		_FishMaskIntensity("Fish Mask Intensity", Range(0,1)) = 0.25

		[Space(10)]
		[Header(IIIIIIII          Ice Values          IIIIIIII)]
		[Space(10)]
		[HDR]_ParalaxColor("Parallax Color", Color) = (0,0,0,1)
		_ParalaxColorScale("Parallax Color Scale", Range(-2,2)) = -0.88
		_IceTrail("Ice Trail Color", Color) = (0.40,0.1,0.01,1)
		_IceScale("Ice Scale", float) = 1
		_TransparencyValue("Ice Transparency", Range(0,1)) = 1

		[Space(10)]
		[Header(IIIIIIII          Custom Fog          IIIIIIII)]
		[Space(10)]
		[NoScaleOffset]_FogTex("Fog Texture", 2D) = "black" {}
		[NoScaleOffset]_FlowTex("Flow Texture", 2D) = "black" {}
		_FlowMultiplier("Flow Multiplier", Range(0,1)) = 0.3
		_FogIntensity("Fog Intensity", Range(0,1)) = 0.3
		_FogScale("Fog Scale", float) = 1
		_FogDirection("Fog Direction", vector) = (1, 0.3, 2, 0)

		[Space(10)]
		[Header(IIIIIIII          Lighting          IIIIIIII)]
		[Space(10)]
		_ProjectedShadowColor("Projected Shadow Color",Color) = (0.17 ,0.56 ,0.1,1)
		_SpecColor("Specular Color", Color) = (1,1,1,1)
		_SpecForce("Specular Force", Float) = 3
		_RoughnessStrength("Ice Roughness Strength", Float) = 1.75
		_ShininessIce("Ice Shininess", Float) = 10
		_ShininessSnow("Snow Shininess", Float) = 25
		[Space]
		_LightOffset("Light Offset", Range(0, 1)) = 0.2
		_LightHardness("Light Hardness", Range(0, 1)) = 0.686
		_ReflectionOpacity("Reflection Opacity", Float) = 0.25
		_ReflectionStrength("Rim Ice Strength", Range(0, 2)) = 0.07
		_ReflectionColor("Rim Ice Color", Color) = (0,0.1,0.15,0.8)
		_RimColor("Rim Snow Color", Color) = (0.03,0.03,0.03,0)
		_LightIntensity("Additional Lights Intensity", Range(0.00, 4)) = 1

		[Space(10)]
		[Header(IIIIIIII          Tessellation          IIIIIIII)]
		[Space(10)]
		_TessellationUniform("Tessellation Ice", Range(1, 16)) = 1
		_TessellationEdgeLength("Tessellation Snow", Range(0, 100)) = 50

		[Space(10)]
		[Header(IIIIIIII          Pragmas          IIIIIIII)]
		[Space(10)]
		[Toggle(IS_ICE)] _ISICE("Is Ice", Float) = 0
		[Toggle(IS_ADD)] _ISADD("Is Additive Snow", Float) = 0
		[HideInInspector][Toggle(USE_INTER)] _USEINTER("Use Intersection", Float) = 0
		[Toggle(USE_AL)] _UseAmbientLight("Use Ambient Light", Float) = 1
		[Toggle(USE_LS)] _UseSunReflection("Use Sun Reflection", Float) = 1
		[Toggle(USE_RT)] _USERT("Use RT", Float) = 1
		[Toggle(IS_T)] _IST("Is Terrain", Float) = 0
		[Toggle(USE_VR)] _UseVR("Use For VR", Float) = 0
		[Toggle(USE_WC)] _USEWC("Use World Displacement", Float) = 1

			// TERRAIN PROPERTIES //
			[HideInInspector] _Control0("Control0 (RGBA)", 2D) = "white" {}
			[HideInInspector] _Control1("Control1 (RGBA)", 2D) = "white" {}
			// Textures
			[HideInInspector] _Splat0("Layer 0 (R)", 2D) = "white" {}
			[HideInInspector] _Splat1("Layer 1 (G)", 2D) = "white" {}
			[HideInInspector] _Splat2("Layer 2 (B)", 2D) = "white" {}
			[HideInInspector] _Splat3("Layer 3 (A)", 2D) = "white" {}
			[HideInInspector] _Splat4("Layer 4 (R)", 2D) = "white" {}
			[HideInInspector] _Splat5("Layer 5 (G)", 2D) = "white" {}
			[HideInInspector] _Splat6("Layer 6 (B)", 2D) = "white" {}
			[HideInInspector] _Splat7("Layer 7 (A)", 2D) = "white" {}

			// Normal Maps
			[HideInInspector] _Normal0("Normal 0 (R)", 2D) = "bump" {}
			[HideInInspector] _Normal1("Normal 1 (G)", 2D) = "bump" {}
			[HideInInspector] _Normal2("Normal 2 (B)", 2D) = "bump" {}
			[HideInInspector] _Normal3("Normal 3 (A)", 2D) = "bump" {}
			[HideInInspector] _Normal4("Normal 4 (R)", 2D) = "bump" {}
			[HideInInspector] _Normal5("Normal 5 (G)", 2D) = "bump" {}
			[HideInInspector] _Normal6("Normal 6 (B)", 2D) = "bump" {}
			[HideInInspector] _Normal7("Normal 7 (A)", 2D) = "bump" {}

			// Normal Scales
			[HideInInspector] _NormalScale0("Normal Scale 0 ", Float) = 1
			[HideInInspector] _NormalScale1("Normal Scale 1 ", Float) = 1
			[HideInInspector] _NormalScale2("Normal Scale 2 ", Float) = 1
			[HideInInspector] _NormalScale3("Normal Scale 3 ", Float) = 1
			[HideInInspector] _NormalScale4("Normal Scale 4 ", Float) = 1
			[HideInInspector] _NormalScale5("Normal Scale 5 ", Float) = 1
			[HideInInspector] _NormalScale6("Normal Scale 6 ", Float) = 1
			[HideInInspector] _NormalScale7("Normal Scale 7 ", Float) = 1

				// Mask Maps
				[HideInInspector] _Mask0("Mask 0 (R)", 2D) = "bump" {}
				[HideInInspector] _Mask1("Mask 1 (G)", 2D) = "bump" {}
				[HideInInspector] _Mask2("Mask 2 (B)", 2D) = "bump" {}
				[HideInInspector] _Mask3("Mask 3 (A)", 2D) = "bump" {}
				[HideInInspector] _Mask4("Mask 4 (R)", 2D) = "bump" {}
				[HideInInspector] _Mask5("Mask 5 (G)", 2D) = "bump" {}
				[HideInInspector] _Mask6("Mask 6 (B)", 2D) = "bump" {}
				[HideInInspector] _Mask7("Mask 7 (A)", 2D) = "bump" {}

				// specs color
				[HideInInspector] _Specular0("Specular 0 (R)", Color) = (1,1,1,1)
				[HideInInspector] _Specular1("Specular 1 (G)", Color) = (1,1,1,1)
				[HideInInspector] _Specular2("Specular 2 (B)", Color) = (1,1,1,1)
				[HideInInspector] _Specular3("Specular 3 (A)", Color) = (1,1,1,1)
				[HideInInspector] _Specular4("Specular 4 (R)", Color) = (1,1,1,1)
				[HideInInspector] _Specular5("Specular 5 (G)", Color) = (1,1,1,1)
				[HideInInspector] _Specular6("Specular 6 (B)", Color) = (1,1,1,1)
				[HideInInspector] _Specular7("Specular 7 (A)", Color) = (1,1,1,1)

					// Metallic
					[HideInInspector] _Metallic0("Metallic0", Float) = 0
					[HideInInspector] _Metallic1("Metallic1", Float) = 0
					[HideInInspector] _Metallic2("Metallic2", Float) = 0
					[HideInInspector] _Metallic3("Metallic3", Float) = 0
					[HideInInspector] _Metallic4("Metallic4", Float) = 0
					[HideInInspector] _Metallic5("Metallic5", Float) = 0
					[HideInInspector] _Metallic6("Metallic6", Float) = 0
					[HideInInspector] _Metallic7("Metallic7", Float) = 0

					[HideInInspector] _Splat0_ST("Size0", Vector) = (1,1,0)
					[HideInInspector] _Splat1_ST("Size1", Vector) = (1,1,0)
					[HideInInspector] _Splat2_ST("Size2", Vector) = (1,1,0)
					[HideInInspector] _Splat3_ST("Size3", Vector) = (1,1,0)
					[HideInInspector] _Splat4_STn("Size4", Vector) = (1,1,0)
					[HideInInspector] _Splat5_STn("Size5", Vector) = (1,1,0)
					[HideInInspector] _Splat6_STn("Size6", Vector) = (1,1,0)
					[HideInInspector] _Splat7_STn("Size7", Vector) = (1,1,0)

					[HideInInspector] _TerrainScale("Terrain Scale", Vector) = (1, 1 ,0)
					// TERRAIN PROPERTIES //
	}


	HLSLINCLUDE
					// Reused functions //
					// TESSELLATION DATA //


#if defined(SHADER_API_D3D12) ||defined(SHADER_API_D3D11) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE) || defined(SHADER_API_VULKAN) || defined(SHADER_API_METAL) || defined(SHADER_API_PSSL)
#define UNITY_CAN_COMPILE_TESSELLATION 1
#   define UNITY_domain                 domain
#   define UNITY_partitioning           partitioning
#   define UNITY_outputtopology         outputtopology
#   define UNITY_patchconstantfunc      patchconstantfunc
#   define UNITY_outputcontrolpoints    outputcontrolpoints
#endif



					float _TessellationUniform;
				float _TessellationEdgeLength;

#pragma shader_feature IS_T
#pragma shader_feature IS_ICE
#pragma shader_feature USE_VR

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

#ifdef IS_T

				// TERRAIN DATA //
				sampler2D _Control0;
				sampler2D _Control1;
				half4 _Specular0, _Specular1, _Specular2, _Specular3, _Specular4, _Specular5, _Specular6, _Specular7;
				float4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST, _Splat4_STn, _Splat5_STn, _Splat6_STn, _Splat7_STn;
				half _Metallic0, _Metallic1, _Metallic2, _Metallic3, _Metallic4, _Metallic5, _Metallic6, _Metallic7;
				half _NormalScale0, _NormalScale1, _NormalScale2, _NormalScale3, _NormalScale4, _NormalScale5, _NormalScale6, _NormalScale7;
				Texture2D _Splat0, _Splat1, _Splat2, _Splat3, _Splat4, _Splat5, _Splat6, _Splat7;
				Texture2D _Normal0, _Normal1, _Normal2, _Normal3, _Normal4, _Normal5, _Normal6, _Normal7;
				sampler2D _Mask0, _Mask1, _Mask2, _Mask3, _Mask4, _Mask5, _Mask6, _Mask7;
				float3 _TerrainScale;
				// TERRAIN DATA //
#endif
				SamplerState my_linear_repeat_sampler;
				SamplerState my_bilinear_repeat_sampler;
				SamplerState my_trilinear_repeat_sampler;
				SamplerState my_linear_clamp_sampler;


				struct TessellatedVert {
					float4 vertex : INTERNALTESSPOS;
					float3 normal : NORMAL;
					float4 tangent : TANGENT;
					float2 uv : TEXCOORD0;
					float4 color : COLOR;
#ifdef USE_VR
					UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
#ifdef IS_ADD
#ifdef USE_INTER
						float2 uv3 : TEXCOORD3;
					float2 uv4 : TEXCOORD4;
					float2 uv6 : TEXCOORD6;
					float2 uv7 : TEXCOORD7;
#endif
#endif
				};

				struct TessellationFactors {
					float edge[3] : SV_TessFactor;
					float inside : SV_InsideTessFactor;
				};

				float TessellationEdgeFactor(float3 p0, float3 p1) {
#if defined(_TESSELLATION_EDGE)
					float edgeLength = distance(p0, p1);

					float3 edgeCenter = (p0 + p1) * 0.5;
					float viewDistance = distance(edgeCenter, _WorldSpaceCameraPos);

					float tessellationFactor = 0;
					if (_TessellationEdgeLength <= 0.01)
					{
						tessellationFactor = 5000;
					}
					else
					{
						tessellationFactor = 300 / _TessellationEdgeLength;
					}
					return edgeLength * _ScreenParams.y /
						(tessellationFactor * viewDistance);
#else
					return _TessellationUniform;
#endif
				}

				TessellationFactors MyPatchConstantFunction(
					InputPatch<TessellatedVert, 3> patch
				) {
					float3 p0 = mul(unity_ObjectToWorld, patch[0].vertex).xyz;
					float3 p1 = mul(unity_ObjectToWorld, patch[1].vertex).xyz;
					float3 p2 = mul(unity_ObjectToWorld, patch[2].vertex).xyz;
					TessellationFactors f;
					f.edge[0] = TessellationEdgeFactor(p1, p2);
					f.edge[1] = TessellationEdgeFactor(p2, p0);
					f.edge[2] = TessellationEdgeFactor(p0, p1);

#ifdef IS_ICE
					f.inside = 1;
#else
#ifdef IS_T
					half4 splat_control = tex2Dlod(_Control0, float4(patch[0].uv, 0, 0));
					if (
						splat_control.r * (1 - _Metallic0) + splat_control.g * (1 - _Metallic1) + splat_control.b * (1 - _Metallic2) + splat_control.a * (1 - _Metallic3)
						< 0.5)
					{
						f.inside = 1;
					}
					else
					{
						f.inside =
							(TessellationEdgeFactor(p1, p2) +
								TessellationEdgeFactor(p2, p0) +
								TessellationEdgeFactor(p0, p1)) * (1 / 3.0);
					}
#else
					if ((patch[0].color.g + patch[0].color.b) / 2 < 0.5 || (patch[0].color.b > 0.95 && patch[0].color.g < 0.05))
					{
						f.inside = _TessellationUniform;
					}
					else
					{
						f.inside =
							(TessellationEdgeFactor(p1, p2) +
								TessellationEdgeFactor(p2, p0) +
								TessellationEdgeFactor(p0, p1)) * (1 / 3.0);
					}

#ifdef IS_ADD
					f.inside =
						(TessellationEdgeFactor(p1, p2) +
							TessellationEdgeFactor(p2, p0) +
							TessellationEdgeFactor(p0, p1)) * (1 / 3.0);
#endif

#endif
#endif
					return f;
				}

				[UNITY_domain("tri")]
				[UNITY_outputcontrolpoints(3)]
				[UNITY_outputtopology("triangle_cw")]
				[UNITY_partitioning("fractional_odd")]
				[UNITY_patchconstantfunc("MyPatchConstantFunction")]
				TessellatedVert Hull(
					InputPatch<TessellatedVert, 3> patch,
					uint id : SV_OutputControlPointID
				) {
					return patch[id];
				}

				float UNITY_PI = 3.1416;

				float4 RotateAroundZInDegrees(float4 vertex, float degrees)
				{
					
					float alpha = degrees * UNITY_PI / 180.0;
					float sina, cosa;
					sincos(alpha, sina, cosa);
					float2x2 m = float2x2(cosa, -sina, sina, cosa);
					return float4(mul(m, vertex.zy), vertex.xw).zyxw;
				}
				float4 RotateAroundXInDegrees(float4 vertex, float degrees)
				{
					float alpha = degrees * UNITY_PI / 180.0;
					float sina, cosa;
					sincos(alpha, sina, cosa);
					float2x2 m = float2x2(cosa, -sina, sina, cosa);
					return float4(mul(m, vertex.xy), vertex.zw).xyzw;
				}

				float4 multQuat(float4 q1, float4 q2) {
					return float4(
						q1.w * q2.x + q1.x * q2.w + q1.z * q2.y - q1.y * q2.z,
						q1.w * q2.y + q1.y * q2.w + q1.x * q2.z - q1.z * q2.x,
						q1.w * q2.z + q1.z * q2.w + q1.y * q2.x - q1.x * q2.y,
						q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
						);
				}

				float3 rotateVector(float4 quat, float3 vec) {
					// https://twistedpairdevelopment.wordpress.com/2013/02/11/rotating-a-vector-by-a-quaternion-in-glsl/
					float4 qv = multQuat(quat, float4(vec, 0.0));
					return multQuat(qv, float4(-quat.x, -quat.y, -quat.z, quat.w)).xyz;
				}

				ENDHLSL

					SubShader{

						Pass {
							Tags {
								"RenderPipeline" = "UniversalRenderPipeline"
							}
								Blend SrcAlpha OneMinusSrcAlpha

							HLSLPROGRAM

							#pragma target 4.6

							#pragma shader_feature _TESSELLATION_EDGE

							#pragma multi_compile _ LOD_FADE_CROSSFADE

							//#pragma multi_compile_fwdbase
							#pragma multi_compile_fog

							#pragma vertex tessellatedVert
							#pragma fragment frag
							#pragma hull Hull
							#pragma domain Domain

							#define FORWARD_BASE_PASS
							#pragma shader_feature USE_AL
							#pragma shader_feature USE_RT
							#pragma shader_feature IS_ADD
							#pragma shader_feature USE_INTER
							#pragma shader_feature USE_WC
							#pragma shader_feature USE_LS


							#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
							#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
							//#include "UnityPBSLighting.cginc"
							//#include "AutoLight.cginc"
							#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
							#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
							#pragma multi_compile _ _SHADOWS_SOFT
							#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
							#pragma multi_compile _ LIGHTMAP_ON

					struct VertexData //appdata
					{
						float4 vertex : POSITION;
						float3 normal : NORMAL;
						float4 tangent : TANGENT;
						float2 uv : TEXCOORD0;
						float4 color : COLOR;

						float4 shadowCoord : TEXCOORD1;
						float fogCoord : TEXCOORD2;
#ifdef USE_VR
							UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
#ifdef IS_ADD
#ifdef USE_INTER
								float2 uv3 : TEXCOORD3;
							float2 uv4 : TEXCOORD4;
							float2 uv6 : TEXCOORD6;
							float2 uv7 : TEXCOORD7;
#endif
#endif
					};

					struct InterpolatorsVertex
					{
						float4 vertex : SV_POSITION;
						float3 normal : TEXCOORD1;
						float4 tangent : TANGENT;
						float4 uv : TEXCOORD0;
						float4 color : COLOR;
						float3 worldPos : TEXCOORD2;
						float3 viewDir: POSITION1;
						float3 normalDir: TEXCOORD3;

						float4 shadowCoord : TEXCOORD4;
						float fogCoord : TEXCOORD5;
#ifdef USE_VR
							UNITY_VERTEX_OUTPUT_STEREO
#endif
					};

					//sampler2D  _DetailTex, _DetailMask;
					sampler2D  _DetailTex;
					//float4 _MainTex_ST, _DetailTex_ST;
					float4 _MainTex_ST, _DetailTex_ST;

					//sampler2D _NormalMap, _DetailNormalMap;
					sampler2D _NormalMap;
					//float _BumpScale, _DetailBumpScale;

					half4 _Color;

					// Render Texture Effects //
					uniform sampler2D _GlobalEffectRT;
					uniform float3 _Position;
					uniform float _OrthographicCamSize;
					uniform sampler2D _GlobalEffectRTAdditional;
					uniform float3 _PositionAdd;
					uniform float _OrthographicCamSizeAdditional;

					sampler2D _MainTex;
					//sampler2D _SpecGlossMap;
					sampler2D _SpecGlossMapNew;
					//sampler2D _BumpMap;
					sampler2D _LittleSpec;
					Texture2D _UnderWaterFish;

					half4 _MountColor;
					half4 _BotColor;

					float _SpecForce, _LittleSpecForce, _UpVector, _NormalVector, _IceScale, _SnowScale, _TransparencyValue;
					float _NormalRTDepth, _NormalRTStrength, _AddSnowStrength, _RemoveSnowStrength, _DisplacementStrength, _NormalMultiplier;

					//ICE Variables
					Texture2D _IceTex;
					Texture2D _NormalTex;
					sampler2D _ParallaxLayers;
					sampler2D _Roughness;
					sampler2D _SnowHeight;
					Texture2D _SnowTransition;
					float _TransitionScale;
					float _TransitionPower;
					float _HeightScale;
					float _LightOffset;
					float _LightHardness;
					float _LightIntensity;
					float _DisplacementColorMult, _ReflectionOpacity, _DisplacementShadowMult;
					float _FogIntensity, _FogScale, _FlowMultiplier;

					Texture2D _FogTex;
					Texture2D _FlowTex;

					half _OffsetScale;
					half _OverallScale;
					half _RoughnessStrength;
					half4 _ParallaxStrength;

					half _NormalScale, _ParalaxColorScale, _DisplacementOffset, _SnowNormalScale, _MainTexMult;
					half4 _IceTint, _ParalaxColor;
					half4 _IceTrail;

					float _ShininessIce, _ShininessSnow;
					float _ReflectionStrength;
					float _HasRT;
					float _FishScale, _FishSpeed, _FishIntensity, _FishAngle, _FishMaskIntensity;
					float4 _ProjectedShadowColor, _TransitionColor, _ReflectionColor, _RimColor;
					float3 _FogDirection;


					float3 calcNormal(float2 texcoord, sampler2D globalEffect)
					{
						const float3 off = float3(-0.0005 * _NormalRTDepth, 0, 0.0005 * _NormalRTDepth); // texture resolution to sample exact texels
						const float2 size = float2(0.002, 0.0); // size of a single texel in relation to world units

						// The "Mount" normal effect has been removed and will be improved
						//float s01 = tex2Dlod(globalEffect, float4(texcoord.xy - 4 * off.xy, 0, 0)).y * 0.162162162 * _NormalRTDepth;
						//float s21 = tex2Dlod(globalEffect, float4(texcoord.xy - 3 * off.zy, 0, 0)).y * 0.540540541 * _NormalRTDepth;
						//float s10 = tex2Dlod(globalEffect, float4(texcoord.xy - 2 * off.yx, 0, 0)).y * 1.216216216 * _NormalRTDepth;
						//float s12 = tex2Dlod(globalEffect, float4(texcoord.xy - 1 * off.yz, 0, 0)).y * 1.945945946 * _NormalRTDepth;

						//float s01 = tex2Dlod(globalEffect, float4(0,0, 0, 0)).y * 0.162162162 * _NormalRTDepth;
						//float s21 = tex2Dlod(globalEffect, float4(0,0, 0, 0)).y * 0.540540541 * _NormalRTDepth;
						//float s10 = tex2Dlod(globalEffect, float4(0,0, 0, 0)).y * 1.216216216 * _NormalRTDepth;
						//float s12 = tex2Dlod(globalEffect, float4(0,0, 0, 0)).y * 1.945945946 * _NormalRTDepth;

						float s01 = tex2Dlod(globalEffect, float4(texcoord.xy + 4 * off.xy * 10, 0, 0)).y * 0.245945946 * _NormalRTDepth;
						float s21 = tex2Dlod(globalEffect, float4(texcoord.xy + 3 * off.zy * 10, 0, 0)).y * 0.216216216 * _NormalRTDepth;
						float s10 = tex2Dlod(globalEffect, float4(texcoord.xy + 2 * off.yx * 10, 0, 0)).y * 0.540540541 * _NormalRTDepth;
						float s12 = tex2Dlod(globalEffect, float4(texcoord.xy + 1 * off.yz * 10, 0, 0)).y * 0.162162162 * _NormalRTDepth;

						float g01 = tex2Dlod(globalEffect, float4(texcoord.xy + 4 * off.xy, 0, 0)).z * 1.945945946 * _NormalRTDepth;
						float g21 = tex2Dlod(globalEffect, float4(texcoord.xy + 3 * off.zy, 0, 0)).z * 1.216216216 * _NormalRTDepth;
						float g10 = tex2Dlod(globalEffect, float4(texcoord.xy + 2 * off.yx, 0, 0)).z * 0.540540541 * _NormalRTDepth;
						float g12 = tex2Dlod(globalEffect, float4(texcoord.xy + 1 * off.yz, 0, 0)).z * 0.162162162 * _NormalRTDepth;

						//float3 va = normalize(float3(size.xy, s21 - s01)) + normalize(float3(size.xy, g21 - g01));
						//float3 vb = normalize(float3(size.yx, s12 - s10)) + normalize(float3(size.xy, g12 - g10));
						float3 va = normalize(float3(size.xy, 0)) + normalize(float3(size.xy, g21 - g01));
						float3 vb = normalize(float3(size.yx, 0)) + normalize(float3(size.xy, g12 - g10));

						float3 vc = normalize(float3(size.xy, 0)) + normalize(float3(size.xy, s21 - s01));
						float3 vd = normalize(float3(size.yx, 0)) + normalize(float3(size.xy, s12 - s10));

						float3 calculatedNormal = normalize(cross(va, vb));
						calculatedNormal.y = normalize(cross(vc, vd)).x;
						return calculatedNormal;
					}

					float4 blendMultiply(float4 baseTex, float4 blendTex, float opacity)
					{
						float4 baseBlend = baseTex * blendTex;
						float4 ret = lerp(baseTex, baseBlend, opacity);
						return ret;
					}

					InterpolatorsVertex vert(VertexData v) {
						InterpolatorsVertex i;

#ifdef USE_VR
						UNITY_SETUP_INSTANCE_ID(v);
						//UNITY_INITIALIZE_OUTPUT(InterpolatorsVertex, i);
						UNITY_TRANSFER_INSTANCE_ID(InterpolatorsVertex, i);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(i);
#endif

						float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
						float3 originalPos = worldPos;

						//RT Cam effects
						float2 uv = worldPos.xz - _Position.xz;
						uv = uv / (_OrthographicCamSize * 2);
						uv += 0.5;

						float2 uvAdd = worldPos.xz - _PositionAdd.xz;
						uvAdd = uvAdd / (_OrthographicCamSizeAdditional * 2);
						uvAdd += 0.5;

						float3 rippleMain = 0;
						float3 rippleMainAdditional = 0;

						float ripples = 0;
						float ripples2 = 0;
						float ripples3 = 0;

						float uvRTValue = 0;

					#ifdef USE_RT
						if (_HasRT == 1)
						{
							// .b(lue) = Snow Dig / .r(ed) = Snow To Ice / .g(reen) = Snow Mount
							rippleMain = tex2Dlod(_GlobalEffectRT, float4(uv, 0, 0));
							rippleMainAdditional = tex2Dlod(_GlobalEffectRTAdditional, float4(uvAdd, 0, 0));
						}

					#ifdef IS_ICE
					#else
						float2 uvGradient = smoothstep(0, 5, length(max(abs(_Position.xz - worldPos.xz) - _OrthographicCamSize + 5, 0.0)));
						uvRTValue = saturate(uvGradient.x);

						if (v.color.b > 0.95 && v.color.g < 0.05)
						{
						}
						else
						{
							ripples = lerp(rippleMain.x, rippleMainAdditional.x, uvRTValue);
							ripples2 = lerp(rippleMain.y, rippleMainAdditional.y, uvRTValue);
							ripples3 = lerp(rippleMain.z, rippleMainAdditional.z, uvRTValue);
						}
					#endif

					#endif

						float slopeValue = 0;
					#ifdef IS_T
						half4 splat_control = tex2Dlod(_Control0, float4(v.uv, 0,0));
						half4 splat_control1 = tex2Dlod(_Control1, float4(v.uv, 0, 0));

						float iceValue = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
							- ripples);

						float snowHeightNew = tex2Dlod(_Mask0, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1).r;
						snowHeightNew = lerp(snowHeightNew, tex2Dlod(_Mask1, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1).r, saturate(splat_control.g * (1 - _Metallic1)));
						snowHeightNew = lerp(snowHeightNew, tex2Dlod(_Mask2, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1).r, saturate(splat_control.b * (1 - _Metallic2)));
						snowHeightNew = lerp(snowHeightNew, tex2Dlod(_Mask3, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1).r, saturate(splat_control.a * (1 - _Metallic3)));

						float snowHeight = snowHeightNew;
					#else
						float iceValue = 0;


#ifdef USE_INTER
#ifdef IS_ADD			
						// custom intersection and slope value //
						float4 midPoint = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0));

						float4 quaternion = float4(v.uv6.x, -v.uv6.y, -v.uv7.x, -v.uv7.y);
						float3 offsetPoint = worldPos.xyz - midPoint;

						float3 rotatedVert = rotateVector(quaternion, -offsetPoint);
						float manualLerp = 0;

						manualLerp = v.uv4.x;

						rotatedVert = RotateAroundZInDegrees(float4(rotatedVert, 0), lerp(6, -6, (manualLerp)));
						rotatedVert = RotateAroundXInDegrees(float4(rotatedVert, 0), lerp(-55, 55, (manualLerp))) + midPoint;

						slopeValue = ((v.color.a) - (rotatedVert.y - 0.5));

						if (slopeValue > 0.0)
						{
							v.color.g = saturate(v.color.g + saturate(slopeValue * 3));
							v.color.b = saturate(v.color.b + saturate(slopeValue * 3));
						}
#endif
#endif

						if (v.color.b > 0.6 && v.color.g < 0.4)
						{
							iceValue = saturate(1 - v.color.b);
						}
						else
						{
							iceValue = saturate((v.color.g + v.color.b) / 2 - ripples);
						}

					#ifdef USE_WC
						float snowHeight = tex2Dlod(_SnowHeight, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1 * _SnowScale).r;
					#else
						float snowHeight = tex2Dlod(_SnowHeight, float4(v.uv, 0, 0) * _HeightScale * _SnowScale).r;
					#endif

					#endif

						i.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject));
					#ifdef IS_ICE
					#else

					#ifdef USE_RT
						if (_HasRT == 1)
						{
							if (v.color.b > 0.95 && v.color.g < 0.05)
							{
								i.normal = normalize(v.normal);
							}
							else
							{
								i.normal = normalize(lerp(v.normal, calcNormal(uv, _GlobalEffectRT).rbb, iceValue));
							}
						} 
						else
						{
							i.normal = normalize(v.normal);
						}
					#else
						i.normal = normalize(v.normal);
					#endif

					#ifdef IS_ADD
						float3 newNormal = normalize(i.normalDir);
						worldPos += ((float4(0, -_RemoveSnowStrength, 0, 0) * _UpVector - newNormal * _RemoveSnowStrength * _NormalVector) * ripples3 + (float4(0, _AddSnowStrength * snowHeight, 0, 0) * _UpVector + newNormal * _AddSnowStrength * snowHeight * _NormalVector) * ripples2 * saturate(1 - ripples3)) * saturate(iceValue * 3);
						worldPos += (float4(0, _DisplacementOffset, 0, 0) * _UpVector + newNormal * _DisplacementOffset * _NormalVector) * saturate(iceValue * 2.5);
						worldPos += (float4(0, 2 * _DisplacementStrength * snowHeight, 0, 0) * _UpVector) + (newNormal * 2 * _DisplacementStrength * snowHeight * _NormalVector * clamp(slopeValue * 20, 1, 2)) * saturate(saturate(iceValue * 2.5));

						worldPos = lerp(worldPos, mul(unity_ObjectToWorld, v.vertex), saturate(v.color.g - saturate(v.color.r + v.color.b)));

						v.vertex.xyz = lerp(mul(unity_WorldToObject, float4(originalPos, 1)).xyz, mul(unity_WorldToObject, float4(worldPos, 1)).xyz, iceValue);
					#else
						float3 newNormal = normalize(i.normalDir);
						worldPos += ((float4(0, -_RemoveSnowStrength, 0, 0) * _UpVector - newNormal * _RemoveSnowStrength * _NormalVector) * ripples3 + (float4(0, _AddSnowStrength * snowHeight, 0, 0) * _UpVector + newNormal * _AddSnowStrength * snowHeight * _NormalVector) * ripples2 * saturate(1 - ripples3)) * saturate(iceValue * 3);
						worldPos += (float4(0, _DisplacementOffset, 0, 0) * _UpVector + newNormal * _DisplacementOffset * _NormalVector) * saturate(iceValue * 2.5);
						worldPos += (float4(0, 2 * _DisplacementStrength * snowHeight, 0, 0) * _UpVector) + (newNormal * 2 * _DisplacementStrength * snowHeight * _NormalVector) * saturate(saturate(iceValue * 2.5));

						worldPos = lerp(worldPos, mul(unity_ObjectToWorld, v.vertex), saturate(v.color.g - saturate(v.color.r + v.color.b)));

						v.vertex.xyz = lerp(mul(unity_WorldToObject, float4(originalPos, 1)).xyz, mul(unity_WorldToObject, float4(worldPos, 1)).xyz, iceValue);

					#endif
					#endif

					#ifdef USE_RT
						if (_HasRT == 1)
						{
							v.color = lerp(v.color, saturate(float4(1, 0, 0, 1)), ripples);
						}
					#endif
						//i.vertex = UnityObjectToClipPos(v.vertex);
						i.vertex = GetVertexPositionInputs(v.vertex).positionCS;
						VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
						i.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
						i.shadowCoord = GetShadowCoord(vertexInput);

						float4 objCam = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0));
						float3 viewDir = v.vertex.xyz - objCam.xyz;

					#ifdef IS_T
						float4 tangent = float4 (1.0, 0.0, 0.0, -1.0);
						tangent.xyz = tangent.xyz - v.normal * dot(v.normal, tangent.xyz); // Orthogonalize tangent to normal.

						float tangentSign = tangent.w * unity_WorldTransformParams.w;
						float3 bitangent = cross(v.normal.xyz, tangent.xyz) * tangentSign;

						i.viewDir = float3(
							dot(viewDir, tangent.xyz),
							dot(viewDir, bitangent.xyz),
							dot(viewDir, v.normal.xyz)
							);

						i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
						i.tangent = tangent;

					#else
						float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
						float3 bitangent = cross(v.normal.xyz, v.tangent.xyz) * tangentSign;

						i.viewDir = float3(
							dot(viewDir, v.tangent.xyz),
							dot(viewDir, bitangent.xyz),
							dot(viewDir, v.normal.xyz)
							);

						i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
						i.tangent = v.tangent;
					#endif

						i.color = v.color;

					#ifdef IS_T
						i.uv.xy = v.uv;
					#else
						i.uv.xy = TRANSFORM_TEX(v.uv, _MainTex) * _OverallScale;
					#endif
						i.uv.zw = TRANSFORM_TEX(v.uv, _DetailTex);

#ifdef IS_ADD
#ifdef USE_INTER
						// NORMAL BASED ON HIT NORMAL //
						i.normalDir = lerp(i.normalDir, normalize(float3(v.uv3.x, v.uv3.y, v.uv4.y)), saturate(slopeValue));
						i.normal = normalize(float3(v.uv3.x, v.uv3.y, v.uv4.y));
#endif
#endif

						return i;
					}


					float4 frag(InterpolatorsVertex i) : SV_Target
					{
#ifdef USE_VR
								UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i)
#endif

								// Linear to Gamma //
half gamma = 0.454545;

					float4 shadowCoord;
					half3 lm = 1;
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					shadowCoord = i.shadowCoord;
#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					shadowCoord = TransformWorldToShadowCoord(i.worldPos);
#else
					shadowCoord = float4(0, 0, 0, 0);
#endif
					Light mainLight = GetMainLight(shadowCoord);


half4 lightColor = half4(mainLight.color.rgb, (mainLight.color.r + mainLight.color.g + mainLight.color.b)/3);


#if !UNITY_COLORSPACE_GAMMA
					_Color = pow(_Color, gamma);
					//_TransitionColor = pow(_TransitionColor, gamma);
					_MountColor = pow(_MountColor, gamma);
					_BotColor = pow(_BotColor, gamma);
					//_IceTint = pow(_IceTint, 0.55)*1.5;
					_IceTint = pow(_IceTint, 0.55)*1.5;
					_ParalaxColor = pow(_ParalaxColor, gamma);
					_IceTrail = pow(_IceTrail, gamma);
					//_ProjectedShadowColor = pow(_ProjectedShadowColor, gamma);
					_SpecColor = pow(_SpecColor, gamma);
					_ReflectionColor = pow(_ReflectionColor, gamma);
					_RimColor = pow(_RimColor, gamma);
					lightColor.rgb = pow(lightColor.rgb, gamma);
					_LittleSpecForce = pow(_LittleSpecForce, gamma)*1.5;
#ifdef IS_T
					_Specular0 = pow(_Specular0, gamma);
					_Specular1 = pow(_Specular1, gamma);
					_Specular2 = pow(_Specular2, gamma);
					_Specular3 = pow(_Specular3, gamma);
#endif
#endif

								SurfaceData surfaceData;
								InitializeStandardLitSurfaceData(i.uv, surfaceData);
								

								float uvRTValue = 0;
								float2 uv = i.worldPos.xz - _Position.xz;
								uv = uv / (_OrthographicCamSize * 2);
								uv += 0.5;

								float2 uvAdd = i.worldPos.xz - _PositionAdd.xz;
								uvAdd = uvAdd / (_OrthographicCamSizeAdditional * 2);
								uvAdd += 0.5;

								float3 calculatedNormal = 0;
								float3 calculatedNormalAdd = 0;
								float3 rippleMain = 0;
								float3 rippleMainAdditional = 0;
								float ripples = 0;
								float ripples2 = 0;
								float ripples3 = 0;
								float iceValue = 1;

					#ifdef IS_ICE
								float snowHeight = 0;
								float snowHeightReal = 0;
					#else
								float snowHeight = saturate(_SnowTransition.Sample(my_linear_repeat_sampler, i.uv * _TransitionScale * _SnowScale).r);
					#ifdef USE_WC
								float snowHeightReal = tex2D(_SnowHeight, float2(i.worldPos.x, i.worldPos.z) * _HeightScale * 0.1 * _SnowScale).r;
					#else
								float snowHeightReal = tex2D(_SnowHeight, i.uv.xy * _HeightScale * _SnowScale).r;
					#endif

					#endif

					#ifdef IS_ICE
					#else
					#ifdef USE_RT
								if (_HasRT == 1)
								{
									rippleMain = tex2D(_GlobalEffectRT, uv);
									rippleMainAdditional = tex2D(_GlobalEffectRTAdditional, uvAdd);

									float2 uvGradient = smoothstep(0, 5, length(max(abs(_Position.xz - i.worldPos.xz) - _OrthographicCamSize + 5, 0.0)));
									uvRTValue = saturate(uvGradient.x);
									ripples = lerp(rippleMain.x, rippleMainAdditional.x, uvRTValue);
								}
					#endif
					#endif



					#ifdef IS_T
								half4 splat_control = tex2D(_Control0, i.uv);
								half4 splat_control1 = tex2D(_Control1, i.uv);


								splat_control.r = lerp(splat_control.r, 0, saturate(ripples));
								splat_control.g = lerp(splat_control.g, 1, saturate(ripples));

								float splatOverall = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3);

								//iceValue = pow(splatOverall, 0.35 + clamp((snowHeight - 0.5) * -_TransitionPower * splatOverall, -0.34, 1));
								iceValue = pow(splatOverall, 1 + clamp(abs((snowHeight - 0.5) * 20) * -_TransitionPower * splatOverall, -1, 1));
								//iceValue = saturate(pow((splatOverall), 1 + clamp(abs((snowHeight - 0.5) * 20) * -_TransitionPower * (saturate(splatOverall)), -1, 1)) * 1.25);

								i.uv = i.uv * _OverallScale;

								float3 SnowSplat0 = _Splat0.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat0_ST.xy));
								SnowSplat0 = lerp(pow(SnowSplat0, 0.4) * _Specular0 * 2, lerp(1, saturate(pow(SnowSplat0, 2)), _MainTexMult), 1 - _Metallic0);
								float3 SnowSplat1 = _Splat1.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat1_ST.xy));
								SnowSplat1 = lerp(pow(SnowSplat1, 0.4) * _Specular1 * 2, lerp(1, saturate(pow(SnowSplat1, 2)), _MainTexMult), 1 - _Metallic1);
								float3 SnowSplat2 = _Splat2.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat2_ST.xy));
								SnowSplat2 = lerp(pow(SnowSplat2, 0.4) * _Specular2 * 2, lerp(1, saturate(pow(SnowSplat2, 2)), _MainTexMult), 1 - _Metallic2);
								float3 SnowSplat3 = _Splat3.Sample(my_linear_repeat_sampler, i.uv * (_TerrainScale.xz / _Splat3_ST.xy));
								SnowSplat3 = lerp(pow(SnowSplat3, 0.4) * _Specular3 * 2, lerp(1, saturate(pow(SnowSplat3, 2)), _MainTexMult), 1 - _Metallic3);

								float3 SnowNormal0 = UnpackNormalScale(_Normal0.Sample(sampler_BumpMap, i.uv * (_TerrainScale.xz / _Splat0_ST.xy)), _NormalScale0);
								SnowNormal0 = lerp(SnowNormal0, SnowNormal0, 1 - _Metallic0);
								float3 SnowNormal1 = UnpackNormalScale(_Normal1.Sample(sampler_BumpMap, i.uv * (_TerrainScale.xz / _Splat1_ST.xy)), _NormalScale1);
								SnowNormal1 = lerp(SnowNormal1, SnowNormal1, 1 - _Metallic1);
								float3 SnowNormal2 = UnpackNormalScale(_Normal2.Sample(sampler_BumpMap, i.uv * (_TerrainScale.xz / _Splat2_ST.xy)), _NormalScale2);
								SnowNormal2 = lerp(SnowNormal2, SnowNormal2, 1 - _Metallic2);
								float3 SnowNormal3 = UnpackNormalScale(_Normal3.Sample(sampler_BumpMap, i.uv * (_TerrainScale.xz / _Splat3_ST.xy)), _NormalScale3);
								SnowNormal3 = lerp(SnowNormal3, SnowNormal3 * iceValue, 1 - _Metallic3);

								// SNOW NORMAL //
								float3 normal = lerp(0, SnowNormal0, splat_control.r * (1 - _Metallic0));
								normal = lerp(normal, SnowNormal1, saturate(splat_control.g * (1 - _Metallic1)));
								normal = lerp(normal, SnowNormal2, saturate(splat_control.b * (1 - _Metallic2)));
								normal = lerp(normal, SnowNormal3, saturate(splat_control.a * (1 - _Metallic3)));

								// ICE NORMAL //

								//half3 normalTex = lerp(UnpackScaleNormal(tex2D(_NormalTex, i.uv * _IceScale), _NormalScale0), SnowNormal0, splat_control.r * (_Metallic0));
								half3 normalTex = lerp(UnpackNormalScale(_Normal0.Sample(sampler_BumpMap, i.uv * _IceScale), _NormalScale), SnowNormal0, splat_control.r * (_Metallic0));
								normalTex = lerp(normalTex, SnowNormal1 * _NormalScale1, splat_control.g * (_Metallic1));
								normalTex = lerp(normalTex, SnowNormal2 * _NormalScale2, splat_control.b * (_Metallic2));
								normalTex = lerp(normalTex, SnowNormal3 * _NormalScale3, splat_control.a * (_Metallic3));

					#else
								iceValue = saturate(pow(saturate(i.color.g) , 1 + clamp(abs((snowHeight - 0.5) * 20) * -_TransitionPower * (saturate(i.color.g)), -1, 1)) * 1.25);

								float3 normal = UnpackNormalScale(_BumpMap.Sample(sampler_BumpMap, i.uv * _SnowNormalScale * _SnowScale), _NormalMultiplier * 2).rgb * iceValue - i.normal;
								half3 normalTex = UnpackNormalScale(_NormalTex.Sample(sampler_BumpMap, i.uv * _IceScale), _NormalScale);
					#endif

					#ifdef IS_T
								half4 c = _Specular0;
								c = lerp(c,_Specular1, splat_control.g * (iceValue) * (1 - _Metallic1));
								c = lerp(c,_Specular2, splat_control.b * (iceValue) * (1 - _Metallic2));
								c = lerp(c,_Specular3, splat_control.a * (iceValue) * (1 - _Metallic3));
					#else
								half4 c = _Color;
					#endif


					#ifdef IS_ICE
					#else
					#ifdef USE_RT

								if (_HasRT == 1)
								{
									if (i.color.b > 0.95 && i.color.g < 0.05)
									{
									}
									else
									{
										ripples2 = lerp(rippleMain.y, rippleMainAdditional.y, uvRTValue);
										ripples3 = lerp(rippleMain.z, rippleMainAdditional.z, uvRTValue);
										calculatedNormal = calcNormal(uv, _GlobalEffectRT);
										calculatedNormal.y = lerp(calculatedNormal.y, 0, saturate(ripples3 * 5));
										calculatedNormalAdd = calcNormal(uvAdd, _GlobalEffectRTAdditional);
										calculatedNormal = lerp(calculatedNormal, 0, uvRTValue);
									}

									c = lerp(
										c,
										_BotColor * 2 - 1,
										ripples3);

									c = lerp(
										c,
										c + _MountColor,
										saturate(saturate(ripples2 - ripples3) * saturate(snowHeight + 0.5) * 1));


									c.rgb = lerp(c.rgb, c.rgb * _BotColor, clamp(ripples3 * saturate(calculatedNormalAdd.r - 0.15) * _NormalRTStrength * 1, 0, 1));
								}
					#endif	 
								c.rgb = c.rgb * lightColor.rgb;
								c.rgb = lerp(c.rgb * _Color * _DisplacementColorMult, c.rgb, snowHeightReal);
					#endif	
								float3 normalEffect = i.normal;
					#ifdef IS_T
								// SNOW LERP //
								float3 snowColor = SnowSplat0;
								snowColor = lerp(snowColor, SnowSplat1, saturate(splat_control.g * (1 - _Metallic1)));
								snowColor = lerp(snowColor, SnowSplat2, saturate(splat_control.b * (1 - _Metallic2)));
								snowColor = lerp(snowColor, SnowSplat3, saturate(splat_control.a * (1 - _Metallic3)));

								c.rgb *= snowColor;
					#else
								c *= lerp(1, saturate(pow(tex2D(_MainTex, i.uv * _SnowScale) + _MainTexMult * 0.225, 2)), _MainTexMult);
					#endif

								// ADD SNOW FOG WITH DUST //
								half3 flowVal = (_FlowTex.Sample(my_bilinear_repeat_sampler, i.uv)) * _FlowMultiplier;

								float dif1 = frac(_Time.y * 0.15 + 0.5);
								float dif2 = frac(_Time.y * 0.15);

								half lerpVal = abs((0.5 - dif1) / 0.5);

								//_FogDirection
								half3 col1 = _FogTex.Sample(my_bilinear_repeat_sampler, i.uv * _FogScale - flowVal.xy * dif1 + (normalize(_FogDirection.xy) * _Time.y * -0.02 * _FogDirection.z));
								half3 col2 = _FogTex.Sample(my_bilinear_repeat_sampler, i.uv * _FogScale - flowVal.xy * dif2 + (normalize(_FogDirection.xy) * _Time.y * -0.02 * _FogDirection.z));

								half3 fogFlow = lerp(col1, col2, lerpVal);
								fogFlow = abs(pow(fogFlow, 5));
								// ICE MATERIAL //
								float3 viewDirTangent = i.viewDir;
					#ifdef IS_T
								// ICE LERP //
								//half4 IceTex = half4(lerp(_TransitionColor, SnowSplat0, iceValue * splat_control.r * (_Metallic0)) ,1); 
								half4 IceTex = half4(SnowSplat1, 1);
								IceTex.rgb = lerp(IceTex, SnowSplat1, splat_control.g * (_Metallic1));
								IceTex.rgb = lerp(IceTex, SnowSplat2, splat_control.b * (_Metallic2));
								IceTex.rgb = lerp(IceTex, SnowSplat2, splat_control.a * (_Metallic3));
					#else
								half4 IceTex = half4(lerp(1, pow(_IceTex.Sample(my_linear_repeat_sampler, i.uv * _IceScale), 0.4), _IceTint.a) * _IceTint.rgb * 2 ,1);
					#endif
								IceTex.rgb = IceTex.rgb * lightColor.rgb;
								float3 viewDirection = normalize(_WorldSpaceCameraPos - i.worldPos.xyz);
								float3 normalDirection = normalize(i.normalDir);
								//float3 normalDirectionWithNormal = normalize(i.normalDir) + normalize(i.normalDir) * normalize(abs(SampleNormal(i.uv *_SnowNormalScale * _SnowScale,TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap )))) * _NormalMultiplier;
								//float3 normalDirectionWithNormal = normalize(i.normalDir) + normalize(i.normalDir) * normalize((UnpackNormalScale(_BumpMap.Sample(sampler_BumpMap, i.uv * _SnowNormalScale * _SnowScale), _NormalMultiplier).rgb)) * _NormalMultiplier;

#ifdef IS_T
								float3 normalDirectionWithNormal = normalize(i.normalDir) + normalize(i.normalDir) * normal;
#else
								float3 normalDirectionWithNormal = normalize(i.normalDir) + normalize(i.normalDir) * normalize((UnpackNormalScale(_BumpMap.Sample(sampler_BumpMap, i.uv * _SnowNormalScale * _SnowScale), _NormalMultiplier).rgb)) * _NormalMultiplier;
#endif

								half fresnelValue = lerp(0, 1, saturate(dot(normalDirection, viewDirection)));
								_OffsetScale = max(0, _OffsetScale);

								half parallax = 0;

								float3 newRoughnessTex = tex2D(_Roughness, i.uv * _IceScale).rgb;
								float alphaIce = 1;
								alphaIce = saturate(1 - saturate((newRoughnessTex.r + newRoughnessTex.g + newRoughnessTex.b) / 3));

								float underWaterFish = 0;

								float rotationAngle = _FishAngle * 3.1416 / 180.0;
								float sina, cosa;
								sincos(rotationAngle, sina, cosa);
								float2x2 m = float2x2(cosa, -sina, sina, cosa);

								for (int j = 0; j < 4; j++) {
									float ratio = (float)j / (float)4;

									if (j == 0)
									{
					#ifdef IS_T
										//float3 j0UV = lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;
										//float j0parallax = tex2D(_Mask0, i.uv * (_TerrainScale.xz / _Splat0_ST.xy) + j0UV).g * _ParallaxStrength.x * _Metallic0 * splat_control.r;
										//j0parallax += tex2D(_Mask1, i.uv * (_TerrainScale.xz / _Splat1_ST.xy) + j0UV).g * _ParallaxStrength.x * _Metallic1 * splat_control.g;
										//j0parallax += tex2D(_Mask2, i.uv * (_TerrainScale.xz / _Splat2_ST.xy) + j0UV).g * _ParallaxStrength.x * _Metallic2 * splat_control.b;
										//j0parallax += tex2D(_Mask3, i.uv * (_TerrainScale.xz / _Splat3_ST.xy) + j0UV).g * _ParallaxStrength.x * _Metallic3 * splat_control.a;
										//parallax += j0parallax;
					#else
										// 0 layer of cracks.
										//parallax += tex2D(_ParallaxLayers, i.uv * _IceScale + lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex).g * _ParallaxStrength.x;
					#endif
									}
									else if (j == 1)
									{
					#ifdef IS_T
										float3 j1UV = lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;
										float j1parallax = tex2D(_Mask0, i.uv * _IceScale * (_TerrainScale.xz / _Splat0_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic0 * splat_control.r;
										j1parallax += tex2D(_Mask1, i.uv * _IceScale * (_TerrainScale.xz / _Splat1_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic1 * splat_control.g;
										j1parallax += tex2D(_Mask2, i.uv * _IceScale * (_TerrainScale.xz / _Splat2_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic2 * splat_control.b;
										j1parallax += tex2D(_Mask3, i.uv * _IceScale * (_TerrainScale.xz / _Splat3_ST.xy) + j1UV).r * _ParallaxStrength.y * _Metallic3 * splat_control.a;
										parallax += j1parallax;

										//float2 compressedUV = i.uv * _IceScale * _FishScale + lerp(0, _OffsetScale * 0.5, ratio) * normalize(viewDirTangent) + normalTex;
										float2 compressedUV = i.uv * _IceScale * _FishScale * (_TerrainScale.xz / _Splat1_ST.xy) + j1UV;
										float2 fishUV = mul(m, compressedUV);

										//underWaterFish += tex2D(_UnderWaterFish, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).r;
										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).r;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);

					#else
										// First layer of cracks.
										parallax += tex2D(_ParallaxLayers, i.uv * _IceScale + lerp(0, _OffsetScale, ratio / 2) * normalize(viewDirTangent) + normalTex).r * _ParallaxStrength.y * alphaIce;
										for (int k = 0; k < 5; k++)
										{
											parallax += tex2D(_ParallaxLayers, i.uv * _IceScale + lerp(0, _OffsetScale, ratio / 2 + k * 0.04) * normalize(viewDirTangent) + normalTex).r * _ParallaxStrength.y * alphaIce;
										}

										float2 compressedUV = i.uv * _IceScale * _FishScale + lerp(0, _OffsetScale * 0.5, ratio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).r;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);
					#endif
									}
									else if (j == 2)
									{
					#ifdef IS_T
										float3 j2UV = lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;
										float j2parallax = tex2D(_Mask0, i.uv * _IceScale * (_TerrainScale.xz / _Splat0_ST.xy) + j2UV).b * _ParallaxStrength.z * _Metallic0 * splat_control.r;
										j2parallax += tex2D(_Mask1, i.uv * _IceScale * (_TerrainScale.xz / _Splat1_ST.xy) + j2UV).b * _ParallaxStrength.z * _Metallic1 * splat_control.g;
										j2parallax += tex2D(_Mask2, i.uv * _IceScale * (_TerrainScale.xz / _Splat2_ST.xy) + j2UV).b * _ParallaxStrength.z * _Metallic2 * splat_control.b;
										j2parallax += tex2D(_Mask3, i.uv * _IceScale * (_TerrainScale.xz / _Splat3_ST.xy) + j2UV).b * _ParallaxStrength.z * _Metallic3 * splat_control.a;
										parallax += j2parallax;

										//float2 compressedUV = i.uv * _IceScale * _FishScale + lerp(0, _OffsetScale * 0.5, ratio) * normalize(viewDirTangent) + normalTex;
										float2 compressedUV = i.uv * _IceScale * _FishScale * (_TerrainScale.xz / _Splat0_ST.xy) + lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										//underWaterFish += tex2D(_UnderWaterFish, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).r;
										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).g;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);

					#else
										// Second layer of cracks.
										parallax += tex2D(_ParallaxLayers, i.uv * _IceScale + lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex).b * _ParallaxStrength.z * alphaIce;
										for (int k = 0; k < 5; k++)
										{
											parallax += tex2D(_ParallaxLayers, i.uv * _IceScale + lerp(0, _OffsetScale, ratio + k * 0.04) * normalize(viewDirTangent) + normalTex).b * _ParallaxStrength.z * alphaIce;
										}

										float2 compressedUV = i.uv * _IceScale * _FishScale + lerp(0, _OffsetScale * 1, ratio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * 1.4 * _FishSpeed + fishUV).g * 0.75;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);
					#endif
									}
									else if (j == 3)
									{
					#ifdef IS_T
										float3 j3UV = lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;
										float j3parallax = tex2D(_Mask0, i.uv * _IceScale * (_TerrainScale.xz / _Splat0_ST.xy) + j3UV).g * _ParallaxStrength.w * _Metallic0 * splat_control.r;
										j3parallax += tex2D(_Mask1, i.uv * _IceScale * (_TerrainScale.xz / _Splat1_ST.xy) + j3UV).g * _ParallaxStrength.w * _Metallic1 * splat_control.g;
										j3parallax += tex2D(_Mask2, i.uv * _IceScale * (_TerrainScale.xz / _Splat2_ST.xy) + j3UV).g * _ParallaxStrength.w * _Metallic2 * splat_control.b;
										j3parallax += tex2D(_Mask3, i.uv * _IceScale * (_TerrainScale.xz / _Splat3_ST.xy) + j3UV).g * _ParallaxStrength.w * _Metallic3 * splat_control.a;
										parallax += j3parallax;

										//float2 compressedUV = i.uv * _IceScale * _FishScale + lerp(0, _OffsetScale * 0.5, ratio) * normalize(viewDirTangent) + normalTex;
										float2 compressedUV = i.uv * _IceScale * _FishScale * (_TerrainScale.xz / _Splat0_ST.xy) + lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										//underWaterFish += tex2D(_UnderWaterFish, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).r;
										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * _FishSpeed + fishUV).b;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);

					#else
										// Third layer of cracks.
										parallax += tex2D(_ParallaxLayers, i.uv * _IceScale + lerp(0, _OffsetScale, ratio) * normalize(viewDirTangent) + normalTex).g * _ParallaxStrength.w * alphaIce - fresnelValue * 0.02;
										for (int k = 0; k < 5; k++)
										{
											parallax += tex2D(_ParallaxLayers, i.uv * _IceScale + lerp(0, _OffsetScale, ratio + k * 0.04) * normalize(viewDirTangent) + normalTex).g * _ParallaxStrength.w * alphaIce - fresnelValue * 0.02;
										}

										float2 compressedUV = i.uv * _IceScale * _FishScale + lerp(0, _OffsetScale * 2, ratio) * normalize(viewDirTangent) + normalTex;
										float2 fishUV = mul(m, compressedUV);

										underWaterFish += _UnderWaterFish.Sample(my_bilinear_repeat_sampler, float2((_Time.y * 0.01), -(_Time.y * 0.1)) * 2 * _FishSpeed + fishUV).b * 0.55;
										underWaterFish = saturate(saturate(underWaterFish) - 0.1 * _FishMaskIntensity * 2);
					#endif
									}
								}
								parallax *= 1.5;
								parallax *= (1 + _ParallaxStrength.x);
								parallax = clamp(parallax, -2, 2);
								half parallaxDepth = clamp(parallax * pow(max(0.0, dot(reflect(-viewDirection, normalDirection * 1.3) * 0.4,viewDirection)), 1)  ,-2,2);

								half4 blended = 0;

								if (i.color.b > 0.95 && i.color.g < 0.05)
								{
									blended = blendMultiply(IceTex, parallax + parallaxDepth * 1, _ParalaxColorScale);
								}
								else
								{
									blended = blendMultiply(IceTex, parallax + parallaxDepth * 1, _ParalaxColorScale) - (_IceTrail * ripples3 * _IceTrail.a);
								}

								blended.rgb = lerp(blended.rgb + newRoughnessTex * 0.4, blended.rgb, alphaIce);

								float3 albedo = 1;
					#ifdef	IS_ICE
								albedo = blended * 0.5 + lerp(0, 1, parallaxDepth);

								if (parallaxDepth < 0)
								{
									albedo.rgb = lerp(albedo.rgb, _ParalaxColor.rgb - underWaterFish * _FishIntensity, saturate(abs(parallaxDepth)));
								}
								else
								{
									albedo.rgb = lerp(albedo.rgb, albedo.rgb + _ParalaxColor.rgb, saturate(abs(parallaxDepth)));
								}
					#else
					#ifdef IS_ADD
								albedo = lerp(c.rgb * _TransitionColor, c.rgb, saturate(pow(iceValue, 3)));
#else
								albedo = lerp(blended * 0.5 + lerp(0, 1, parallaxDepth), c.rgb, saturate(pow(iceValue, 3)));

								if (parallaxDepth < 0)
								{
									albedo.rgb = lerp(albedo.rgb, _ParalaxColor.rgb - underWaterFish * _FishIntensity, saturate(abs(parallaxDepth)) * saturate(1 - pow(iceValue, 5)));
								}
								else
								{
#ifdef IS_T
									albedo.rgb = lerp(albedo.rgb, albedo.rgb + _ParalaxColor.rgb - underWaterFish * _FishIntensity, saturate(abs(parallaxDepth)) * saturate(1 - pow(iceValue, 5)));
#else
									albedo.rgb = lerp(albedo.rgb, albedo.rgb + _ParalaxColor.rgb, saturate(abs(parallaxDepth)) * saturate(1 - pow(iceValue, 5)));
#endif								
								}
					#endif
					#endif


								float3 lightDirection;
								float attenuation;
								//float shadowmap = SHADOW_ATTENUATION(i);
								float shadowmap = mainLight.shadowAttenuation;
								half3 worldNormal;

								half3 normalSample = UnpackNormalScale(_BumpMap.Sample(sampler_BumpMap, i.uv * _SnowScale), _NormalMultiplier * 3).rgb;
								
								worldNormal.x = dot(normalDirection.x, lerp(normalTex, normalSample, iceValue));
								worldNormal.y = dot(normalDirection.y, lerp(normalTex, normalSample, iceValue));
								worldNormal.z = dot(normalDirection.z, lerp(normalTex, normalSample, iceValue));

								//half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
								half3 worldViewDir = normalize(viewDirection);
								float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);

								BRDFData brdfData;
								InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);

#ifdef LIGHTMAP_ON
								half3 bakedGI = SampleLightmap(i.uv, worldNormal);
#else
								half3 bakedGI = SampleSH(worldNormal);
#endif

								half3 reflectionURP = GlobalIllumination(brdfData, bakedGI, surfaceData.occlusion, worldNormal, viewDirection);
								reflectionURP += LightingPhysicallyBased(brdfData, mainLight, worldNormal, viewDirection);

								//half3 worldRefl = reflect(-worldViewDir, worldNormal);
								//half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
								//half3 reflection = DecodeHDR(skyData, unity_SpecCube0_HDR);
								//half reflectionMultiplier = lerp(1, 2, saturate(1 - shadowmap));
								
								half3 worldRefl = reflect(-worldViewDir, worldNormal);
								half mip = PerceptualRoughnessToMipmapLevel(brdfData.perceptualRoughness);
								half4 skyData = SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, worldRefl, mip);
								half3 reflection = DecodeHDREnvironment(skyData, unity_SpecCube0_HDR);
								half reflectionMultiplier = lerp(1, 2, saturate(1 - shadowmap));


#if !UNITY_COLORSPACE_GAMMA
								reflection = pow(reflection, gamma);
#endif

								_ShininessIce = max(0.1, _ShininessIce);
								_ShininessSnow = max(0.1, _ShininessSnow);

								if (0.0 == _MainLightPosition.w) // directional light
								{
									attenuation = 1.0; // no attenuation
									lightDirection = normalize(_MainLightPosition.xyz);
								}
								else // point or spot light
								{
									float3 vertexToLightSource =
										_MainLightPosition.xyz - i.worldPos.xyz;
									float distance = length(vertexToLightSource);
									attenuation = 1.0 / distance; // linear attenuation 
									lightDirection = normalize(vertexToLightSource);
								}

								float3 ambientLighting = UNITY_LIGHTMODEL_AMBIENT.rgb;
#if !UNITY_COLORSPACE_GAMMA
								ambientLighting = pow(ambientLighting, gamma);
#endif
								ambientLighting *= _Color.rgb;

								float3 diffuseReflection =
									attenuation * lightColor.rgb * _Color.rgb
									* max(0.0, dot(normalDirection, lightDirection));
								diffuseReflection = diffuseReflection + reflection * reflection * reflectionMultiplier * _ReflectionOpacity;

								float3 specularReflection;
								if (dot(normalDirection, lightDirection) < 0.0)
									// light source on the wrong side
								{
									specularReflection = float3(0.0, 0.0, 0.0);
									// no specular reflection
								}
								else // light source on the right side
								{
									specularReflection = attenuation * lightColor.rgb
										* _SpecColor.rgb * pow(max(0.0, dot(
											reflect(-lightDirection, normalDirection),
											viewDirection)), lerp(_ShininessIce, _ShininessSnow, iceValue));

									specularReflection = specularReflection + reflection * 0.2 * reflectionMultiplier;
								}
								//float NdotL = dot(_WorldSpaceLightPos0.xyz, lerp( normalDirection, normalDirectionWithNormal, saturate(iceValue* iceValue-0.2)));
								float NdotL = 0.5 * (dot(_MainLightPosition.xyz, normalDirectionWithNormal)) + 0.5; // Lambert Normal adjustement 
								NdotL = lerp(NdotL,NdotL + saturate(snowHeightReal) * 0.1 * _DisplacementStrength - saturate(1 - snowHeightReal) * 0.1 * _DisplacementStrength, iceValue * _DisplacementShadowMult);
								NdotL = saturate(NdotL);

								float lightIntensity = smoothstep(0.1 + _LightOffset * clamp((_LightHardness + 0.5) * 2,1,10), (0.101 + _LightOffset) * clamp((_LightHardness + 0.5) * 2, 1, 10), NdotL * shadowmap);
								_SpecForce = max(0.1, _SpecForce);

								half3 shadowmapColor = lerp(_ProjectedShadowColor, 1, lightIntensity);
								albedo.xyz = albedo.xyz * saturate(shadowmapColor);

					#ifdef IS_T
								float4 specGloss = pow(tex2D(_SpecGlossMapNew, i.uv * 2 * (_TerrainScale.xz / _Splat0_ST.xy)), _SpecForce);
								float4 littleSpec = tex2D(_LittleSpec, i.uv * (_TerrainScale.xz / _Splat0_ST.xy)) * saturate(1 - ripples3);
					#else
								float4 specGloss = pow(tex2D(_SpecGlossMapNew, i.uv * 2 * _SnowScale), _SpecForce);
								float4 littleSpec = tex2D(_LittleSpec, i.uv * _SnowScale) * saturate(1 - ripples3);
					#endif

					#ifdef IS_T
								half rougnessTemp = tex2D(_Mask0, i.uv * _IceScale * (_TerrainScale.xz / _Splat0_ST.xy)).r * 2 * _RoughnessStrength;
								rougnessTemp = lerp(rougnessTemp, tex2D(_Mask1, i.uv * _IceScale * (_TerrainScale.xz / _Splat1_ST.xy)).r * 2 * _RoughnessStrength, _Metallic1 * splat_control.g);
								rougnessTemp = lerp(rougnessTemp, tex2D(_Mask2, i.uv * _IceScale * (_TerrainScale.xz / _Splat2_ST.xy)).r * 2 * _RoughnessStrength, _Metallic2 * splat_control.b);
								rougnessTemp = lerp(rougnessTemp, tex2D(_Mask3, i.uv * _IceScale * (_TerrainScale.xz / _Splat3_ST.xy)).r * 2 * _RoughnessStrength, _Metallic3 * splat_control.a);
					#ifdef	IS_ICE
								half rougnessTex = rougnessTemp * saturate(1 - ripples3);
					#else
								half rougnessTex = rougnessTemp * saturate(1 - ripples3) * (1 - iceValue);
					#endif

					#else
					#ifdef	IS_ICE
								half rougnessTex = tex2D(_ParallaxLayers, i.uv * _IceScale).r * 2 * _RoughnessStrength * saturate(1 - ripples3) * 1;
					#else
								half rougnessTex = tex2D(_ParallaxLayers, i.uv * _IceScale).r * 2 * _RoughnessStrength * saturate(1 - ripples3) * (1 - iceValue);
					#endif
					#endif

					#ifdef	IS_ICE
								specGloss.r = specGloss.r * saturate(normal);
					#else
					#ifdef USE_RT
								if (_HasRT == 1)
								{
									specGloss.r = specGloss.r * saturate(normal) + saturate(ripples3 * 30) * lerp(0, 1, saturate(saturate(1 - ripples3 * 5) * calculatedNormal.x * reflect(-lightDirection, normalDirection)).x * _NormalRTStrength * saturate(shadowmapColor * 2));
									specGloss.r = specGloss.r * saturate(normal) + saturate(ripples2 * 30) * lerp(0, 0.1, saturate(saturate(1 - ripples2 * 5) * calculatedNormal.y * reflect(lightDirection, -normalDirection)).x * _NormalRTStrength * saturate(shadowmapColor * 2));
								}
								else
								{
									specGloss.r = specGloss.r * saturate(normal);
								}
					#else
								specGloss.r = specGloss.r * saturate(normal);
					#endif
					#endif
								_LittleSpecForce = max(0, _LittleSpecForce);

					#ifdef	IS_ICE
								specularReflection = parallax + specularReflection;
					#else
								specularReflection = lerp(parallax + specularReflection, specularReflection * (specGloss.r + lerp(littleSpec.g * _LittleSpecForce * 0.2 , littleSpec.g * _LittleSpecForce, specularReflection)), saturate(iceValue * 3)); // multiply the *3 for a better snow ice transition
					#endif
								float smallSpec = pow(max(0.0, dot(reflect(-lightDirection , normalDirection + parallax * 0.01), viewDirection)), 800);
								//smallSpec = saturate(smallSpec * saturate(UnpackScaleNormal(tex2D(_NormalTex, i.uv * _IceScale), 20) + 0.8));
								//smallSpec = saturate(smallSpec * saturate(SampleNormal(i.uv * _IceScale, TEXTURE2D_ARGS(_NormalTex, sampler_BumpMap))* 20 + 0.8));
								smallSpec = saturate(smallSpec * saturate(UnpackNormalScale(_BumpMap.Sample(sampler_BumpMap, i.uv * _IceScale), _NormalMultiplier).rgb * 20 + 0.8));

					#ifdef	IS_ICE
								specularReflection = diffuseReflection * 0.1 + specularReflection * rougnessTex + smallSpec;
					#else
					#ifdef USE_RT
								if (_HasRT == 1)
								{
									specularReflection = specularReflection - lerp(0, saturate(0.075), saturate(saturate(lightColor.a * lightIntensity + lightColor.a * 0.15) * saturate(1 - ripples3 * 4) * calculatedNormal.x * reflect(lightDirection, normalDirection)).x * _NormalRTStrength);
									specularReflection = specularReflection + lerp(0, saturate(0.125), saturate(saturate(lightColor.a * lightIntensity + lightColor.a * 0.15) * saturate(1 - ripples3 * 8) * calculatedNormal.x * reflect(-lightDirection, normalDirection)).x * _NormalRTStrength);
									specularReflection = specularReflection - lerp(0, saturate(0.1), saturate(saturate(1 - ripples2 * 1) * calculatedNormal.y * reflect(-lightDirection, normalDirection)).x * _NormalRTStrength);
									specularReflection = specularReflection + lerp(0, saturate(0.1), saturate(saturate(1 - ripples2 * 6) * calculatedNormal.y * reflect(lightDirection, normalDirection)).x * _NormalRTStrength * 0.5);
								}
					#endif
					#ifdef USE_LS
								specularReflection = lerp(specularReflection, diffuseReflection * 0.1 + specularReflection * rougnessTex + smallSpec, saturate(pow(1 - iceValue, 2) * 3));
					#else
								specularReflection = lerp(specularReflection, diffuseReflection * 0.1 + specularReflection * rougnessTex, saturate(pow(1 - iceValue, 2) * 3));
					#endif					
					#endif

					#ifdef USE_AL //ambient
								//half3 ambientColor = SampleSH(half4(lerp(normalDirection, normalDirection + normalEffect * 2.5, saturate(ripples3)), 1));
								half3 ambientColor = SampleSH(half4(normalDirection, 1));
								//albedo.rgb = saturate(albedo.rgb + (ShadeSH9(half4(lerp(normalDirection, normalDirection + normalEffect * 2.5, saturate(ripples + ripples3)) ,1)) - 0.5) * 0.75);
#if !UNITY_COLORSPACE_GAMMA
								ambientColor = pow(ambientColor, gamma) * 1.0;
#endif
								albedo.rgb = saturate(albedo.rgb + (ambientColor - 0.5) * 0.75);
					#endif
								half fresnelRefl = lerp(1, 0, saturate(dot(normalDirection, viewDirection) * 2.65 * _ReflectionColor.a));

					#ifdef	IS_ICE
								albedo.rgb = lerp(albedo.rgb, albedo.rgb + reflection * _ReflectionStrength + _ReflectionColor.rgb, saturate(fresnelRefl));
					#else
								albedo.rgb = lerp(albedo.rgb, albedo.rgb + reflection * _ReflectionStrength + _ReflectionColor.rgb, saturate((1 - iceValue) * (fresnelRefl + normalTex * fresnelRefl)));
								albedo.rgb = lerp(albedo.rgb, albedo.rgb + _RimColor, saturate(iceValue * (fresnelRefl + normal * fresnelRefl * 0.2)));
					#endif
								albedo.rgb = lerp((albedo.rgb + reflection * 0.15 + 0.2) , albedo.rgb, saturate(1 - fresnelRefl * 0.25));

								albedo += float4(specularReflection.r, specularReflection.r, specularReflection.r, 1.0) * _SpecColor.rgb;

								// ADD CUSTOM FOG //
								albedo.rgb += fogFlow * _FogIntensity;

								// Additional light pass in URP, thank you Unity for this //
								int additionalLightsCount = GetAdditionalLightsCount();
								half3 addLightColor = 0;
								for (int ii = 0; ii < additionalLightsCount; ii++)
								{
									Light light = GetAdditionalLight(ii, i.worldPos);
									addLightColor += ( light.color * light.distanceAttenuation * _LightIntensity );
								}

								albedo.rgb += addLightColor;

								half transparency = 1;
#ifdef	IS_ADD
								transparency = saturate(lerp(-0.5, 2, saturate(pow(iceValue, 1))));
								if (iceValue < 0.30)
								{
									discard;
								}
#else
								transparency = saturate(lerp(_TransparencyValue, 1, saturate(pow(iceValue, 2))));
#endif

								albedo = max(0, albedo);

								albedo.xyz = MixFog(albedo, i.fogCoord);
								return float4(albedo, transparency);
					}

					TessellatedVert tessellatedVert(VertexData v) {
						TessellatedVert p;

#ifdef USE_VR
						UNITY_SETUP_INSTANCE_ID(v);
						UNITY_TRANSFER_INSTANCE_ID(v, p);
#endif

						p.vertex = v.vertex;
						p.normal = v.normal;
						p.tangent = v.tangent;
						p.uv = v.uv;
						p.color = v.color;

#ifdef IS_ADD
#ifdef USE_INTER
						p.uv3 = v.uv3;
						p.uv4 = v.uv4;
						p.uv6 = v.uv6;
						p.uv7 = v.uv7;
#endif
#endif
						return p;
					}

					[UNITY_domain("tri")]
					InterpolatorsVertex Domain(
						TessellationFactors factors,
						OutputPatch<TessellatedVert, 3> patch,
						float3 barycentricCoordinates : SV_DomainLocation
						)
						{
						VertexData data;

						#define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) data.fieldName = \
							patch[0].fieldName * barycentricCoordinates.x + \
							patch[1].fieldName * barycentricCoordinates.y + \
							patch[2].fieldName * barycentricCoordinates.z;

						MY_DOMAIN_PROGRAM_INTERPOLATE(vertex)
						MY_DOMAIN_PROGRAM_INTERPOLATE(normal)
						MY_DOMAIN_PROGRAM_INTERPOLATE(tangent)
						MY_DOMAIN_PROGRAM_INTERPOLATE(uv)
						MY_DOMAIN_PROGRAM_INTERPOLATE(color)
#ifdef IS_ADD
#ifdef USE_INTER
							MY_DOMAIN_PROGRAM_INTERPOLATE(uv3)
							MY_DOMAIN_PROGRAM_INTERPOLATE(uv4)
							MY_DOMAIN_PROGRAM_INTERPOLATE(uv6)
							MY_DOMAIN_PROGRAM_INTERPOLATE(uv7)
#endif
#endif

#ifdef USE_VR
							UNITY_SETUP_INSTANCE_ID(data);
						UNITY_TRANSFER_INSTANCE_ID(patch[0], data);
#endif

						return vert(data);
						}

					ENDHLSL
					}

					// SHADOW CASTER PASS
					Pass{
					Tags {
						"LightMode" = "ShadowCaster"
					}

					HLSLPROGRAM

					#pragma target 4.6

							#pragma shader_feature _TESSELLATION_EDGE

							#pragma multi_compile _ LOD_FADE_CROSSFADE

							#pragma multi_compile_fwdbase
							#pragma multi_compile_fog

							#pragma vertex tessellatedVert
							#pragma fragment frag
							#pragma hull Hull
							#pragma domain Domain

							#define FORWARD_BASE_PASS
							#pragma shader_feature USE_AL
							#pragma shader_feature USE_RT
							#pragma shader_feature IS_ADD
							#pragma shader_feature USE_INTER
							#pragma shader_feature USE_WC

							#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
							#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
							#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
							float3 _LightDirection;
							float3 _LightPosition;
							//#include "UnityPBSLighting.cginc"
							//#include "AutoLight.cginc"

					sampler2D  _DetailTex;
					float4 _MainTex_ST, _DetailTex_ST;

					// Render Texture Effects //
					uniform sampler2D _GlobalEffectRT;
					uniform float3 _Position;
					uniform float _OrthographicCamSize;
					uniform sampler2D _GlobalEffectRTAdditional;
					uniform float3 _PositionAdd;
					uniform float _OrthographicCamSizeAdditional;

					sampler2D _MainTex;

					float _UpVector, _NormalVector;
					float _AddSnowStrength, _RemoveSnowStrength, _DisplacementStrength;

					//ICE Variables
					sampler2D _SnowHeight;
					sampler2D _SnowTransition;
					float _TransitionScale;
					float _TransitionPower;
					float _HeightScale, _SnowScale;

					float _HasRT;
					half _OverallScale;

					half _DisplacementOffset;

					struct VertexData //appdata
					{
						float4 vertex : POSITION;
						float3 normal : NORMAL;
						float4 tangent : TANGENT;
						float2 uv : TEXCOORD0;
						float4 color : COLOR;

						float4 shadowCoord : TEXCOORD1;
#ifdef USE_VR
							UNITY_VERTEX_INPUT_INSTANCE_ID
#endif
#ifdef IS_ADD
#ifdef USE_INTER
								float2 uv3 : TEXCOORD3;
							float2 uv4 : TEXCOORD4;
							float2 uv6 : TEXCOORD6;
							float2 uv7 : TEXCOORD7;
#endif
#endif
					};

					struct InterpolatorsVertex
					{
						float4 pos : SV_POSITION;
						float3 normal : TEXCOORD1;
						float4 tangent : TANGENT;
						float4 uv : TEXCOORD0;
						float4 color : COLOR;
						float3 worldPos : TEXCOORD2;
						float3 viewDir: POSITION1;
						float3 normalDir: TEXCOORD3;

						float4 shadowCoord : TEXCOORD4;
#ifdef USE_VR
							UNITY_VERTEX_OUTPUT_STEREO
#endif
					};

					InterpolatorsVertex vert(VertexData v) {
						InterpolatorsVertex i;

#ifdef USE_VR
						UNITY_SETUP_INSTANCE_ID(v);
						//UNITY_INITIALIZE_OUTPUT(InterpolatorsVertex, i);
						UNITY_TRANSFER_INSTANCE_ID(InterpolatorsVertex, i);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(i);
#endif

						float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
						float3 originalPos = worldPos;

						//RT Cam effects
						float2 uv = worldPos.xz - _Position.xz;
						uv = uv / (_OrthographicCamSize * 2);
						uv += 0.5;

						float2 uvAdd = worldPos.xz - _PositionAdd.xz;
						uvAdd = uvAdd / (_OrthographicCamSizeAdditional * 2);
						uvAdd += 0.5;

						float3 rippleMain = 0;
						float3 rippleMainAdditional = 0;

						float ripples = 0;
						float ripples2 = 0;
						float ripples3 = 0;

						float uvRTValue = 0;

					#ifdef USE_RT
						if (_HasRT == 1)
						{
							// .b(lue) = Snow Dig / .r(ed) = Snow To Ice / .g(reen) = Snow Mount
							rippleMain = tex2Dlod(_GlobalEffectRT, float4(uv, 0, 0));
							rippleMainAdditional = tex2Dlod(_GlobalEffectRTAdditional, float4(uvAdd, 0, 0));
						}

					#ifdef IS_ICE
					#else
						float2 uvGradient = smoothstep(0, 5, length(max(abs(_Position.xz - worldPos.xz) - _OrthographicCamSize + 5, 0.0)));
						uvRTValue = saturate(uvGradient.x);

						ripples = lerp(rippleMain.x, rippleMainAdditional.x, uvRTValue);
						ripples2 = lerp(rippleMain.y, rippleMainAdditional.y, uvRTValue);
						ripples3 = lerp(rippleMain.z, rippleMainAdditional.z, uvRTValue);
					#endif

					#endif

						float slopeValue = 0;
					#ifdef IS_T
						half4 splat_control = tex2Dlod(_Control0, float4(v.uv, 0, 0));
						half4 splat_control1 = tex2Dlod(_Control1, float4(v.uv, 0, 0));

						float iceValue = saturate(1 - splat_control.r * _Metallic0 - splat_control.g * _Metallic1 - splat_control.b * _Metallic2 - splat_control.a * _Metallic3
							- ripples);

						float snowHeightNew = tex2Dlod(_Mask0, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1).r;
						snowHeightNew = lerp(snowHeightNew, tex2Dlod(_Mask1, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1).r, saturate(splat_control.g * (1 - _Metallic1)));
						snowHeightNew = lerp(snowHeightNew, tex2Dlod(_Mask2, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1).r, saturate(splat_control.b * (1 - _Metallic2)));
						snowHeightNew = lerp(snowHeightNew, tex2Dlod(_Mask3, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1).r, saturate(splat_control.a * (1 - _Metallic3)));

						float snowHeight = snowHeightNew;
					#else
						float iceValue = saturate((v.color.g + v.color.b) / 2 - ripples);

#ifdef USE_INTER
#ifdef IS_ADD			// custom intersection and slope value //
						float4 midPoint = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0));

						float4 quaternion = float4(v.uv6.x, -v.uv6.y, -v.uv7.x, -v.uv7.y);
						float3 offsetPoint = worldPos.xyz - midPoint;

						float3 rotatedVert = rotateVector(quaternion, -offsetPoint);
						float manualLerp = 0;

						manualLerp = v.uv4.x;

						rotatedVert = RotateAroundZInDegrees(float4(rotatedVert, 0), lerp(6, -6, (manualLerp)));
						rotatedVert = RotateAroundXInDegrees(float4(rotatedVert, 0), lerp(-55, 55, (manualLerp))) + midPoint;

						slopeValue = ((v.color.a) - (rotatedVert.y - 0.5));

						if (slopeValue > 0.0)
						{
							v.color.g = saturate(v.color.g + saturate(slopeValue * 3));
							v.color.b = saturate(v.color.b + saturate(slopeValue * 3));
						}
#endif
#endif
						if (v.color.b > 0.6 && v.color.g < 0.4)
						{
							iceValue = saturate(1 - v.color.b);
						}
						else
						{
							iceValue = saturate((v.color.g + v.color.b) / 2 - ripples);
						}
					#ifdef USE_WC
						float snowHeight = tex2Dlod(_SnowHeight, float4(originalPos.xz, 0, 0) * _HeightScale * 0.1).r;
					#else
						float snowHeight = tex2Dlod(_SnowHeight, float4(v.uv, 0, 0) * _HeightScale).r;
					#endif
					#endif

						i.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject));
					#ifdef IS_ICE
					#else

						v.color = lerp(v.color, saturate(float4(1, 0, 0, 0)), ripples);
						i.normal = normalize(v.normal);

					#ifdef IS_ADD
						float3 newNormal = normalize(i.normalDir);
						worldPos += ((float4(0, -_RemoveSnowStrength, 0, 0) * _UpVector - newNormal * _RemoveSnowStrength * _NormalVector) * ripples3 + (float4(0, _AddSnowStrength * snowHeight, 0, 0) * _UpVector + newNormal * _AddSnowStrength * snowHeight * _NormalVector) * ripples2 * saturate(1 - ripples3)) * saturate(iceValue * 3);
						worldPos += (float4(0, _DisplacementOffset, 0, 0) * _UpVector + newNormal * _DisplacementOffset * _NormalVector) * saturate(iceValue * 2.5);
						worldPos += (float4(0, 2 * _DisplacementStrength * snowHeight, 0, 0) * _UpVector) + (newNormal * 2 * _DisplacementStrength * snowHeight * _NormalVector * clamp(slopeValue * 20, 1, 2)) * saturate(saturate(iceValue * 2.5));

						worldPos = lerp(worldPos, mul(unity_ObjectToWorld, v.vertex), saturate(v.color.g - saturate(v.color.r + v.color.b)));

						v.vertex.xyz = lerp(mul(unity_WorldToObject, float4(originalPos, 1)).xyz, mul(unity_WorldToObject, float4(worldPos, 1)).xyz, iceValue);
					#else
						float3 newNormal = normalize(i.normalDir);
						worldPos += ((float4(0, -_RemoveSnowStrength, 0, 0) * _UpVector - newNormal * _RemoveSnowStrength * _NormalVector) * ripples3 + (float4(0, _AddSnowStrength * snowHeight, 0, 0) * _UpVector + newNormal * _AddSnowStrength * snowHeight * _NormalVector) * ripples2 * saturate(1 - ripples3)) * saturate(iceValue * 3);
						worldPos += (float4(0, _DisplacementOffset, 0, 0) * _UpVector + newNormal * _DisplacementOffset * _NormalVector) * saturate(iceValue * 2.5);
						worldPos += (float4(0, 2 * _DisplacementStrength * snowHeight, 0, 0) * _UpVector) + (newNormal * 2 * _DisplacementStrength * snowHeight * _NormalVector) * saturate(saturate(iceValue * 2.5));

						worldPos = lerp(worldPos, mul(unity_ObjectToWorld, v.vertex), saturate(v.color.g - saturate(v.color.r + v.color.b)));

						v.vertex.xyz = lerp(mul(unity_WorldToObject, float4(originalPos, 1)).xyz, mul(unity_WorldToObject, float4(worldPos, 1)).xyz, iceValue);

					#endif
					#endif

						//i.pos = UnityObjectToClipPos(v.vertex);
						//i.pos = GetVertexPositionInputs(v.vertex).positionCS;
						i.pos = TransformWorldToHClip(ApplyShadowBias(GetVertexPositionInputs(v.vertex).positionWS, GetVertexNormalInputs(v.normal).normalWS, _LightDirection));

						float4 objCam = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos, 1.0));
						float3 viewDir = v.vertex.xyz - objCam.xyz;

					#ifdef IS_T
						float4 tangent = float4 (1.0, 0.0, 0.0, -1.0);
						tangent.xyz = tangent.xyz - v.normal * dot(v.normal, tangent.xyz); // Orthogonalize tangent to normal.

						float tangentSign = tangent.w * unity_WorldTransformParams.w;
						float3 bitangent = cross(v.normal.xyz, tangent.xyz) * tangentSign;

						i.viewDir = float3(
							dot(viewDir, tangent.xyz),
							dot(viewDir, bitangent.xyz),
							dot(viewDir, v.normal.xyz)
							);

						i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
						i.tangent = tangent;

					#else
						float tangentSign = v.tangent.w * unity_WorldTransformParams.w;
						float3 bitangent = cross(v.normal.xyz, v.tangent.xyz) * tangentSign;

						i.viewDir = float3(
							dot(viewDir, v.tangent.xyz),
							dot(viewDir, bitangent.xyz),
							dot(viewDir, v.normal.xyz)
							);

						i.worldPos.xyz = mul(unity_ObjectToWorld, v.vertex);
						i.tangent = v.tangent;
					#endif

						i.color = v.color;

					#ifdef IS_T
						i.uv.xy = v.uv;
					#else
						i.uv.xy = TRANSFORM_TEX(v.uv, _MainTex) * _OverallScale;
					#endif
						i.uv.zw = TRANSFORM_TEX(v.uv, _DetailTex);


						//TRANSFER_SHADOW_CASTER_NORMALOFFSET(i)

						return i;
					}

								float4 frag(InterpolatorsVertex i) : SV_Target
								{
#ifdef USE_VR
								UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i)
#endif
					#ifdef	IS_ADD
										float3 worldPos = mul(unity_ObjectToWorld, i.pos);
						float3 originalPos = worldPos;

						float2 uv = worldPos.xz - _Position.xz;
						uv = uv / (_OrthographicCamSize * 2);
						uv += 0.5;

						float2 uvAdd = worldPos.xz - _PositionAdd.xz;
						uvAdd = uvAdd / (_OrthographicCamSizeAdditional * 2);
						uvAdd += 0.5;

						float3 rippleMain = 0;
						float3 rippleMainAdditional = 0;
										rippleMain = tex2Dlod(_GlobalEffectRT, float4(uv, 0, 0));
						rippleMainAdditional = tex2Dlod(_GlobalEffectRTAdditional, float4(uvAdd, 0, 0));

						float snowHeight = tex2D(_SnowTransition, (i.uv * _TransitionScale * _SnowScale)).r;
						float iceValue = saturate(pow((i.color.g + i.color.b) / 2, 0.35 + clamp((snowHeight - 0.5) * -_TransitionPower * (saturate(i.color.g + i.color.b)), -0.34, 1)));
						if (iceValue < 0.30)
						{
							discard;
						}
					#endif
								//SHADOW_CASTER_FRAGMENT(i)
						return 0; //Same as SHADOW_CASTER_FRAGMENT(i)
								}


								TessellatedVert tessellatedVert(VertexData v) {
									TessellatedVert p;
#ifdef USE_VR
									UNITY_SETUP_INSTANCE_ID(v);
									UNITY_TRANSFER_INSTANCE_ID(v, p);
#endif
									p.vertex = v.vertex;
									p.normal = v.normal;
									p.tangent = v.tangent;
									p.uv = v.uv;
									p.color = v.color;
#ifdef IS_ADD
#ifdef USE_INTER
									p.uv3 = v.uv3;
									p.uv4 = v.uv4;
									p.uv6 = v.uv6;
									p.uv7 = v.uv7;
#endif
#endif
									return p;
								}

								[UNITY_domain("tri")]
								InterpolatorsVertex Domain(
									TessellationFactors factors,
									OutputPatch<TessellatedVert, 3> patch,
									float3 barycentricCoordinates : SV_DomainLocation
								)
								{
									VertexData data;

					#define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) data.fieldName = \
							patch[0].fieldName * barycentricCoordinates.x + \
							patch[1].fieldName * barycentricCoordinates.y + \
							patch[2].fieldName * barycentricCoordinates.z;

									MY_DOMAIN_PROGRAM_INTERPOLATE(vertex)
										MY_DOMAIN_PROGRAM_INTERPOLATE(normal)
										MY_DOMAIN_PROGRAM_INTERPOLATE(tangent)
										MY_DOMAIN_PROGRAM_INTERPOLATE(uv)
										MY_DOMAIN_PROGRAM_INTERPOLATE(color)
#ifdef IS_ADD
#ifdef USE_INTER
										MY_DOMAIN_PROGRAM_INTERPOLATE(uv3)
										MY_DOMAIN_PROGRAM_INTERPOLATE(uv4)
										MY_DOMAIN_PROGRAM_INTERPOLATE(uv6)
										MY_DOMAIN_PROGRAM_INTERPOLATE(uv7)
#endif
#endif
#ifdef USE_VR
										UNITY_SETUP_INSTANCE_ID(data);
									UNITY_TRANSFER_INSTANCE_ID(patch[0], data);
#endif

										return vert(data);
								}

								ENDHLSL
							}
				}
}