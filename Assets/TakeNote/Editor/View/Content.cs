using UnityEngine;

namespace FuguFirecracker.TakeNote
{
    public class Content
    {
        private static readonly GUIContent TaskCollapsedIkon = new GUIContent {image = Ikon.More};
        private static readonly GUIContent TaskExpandedIkon = new GUIContent {image = Ikon.MoreDown};
        
        internal static readonly GUIContent[] TaskMoreIkons = {TaskCollapsedIkon, TaskExpandedIkon};
    }
}