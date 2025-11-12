using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlaceMesh))]
public class PlaceMeshEditor : Editor {

    SerializedProperty icon;
    SerializedProperty iconLite;


    private void OnEnable() {
        icon = serializedObject.FindProperty("icon");
        iconLite = serializedObject.FindProperty("iconLite");
    }


    public override void OnInspectorGUI() {

        serializedObject.Update();
        EditorGUILayout.PropertyField(icon);
        EditorGUILayout.PropertyField(iconLite);
        serializedObject.ApplyModifiedProperties();

    }

}

/*
 PlaceMeshEditor.cs - 自定义编辑器（中文注释）

 说明：
 - 该脚本是为 PlaceMesh 类型提供的自定义编辑器，使用 [CustomEditor(typeof(PlaceMesh))] 进行关联。
 - 定义了两个序列化属性引用：icon 和 iconLite，分别通过 serializedObject.FindProperty 获取。
 - OnEnable() 中获取属性引用以便在 Inspector 中以序列化方式显示和编辑这些字段。
 - OnInspectorGUI() 中使用 serializedObject.Update() / ApplyModifiedProperties() 包裹对属性的绘制，
     以确保对属性的修改能够正确支持撤销/重做、序列化和多对象编辑。

 注意：
 - 本文件仅负责在 Inspector 中绘制字段，不应在编辑器绘制函数中执行耗时操作或修改场景对象的运行时逻辑。
 - 若 PlaceMesh 脚本中字段名发生更改（例如 icon 或 iconLite 改名），需要同时更新此处的 FindProperty 调用，否则属性引用会为 null。
 - 保持此文件只做显示/编辑相关的工作，其他逻辑应放在目标 MonoBehaviour（PlaceMesh）中实现。

 结束。
*/