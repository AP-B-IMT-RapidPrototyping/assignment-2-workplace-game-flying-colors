using Godot;
using System;
using System.ComponentModel;

public partial class Checkpoint : StaticBody3D
{
	[Export] private Node3D _player;
	[Export] private Timer anomalyTimer;
	[Export] private Timer tempTimer;
	[Export] public bool hasAnomaly;
	[Export] public int ID = 1;
	[Export] private AnimationPlayer animations;
	
	[Signal]
	public delegate void LoseEnergyEventHandler();

	public override void _Ready()
	{
		SetRandomWaitTime();
		anomalyTimer.Start();
		animations.Play("close");
		anomalyTimer.Timeout += MakeAnomaly;
		//tempTimer.Timeout += fixAnomaly;
	}

	public override void _Process(double delta)
	{
	}


	//Roep signaal aan om energie te verliezen wanneer de speler binnen de range komt
	private void OnPLayerEnteredCheckpoint(Node3D body)
	{
		if (body == _player)
		{
			GD.Print("PLayer entered checkpoint.");
			EmitSignal(SignalName.LoseEnergy);
		}
	}

	// Maak een nieuwe anomaly (momenteel kan de deur alleen open gaan)
	private void MakeAnomaly()
	{
		if (hasAnomaly)
			return;
		
		animations.Play("open");
		hasAnomaly = true;
		//tempTimer.Start();
	}

	//Stel een nieuwe random tijd in om een nieuwe anomaly aan te maken
	private void SetRandomWaitTime()
	{
		anomalyTimer.WaitTime = GD.RandRange(10, 60);
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
