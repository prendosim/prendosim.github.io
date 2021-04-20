namespace FuguFirecracker.TakeNote
{
	internal partial class Window
	{
		internal void OnEnable()
		{
			Main.Window = this;
			Ledger.Manifest = SoBuilder.FindOrCreateInRelativePath<Ledger>("TakeNote/Editor/Persistence/Ledger.asset");

			SetEnumeratedLabel(TaskCollection.Outstanding);
			SetEnumeratedLabel(TaskCollection.Completed);
			SetEnumeratedLabel(TaskCollection.Deferred);
		}
	}
}