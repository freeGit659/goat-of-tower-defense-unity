using UnityEditor;
using UnityEngine;
using System.IO;

public static class ProjectFolderGenerator
{
    [MenuItem("Tools/Setup Project/Generate Folder Structure")]
    public static void GenerateFolders()
    {
        string rootPath = Application.dataPath + "/_Project";

        string[] folders = new string[]
        {
            // Art
            "_Project/Art/Animations",
            "_Project/Art/Materials",
            "_Project/Art/Shaders",
            "_Project/Art/Sprites/Backgrounds",
            "_Project/Art/Sprites/Characters",
            "_Project/Art/Sprites/Environment",
            "_Project/Art/Sprites/UI",
            "_Project/Art/Textures",

            // Audio
            "_Project/Audio/BGM",
            "_Project/Audio/SFX",

            // Data (ScriptableObjects)
            "_Project/Data/Eras",
            "_Project/Data/Towers",
            "_Project/Data/Enemies",
            "_Project/Data/Projectiles",
            "_Project/Data/Waves",

            // Prefabs
            "_Project/Prefabs/Characters",
            "_Project/Prefabs/Towers",
            "_Project/Prefabs/Projectiles",
            "_Project/Prefabs/VFX",
            "_Project/Prefabs/UI",

            // Scenes
            "_Project/Scenes",

            // Scripts
            "_Project/Scripts/Core/Events",
            "_Project/Scripts/Core/StateMachine",
            "_Project/Scripts/Gameplay/Base",
            "_Project/Scripts/Gameplay/Towers",
            "_Project/Scripts/Gameplay/Enemies",
            "_Project/Scripts/Gameplay/Combat",
            "_Project/Scripts/Gameplay/Waves",
            "_Project/Scripts/UI/Core",
            "_Project/Scripts/UI/GameplayHUD",
            "_Project/Scripts/UI/Popups",
            "_Project/Scripts/Utilities/ObjectPool",
            "_Project/Scripts/Utilities/Extensions",
            "_Project/Scripts/Utilities/Helpers",

            // Settings & Plugins
            "Settings/Input",
            "Settings/URP",
            "Plugins"
        };

        foreach (string folder in folders)
        {
            string fullPath = Path.Combine(Application.dataPath, folder);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("<color=green><b>[Project Setup]</b> Đã tạo xong toàn bộ cây thư mục chuẩn!</color>");
    }
}