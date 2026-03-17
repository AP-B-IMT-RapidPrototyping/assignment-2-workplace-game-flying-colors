using Godot;
using System;

public partial class EnergyBar : TextureProgressBar
{
	
	public override void _Ready()
	{
		this.Value = 100;
	}

	public void ChangeValue(int energy)
	{
		this.Value = energy;
	}
}
