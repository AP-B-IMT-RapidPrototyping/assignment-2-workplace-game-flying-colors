using Godot;
using System;

public partial class TestEnergyLabel : Label
{
	private void LoseEnergy()
	{
		Text = "Energy: no";
	}
}
