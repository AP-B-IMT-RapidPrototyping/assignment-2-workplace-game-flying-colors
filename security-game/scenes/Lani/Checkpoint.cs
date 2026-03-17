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

	private bool pretending = false;

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
			EmitSignal(SignalName.LoseEnergy, -20);
		}
	}

	// Maak een nieuwe anomaly (momenteel kan de deur alleen open gaan)
	private void MakeAnomaly()
	{
		if (hasAnomaly)
			return;
		if (!pretending)
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

	public void PretendClosed()
	{
		pretending = true;
		if (hasAnomaly)
		{
			animations.Play("close");
			GD.Print($"Pretends to be closed ID:{ID}");
		}
	}

	public void StopPretending()
	{
		pretending = false;
		if (hasAnomaly)
		{
			animations.Play("open");
			GD.Print($"Checkpoint: {ID} no longer pretends to be closed.");
		} 
	}
}
