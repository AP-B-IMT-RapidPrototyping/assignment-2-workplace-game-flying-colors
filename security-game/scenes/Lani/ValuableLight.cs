using Godot;
using System;

public partial class ValuableLight : SpotLight3D
{
	private Color green = new Color(0.686f, 1.0f, 0.659f);
	private Color red = new Color(1.0f, 0.698f, 0.659f);
	
	[Export] public int ID;

	public override void _Ready()
	{
		this.LightColor = green;
	}

	public void TurnGreen()
	{
		GD.Print($"Light turn green {ID}");
		this.LightColor = green;
	}

	public void TurnRed()
	{
		GD.Print($"Light turn red {ID}");
		this.LightColor = red;
	}
}
