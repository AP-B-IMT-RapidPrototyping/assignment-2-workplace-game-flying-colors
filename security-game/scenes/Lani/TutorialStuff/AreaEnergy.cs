using Godot;
using System;

public partial class AreaEnergy : Area3D
{
	[Signal]
	public delegate void StartTimerEventHandler();

	[Export] private CharacterBody3D _player;
	private void playerEntered(Node3D node)
	{
		if (node == _player)
		{
			GD.Print("player entered");
			EmitSignal(SignalName.StartTimer);
		}
	}
}
