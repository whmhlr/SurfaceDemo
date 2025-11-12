#if UNITY_EDITOR // 条件编译指令，仅在Unity编辑器环境下编译此段代码
using UnityEngine; // 导入UnityEngine命名空间，用于访问Unity引擎的基础功能
using System.Collections; // 导入System.Collections命名空间，用于访问集合类
using System.Collections.Generic; // 导入System.Collections.Generic命名空间，用于访问泛型集合类

[AddComponentMenu("")] // 隐藏在Unity编辑器组件菜单中，不显示在Add Component列表里
[ExecuteInEditMode] // 使脚本在编辑模式下也能执行，而仅限于运行时
public class SurforgeTexturePreview : MonoBehaviour { // 定义SurforgeTexturePreview类，继承自MonoBehaviour

	[HideInInspector] // 在Inspector面板中隐藏此字段
	public SurforgeSettings surforgeSettings; // Surforge的设置对象引用

	public Transform cameraHolder; // 相机持有者的Transform引用

	public SurforgeComposer composer; // 材质合成器的引用

	public Camera previewCamera; // 预览相机引用
	public RenderTexture previewRenderTexture; // 预览渲染纹理引用

	public Transform previewCameraFocus; // 预览相机焦点的Transform引用

	public GameObject mapOnScreenshotPrefab; // 截图上贴图的预制体引用

	public Shader wireframeShader; // 线框着色器引用

	public GameObject floorPrefab; // 地板预制体引用


	bool skipDestroy; // 跳过销毁的标志

	void OnDestroy(){ // 当对象被销毁时调用的方法
		if (surforgeSettings != null) { // 检查surforgeSettings是否存在
			if (!skipDestroy) { // 如果未设置跳过销毁
				surforgeSettings.SkipDestroyTexturePreview(); // 调用surforgeSettings的SkipDestroyTexturePreview方法
				DestroyImmediate(surforgeSettings.gameObject); // 立即销毁surforgeSettings的游戏对象
			}
		}
	}

	public void SkipDestroy() { // 设置跳过销毁的公共方法
		skipDestroy = true; // 将skipDestroy标志设为true
	}
	
} // 类定义结束
#endif // 结束条件编译块