using UnityEngine;
using UnityEditor;

namespace LevelGenerator
{
    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var levelGen = (LevelGenerator) target;

            if (GUILayout.Button("Generate Level"))
            {
                levelGen.GenerateLevel();
            }
        }
    }
}