using UnityEngine; // 导入UnityEngine命名空间，用于访问Unity引擎的基础功能
using System.Collections; // 导入System.Collections命名空间，用于访问集合类
using System.Collections.Generic; // 导入System.Collections.Generic命名空间，用于访问泛型集合类

[AddComponentMenu("")] // 隐藏在Unity编辑器组件菜单中，不显示在Add Component列表里
public class SurforgeLayer : MonoBehaviour { // 定义SurforgeLayer类，继承自MonoBehaviour

	public List<SurforgeSceneCluster> sceneClusters = new List<SurforgeSceneCluster>(); // 存储场景聚类的列表，用于管理场景中的对象组
	public int generation; // 表示当前图层的世代或版本号

} // 类定义结束