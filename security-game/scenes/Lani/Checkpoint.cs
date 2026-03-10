using Godot;
using System;
using System.ComponentModel;

public partial class Checkpoint : StaticBody3D
{
	[Export] private Node3D _player;
	
	[Signal]
	public delegate void LoseEnergyEventHandler();
	private void OnPLayerEnteredCheckpoint(Node3D body)
	{
		if (body == _player)
		{
			GD.Print("PLayer entered checkpoint.");
			EmitSignal(SignalName.LoseEnergy);
		}
	}
}
