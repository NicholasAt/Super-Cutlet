using CodeBase.Data;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public class CheatWindowEditor : EditorWindow
    {
        private const int MaxLevels = 9;
        private float _record = 5;
        private int _levels = MaxLevels - 1;

        [MenuItem("Tools/Cheat tool")]
        private static void CreatWindow()
        {
            GetWindow<CheatWindowEditor>(false, "Cheat tool", true);
        }

        private void OnGUI()
        {
            _levels = EditorGUILayout.IntField("Unlock levels", _levels);
            _record = EditorGUILayout.FloatField("Record", _record);

            if (GUILayout.Button("Unlock"))
            {
                if (Application.isPlaying == false)
                {
                    Debug.LogError("editor only");
                }
                else
                {
                    Unlock();
                }
            }
        }
        private void Unlock()
        {
            IPersistentProgressService persistentProgress = AllServices.Container.Single<IPersistentProgressService>();
            if (persistentProgress.Progress == null)
            {
                Debug.LogError("need start first level");
                return;
            }
            ISaveLoadService saveLoad = AllServices.Container.Single<ISaveLoadService>();

            string[] scenes = SceneKeys();
            foreach (string level in scenes)
            {
                persistentProgress.Progress.LevelDataDictionary.Dictionary[level] = new LevelData(_record);
            }
            Debug.LogError($"unlocked: {scenes.Length} levels");
            saveLoad.SavePlayerProgress();
        }
        private string[] SceneKeys()
        {
            List<string> strings = new();
            for (int i = 0; i < Mathf.Min(_levels, MaxLevels); i++)
            {
                string key = $"Level {i + 1}";
                strings.Add(key);
            }
            return strings.ToArray();
        }
    }
}