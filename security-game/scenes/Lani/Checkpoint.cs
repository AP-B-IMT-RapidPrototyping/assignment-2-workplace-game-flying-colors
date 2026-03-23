using Godot;
using System;
using System.ComponentModel;

public partial class Checkpoint : Node3D
{
	[Export] protected Timer anomalyTimer;
	[Export] private Timer tempTimer;
	[Export] public bool hasAnomaly;
	[Export] public int ID = 1;
	[Export] private AnimationPlayer animations;

	[Export] private string openAnimation;
	[Export] private string closeAnimation;
	

	private bool pretending = false;

	public override void _Ready()
	{
		animations.Play(closeAnimation);
		//tempTimer.Timeout += fixAnomaly;
	}

	public override void _Process(double delta)
	{
	}

	// Maak een nieuwe anomaly (momenteel kan de deur alleen open gaan)
	public virtual void MakeAnomaly()
	{
		GD.Print($"Anomaly made at checkpoint {ID}");
		if (hasAnomaly)
			return;
		if (!pretending)
			animations.Play(openAnimation);
		hasAnomaly = true;
		//tempTimer.Start();
	}

	//Stel een nieuwe random tijd in om een nieuwe anomaly aan te maken
	public virtual void SetRandomWaitTime()
	{
		anomalyTimer.WaitTime = GD.RandRange(10, 20);
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
