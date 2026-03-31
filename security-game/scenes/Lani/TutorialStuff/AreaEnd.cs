using Godot;
using System;

public partial class AreaEnd : Area3D
{
	[Export] private CharacterBody3D _player;
	[Export] private Label3D label;
	[Export] private Timer endTimer;

	public override void _Ready()
	{
		endTimer.Timeout += endTutorial;
	}


	private void playerEntered(Node3D node)
	{
		if (node == _player)
		{
			GD.Print("player entered");
			label.Text = "0:00";
			endTimer.Start();
		}
	}

	private void endTutorial()
	{
		GetTree().ChangeSceneToFile("res://scenes/Levels/full_level_2.tscn");
	}
}
