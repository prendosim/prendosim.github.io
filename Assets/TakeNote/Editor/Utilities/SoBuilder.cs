using UnityEditor;
using UnityEngine;

namespace FuguFirecracker.TakeNote
{
    public static class SoBuilder
    {
        public static T FindOrCreateInRelativePath<T>(string relativePath) where T : ScriptableObject
        {
            var path = PathFinder.GetPathFromSequence(relativePath);
            var so = AssetDatabase.LoadAssetAtPath<T>(path);
            return so ? so : Create<T>(path);
        }

        private static T Create<T>(string path) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return asset;
        }
    }
}