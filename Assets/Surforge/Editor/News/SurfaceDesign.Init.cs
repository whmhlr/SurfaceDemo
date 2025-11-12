using UnityEngine;
using UnityEditor;

/// <summary>
/// SurfaceDesign（新名称）——初始化与激活流程的包装层。
/// 说明：
/// - 仅包装 Surforge 中“公开（public）”的初始化相关 API，逻辑完全等价；
/// - 目标是让调用方可以无缝改用 SurfaceDesign 名称；
/// - 后续若需要，可将具体实现逐步迁移至 SurfaceDesign 并弃用 Surforge。
/// </summary>
public static partial class SurfaceDesign
{
    /// <summary>
    /// 初始化工程层面的设置（留空或按需设置 Quality 等）。
    /// 等价调用：Surforge.SetupProject()
    /// </summary>
    public static void SetupProject()
    {
        Surforge.SetupProject();
    }

    /// <summary>
    /// 激活体素/渲染管线的主入口。会：
    /// - 创建/查找全局设置对象；
    /// - 初始化八叉树、纹理与默认层；
    /// - 创建背景与纹理边界；
    /// 等价调用：Surforge.VoxelEngineActivate(isMetallic)
    /// </summary>
    /// <param name="isMetallic">是否使用金属度工作流（true：金属度，false：高光工作流）。</param>
    public static void VoxelEngineActivate(bool isMetallic)
    {
        Surforge.VoxelEngineActivate(isMetallic);
    }

    /// <summary>
    /// 显式创建全局设置对象并做资源绑定（模型、Shader 等）。
    /// 等价调用：Surforge.CreateSettingsObject(isMetallic)
    /// </summary>
    /// <param name="isMetallic">是否使用金属度工作流。</param>
    public static void CreateSettingsObject(bool isMetallic)
    {
        Surforge.CreateSettingsObject(isMetallic);
    }

    /// <summary>
    /// 加载并缓存可用的天空盒资源。
    /// 等价调用：Surforge.LoadSkyboxes()
    /// </summary>
    public static void LoadSkyboxes()
    {
        Surforge.LoadSkyboxes();
    }

    /// <summary>
    /// 加载 Greebles（细节件/内容包）资源。
    /// 等价调用：Surforge.LoadGreebles()
    /// </summary>
    public static void LoadGreebles()
    {
        Surforge.LoadGreebles();
    }
}

