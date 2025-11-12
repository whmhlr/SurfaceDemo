using UnityEngine;
using UnityEditor;

/// <summary>
/// SurfaceDesign（新名称）——路径与全局设置访问的包装层。
/// 目的：
/// 1) 以“SurfaceDesign”名暴露与 Surforge 等价的入口，便于逐步迁移调用；
/// 2) 保持现有逻辑不变，仅作转调；
/// 3) 提供中文文档注释，便于阅读与维护。
/// 注意：放置在 Editor 目录下，允许使用 UnityEditor API（原始 Surforge 实现依赖 Editor 功能）。
/// </summary>
public static partial class SurfaceDesign
{
    /// <summary>
    /// 获取资源根路径（自动扫描或使用默认）。
    /// 等价调用：Surforge.GetSurforgePath()
    /// 返回值示例："Assets/Surforge"（路径分隔符统一为斜杠）。
    /// </summary>
    public static string GetSurforgePath()
    {
        return Surforge.GetSurforgePath();
    }

    /// <summary>
    /// 获取 Surforge 的全局设置对象（只读访问入口）。
    /// 注意：这是对 Surforge.surforgeSettings 的镜像访问，未复制对象。
    /// </summary>
    public static SurforgeSettings Settings => Surforge.surforgeSettings;
}

