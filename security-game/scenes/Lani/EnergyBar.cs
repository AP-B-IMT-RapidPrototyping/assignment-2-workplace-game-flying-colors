using Godot;
using System;

public partial class EnergyBar : TextureProgressBar
{
	
	public override void _Ready()
	{
		this.Value = 1f;
	}

	public void ChangeValue(float energy)
	{
		this.Value = energy;
	}
}
