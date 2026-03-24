using Godot;
using System;

public partial class Startscherm : Node3D
{
	[Export] private AnimationPlayer alarmAnimation;
	[Export] private AnimationPlayer TitelAnimation;
	[Export] private EndScreen endScreen;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
		alarmAnimation.Play("rotate");
		TitelAnimation.Play("flicker");

		if (GameStats.gameJustPLayed)
		{
			endScreen.UpdateStats();
			endScreen.Visible = true;
			GameStats.gameJustPLayed = false;
		} else
		{
			endScreen.Visible = false;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
