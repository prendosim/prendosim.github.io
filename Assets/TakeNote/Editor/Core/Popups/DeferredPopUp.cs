using UnityEditor;
using UnityEngine;

namespace FuguFirecracker.TakeNote
{
	public class DeferredPopUp : PopupWindowContent
	{
		private readonly Task _task;

		public DeferredPopUp(Task task)
		{
			_task = task;
		}

		public override Vector2 GetWindowSize()
		{
			return new Vector2(110, 78);
		}

		public override void OnGUI(Rect rect)
		{

			if (GUILayout.Button("Remove", GUILayout.Height(22)))
			{
				Scribe.RemoveTask(_task, ref Ledger.Manifest.DeferredTasks);
				editorWindow.Close();
			}

			if (GUILayout.Button("Outstanding", GUILayout.Height(22)))
			{
				Scribe.MoveToOutstanding(_task, ref Ledger.Manifest.DeferredTasks);
				editorWindow.Close();
			}


			if (GUILayout.Button("Cancel", GUILayout.Height(22)))
			{
				editorWindow.Close();
			}

			GUI.SetNextControlName("ClearFix");
			GUILayout.Space(20);
			EditorGUILayout.TextField("", GUIStyle.none);
			ClearFix();
		}


		private void ClearFix()
		{
			EditorGUI.FocusTextInControl("ClearFix");
		}
	}
}