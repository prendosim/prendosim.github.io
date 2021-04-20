using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FuguFirecracker.TakeNote
{
    public static class TaskMaster
    {
        public static void DrawTask(Task task, Event e)
        {
            if (task.IsColored)
            {
                GUI.backgroundColor = task.DrawColor;
            }

            EditorGUILayout.BeginVertical(Style.TaskBlock);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(task.Title, Style.AlignBold);


            var index = task.DoShowMore ? 1 : 0;
            task.DoShowMore = GUILayout.Toggle(task.DoShowMore, Content.TaskMoreIkons[index], Style.ZButton);

            EditorGUILayout.EndHorizontal();

            if (task.DoShowMore)
            {
                if (task.HasDetails)
                {
                    EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2, Style.EditorLine),
                        Style.SeperatorColor);
                    EditorGUILayout.LabelField(task.Details, Style.Align);
                }

                EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2, Style.EditorLine), Style.SeperatorColor);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField(string.Format("Created on :     {0}", task.CreationDate),
                    Style.Mini);
                if (task.IsCompleted)
                {
                    EditorGUILayout.LabelField(string.Format("Completed on : {0}", task.CompletionDate),
                       Style.Mini);
                }

                EditorGUILayout.EndVertical();

                if (GUILayout.Button(Ikon.Edit, Style.ZButton))
                {
                    PopupWindow.Show(task.ClickOnRect, new EditTaskPopUp(task));
                }

                if (e.type == EventType.Repaint)
                {
                    task.ClickOnRect = new Rect(10, GUILayoutUtility.GetLastRect().y, 0, 0);
                }

                if (!task.IsCompleted && !task.IsDeferred)
                {
                    if (GUILayout.Button(Ikon.UpDown, Style.ZButton))
                    {
                        PopupWindow.Show(new Rect(Main.Window.position.width - 160, task.ClickOnRect.y, 0, 0),
                            new ShiftRankPopUp(task));
                    }
                }

                if (task.IsCompleted)
                {
                    if (GUILayout.Button(Ikon.Trash, Style.ZButton))
                    {
                        PopupWindow.Show(new Rect(Main.Window.position.width - 134, task.ClickOnRect.y, 0, 0),
                            new CompletedPopUp(task));
                    }
                }
                else if (task.IsDeferred)
                {
                    if (GUILayout.Button(Ikon.Trash, Style.ZButton))
                    {
                        PopupWindow.Show(new Rect(Main.Window.position.width - 134, task.ClickOnRect.y, 0, 0),
                            new DeferredPopUp(task));
                    }
                }
                else
                {
                    if (GUILayout.Button(Ikon.Check, Style.ZButton))
                    {
                        PopupWindow.Show(new Rect(Main.Window.position.width - 134, task.ClickOnRect.y, 0, 0),
                            new OutstandingPopUp(task));
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(4);
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = Style.ResetColor;
        }

        public static Task Clone(Task task)
        {
            return new Task
            {
                Title = task.Title,
                CreationDate = task.CreationDate,
                CompletionDate = task.CompletionDate,
                HasDetails = task.HasDetails,
                Details = task.Details,
                IsCompleted = task.IsCompleted,
                IsColored = task.IsColored,
                DrawColor = task.DrawColor
            };
        }

        public static void Assimilate(Task tempTask, Task taskAtHand)
        {
            taskAtHand.Title = tempTask.Title;
            taskAtHand.CreationDate = tempTask.CreationDate;
            taskAtHand.CompletionDate = tempTask.CompletionDate;
            taskAtHand.HasDetails = tempTask.HasDetails;
            taskAtHand.Details = tempTask.Details;
            taskAtHand.IsCompleted = tempTask.IsCompleted;
            taskAtHand.IsColored = tempTask.IsColored;
            taskAtHand.DrawColor = tempTask.DrawColor;
        }

        public static void TraverseRanks(Task task, int delta)
        {
            var taskList = new List<Task>(Ledger.Manifest.OutstandingTasks);
            var index = taskList.IndexOf(task);
            taskList.RemoveAt(index);
            taskList.Insert(index + delta, task);
            Ledger.Manifest.OutstandingTasks = taskList.ToArray();
        }

        internal static void Save(ScriptableObject so)
        {
            EditorUtility.SetDirty(so);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}