using Godot;
using System;

public partial class AreaRemoveEnergy : Area3D
{
	[Signal]
	public delegate void StopTimerEventHandler();

	[Signal]
	public delegate void RemoveEnergyEventHandler();

	[Export] private CharacterBody3D _player;
	private void playerEntered(Node3D node)
	{
		if (node == _player)
		{
			GD.Print("player entered");
			EmitSignal(SignalName.StopTimer);
			EmitSignal(SignalName.RemoveEnergy, 0f);
		}
	}
}
