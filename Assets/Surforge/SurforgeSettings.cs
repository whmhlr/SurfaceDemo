#if UNITY_EDITOR  // 仅在Unity编辑器中编译
using UnityEngine;  // 导入Unity引擎命名空间
using System.Collections;  // 导入System.Collections命名空间
using System.Collections.Generic;  // 导入System.Collections.Generic命名空间

[AddComponentMenu("")]  // 不显示在组件菜单中
[ExecuteInEditMode]  // 在编辑模式下执行
public class SurforgeSettings : MonoBehaviour {  // 定义SurforgeSettings类，继承自MonoBehaviour

	public SurforgeOctree octree;  // 八叉树数据结构，用于空间划分和加速

	public int currentLayer;  // 当前选中的图层索引
	public float scale = 1;  // 缩放比例，默认为1
	public int maxBranches = 10;  // 最大分支数量，默认为10

	public int activeSet;  // 当前激活的设置集

	public int activeContentPack;  // 当前激活的内容包索引
	public SurforgeContentPack[] contentPacks;  // 内容包数组
	
	[HideInInspector]  // 在Inspector面板中隐藏
	public List<SurforgeLayer> layers;  // 图层列表

	[HideInInspector]  // 在Inspector面板中隐藏
	public OctreeGizmosDraw octreeDrawPrefab;  // 八叉树 gizmo 绘制预设
	public GameObject root;  // 根游戏对象
 
	public int groupCounter;  // 组计数器

	public int lastAction;  // 上一个动作的标识

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isBrushActive;  // 画笔工具是否激活

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isPolygonLassoActive;  // 多边形套索工具是否激活

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isPlaceToolActive;  // 放置工具是否激活

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isLimitsActive;  // 限制工具是否激活

	public SurforgeLimits limits;  // 边界限制设置
	public SurforgeLimits textureBorders;  // 纹理边界设置

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool canIvyOnGlueproof;  // 是否可以在防粘区域生成藤蔓

	public SurforgeTexturePreview extentTexturePreview;  // 扩展纹理预览组件

	//rendering	// 渲染相关设置
	public Texture2D normalMap;  // 法线贴图

	public Texture2D emissionMap;  	// 自发光贴图

	public Texture2D objectMasks;  // 对象遮罩贴图

	public Texture2D objectMasks2;  // 第二对象遮罩贴图

	public Texture2D aoEdgesDirtDepth;  // AO(环境光遮蔽)、边缘、污垢和深度信息贴图

	public Texture2D labelsMap;  // 标签贴图

	public Texture2D labelsAlpha;  // 标签Alpha贴图


	//rendering temp  // 临时渲染资源

	[System.NonSerialized]  // 不序列化
	public RenderTexture renderTexture;  // 主渲染纹理

	[System.NonSerialized]  // 不序列化
	public RenderTexture renderTexture2;  // 第二渲染纹理


	[System.NonSerialized]  // 不序列化
	public RenderTexture edgesRenderTexture;  // 边缘渲染纹理

	[System.NonSerialized]  // 不序列化
	public RenderTexture glowRenderTexture;  // 发光效果渲染纹理


	[System.NonSerialized]  // 不序列化
	public Texture2D edgesTexture;  // 边缘纹理


	[System.NonSerialized]  // 不序列化
	public Texture2D glowTexture;  // 发光纹理


	[System.NonSerialized]  // 不序列化
	public Texture2D bucketTexture;  // 桶纹理



	public float backgroundQuadBorder;  // 背景四边形边框大小

	[HideInInspector]  // 在Inspector面板中隐藏
	public GameObject backgroundQuad;  // 背景四边形游戏对象

	[HideInInspector]  // 在Inspector面板中隐藏
	public GameObject polygonLassoPlane;  // 多边形套索平面游戏对象

	public PolyLassoProfiles polyLassoProfiles;  // 多边形套索配置文件
	public int activePolyLassoProfile;  // 当前激活的多边形套索配置索引

	public DecalSets decalSets;  // 贴图集
	public int activeDecal;  // 当前激活的贴图图索引

	public NoisePresets noisePresets;  // 噪点预设
	public int activeNoisePreset;  // 当前激活的噪点预设索引


	//gpu render  // GPU渲染相关设置
	int gpuRenderResolution = 0; //1024, 2048, 4096  // GPU渲染分辨率 (0:1024, 1:2048, 2:4096)

	int supersamplingMode = 0; //1, 2  // 超采样模式 (0:1x, 1:2x)

	//map export  // 地图导出相关设置
	[HideInInspector]  // 在Inspector面板中隐藏
	public string exportPath = "";  // 导出路径
	[HideInInspector]  // 在Inspector面板中隐藏
	public string textureName = "texture";  // 纹理名称

	int maskExportMode = 0; //1, 2  // 遮罩导出模式

	int aoMode = 0; //1  // AO模式

	//presets  // 预设相关设置
	public SurforgeComposerPresets composerPresets;  // 合成器预设
	[HideInInspector]  // 在Inspector面板中隐藏
	public int currentPreset = 0;  // 当前预设索引

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool seamless = false;  // 是否无缝模式

	[HideInInspector]  // 在Inspector面板中隐藏
	public List<GameObject> seamlessInstances = new List<GameObject>();  // 无缝实例列表

	public Shader[] materialGroups;  // 材质组着色器数组
	public Shader[] floaterMaterialGroups;  // 浮动材质组着色器数组
		

	public Shader exportDiffuse;  // 导出漫反射着色器
	public Shader exportNormals;  // 导出法线着色器
	public Shader exportSpecular;  // 导出镜面反射着色器
	public Shader exportGlossiness;  // 导出光泽度着色器
	public Shader exportAo;  // 导出AO着色器
	public Shader exportEmission;  // 导出自发光着色器
	public Shader exportHeightmap;  // 导出高度图着色器
	public Shader exportMask;  // 导出遮罩着色器
	public Shader exportMaskSeparate;  // 导出独立遮罩着色器
	public Shader exportAlpha;  // 导出Alpha着色器

    public Shader exportDiffuseMetallic;  // 导出金属漫反射着色器
    public Shader exportMetallic;  // 导出金属着色器


    public Shader grayBackgroundPost;  // 灰色背景后处理着色器

	//model  // 模型相关设置
	public Mesh model;  // 主模型网格

	public Mesh cubePreviewModel;  // 立方体预览模型网格

	//model bakedAO  // 模型烘焙的AO
	[HideInInspector]  // 在Inspector面板中隐藏
	public Texture2D modelBakedAO;  // 模型烘焙的AO贴图

	//model bakedNormalMap  // 模型烘焙的法线贴图
	[HideInInspector]  // 在Inspector面板中隐藏
	public Texture2D modelBakedNormal;  // 模型烘焙的法线贴图

	//model bakedEdgeMap  // 模型烘焙的边缘贴图
	[HideInInspector]  // 在Inspector面板中隐藏
	public Texture2D modelBakedEdgeMap;  // 模型烘焙的边缘贴图

	public Shader normalsBakedAddPost;  // 烘焙法线添加后处理着色器

	//toggle grid  // 网格显示开关
	[HideInInspector]  // 在Inspector面板中隐藏
	public bool showGrid;  // 是否显示网格

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool showUvs;  // 是否显示UV

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool showUvGrid = true;  // 是否显示UV网格

	[HideInInspector]  // 在Inspector面板中隐藏
	[System.NonSerialized]  // 不序列化
	public bool showLastAction;  // 是否显示上一个动作

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool showTextureEdges;  // 是否显示纹理边缘

	//toggle snap  // 吸附功能开关
	[HideInInspector]  // 在Inspector面板中隐藏
	public bool gloabalSnapActive = true;  // 全局吸附是否激活

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool gridSnapActive = true;  // 网格吸附是否激活

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool uvSnapActive = true;  // UV吸附是否激活

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool objectSnapActive = true;  // 对象吸附是否激活

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool selfSnapActive = true;  // 自身吸附是否激活

	//texture preview  // 纹理预览相关设置
	[HideInInspector]  // 在Inspector面板中隐藏
	public bool texturePreviewUpdated;  // 纹理预览是否已更新

	public List<PolyLassoObject> polyLassoObjects;  // 多边形套索对象列表

	public List<Material> sceneMaterials;  // 场景材质列表

	[HideInInspector]  // 在Inspector面板中隐藏
	public int activeSceneMaterialNumber;  // 当前激活的场景材质编号

	[HideInInspector]  // 在Inspector面板中隐藏
	public int newMaterialSetsCount;  // 新材料集数量

	public int GetGpuRenderResolution() {  // 获取GPU渲染分辨率的方法
		return gpuRenderResolution;  // 返回GPU渲染分辨率
	}
	public void SetGpuRenderResolution(int value) {  // 设置GPU渲染分辨率的方法
		gpuRenderResolution = value;  // 设置GPU渲染分辨率
	}

	public int GetSupersamplingMode() {  // 获取超采样模式的方法
		return supersamplingMode;  // 返回超采样模式
	}
	public void SetSupersamplingMode(int value) {  // 设置超采样模式的方法
		supersamplingMode = value;  // 设置超采样模式
	}

	public int GetActiveSceneMaterialNumber() {  // 获取当前激活场景材质编号的方法
		return activeSceneMaterialNumber;  // 返回当前激活的场景材质编号
	}

	public void SetActiveSceneMaterialNumber(int numberToSet) {  // 设置当前激活场景材质编号的方法
		activeSceneMaterialNumber = numberToSet;  // 设置当前激活的场景材质编号
	}

	public int GetMaskExportMode() {  // 获取遮罩导出模式的方法
		return maskExportMode;  // 返回遮罩导出模式
	}
	public void SetMaskExportMode(int value) {  // 设置遮罩导出模式的方法
		maskExportMode = value;  // 设置遮罩导出模式
	}

	public int GetAoMode() {  // 获取AO模式的方法
		return aoMode;  // 返回AO模式
	}
	public void SetAoMode(int value) {  // 设置AO模式的方法
		aoMode = value;  // 设置AO模式
	}


	public PlaceMeshes placeMeshes;  // 放置网格组件
	public int activePlaceMesh;  // 当前激活的放置网格索引

	[HideInInspector]  // 在Inspector面板中隐藏
	public GameObject placeToolPlane;  // 放置工具平面游戏对象

	public GameObject placeToolPreview;  // 放置工具预览游戏对象

	public GameObject placeToolPreviewSymmX;  // X轴对称放置工具预览
	public GameObject placeToolPreviewSymmZ;  // Z轴对称放置工具预览
	public GameObject placeToolPreviewSymmXZ;  // XZ轴对称放置工具预览

	public GameObject placeToolPreviewSymmDiagonal;  // 对角对称放置工具预览
	public GameObject placeToolPreviewSymmDiagonalX;  // X轴对角对称放置工具预览
	public GameObject placeToolPreviewSymmDiagonalZ;  // Z轴对角对称放置工具预览
	public GameObject placeToolPreviewSymmDiagonalXZ;  // XZ轴对角对称放置工具预览

	public float placeToolVerticalOffset;  // 放置工具垂直偏移

	//symmetry  // 对称相关设置
	public Vector3 symmetryPoint = Vector3.zero;  // 对称中心点，默认为原点

	[HideInInspector]  // 在Inspector面板中隐藏
	public GameObject symmetryParent;  // 对称父游戏对象

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool symmetry;  // 是否启用对称
 
	[HideInInspector]  // 在Inspector面板中隐藏
	public bool symmetryX = true;  // 是否启用X轴对称

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool symmetryZ;  // 是否启用Z轴对称

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool symmetryDiagonal;  // 是否启用对角对称

	public SurforgeSkybox[] skyboxes;  // 天空盒数组
	public int activeSkybox;  // 当前激活的天空盒索引
	public int materialIconsRenderedWithSkybox;  // 使用天空盒渲染的材质图标数量

	public Material[] materials;  // 材质数组
	public Material[] loadedMaterials;  // 已加载材质数组
	public int activeMaterial;  // 当前激活的材质索引

	public float activeShowID;  // 当前显示的ID值
 
	public Camera renderMaterialIconCameraPrefab;  // 渲染材质图标摄像机预设
	public Camera renderMaterialIconCameraLitePrefab;  // 渲染材质图标轻量摄像机预设
	public GameObject renderMaterialIconPrefab;  // 渲染材质图标预设
	public Shader renderMaterialIconShader;  // 渲染材质图标着色器
	public Texture2D rgbaNoise;  // RGBA噪点贴图
	public Texture2D renderMaterialIconNormal;  // 渲染材质图标法线贴图
	public Texture2D renderMaterialIconAoEdgesDirtDepth;  // 渲染材质图标AO/边缘/污垢/深度贴图
	public Texture2D renderMaterialIconNoise;  // 渲染材质图标噪点贴图

	public Texture2D editorPreviewAoEdgesDirtDepth;  // 编辑器预览AO/边缘/污垢/深度贴图
	public Texture2D editorPreviewMasks;  // 编辑器预览遮罩贴图
	public Texture2D editorPreviewMasks2;  // 编辑器预览第二遮罩贴图
	
	public Texture2D[] materialButtons;  // 材质按钮纹理数组

	public float globalScale = 1.0f;  // 全局缩放比例，默认为1.0

	public GameObject wireframeObject;  // 线框对象游戏对象
	public bool showWireframe;  // 是否显示线框

	public GameObject floorObject;  // 地面对象游戏对象
	public bool showFloor;  // 是否显示地面

	[HideInInspector]  // 在Inspector面板中隐藏
	public List<Vector3> mirrorLineSolid = new List<Vector3> {Vector3.zero, new Vector3(0, 0, 1.0f)};  // 实线镜像线（Z轴）

	[HideInInspector]  // 在Inspector面板中隐藏
	public List<Vector3> mirrorLineDotted = new List<Vector3> {Vector3.zero, new Vector3(1.0f, 0, 0)};  // 虚线镜像线（X轴）

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool showSymAxis = true;  // 是否显示对称轴

	public ShatterPresets shatterPresets;  // 破碎预设
	public int activeShatterPreset;  // 当前激活的破碎预设索引

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool seamlessNeedUpdate;  // 无缝模式是否需要更新

	public List<List<Vector3>> storedShapes = new List<List<Vector3>>();  // 存储的形状列表（点集合）
	public List<int> storedGroups = new List<int>();  // 存储的组列表


	[HideInInspector]  // 在Inspector面板中隐藏
	public string lastActionText = "";  // 上一个动作的文本描述

	[HideInInspector]  // 在Inspector面板中隐藏
	public string lastActionHotkey = "";  // 上一个动作的快捷键

	[HideInInspector]  // 在Inspector面板中隐藏
	public string lastActionSecondHotkey = "";  // 上一个动作的第二个快捷键

	[HideInInspector]  // 在Inspector面板中隐藏
	public float lastActionTimer = 0;  // 上一个动作的计时器

	[HideInInspector]  // 在Inspector面板中隐藏
	public List<Vector3> warpShape;  // 扭曲形状的点集合

	[HideInInspector]  // 在Inspector面板中隐藏
	public Vector3 warpShapeCenterLinePoint = Vector3.zero;  // 扭曲形状中心线点

	[HideInInspector]  // 在Inspector面板中隐藏
	public float doubleclickTimer = 0;  // 双击计时器

	[HideInInspector]  // 在Inspector面板中隐藏
	public float skyboxWindowRepaintTimer = 0;  // 天空盒窗口重绘计时器

    [HideInInspector]  // 在Inspector面板中隐藏
    public float cameraDeactivateTimer = 0;  // 摄像机停用计时器

    [HideInInspector]  // 在Inspector面板中隐藏
	public bool skyboxNeedWindowUpdate;  // 天空盒窗口是否需要更新

	public int uvGridStep;  // UV网格步长

	public Material seamlessHighlight;  // 无缝高亮材质

	public GameObject mapsExportCameraPrefab;  // 地图导出摄像机预设

	public Shader bucketDownscale;  // 桶缩放着色器
	public Shader bucketsCollect;  // 桶收集着色器

	bool skipDestroyRoot;  // 是否跳过销毁根对象
	bool skipDestoryTexturePreview;  // 是否跳过销毁纹理预览

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool modelNeedUpdate;  // 模型是否需要更新

	public void SkipDestroyRoot() {  // 跳过销毁根对象的方法
		skipDestroyRoot = true;  // 设置跳过销毁根对象标志
	}

	public void SkipDestroyTexturePreview() {  // 跳过销毁纹理预览的方法
		skipDestoryTexturePreview = true;  // 设置跳过销毁纹理预览标志
	}

	[HideInInspector]  // 在Inspector面板中隐藏
	public Material composerMaterialAsset;  // 合成器材质资源

	public Object modelObject;  // 模型对象


	//linear color space texture flags  // 线性色彩空间纹理标志
	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isPreviewRenderTextureLinear = false;  // 预览渲染纹理是否为线性空间


	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isNormalMapLinear = false;  // 法线贴图是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isEmissionMapLinear = false;  // 自发光贴图是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isObjectMasksLinear = false;  // 对象遮罩贴图是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isObjectMasks2Linear = false;  // 第二对象遮罩贴图是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isAoEdgesDirtDepthLinear = false;  // AO/边缘/污垢/深度贴图是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isLabelsMapLinear = false;  // 标签贴图是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isLabelsAlphaLinear = false;  // 标签Alpha贴图是否为线性空间


	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isRenderTextureLinear = false;  // 渲染纹理是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isRenderTexture2Linear = false;  // 第二渲染纹理是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isEdgesRenderTextureLinear = false;  // 边缘渲染纹理是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isGlowRenderTextureLinear = false;  // 发光渲染纹理是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isEdgesTextureLinear = false;  // 边缘纹理是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isGlowTextureLinear = false;  // 发光纹理是否为线性空间

	[HideInInspector]  // 在Inspector面板中隐藏
	public bool isBucketTextureLinear = false;  // 桶纹理是否为线性空间

	[System.NonSerialized]  // 不序列化
	public bool greeblesVoxelized = false;  // 细节元素是否已体素化



	[HideInInspector]  // 在Inspector面板中隐藏
	[System.NonSerialized]  // 不序列化
	public string[] materialFoldersNames;  // 材质文件夹名称数组

	[HideInInspector]  // 在Inspector面板中隐藏
	[System.NonSerialized]  // 不序列化
	public int[] materialFoldersMatCount;  // 材质文件夹中材质数量数组

	[HideInInspector]  // 在Inspector面板中隐藏
	[System.NonSerialized]  // 不序列化
	public bool[] materialFoldersFoldout;  // 材质文件夹展开状态数组

    
    public bool isMetallic;  // 是否为金属材质

    public string surforgePath;  // Surforge路径


    void OnDestroy(){  // 对象销毁时调用
		if (root != null) {  // 如果根对象不为空
			DestroySeamlessInstances();  // 销毁无缝实例
			if (!skipDestroyRoot) {  // 如果不跳过销毁根对象
				SurforgeRoot surforgeRoot = (SurforgeRoot)root.GetComponent<SurforgeRoot>();  // 获取根对象的SurforgeRoot组件
				surforgeRoot.SkipDestroy();  // 通知根对象跳过销毁
				DestroyImmediate(root.gameObject);  // 立即销毁根游戏对象
			}
			if (!skipDestoryTexturePreview) {  // 如果不跳过销毁纹理预览
				if (extentTexturePreview != null) {  // 如果扩展纹理预览不为空
					extentTexturePreview.SkipDestroy();  // 通知扩展纹理预览跳过销毁
					DestroyImmediate(extentTexturePreview.gameObject);  // 立即销毁扩展纹理预览游戏对象
				}
			}
		}
	}


	void DestroySeamlessInstances() {  // 销毁无缝实例的方法
		foreach (Transform child in root.transform) {  // 遍历根对象的所有子对象

			PolyLassoObject pObj = (PolyLassoObject)child.gameObject.GetComponent<PolyLassoObject>();  // 尝试获取多边形套索对象组件
			if (pObj != null) {  // 如果存在多边形套索对象
				if (pObj.seamlessInstances != null) {  // 如果无缝实例数组不为空
					for (int i=0; i<pObj.seamlessInstances.Length; i++) {  // 遍历所有无缝实例
						if (pObj.seamlessInstances[i] != null) {  // 如果实例不为空
							if (pObj.seamlessInstances[i].gameObject != null) DestroyImmediate(pObj.seamlessInstances[i].gameObject);  // 立即销毁实例游戏对象
						}
					}
				}
			}

			else {  // 如果不是多边形套索对象
				PlaceMesh placeMeshObj = (PlaceMesh)child.gameObject.GetComponent<PlaceMesh>();  // 尝试获取放置网格组件
				if (placeMeshObj != null) {  // 如果存在放置网格组件
					if (placeMeshObj.seamlessInstances != null) {  // 如果无缝实例数组不为空
						for (int i=0; i<placeMeshObj.seamlessInstances.Length; i++) {  // 遍历所有无缝实例
							if (placeMeshObj.seamlessInstances[i] != null) {  // 如果实例不为空
								if (placeMeshObj.seamlessInstances[i].gameObject != null) DestroyImmediate(placeMeshObj.seamlessInstances[i].gameObject);  // 立即销毁实例游戏对象
							}
						}
					}
				}
			}


		}

	}



}
#endif