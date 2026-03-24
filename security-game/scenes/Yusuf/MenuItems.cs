 
using Godot;
using System;

public partial class MenuItems : Node3D
{
	[Export] private AnimationPlayer _animationPlayer;

	[Export] private MeshInstance3D playMesh;
	[Export] private MeshInstance3D quitMesh;
	[Export] private MeshInstance3D gameMesh;
	[Export] private MeshInstance3D tutorialMesh;
	[Export] private MeshInstance3D backMesh;

	private StandardMaterial3D mat;


	private void StartButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			_animationPlayer.Play("chooseScreen");
		}
	}

	private void QuitButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			GetTree().Quit();
		}
	}

	private void GameButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			Game();
		}
	}

	private void TutorialButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			Tutorial();
		}
	}

	private void BackButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			Back();
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

	private void GameHighlight()
	{
		mat = gameMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			mat.EmissionEnabled = true;
		}
	}

	private void GameUnHighlight()
	{
		mat = gameMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			mat.EmissionEnabled = false;
		}
	}

	private void TutorialHighlight()
	{
		mat = tutorialMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			mat.EmissionEnabled = true;
		}
	}

	private void TutorialUnHighlight()
	{
		mat = tutorialMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			mat.EmissionEnabled = false;
		}
	}

	private void BackHighlight()
	{
		mat = backMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			mat.EmissionEnabled = true;
		}
	}

	private void BackUnHighlight()
	{
		mat = backMesh.GetActiveMaterial(0) as StandardMaterial3D;
		if (mat != null)
		{
			mat.EmissionEnabled = false;
		}
	}

	private void Tutorial()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Levels/tutorial.tscn");
	}

	private void Game()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Levels/full_level_2.tscn");
	}

	private void Back()
	{
		_animationPlayer.PlayBackwards("chooseScreen");
	}
}
