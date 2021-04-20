using UnityEditor;
using UnityEngine;

namespace FuguFirecracker.TakeNote
{
    internal static class Style
    {
        private static readonly RectOffset ZeroPadding = new RectOffset(0, 0, 0, 0);
        private static readonly RectOffset LineSpacer = new RectOffset(0, 0, 8, 8);

        internal static readonly Color ResetColor = Color.white;
        internal static readonly Color DimColor = new Color(0.74f, 0.74f, 0.74f);
        internal static readonly Color SeperatorColor = new Color(0f, 0.03f, 0.24f, 0.14f);
        internal static readonly Color FlashAlertColor = new Color(0.77f, 0.01f, 0.01f);

        internal static readonly GUIStyle WordWrap = new GUIStyle(EditorStyles.textArea) {wordWrap = true,};

        internal static readonly GUIStyle ZButton = new GUIStyle("Button")
        {
            border = new RectOffset(1, 1, 1, 1),
            normal = {background = Ikon.BackgroundWhite},
            padding = ZeroPadding,
            margin = new RectOffset(0, 4, 3, 0),
            fixedWidth = 24,
            fixedHeight = 24
        };
        
        internal static readonly GUIStyle CButton = new GUIStyle("Button")
        {
            border = new RectOffset(1, 1, 1, 1),
            normal = {background = Ikon.Background},
            padding = ZeroPadding,
            margin = new RectOffset(0,1,4,0),
            fixedWidth = 16,
            fixedHeight = 16
        };

        internal static readonly GUIStyle PopUp = new GUIStyle {padding = new RectOffset(8, 8, 8, 8)};


        internal static readonly GUIStyle AlignBold = new GUIStyle(EditorStyles.boldLabel)
        {
            wordWrap = true,
            padding = new RectOffset(4, 0, 4, 0),
            alignment = TextAnchor.MiddleLeft,
            normal = {textColor = Color.black}
        };

        internal static readonly GUIStyle Align = new GUIStyle()
        {
            wordWrap = true, padding = new RectOffset(4, 0, 4, 0), alignment = TextAnchor.MiddleLeft
        };

        internal static readonly GUIStyle Mini = new GUIStyle
        {
            wordWrap = true,
            padding = new RectOffset(4, 0, 4, 0),
            alignment = TextAnchor.MiddleLeft,
            normal = {textColor = Color.black},
            fontSize = 9
        };

        internal static readonly GUIStyle EditorLine = new GUIStyle() {margin = LineSpacer};

        internal static readonly GUIStyle AlignCenter = new GUIStyle("Label") {alignment = TextAnchor.MiddleCenter};

        internal static readonly GUIStyle TaskBlock = new GUIStyle
        {
            border = new RectOffset(2, 2, 2, 2),
            padding = new RectOffset(2, 2, 2, 2),
            margin = new RectOffset(2, 2, 2, 2),
            normal = {background = Ikon.Background}
        };

        internal static readonly GUIStyle OnOffSwitch = new GUIStyle("Button")
        {
            normal = new GUIStyleState() {background = Ikon.OffSwitch},
            onNormal = new GUIStyleState() {background = Ikon.OnSwitch},
            fixedHeight = 24,
            fixedWidth = 24,
        };

      

    }
}