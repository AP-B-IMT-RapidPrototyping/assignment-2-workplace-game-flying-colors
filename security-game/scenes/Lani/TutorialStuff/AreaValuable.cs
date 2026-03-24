using Godot;
using System;

public partial class AreaValuable : Area3D
{
	[Export] private CharacterBody3D _player;
	[Export] private Valuable valuable;
	[Export] private ValuableLight valuableLight;
	private void playerEntered(Node3D node)
	{
		if (node == _player)
		{
			GD.Print("player entered");
			valuable.Visible = false;
			valuableLight.TurnRed();
		}
	}
}
