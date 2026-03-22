using Godot;
using System;
using System.ComponentModel;

public partial class Checkpoint : Node3D
{
	[Export] private Timer anomalyTimer;
	[Export] private Timer tempTimer;
	[Export] public bool hasAnomaly;
	[Export] public int ID = 1;
	[Export] private AnimationPlayer animations;

	[Export] private string openAnimation;
	[Export] private string closeAnimation;
	

	private bool pretending = false;

	public override void _Ready()
	{
		SetRandomWaitTime();
		anomalyTimer.Start();
		animations.Play(closeAnimation);
		anomalyTimer.Timeout += MakeAnomaly;
		//tempTimer.Timeout += fixAnomaly;
	}

	public override void _Process(double delta)
	{
	}

	// Maak een nieuwe anomaly (momenteel kan de deur alleen open gaan)
	public void MakeAnomaly()
	{
		if (hasAnomaly)
			return;
		if (!pretending)
			animations.Play(openAnimation);
		hasAnomaly = true;
		//tempTimer.Start();
	}

	//Stel een nieuwe random tijd in om een nieuwe anomaly aan te maken
	private void SetRandomWaitTime()
	{
		anomalyTimer.WaitTime = GD.RandRange(10, 120);
	}

	public void FixAnomaly()
	{
		if (!hasAnomaly)
			return;

		animations.Play("close");
		hasAnomaly = false;
		GD.Print($"Checkpoint {ID} anomaly fixed");

		SetRandomWaitTime();
		anomalyTimer.Start();
	}

}
