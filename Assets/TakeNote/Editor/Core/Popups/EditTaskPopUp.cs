using UnityEditor;
using UnityEngine;

namespace FuguFirecracker.TakeNote
{
	internal class EditTaskPopUp : PopupWindowContent
	{
		private const float MAX_HEIGHT = 162;
		private const float COLOR_HEIGHT = 20;
		private const float DETAILS_HEIGHT = 60;

		private float _colorHeight;
		private float _detailsHeight;

		private readonly Task _tempTask;
		private readonly Task _task;

		public EditTaskPopUp(Task task)
		{
			_task = task;
			_tempTask = TaskMaster.Clone(task);
		}

		public override Vector2 GetWindowSize()
		{
			return new Vector2(Main.Window.position.width - 42, MAX_HEIGHT + _colorHeight + _detailsHeight);
		}

		public override void OnGUI(Rect rect)
		{
			GUILayout.Space(6);
			EditorGUILayout.BeginVertical(Style.PopUp);
			GUI.backgroundColor = Color.white;

			EditorGUILayout.LabelField("Edit Task", EditorStyles.boldLabel);

			GUI.SetNextControlName("TaskString");
			_tempTask.Title = EditorGUILayout.TextField(string.Empty, _tempTask.Title);

			GUILayout.Space(8);

			EditorGUILayout.BeginHorizontal("Button");
			_tempTask.HasDetails = GUILayout.Toggle(_tempTask.HasDetails, "Add Details", Style.AlignCenter, GUILayout.Height(24));
			_tempTask.HasDetails = GUILayout.Toggle(_tempTask.HasDetails, string.Empty, Style.OnOffSwitch);
			EditorGUILayout.EndHorizontal();

			if (_tempTask.HasDetails)
			{
				_detailsHeight = DETAILS_HEIGHT;
				_tempTask.Details = EditorGUILayout.TextArea(_tempTask.Details, Style.WordWrap, GUILayout.Height(58));
			}
			else
			{
				_detailsHeight = 0;
			}

			EditorGUILayout.BeginHorizontal("Button");
			_tempTask.IsColored = GUILayout.Toggle(_tempTask.IsColored, "Colorize", Style.AlignCenter, GUILayout.Height(24));
			_tempTask.IsColored = GUILayout.Toggle(_tempTask.IsColored, string.Empty, Style.OnOffSwitch);
			EditorGUILayout.EndHorizontal();

			if (_tempTask.IsColored)
			{
				_colorHeight = COLOR_HEIGHT;
				_tempTask.DrawColor = EditorGUILayout.ColorField(_tempTask.DrawColor);
			}
			else
			{
				_colorHeight = 0;
			}

			GUILayout.Space(8);

			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Apply", GUILayout.Height(22)))
			{
				TaskMaster.Assimilate(_tempTask, _task);
				Ledger.Manifest.Save();
 				editorWindow.Close();
			}

			if (GUILayout.Button("Cancel", GUILayout.Height(22)))
			{
				editorWindow.Close();
			}

			EditorGUILayout.EndHorizontal();
			GUILayout.Space(6);
			EditorGUILayout.EndVertical();
		}
	}
}