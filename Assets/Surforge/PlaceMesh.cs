#if UNITY_EDITOR // 仅在Unity编辑器模式下编译此代码
using UnityEngine; // 引入UnityEngine命名空间
using UnityEditor; // 引入UnityEditor命名空间
using System.Collections; // 引入System.Collections命名空间

[AddComponentMenu("")] // 不在组件菜单中显示此组件
[ExecuteInEditMode] // 允许脚本在编辑模式下执行
public class PlaceMesh : MonoBehaviour { // 定义PlaceMesh类，继承自MonoBehaviour

	public Texture icon; // 图标纹理
	public Texture iconLite; // 精简版图标纹理
	public bool isText;  // 如果为true，右键随机化文本而不是翻转，并且对称操作不会翻转此资源

	[HideInInspector] // 在Inspector面板中隐藏此字段
	public GameObject[] seamlessInstances; // 无缝实例数组

	[HideInInspector] // 在Inspector面板中隐藏此字段
	public SurforgeSettings surforgeSettings; // Surforge设置引用

	public Texture2D bumpMap; //bumpMap seamless texture for making rocks, etc
	// 凹凸贴图，用于制作岩石等的无缝纹理
	public float bumpMapIntensity = 1; //bumpMap intensity 0 to 2
	// 凹凸贴图强度，范围0到2
	public Vector2 bumpMapTiling; //bump and ao maps tiling settings
	// 凹凸贴图和AO贴图的平铺设置
	public Texture2D aoMap; //aoMap seamless texture
	// AO贴图无缝纹理
	public float aoMapIntensity; //aoMap intensity -1 to 1. Usually you whant to use something like 1 to 1.1 to get more worn edges
	// AO贴图强度，范围-1到1。通常使用1到1.1来获得更磨损的边缘效果
	
	public bool randomUvOffset; //randomize profile texture UV offset
	// 随机化配置文件纹理的UV偏移

	public PlaceMesh[] shuffleArray; //if any, shuffle with right button
	// 如果不为空，右键点击时随机切换
    public bool flipWhileShuffle; //if true, x or z flipped randomly
	// 如果为true，随机翻转x或z轴
	public bool shuffleScale; //if true, also shuffle scale
	// 如果为true，同时随机化缩放
	public bool shuffleRotation; //if true, also shuffle rotation randomly and scale x/z flip
	// 如果为true，同时随机化旋转和缩放x/z轴翻转

    public bool shuffleRotation90; //if true, also shuffle rotation 90 degree step
	// 如果为true，同时以90度为步长随机化旋转
    public bool shuffleRotation180; //if true, also shuffle rotation 180 degree step
	// 如果为true，同时以180度为步长随机化旋转


    //Settings below work only if "shuffleArray" used.
	// 以下设置仅在使用"shuffleArray"时有效
    public Mesh[] shuffleMeshes; //if any, shuffle mesh variants while "shuffleArray" feature used. 
	// 如果不为空，在使用"shuffleArray"功能时随机切换网格变体
    //This arrays should be all of the same size (not necessarily equal to "shuffleArray" size). If arrays size 0, have no effect
	// 这些数组的大小应该相同（不一定等于"shuffleArray"的大小）。如果数组大小为0，则无效
    public Texture2D[] shuffleBumpMaps; // 随机凹凸贴图数组
    public float[] shuffleBumpMapsIntensity; // 随机凹凸贴图强度数组
    public Texture2D[] shuffleAoMaps; // 随机AO贴图数组
    public float[] aoMapsIntensity; // AO贴图强度数组
    public float[] aoMapsIntensityRandomRange; //values added or substracted randomly to aoMapsIntensity of child objects.
	// 随机添加或减去的值，应用于子对象的aoMapsIntensity

    public bool childrenHidden; //child objects are hidden in the Scene View, shown only while rendering. 
	// 子对象在场景视图中隐藏，仅在渲染时显示
                                //this also prevents creating colliders for children.
	// 这也会阻止为子对象创建碰撞体

    public bool autoShuffle; //if "shuffleArray" is used, auto shuffles every time after use.
	// 如果使用"shuffleArray"，每次使用后自动随机切换
    public bool isProxy; //invisible on render, when recieve Material ID, apply it to children instead.
	// 渲染时不可见，当接收到材质ID时，将其应用于子对象


    [SerializeField] int instanceID = 0; // 序列化的实例ID，初始值为0
	void Awake(){ // Awake方法，当脚本被加载时调用
		if (instanceID != GetInstanceID()) { // 如果当前实例ID不等于获取的实例ID
			if (instanceID == 0){ // 如果实例ID为0
				instanceID = GetInstanceID(); // 设置实例ID为当前对象的实例ID
			}
			else { // 如果实例ID不为0
				instanceID = GetInstanceID(); // 设置实例ID为当前对象的实例ID
				if (instanceID < 0){ // 如果实例ID小于0
					if (surforgeSettings != null) { // 如果surforgeSettings不为空
						if (!surforgeSettings.isPlaceToolActive) { // 如果放置工具未激活
							surforgeSettings.seamlessNeedUpdate = true; // 标记无缝需要更新
						}
					}
				}
			}
		}
	}


	public void Update() { // Update方法，每帧调用一次
		if (surforgeSettings) { // 如果surforgeSettings不为空
			if (surforgeSettings.seamless) { // 如果启用了无缝模式
				if (this.seamlessInstances != null) { // 如果seamlessInstances不为空
					if (this.seamlessInstances.Length == 8) { // 如果seamlessInstances的长度为8
						float offsetZ = Mathf.Abs(surforgeSettings.textureBorders.minZ - surforgeSettings.textureBorders.maxZ) * surforgeSettings.root.transform.localScale.z; // 计算Z轴偏移量
						float offsetX = Mathf.Abs(surforgeSettings.textureBorders.minX - surforgeSettings.textureBorders.maxX) * surforgeSettings.root.transform.localScale.x; // 计算X轴偏移量
						for (int i=0; i<8; i++) { // 遍历8个无缝实例
							if (this.seamlessInstances[i]) { // 如果当前实例不为空
								UpdateSeamlessInstanceTransformRealtime(this.seamlessInstances[i], i, offsetX, offsetZ, this.gameObject); // 更新无缝实例的实时变换
							}
						}
					}
				}
			}
		}
	}

	void UpdateSeamlessInstanceTransformRealtime(GameObject inst, int i, float offsetX, float offsetZ, GameObject source) { // 更新无缝实例的实时变换方法
		if (i == 0) inst.transform.position = new Vector3(source.transform.position.x + offsetX, source.transform.position.y, source.transform.position.z); // 设置第0个实例的位置（右侧）
		if (i == 1) inst.transform.position = new Vector3(source.transform.position.x - offsetX, source.transform.position.y, source.transform.position.z); // 设置第1个实例的位置（左侧）
		
		if (i == 2) inst.transform.position = new Vector3(source.transform.position.x + offsetX, source.transform.position.y, source.transform.position.z + offsetZ); // 设置第2个实例的位置（右上）
		if (i == 3) inst.transform.position = new Vector3(source.transform.position.x + offsetX, source.transform.position.y, source.transform.position.z - offsetZ); // 设置第3个实例的位置（右下）
		
		if (i == 4) inst.transform.position = new Vector3(source.transform.position.x - offsetX, source.transform.position.y, source.transform.position.z + offsetZ); // 设置第4个实例的位置（左上）
		if (i == 5) inst.transform.position = new Vector3(source.transform.position.x - offsetX, source.transform.position.y, source.transform.position.z - offsetZ); // 设置第5个实例的位置（左下）
		
		if (i == 6) inst.transform.position = new Vector3(source.transform.position.x, source.transform.position.y, source.transform.position.z  + offsetZ); // 设置第6个实例的位置（上侧）
		if (i == 7) inst.transform.position = new Vector3(source.transform.position.x, source.transform.position.y, source.transform.position.z - offsetZ); // 设置第7个实例的位置（下侧）

		if (source.transform.parent == surforgeSettings.root.transform) { // 如果源对象的父对象是根对象
			inst.transform.localScale = new Vector3(source.transform.localScale.x * surforgeSettings.root.transform.localScale.x, // 设置实例的缩放，考虑根对象的缩放
			                                      source.transform.localScale.y * surforgeSettings.root.transform.localScale.y,
			                                      source.transform.localScale.z * surforgeSettings.root.transform.localScale.z);
		}
		else { // 如果源对象的父对象不是根对象
			inst.transform.localScale = new Vector3(source.transform.localScale.x, // 设置实例的缩放，不考虑根对象的缩放
			                                      source.transform.localScale.y,
			                                      source.transform.localScale.z);
		}
		
		inst.transform.rotation = source.transform.rotation; // 设置实例的旋转与源对象相同
	}


	void OnDestroy(){ // OnDestroy方法，当对象被销毁时调用
		if (surforgeSettings != null) { // 如果surforgeSettings不为空
			if (!surforgeSettings.isPlaceToolActive) { // 如果放置工具未激活
				surforgeSettings.seamlessNeedUpdate = true; // 标记无缝需要更新
			}
		}
	}


}
#endif