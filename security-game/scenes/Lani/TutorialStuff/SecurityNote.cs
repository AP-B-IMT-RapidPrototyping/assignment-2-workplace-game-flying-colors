using Godot;
using System;

public partial class SecurityNote : Note
{

	[Export] private CollisionShape3D doorCollision;
	[Export] private AnimationPlayer doorAnimations;
	private bool doorOpen = false;

	public override void CloseNote()
	{
		base.CloseNote();
		if (!doorOpen)
		{
			doorCollision.Disabled = true;
			doorAnimations.Play("open");
			doorOpen = true;
		}
	}
}
