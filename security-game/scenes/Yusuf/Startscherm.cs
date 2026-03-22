using Godot;
using System;

public partial class Startscherm : Node3D
{
	[Export] private AnimationPlayer alarmAnimation;
	[Export] private AnimationPlayer TitelAnimation;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;
		alarmAnimation.Play("rotate");
		TitelAnimation.Play("flicker");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
