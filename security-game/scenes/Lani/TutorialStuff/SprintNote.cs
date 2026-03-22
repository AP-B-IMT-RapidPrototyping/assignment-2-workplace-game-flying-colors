using Godot;
using System;

public partial class SprintNote : CollisionNote
{
	[Export] private CollisionShape3D noGoBackCollision;

	public override void CloseNote()
	{
		base.CloseNote();
		noGoBackCollision.Disabled = false;
	}
}
