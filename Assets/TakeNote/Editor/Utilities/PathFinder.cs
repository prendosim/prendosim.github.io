using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FuguFirecracker.TakeNote
{
    public static class PathFinder
    {
        public static T LoadAsset<T>(string relativePath) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(GetPathFromSequence(relativePath));
        }

        public static string GetPathFromSequence(string relativePath)
        {
            var folderNames = relativePath.Split('/');
            var fileIndex = folderNames.Length - 1;
            var fileName = folderNames[fileIndex];

#if NET_4_6
            var path = Directory.EnumerateDirectories(Application.dataPath, folderNames[0], SearchOption.AllDirectories)
                .First();

            for (var i = 1; i < fileIndex; i++)
            {
                path = Directory.EnumerateDirectories(path, folderNames[i], SearchOption.TopDirectoryOnly).First();
            }
#else
             var path = Directory.GetDirectories(Application.dataPath, folderNames[0], SearchOption.AllDirectories)
                .First();
             
             for (var i = 1; i < fileIndex; i++)
             {
                 path = Directory.GetDirectories(path, folderNames[i], SearchOption.TopDirectoryOnly).First();
             }
#endif
            return string.Format("Assets{0}{1}{2}", path.Substring(Application.dataPath.Length),
                Path.DirectorySeparatorChar, fileName);
        }
    }
}