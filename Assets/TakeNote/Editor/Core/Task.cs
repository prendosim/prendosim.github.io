using System;
using UnityEngine;

namespace FuguFirecracker.TakeNote
{
	[Serializable]
	public class Task
	{
		public string Title;
		public string CreationDate;
		public string CompletionDate;
		public bool HasDetails;
		public string Details;
		public bool IsCompleted;
		public bool IsDeferred;
		public bool IsColored;
		public Color DrawColor;
		public bool DoShowMore { get; set; }
	    public  Rect ClickOnRect { get; set; }
	}
}