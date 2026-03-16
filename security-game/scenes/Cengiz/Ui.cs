using Godot;
using System;

public partial class Ui : Control
{
	ProgressBar energyBar;
	Player player;

	public override void _Ready()
	{
		energyBar = GetNode<ProgressBar>("EnergyBar");
		player = GetNode<Player>("../Player"); // pas pad aan indien nodig
	}

	public override void _Process(double delta)
	{
		energyBar.Value = player.Energy;
	}
}
