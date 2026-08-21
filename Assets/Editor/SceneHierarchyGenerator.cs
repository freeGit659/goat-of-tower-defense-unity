#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class SceneHierarchyGenerator
{
    [MenuItem("Tools/Setup Project/Generate Scene Hierarchy")]
    public static void GenerateHierarchy()
    {
        // 1. CORE MANAGERS
        GameObject gameContext = CreateNode("[GameContext]");
        CreateChildNode(gameContext, "AudioSystem");
        CreateChildNode(gameContext, "SaveSystem");
        CreateChildNode(gameContext, "InputAdapter");

        GameObject sceneContext = CreateNode("[SceneContext]");
        CreateChildNode(sceneContext, "GameManager");
        CreateChildNode(sceneContext, "WaveController");
        CreateChildNode(sceneContext, "EconomyController");
        CreateChildNode(sceneContext, "CameraShakeService");

        // 2. CAMERAS & ENVIRONMENT
        GameObject env = CreateNode("[Environment]");
        CreateChildNode(env, "Background_Sprite");
        CreateChildNode(env, "Grid_Overlay");

        // 3. SPAWN & COMBAT ZONES
        GameObject spawnAreas = CreateNode("[SpawnAreas]");
        CreateChildNode(spawnAreas, "TopLeft_Anchor", new Vector3(-2.5f, 6f, 0f));
        CreateChildNode(spawnAreas, "TopCenter_Anchor", new Vector3(0f, 6f, 0f));
        CreateChildNode(spawnAreas, "TopRight_Anchor", new Vector3(2.5f, 6f, 0f));

        CreateNode("[EnemiesContainer]");
        CreateNode("[ProjectilesContainer]");
        CreateNode("[VFXContainer]");

        // 4. DEFENSE BASE (BOTTOM ZONE)
        GameObject castleBase = CreateNode("[CastleBase]", new Vector3(0f, -4f, 0f));
        CreateChildNode(castleBase, "Castle_Visual");

        GameObject castleHitBox = CreateChildNode(castleBase, "Castle_HitBox");
        BoxCollider2D castleCollider = castleHitBox.AddComponent<BoxCollider2D>();
        castleCollider.size = new Vector2(6f, 1.5f);
        castleCollider.isTrigger = true;

        GameObject turretSlots = CreateChildNode(castleBase, "[TurretSlots]");

        GameObject slot1 = CreateChildNode(turretSlots, "Slot_01_Left", new Vector3(-1.8f, 0.5f, 0f));
        SetupTurretStructure(slot1, "Turret_Archer");

        GameObject slot2 = CreateChildNode(turretSlots, "Slot_02_Center", new Vector3(0f, 0.7f, 0f));
        SetupTurretStructure(slot2, "Turret_Cannon");

        GameObject slot3 = CreateChildNode(turretSlots, "Slot_03_Right", new Vector3(1.8f, 0.5f, 0f));
        SetupTurretStructure(slot3, "Turret_Gatling");

        // 5. POOLING CONTAINERS
        GameObject pools = CreateNode("[ObjectPools]");
        CreateChildNode(pools, "Pool_Enemies");
        CreateChildNode(pools, "Pool_Projectiles");
        CreateChildNode(pools, "Pool_VFX");

        // 6. UI CANVAS (PORTRAIT 1080x1920)
        SetupCanvasHierarchy();

        Debug.Log("<color=green><b>[Scene Setup]</b> Đã dựng hoàn chỉnh cây Hierarchy chuẩn cho Portrait Defense Game!</color>");
    }

    private static GameObject CreateNode(string name, Vector3 position = default)
    {
        GameObject go = new GameObject(name);
        go.transform.position = position;
        Undo.RegisterCreatedObjectUndo(go, $"Create {name}");
        return go;
    }

    private static GameObject CreateChildNode(GameObject parent, string name, Vector3 localPosition = default)
    {
        GameObject child = new GameObject(name);
        child.transform.SetParent(parent.transform, false);
        child.transform.localPosition = localPosition;
        Undo.RegisterCreatedObjectUndo(child, $"Create {name}");
        return child;
    }

    private static void SetupTurretStructure(GameObject slot, string turretName)
    {
        GameObject turret = CreateChildNode(slot, turretName);
        GameObject pivot = CreateChildNode(turret, "BarrelPivot");
        CreateChildNode(pivot, "FirePoint", new Vector3(0f, 0.5f, 0f));
    }

    private static void SetupCanvasHierarchy()
    {
        // Kiểm tra EventSystem
        if (Object.FindAnyObjectByType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            Undo.RegisterCreatedObjectUndo(eventSystem, "Create EventSystem");
        }

        // Tạo Canvas chính
        GameObject canvasGo = new GameObject("Canvas_Gameplay");
        Canvas canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGo.AddComponent<GraphicRaycaster>();

        CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 0f; // Khóa theo chiều ngang (Match Width) cho màn hình dọc

        Undo.RegisterCreatedObjectUndo(canvasGo, "Create Canvas");

        // Hierarchy UI con
        GameObject safeArea = CreateUIObject(canvasGo, "[SafeAreaPanel]", true);

        GameObject topHud = CreateUIObject(safeArea, "Top_HUD");
        SetRectTransformAnchor(topHud, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0.5f, 1), new Vector2(0, -150), new Vector2(0, 200));

        GameObject notif = CreateUIObject(safeArea, "Center_Notification");
        SetRectTransformAnchor(notif, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(600, 150));

        GameObject bottomControl = CreateUIObject(safeArea, "Bottom_ControlBar");
        SetRectTransformAnchor(bottomControl, new Vector2(0, 0), new Vector2(1, 0), new Vector2(0.5f, 0), new Vector2(0, 100), new Vector2(0, 200));

        CreateUIObject(bottomControl, "SkillButton_01");
        CreateUIObject(bottomControl, "SkillButton_02");
        CreateUIObject(bottomControl, "UpgradeTower_Button");

        CreateUIObject(safeArea, "Castle_HealthBar_UI");

        // Popups Container
        GameObject popups = CreateUIObject(canvasGo, "[Popups]", true);
        CreateUIObject(popups, "WinPopup", true);
        CreateUIObject(popups, "DefeatPopup", true);
        CreateUIObject(popups, "EraEvolutionPopup", true);

        // World Space Damage Texts Container
        CreateNode("[WorldSpace_DamageTexts]");
    }

    private static GameObject CreateUIObject(GameObject parent, string name, bool stretch = false)
    {
        GameObject uiObj = new GameObject(name, typeof(RectTransform));
        uiObj.transform.SetParent(parent.transform, false);

        if (stretch)
        {
            RectTransform rect = uiObj.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition = Vector2.zero;
        }

        Undo.RegisterCreatedObjectUndo(uiObj, $"Create {name}");
        return uiObj;
    }

    private static void SetRectTransformAnchor(GameObject go, Vector2 min, Vector2 max, Vector2 pivot, Vector2 pos, Vector2 size)
    {
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.pivot = pivot;
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
    }
}
#endif