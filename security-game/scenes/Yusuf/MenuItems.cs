
using Godot;
using System;

public partial class MenuItems : Node3D
{
	[Export] private AnimationPlayer _animationPlayer;

	[Export] private MeshInstance3D playMesh;
	[Export] private MeshInstance3D quitMesh;


	private StandardMaterial3D mat;


	private void StartButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			Game();
		}
	}

	private void QuitButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			GetTree().Quit();
		}
	}


	private void playHighlight()
	{
		mat = playMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			mat.EmissionEnabled = true;
		}
	}

	private void playUnHighlight()
	{
		mat = playMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			mat.EmissionEnabled = false;
		}
	}

	private void quitHighlight()
	{
		mat = quitMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			mat.EmissionEnabled = true;
		}
	}

	private void quitUnHighlight()
	{
		mat = quitMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			mat.EmissionEnabled = false;
		}
	}


	private void Game()
	{
		GetTree().ChangeSceneToFile("res://scenes/Levels/tutorial.tscn");
	}

}
