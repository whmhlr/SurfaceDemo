Shader "Hidden/SurforgeSingle" {  // 定义着色器名称，Hidden表示在材质选择器中不可见
	Properties {  // 着色器属性定义块
		_LabelTint ("Label Tint", Color) = (1,1,1,1)  // 标签颜色色调
		_LabelTexture ("Label Texture", 2D) = "black" {}  // 标签纹理
		_LabelSpecularIntensity ("Label Specular Intensity", Range (0.0, 1.0)) = 0.136  // 标签镜面反射强度
		_LabelGlossiness ("Label Glossiness", Range (0.0, 1.0)) = 0.843  // 标签光泽度
		
		
		_NormalMap ("Normal(RGB) Alpha(A)", 2D) = "bump" {}  // 法线贴图(RGB通道)和Alpha通道
		
		_AoEdgesDirtDepth ("Ao(R) Edges(G) Dirt(B) Depth(A)", 2D) = "white" {}  // AO(红)、边缘(绿)、污垢(蓝)、深度(Alpha)贴图
			
		//noise
		_RGBAnoise ("RGBA Noise", 2D) = "black" {}  // RGBA噪点贴图
		
		//masks
		_ObjectMasks ("Object Masks", 2D) = "white" {}  // 对象遮罩
		_ObjectMasks2 ("Object Masks2", 2D) = "black" {}  // 第二个对象遮罩
		
		_Tint0 ("0 Tint", Color) = (1,1,1,1)  // 基础色调
		_SpecularTint0 ("0 Specular Tint", Color) = (1,1,1,1)  // 基础镜面反射色调
		
		_SpecularIntensity0 ("0 Specular Intensity", Range (0.0, 1.0)) = 0.2  // 基础镜面反射强度
		_0SpecularContrast ("0 Specular Contrast", Range (0, 1.0)) = 0.1  // 基础镜面反射对比度
		_0SpecularBrightness ("0 Specular Brightness", Range (0, 1.0)) = 0.5  // 基础镜面反射亮度
		
		_Glossiness0 ("0 Glossiness", Range (0.0, 1.0)) = 0.4  // 基础光泽度
		_GlossinessIntensity0 ("0 Glossiness Intensity", Range (0.0, 1.0)) = 0.2  // 基础光泽度强度
		_0GlossinessContrast ("0 Glossiness Contrast", Range (0.0, 1.0)) = 0.2  // 基础光泽度对比度
		_0GlossinessBrightness ("0 Glossiness Brightness", Range (0.0, 1.0)) = 0.5  // 基础光泽度亮度
		
		_0Paint1Intensity ("0 Paint 1 Intensity", Range (0.0, 1.0)) = 0.0  // 第一层油漆强度
		_0Paint2Intensity ("0 Paint 2 Intensity", Range (0.0, 1.0)) = 0.0  // 第二层油漆强度
		_0WornEdgesNoiseMix ("0 Worn Edges Noise Mix", Vector) = (0,0,0,1)  // 磨损边缘噪点混合
		_0WornEdgesAmount ("0 Worn Edges Amount", Range (0.0, 100.0)) = 1.0  // 磨损边缘数量
		_0WornEdgesOpacity ("0 Worn Edges Opacity", Range (0.0, 1.0)) = 0.0  // 磨损边缘透明度
		_0WornEdgesContrast ("0 Worn Edges Contrast", Range (1.0, 10.0)) = 2.0  // 磨损边缘对比度
		_0WornEdgesBorder ("0 Worn Edges Border", Range (0.0, 10.0)) = 0.0  // 磨损边缘边框
		_0WornEdgesBorderTint("0 Worn Edges Border Tint", Color) = (0.5,0.5,0.5,1)  // 磨损边缘边框色调
		_0UnderlyingDiffuseTint ("0 Underlying Diffuse Tint", Color) = (1,1,1,1)  // 底层漫反射色调
		_0UnderlyingSpecularTint ("0 Underlying Specular Tint", Color) = (1,1,1,1)  // 底层镜面反射色调
		_0UnderlyingDiffuse ("0 Underlying Diffuse", Range (0.0, 1.0)) = 1.0  // 底层漫反射强度
		_0UnderlyingSpecular ("0 Underlying Specular", Range (0.0, 1.0)) = 0.8  // 底层镜面反射强度
		_0UnderlyingGlossiness ("0 Underlying Glossiness", Range (0.0, 1.0)) = 0.5  // 底层光泽度
		_NormalsStrength0 ("0 Normals Strength", Range (-1.0, 1)) = 0.1  // 法线强度
		_Texture0 ("0 Texture", 2D) = "white" {}  // 基础纹理
		_BumpMap0("0 Normal Map", 2D) = "bump" {}  // 基础法线贴图
		_BumpMapStrength0 ("0 Normal Map Strength", Range (-2.0, 2)) = 1.0  // 基础法线贴图强度
		
		_OcclusionMap0("0 Occlusion Map", 2D) = "white" {}  // 基础环境光遮蔽贴图
		_OcclusionMapStrength0 ("0 Occlusion Map Strength", Range (0.0, 2)) = 1.0  // 基础环境光遮蔽贴图强度
		
		_SpecularMap0("0 Specular Map", 2D) = "black" {}  // 基础镜面反射贴图
		_UseSpecularMap0 ("Use Specular Map 0", Range (0.0, 1)) = 0.0  // 是否使用基础镜面反射贴图
		_GlossinessFromAlpha0 ("Glossiness From Alpha 0", Range (0.0, 1)) = 1.0  // 是否从Alpha通道获取光泽度
		
		_EmissionMap0("0 Emission Map", 2D) = "white" {}  // 基础自发光贴图
		_EmissionMapTint0 ("0 Emission Map Tint", Color) = (0,0,0,1)  // 基础自发光贴图色调
		_EmissionMapIntensity0 ("0 Emission Map Intensity", Range (0.0, 10.0)) = 1.0  // 基础自发光贴图强度
		
		
		_0Paint1Specular ("0 Paint 1 Specular", Range (0.0, 1.0)) = 0.0  // 第一层油漆镜面反射强度
		_0Paint1Glossiness ("0 Paint 1 Glossiness", Range (0.0, 1.0)) = 0.0  // 第一层油漆光泽度
		_0Paint1Color ("0 Paint 1 Color", Color) = (0.5,0.5,0.5,1)  // 第一层油漆颜色
		_0Paint1NoiseMix ("0 Paint 1 Noise Mix", Vector) = (0,0,0,0)  // 第一层油漆噪点混合
		_0Paint1MaskTex ("0 Paint 1 Mask", 2D) = "white" {}  // 第一层油漆遮罩纹理
		
		_0Paint2Specular ("0 Paint 2 Specular", Range (0.0, 1.0)) = 0.0  // 第二层油漆镜面反射强度
		_0Paint2Glossiness ("0 Paint 2 Glossiness", Range (0.0, 1.0)) = 0.0  // 第二层油漆光泽度
		_0Paint2Color ("0 Paint 2 Color", Color) = (0.5,0.5,0.5,1)  // 第二层油漆颜色
		_0Paint2NoiseMix ("0 Paint 2 Noise Mix", Vector) = (0,0,0,0)  // 第二层油漆噪点混合
		_0Paint2MaskTex ("0 Paint 2 Mask", 2D) = "white" {}  // 第二层油漆遮罩纹理
		
		_0GlobalTransparency ("0 Global Transparency", Range (0.0, 1.0)) = 0.0  // 全局透明度
		_0AlbedoTransparency ("0 Albedo Transparency", Range (0.0, 1.0)) = 0.0  // 反照率透明度
		_0Paint1Transparency ("0 Paint 1 Transparency", Range (0.0, 1.0)) = 0.0  // 第一层油漆透明度
		_0Paint2Transparency ("0 Paint 2 Transparency", Range (0.0, 1.0)) = 0.0  // 第二层油漆透明度
		
		_0MaterialRotation ("0 Material Rotation", Range (-360.0, 360.0)) = 0.0  // 材质旋转角度
		
		_0AlbedoIntensity("0 Albedo Intensity", Range(0.0, 1.0)) = 1.0  // 反照率强度
		_0MAlbedoIntensity("0 Albedo Intensity", Range(0.0, 1.0)) = 1.0  // 金属反照率强度
		_MTint0("0 Tint", Color) = (1,1,1,1)  // 金属色调
		_Metallic0("0 Metallic", Range(0.0, 1.0)) = 1.0  // 金属度
		_0MUnderlyingDiffuseTint("0 Underlying Diffuse Tint", Color) = (1,1,1,1)  // 金属底层漫反射色调
		_0UnderlyingMetallic("0 Underlying Metallic", Range(0.0, 1.0)) = 1.0  // 底层金属度
		_0Paint1Metallic("0 Paint 1 Metallic", Range(0.0, 1.0)) = 0.0  // 第一层油漆金属度
		_0Paint2Metallic("0 Paint 2 Metallic", Range(0.0, 1.0)) = 0.0  // 第二层油漆金属度
		_0Paint1MColor("0 Paint 1 Color", Color) = (0.5,0.5,0.5,1)  // 第一层油漆金属颜色
		_0Paint2MColor("0 Paint 2 Color", Color) = (0.5,0.5,0.5,1)  // 第二层油漆金属颜色


		
		//dirt
		_Dirt1Tint ("Dirt 1 Tint", Color) = (1, 1, 1, 1)  // 第一层污垢色调
		_DirtNoise1Mix ("Dirt 1 Noise Mix", Vector) = (1,0.5,0,0)  // 第一层污垢噪点混合
		_Dirt1Amount ("Dirt 1 Amount", Range (0.0, 20.0)) = 0.0  // 第一层污垢数量
		_Dirt1Contrast ("Dirt 1 Contrast", Range (1.0, 8.0)) = 2.0  // 第一层污垢对比度
		_Dirt1Opacity ("Dirt 1 Opacity", Range (0.0, 1.0)) = 1.0  // 第一层污垢透明度
		_DirtTexture1 ("Dirt 1 Texture", 2D) = "white" {}  // 第一层污垢纹理
		
		_Dirt2Tint ("Dirt 2 Tint", Color) = (1, 1, 1, 1)  // 第二层污垢色调
		_DirtNoise2Mix ("Dirt 2 Noise Mix", Vector) = (1,0.5,0,0)  // 第二层污垢噪点混合
		_Dirt2Amount ("Dirt 2 Amount", Range (0.0, 20.0)) = 0  // 第二层污垢数量
		_Dirt2Contrast ("Dirt 2 Contrast", Range (1.0, 8.0)) = 2.0  // 第二层污垢对比度
		_Dirt2Opacity ("Dirt 2 Opacity", Range (0.0, 1.0)) = 1.0  // 第二层污垢透明度
		_DirtTexture2 ("Dirt 2 Texture", 2D) = "white" {}  // 第二层污垢纹理
	
		
		//emission
		_0EmissionTint ("Emission 0 Tint", Color) = (1,1,1,1)  // 自发光0色调
		_0EmissionIntensity ("Emission 0 Intensity", Range (0.0, 20.0)) = 1.0  // 自发光0强度
		_1EmissionTint ("Emission 1 Tint", Color) = (1,1,1,1)  // 自发光1色调
		_1EmissionIntensity ("Emission 1 Intensity", Range (0.0, 20.0)) = 1.0  // 自发光1强度
		_2EmissionTint ("Emission 2 Tint", Color) = (1,1,1,1)  // 自发光2色调
		_2EmissionIntensity ("Emission 2 Intensity", Range (0.0, 20.0)) = 1.0  // 自发光2强度

		_EmissionMask ("Emission Masks", 2D) = "black" {}  // 自发光遮罩
		
		
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.0  // Alpha截断值
		
		//specular and glossiness mapping
		_specMin ("Specular Min", Range (0.0, 1.0)) = 0  // 镜面反射最小值
		_specMax ("Specular Max", Range (0.0, 1.0)) = 1  // 镜面反射最大值
		
		_glossMin ("Glossiness Min", Range (0.0, 1.0)) = 0  // 光泽度最小值
		_glossMax ("Glossiness Max", Range (0.0, 1.0)) = 1  // 光泽度最大值
		
		//levels
		_gamma ("Gamma", Range (0.0, 10.0)) = 1.0  // 伽马值
		_minInput ("Min Input", Range (0.0, 1.0)) = 0  // 最小输入值
		_maxInput ("Max Input", Range (0.0, 1.0)) = 1  // 最大输入值
		_minOutput ("Min Output", Range (0.0, 1.0)) = 0  // 最小输出值
		_maxOutput ("Max Output", Range (0.0, 1.0)) = 1  // 最大输出值
		
		
		//HSBC final albedo
		_Hue ("Hue", Range (0.0, 1.0)) = 0  // 色相
		_Saturation ("Saturation", Range (0.0, 1.0)) = 0.5  // 饱和度
		_Brightness ("Brightness", Range (0.0, 1.0)) = 0.5  // 亮度
		_Contrast ("Contrast", Range (0.0, 1.0)) = 0.5  // 对比度
		
		_GlobalScale ("GlobalScale", Range (0.1, 2.0)) = 1.0  // 全局缩放
		
		_LinearColorSpace ("LinearColorSpace", Int) = 0  // 是否使用线性色彩空间
	}  // 属性定义块结束
	
	CGINCLUDE  // CG代码包含块开始
        #define _GLOSSYENV 1  // 定义使用光泽环境
        #define UNITY_SETUP_BRDF_INPUT SpecularSetup  // 设置BRDF输入为镜面反射设置
    ENDCG  // CG代码包含块结束
	
	
	SubShader {  // 子着色器定义开始
		Tags { "RenderType"="Opaque" "RenderMaterialIcon"="1"}  // 渲染标签，设置为不透明，并且渲染材质图标
		LOD 300  // 细节级别为300
		
		// Mask 0
		CGPROGRAM  // CG程序块开始
		#pragma target 3.0  // 设置着色器目标为3.0
		#include "UnityPBSLighting.cginc"  // 包含Unity PBS光照函数库
		#include "Surforge.cginc"  // 包含Surforge自定义函数库
		#pragma surface surf StandardSpecular  fullforwardshadows addshadow nolightmap vertex:vert  // 定义表面着色器，使用StandardSpecular光照模型，全前向阴影，添加阴影，无光照贴图，使用vert顶点函数
		
		sampler2D _Texture0;  // 声明基础纹理采样器
		sampler2D _BumpMap0;  // 声明基础法线贴图采样器
		sampler2D _OcclusionMap0;  // 声明基础环境光遮蔽贴图采样器
		fixed4 _Tint0;  // 声明基础色调变量
		
		fixed4 _SpecularTint0;  // 声明基础镜面反射色调变量
		half _SpecularIntensity0;  // 声明基础镜面反射强度变量
		float _0SpecularContrast;  // 声明基础镜面反射对比度变量
		float _0SpecularBrightness;  // 声明基础镜面反射亮度变量

		half _Glossiness0;  // 声明基础光泽度变量
		half _GlossinessIntensity0;  // 声明基础光泽度强度变量
		float _0GlossinessContrast;  // 声明基础光泽度对比度变量
		float _0GlossinessBrightness;  // 声明基础光泽度亮度变量
			
		float _NormalsStrength0;  // 声明法线强度变量
		float _BumpMapStrength0;  // 声明法线贴图强度变量
		float _OcclusionMapStrength0;  // 声明环境光遮蔽贴图强度变量
		
		sampler2D _SpecularMap0;  // 声明镜面反射贴图采样器
		float _UseSpecularMap0;  // 声明是否使用镜面反射贴图变量
		float _GlossinessFromAlpha0;  // 声明是否从Alpha通道获取光泽度变量
		
		sampler2D _EmissionMap0;  // 声明自发光贴图采样器
		fixed4 _EmissionMapTint0;  // 声明自发光贴图色调变量
		float _EmissionMapIntensity0;  // 声明自发光贴图强度变量
		
		half _0WornEdgesContrast;  // 声明磨损边缘对比度变量
		half _0WornEdgesAmount;  // 声明磨损边缘数量变量
		half _0WornEdgesBorder;  // 声明磨损边缘边框变量
		half _0WornEdgesOpacity;  // 声明磨损边缘透明度变量
		fixed4 _0WornEdgesBorderTint;  // 声明磨损边缘边框色调变量
		fixed4 _0WornEdgesNoiseMix;  // 声明磨损边缘噪点混合变量
		fixed4 _0UnderlyingDiffuseTint;  // 声明底层漫反射色调变量
		fixed4 _0UnderlyingSpecularTint;  // 声明底层镜面反射色调变量
		half _0UnderlyingDiffuse;  // 声明底层漫反射强度变量
		half _0UnderlyingSpecular;  // 声明底层镜面反射强度变量
		half _0UnderlyingGlossiness;  // 声明底层光泽度变量
		
		half _0Paint1Intensity;  // 声明第一层油漆强度变量
		half _0Paint2Intensity;  // 声明第二层油漆强度变量
		
		half _0Paint1Specular;  // 声明第一层油漆镜面反射强度变量
		half _0Paint1Glossiness;  // 声明第一层油漆光泽度变量
		fixed4 _0Paint1Color;  // 声明第一层油漆颜色变量
		sampler2D _0Paint1MaskTex;  // 声明第一层油漆遮罩纹理采样器
		fixed4 _0Paint1NoiseMix;  // 声明第一层油漆噪点混合变量
			
		half _0Paint2Specular;  // 声明第二层油漆镜面反射强度变量
		half _0Paint2Glossiness;  // 声明第二层油漆光泽度变量
		fixed4 _0Paint2Color;  // 声明第二层油漆颜色变量
		sampler2D _0Paint2MaskTex;  // 声明第二层油漆遮罩纹理采样器
		fixed4 _0Paint2NoiseMix;  // 声明第二层油漆噪点混合变量
		
		half _0GlobalTransparency;  // 声明全局透明度变量
		half _0AlbedoTransparency;  // 声明反照率透明度变量
		half _0Paint1Transparency;  // 声明第一层油漆透明度变量
		half _0Paint2Transparency;  // 声明第二层油漆透明度变量

		float _0MaterialRotation;  // 声明材质旋转角度变量
		
		float4 _Texture0_ST;  // 声明基础纹理的缩放和平移变量
		float4 _BumpMap0_ST;  // 声明基础法线贴图的缩放和平移变量
		float4 _EmissionMap0_ST;  // 声明自发光贴图的缩放和平移变量
		float4 _0Paint1MaskTex_ST;  // 声明第一层油漆遮罩纹理的缩放和平移变量
		float4 _0Paint2MaskTex_ST;  // 声明第二层油漆遮罩纹理的缩放和平移变量
		
		float4 _EmissionMask_ST;  // 声明自发光遮罩的缩放和平移变量


		struct Input {  // 输入结构体定义
			float2 uv_NormalMap;  // 法线贴图的UV坐标
			
			float2 texcoord_Texture0;  // 基础纹理的UV坐标
			float2 texcoord_BumpMap0;  // 基础法线贴图的UV坐标
			float2 texcoord_EmissionMap0;  // 自发光贴图的UV坐标
			float2 texcoord_0Paint1MaskTex;  // 第一层油漆遮罩纹理的UV坐标
			float2 texcoord_0Paint2MaskTex;  // 第二层油漆遮罩纹理的UV坐标
			float2 texroord_Noise;  // 噪点纹理的UV坐标
			
			float4 screenPos;  // 屏幕坐标
		};  // 输入结构体定义结束
		
		
		void vert (inout appdata_full v, out Input o) {  // 顶点函数定义
			UNITY_INITIALIZE_OUTPUT(Input,o);  // 初始化输出结构体
			
            float rad = radians(_0MaterialRotation);  // 将角度转换为弧度
			float s = sin (rad);  // 计算旋转角度的正弦值
			float c = cos (rad);  // 计算旋转角度的余弦值
			float2x2 rotationMatrix = float2x2( c, -s, s, c);  // 创建2x2旋转矩阵
			
			o.texcoord_Texture0 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _Texture0) + 0.5;  // 应用旋转并变换基础纹理UV
			o.texcoord_BumpMap0 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _BumpMap0) + 0.5;  // 应用旋转并变换基础法线贴图UV
			o.texcoord_EmissionMap0 = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMap0) + 0.5;  // 应用旋转并变换自发光贴图UV
			o.texcoord_0Paint1MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _0Paint1MaskTex) + 0.5;  // 应用旋转并变换第一层油漆遮罩纹理UV
			o.texcoord_0Paint2MaskTex = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _0Paint2MaskTex) + 0.5;  // 应用旋转并变换第二层油漆遮罩纹理UV
			o.texroord_Noise = TRANSFORM_TEX(mul ( v.texcoord.xy - 0.5, rotationMatrix ), _EmissionMask) + 0.5;  // 应用旋转并变换噪点纹理UV
        }  // 顶点函数定义结束


		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {  // 表面着色器函数定义
			fixed4 objectLabel = tex2D(_LabelTexture, IN.uv_NormalMap) * _LabelTint;  // 采样标签纹理并应用色调
			fixed4 objectLabelAlpha = tex2D(_LabelTexture, IN.uv_NormalMap).a;  // 采样标签纹理的Alpha通道

			fixed ao = GetResultAoMap(tex2D(_AoEdgesDirtDepth, IN.uv_NormalMap).r);  // 从AO贴图获取环境光遮蔽值
            fixed4 objectMasks = tex2D(_ObjectMasks, IN.uv_NormalMap);  // 采样对象遮罩
            fixed4 objectTexture0 = tex2D(_Texture0, IN.texcoord_Texture0);  // 采样基础纹理
            objectTexture0.a = 1;  // 设置基础纹理的Alpha通道为1
            fixed edgeMask = tex2D(_AoEdgesDirtDepth, IN.uv_NormalMap).g;  // 从AO贴图获取边缘遮罩
            fixed dirtMask = tex2D(_AoEdgesDirtDepth, IN.uv_NormalMap).b;  // 从AO贴图获取污垢遮罩

     
            fixed noiseR = tex2D(_EmissionMask, IN.texroord_Noise / _GlobalScale).b;  // 采样噪点纹理的蓝色通道
            fixed noiseB = tex2D(_EmissionMask, IN.texroord_Noise / _GlobalScale).a;  // 采样噪点纹理的Alpha通道
            fixed noiseG1 = tex2D(_EmissionMask, IN.texroord_Noise / _GlobalScale + 0.5).b;  // 采样偏移后的噪点纹理蓝色通道
            fixed noiseG2 = tex2D(_EmissionMask, 1 - IN.texroord_Noise / _GlobalScale + 0.5).a;  // 采样反向偏移后的噪点纹理Alpha通道
            fixed noiseG = clamp( (1 - noiseG1) * 0.5 + (1 - noiseG2) * 0.5, 0, 1);  // 计算G通道噪点值
            
            fixed4 rgbaNoise = fixed4(noiseR, noiseG, noiseB, 1);  // 创建RGBA噪点值
            
            //worn edges
            fixed edgeNoise = GetEdgeNoise(rgbaNoise, _0WornEdgesNoiseMix);  // 获取边缘噪点
        	fixed wornEdges = GetWornEdges(ao, edgeMask, edgeNoise, _0WornEdgesContrast, _0WornEdgesAmount, _0WornEdgesOpacity);  // 获取磨损边缘效果
            fixed wornEdgesCenter = GetWornEdgesCenter(ao, edgeMask, edgeNoise, _0WornEdgesContrast, _0WornEdgesAmount, _0WornEdgesOpacity, _0WornEdgesBorder);  // 获取磨损边缘中心效果

            
            //paint
			//1
			fixed4 basePaint1 = tex2D(_0Paint1MaskTex, IN.texcoord_0Paint1MaskTex);  // 采样第一层油漆基础纹理
			fixed4 basePaint1Mask = tex2D(_0Paint1MaskTex, IN.texcoord_0Paint1MaskTex).a;  // 采样第一层油漆遮罩的Alpha通道
			
			fixed maskNoiseBorder1 = GetMaskNoiseBorder(rgbaNoise, _0Paint1NoiseMix);  // 获取第一层油漆遮罩的噪点边框
			fixed4 pMask1 = GetPMask(basePaint1Mask, objectMasks.r, _0Paint1Intensity, maskNoiseBorder1);  // 计算第一层油漆遮罩
			fixed4 paintCol1 = GetPaintCol(pMask1, basePaint1, _0Paint1Color);  // 计算第一层油漆颜色
			fixed paintMask1 = pMask1.a;  // 获取第一层油漆遮罩的Alpha值
			
			
			//2
			fixed4 basePaint2 = tex2D(_0Paint2MaskTex, IN.texcoord_0Paint2MaskTex);  // 采样第二层油漆基础纹理
			fixed4 basePaint2Mask = tex2D(_0Paint2MaskTex, IN.texcoord_0Paint2MaskTex).a;  // 采样第二层油漆遮罩的Alpha通道
				
			fixed maskNoiseBorder2 = GetMaskNoiseBorder(rgbaNoise, _0Paint2NoiseMix);  // 获取第二层油漆遮罩的噪点边框
			fixed4 pMask2 = GetPMask(basePaint2Mask, objectMasks.r, _0Paint2Intensity, maskNoiseBorder2);  // 计算第二层油漆遮罩
			fixed4 paintCol2 = GetPaintCol(pMask2, basePaint2, _0Paint2Color);  // 计算第二层油漆颜色
			fixed paintMask2 = pMask2.a;  // 获取第二层油漆遮罩的Alpha值
			
			
			//dirt
			fixed baseDirt = GetBaseDirt(dirtMask);  // 获取基础污垢值
			
			//dirts1
			fixed dirtNoise1 = GetDirtNoise(rgbaNoise, _DirtNoise1Mix);  // 获取第一层污垢噪点
			fixed dirts1Mask = GetDirtMask(dirtNoise1, dirtMask, _Dirt1Amount, _Dirt1Contrast, _Dirt1Opacity);  // 计算第一层污垢遮罩
			fixed4 dirts1 = GetDirt(dirts1Mask, _Dirt1Tint);  // 计算第一层污垢颜色
				
			//dirts2
			fixed dirtNoise2 = GetDirtNoise(rgbaNoise, _DirtNoise2Mix);  // 获取第二层污垢噪点
			fixed dirts2Mask = GetDirtMask(dirtNoise2, dirtMask, _Dirt2Amount, _Dirt2Contrast, _Dirt2Opacity);  // 计算第二层污垢遮罩
			fixed4 dirts2 = GetDirt(dirts2Mask, _Dirt2Tint);  // 计算第二层污垢颜色
             
			
			//Specular/Glossiness
			fixed4 resultSpecularMap = 0;  // 初始化镜面反射贴图结果
			fixed resultGlossinessMap = 0;  // 初始化光泽度贴图结果
			fixed objectTexture0gloss = 0;  // 初始化基础纹理光泽度
			
			if (_UseSpecularMap0 > 0.5) {  // 如果使用镜面反射贴图
				fixed4 objectTexture0specRGB = GetSpecMap(tex2D(_SpecularMap0, IN.texcoord_BumpMap0 / _GlobalScale), _0SpecularContrast, _0SpecularBrightness);  // 获取RGB镜面反射贴图
				resultSpecularMap = GetResultSpecularMapRGB(objectTexture0specRGB, _SpecularIntensity0, _SpecularTint0, paintMask1, paintMask2, _0Paint1Specular, _0Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _0UnderlyingSpecular, _0UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _0GlobalTransparency);  // 计算最终镜面反射值
				
				if (_GlossinessFromAlpha0 > 0.5) objectTexture0gloss = GetGlossMap(tex2D(_SpecularMap0, IN.texcoord_BumpMap0 / _GlobalScale).a, _0GlossinessContrast, _0GlossinessBrightness);  // 从Alpha通道获取光泽度
				else {  // 否则
					if (_LinearColorSpace != 0) objectTexture0gloss = GetGlossMap(pow (tex2D(_SpecularMap0, IN.texcoord_BumpMap0 / _GlobalScale), 0.454545), _0GlossinessContrast, _0GlossinessBrightness);  // 线性色彩空间下的光泽度计算
					else objectTexture0gloss = GetGlossMap(tex2D(_SpecularMap0, IN.texcoord_BumpMap0 / _GlobalScale), _0GlossinessContrast, _0GlossinessBrightness);  // 伽马色彩空间下的光泽度计算
				}  
				resultGlossinessMap = GetResultGlossinessMapFromAlpha(objectTexture0gloss, _GlossinessIntensity0, _Glossiness0, paintMask1, paintMask2, _0Paint1Glossiness, _0Paint2Glossiness, objectLabelAlpha, _LabelGlossiness, wornEdges, _0UnderlyingGlossiness, _glossMin, _glossMax, _0GlobalTransparency);  // 计算最终光泽度
			}
			else {  // 不使用镜面反射贴图
			  	fixed objectTexture0spec = GetSpecMap(tex2D(_Texture0, IN.texcoord_Texture0 / _GlobalScale), _0SpecularContrast, _0SpecularBrightness);  // 从基础纹理获取镜面反射值
				resultSpecularMap = GetResultSpecularMap(objectTexture0spec, _SpecularIntensity0, _SpecularTint0, paintMask1, paintMask2, _0Paint1Specular, _0Paint2Specular, objectLabelAlpha, _LabelSpecularIntensity, wornEdges, _0UnderlyingSpecular, _0UnderlyingSpecularTint, dirts1Mask, dirts2Mask, _specMin, _specMax, _0GlobalTransparency);  // 计算最终镜面反射值
				
				if (_LinearColorSpace != 0) objectTexture0gloss = GetGlossMap(pow (tex2D(_Texture0, IN.texcoord_Texture0 / _GlobalScale), 0.454545), _0GlossinessContrast, _0GlossinessBrightness);  // 线性色彩空间下的光泽度计算
				else objectTexture0gloss = GetGlossMap(tex2D(_Texture0, IN.texcoord_Texture0 / _GlobalScale), _0GlossinessContrast, _0GlossinessBrightness);  // 伽马色彩空间下的光泽度计算

				
				resultGlossinessMap = GetResultGlossinessMap(objectTexture0gloss, _GlossinessIntensity0, _Glossiness0, paintMask1, paintMask2, _0Paint1Glossiness, _0Paint2Glossiness, objectLabelAlpha, _LabelGlossiness, wornEdges, _0UnderlyingGlossiness, _glossMin, _glossMax, _0GlobalTransparency);  // 计算最终光泽度
			} 
			
			
			
			//Diffuse
			fixed4 diffuse = GetDiffuse(objectTexture0, _Tint0, paintMask1, paintMask2, _0Paint1Color, _0Paint2Color, paintCol1, paintCol2, objectLabelAlpha, objectLabel, dirts1Mask, dirts2Mask, dirts1, dirts2, _0UnderlyingDiffuseTint, wornEdges, _0WornEdgesBorderTint, _0UnderlyingDiffuse, wornEdgesCenter);  // 计算最终漫反射颜色

				
			//Emission
			fixed4 emissionMask = tex2D(_EmissionMask, IN.uv_NormalMap);  // 采样自发光遮罩
			fixed4 emissionMap = tex2D(_EmissionMap0, IN.texcoord_EmissionMap0 / _GlobalScale);  // 采样自发光贴图
			fixed4 finalEmission = GetEmission(emissionMask, _0EmissionTint, _1EmissionTint, _0EmissionIntensity, _1EmissionIntensity, emissionMap, _EmissionMapTint0, _EmissionMapIntensity0);  // 计算最终自发光颜色
			
			
			//Material AO
			fixed materialAO = GetMaterialAO(tex2D(_OcclusionMap0, IN.texcoord_BumpMap0 / _GlobalScale).r, _OcclusionMapStrength0);  // 获取材质环境光遮蔽值
			

			//levels 
			fixed4 finalAlbedo = diffuse * ao * materialAO;  // 计算最终反照率
	
			float3 finalAlbedoLevels = LevelsControl(float3 (finalAlbedo.r, finalAlbedo.g, finalAlbedo.b), _minInput, _gamma, _maxInput, _minOutput, _maxOutput);  // 应用色阶控制
			
			float4 finalColor = fixed4(finalAlbedoLevels.r, finalAlbedoLevels.g, finalAlbedoLevels.b, 1);  // 创建最终颜色

				
			//Normal detailing
			float me = tex2D(_Texture0,IN.texcoord_Texture0).x;  // 采样当前像素的纹理值
			float n = tex2D(_Texture0,float2(IN.texcoord_Texture0.x,IN.texcoord_Texture0.y+1.0/2048)).x;  // 采样上方像素的纹理值
			float s = tex2D(_Texture0,float2(IN.texcoord_Texture0.x,IN.texcoord_Texture0.y-1.0/2048)).x;  // 采样下方像素的纹理值
			float e = tex2D(_Texture0,float2(IN.texcoord_Texture0.x-1.0/2048,IN.texcoord_Texture0.y)).x;  // 采样左方像素的纹理值
			float w = tex2D(_Texture0,float2(IN.texcoord_Texture0.x+1.0/2048,IN.texcoord_Texture0.y)).x;  // 采样右方像素的纹理值
			
			float3 finalNormal = GetFinalNormal(tex2D(_NormalMap, IN.uv_NormalMap), me, n, s, e, w, objectMasks.r, _NormalsStrength0, tex2D(_BumpMap0, IN.texcoord_BumpMap0), _BumpMapStrength0, _0MaterialRotation);  // 计算最终法线


			o.Albedo = applyHSBEffect(finalColor, float4(_Hue, _Brightness, _Contrast, _Saturation));  // 设置反照率，应用HSB效果
			o.Occlusion = ao * materialAO;  // 设置环境光遮蔽
			o.Specular = resultSpecularMap;  // 设置镜面反射
			o.Smoothness = resultGlossinessMap;  // 设置平滑度
			o.Normal = finalNormal * 2-1;  // 设置法线，转换为-1到1范围
			o.Emission = applyHSBEffect(finalEmission, float4(_Hue, _Brightness, _Contrast, _Saturation));  // 设置自发光，应用HSB效果
		}  // 表面着色器函数定义结束
		ENDCG  // CG程序块结束
	
		
		
		
	}  // 子着色器定义结束

	//FallBack "Specular"
	CustomEditor "SurforgeSingleShaderGUI"  // 自定义着色器编辑器
}  // 着色器定义结束