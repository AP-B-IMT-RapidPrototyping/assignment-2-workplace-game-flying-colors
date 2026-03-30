using Godot;
using System;

public partial class CollisionNote : Note
{
	[Export] private CollisionShape3D collision;
	[Export] private SpotLight3D spot;

	private bool open = false;

	public override void CloseNote()
	{
		base.CloseNote();
		if (!open)
		{
			collision.Disabled = true;
			open = true;
			spot.Visible = true;
		}
	}
}
