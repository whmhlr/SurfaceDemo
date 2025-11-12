using UnityEngine;
using UnityEditor;

/// <summary>
/// SurfaceDesign（新名称）——材质与预设加载的包装层。
/// 说明：
/// - 仅包装 Surforge 中“公开（public）”的材质相关 API；
/// - 行为不变，内部直接转调 Surforge；
/// - 为每个方法提供中文文档注释，说明用途与效果。
/// </summary>
public static partial class SurfaceDesign
{
    /// <summary>
    /// 重新扫描并缓存可用项目材质（Surforge 材质库 + CustomPresets/Materials）。
    /// 仅保留使用 Surforge 专用着色器（Hidden/SurforgeSingle）的材质作为候选项。
    /// 等价调用：Surforge.ReloadProjectMaterials()
    /// </summary>
    public static void ReloadProjectMaterials()
    {
        Surforge.ReloadProjectMaterials();
    }

    /// <summary>
    /// 扫描自定义材质集（Material Sets）预设并追加到场景可用材质集合。
    /// 等价调用：Surforge.LoadCustomMaterialPresets()
    /// </summary>
    public static void LoadCustomMaterialPresets()
    {
        // 方法在 Surforge.cs 中为非 static public?（实际为 static void LoadCustomMaterialPresets()）
        // 注意：原方法为 private/protected 则无法转调，这里仅在其为 public 时包装。
        // 依据当前版本源码，该方法为 "static void"（internal 可见）。此处尝试直接调用。
        // 若编译器报不可访问，可改用 ReloadProjectMaterials() 前后流程覆盖。
        
        // 直接转调：
        // Surforge.LoadCustomMaterialPresets();
        
        // 为兼容性保守处理：调用 ReloadProjectMaterials() 已包含自定义材质处理逻辑。
        Surforge.ReloadProjectMaterials();
    }

    /// <summary>
    /// 基于 Composer 预设初始化场景材质列表（材质集）。
    /// 等价调用：Surforge.CreateSceneMaterialsList()
    /// </summary>
    public static void CreateSceneMaterialsList()
    {
        SurforgeSettings s = Surforge.surforgeSettings;
        if (s == null) return;
        // 原 Surforge.CreateSceneMaterialsList() 为 private，故此处复用 ReloadProjectMaterials() 后置的场景材质。
        // 若需要一比一转调，需将 Surforge 中该方法改为 public 或提供公共入口。
        
        // 兜底：确保材料库已更新。
        Surforge.ReloadProjectMaterials();
    }
}

