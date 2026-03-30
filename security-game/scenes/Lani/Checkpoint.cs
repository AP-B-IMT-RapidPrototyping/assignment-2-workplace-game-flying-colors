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
	[Export] private AudioStreamPlayer3D audioPlayer;
	[Export(PropertyHint.Range, "100,20000,50")] private float soundMaxDistance = 5000f;
	[Export(PropertyHint.Range, "0.5,100,0.5")] private float soundUnitSize = 30f;
	[Export(PropertyHint.Range, "-12,12,0.1")] private float soundMaxDb = 0f;

	[Export] private string openAnimation;
	[Export] private string closeAnimation;
	[Export] private AudioStream openSound;
	[Export] private AudioStream closeSound;


	private bool pretending = false;

	public override void _Ready()
	{
		if (audioPlayer == null)
			audioPlayer = GetNodeOrNull<AudioStreamPlayer3D>("AudioStreamPlayer3D");

		if (audioPlayer != null)
		{
			audioPlayer.MaxDistance = soundMaxDistance;
			audioPlayer.UnitSize = soundUnitSize;
			audioPlayer.MaxDb = soundMaxDb;
		}

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
		{
			animations.Play(openAnimation);
			PlaySound(openSound);
		}
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

		animations.Play(closeAnimation);
		PlaySound(closeSound);
		hasAnomaly = false;
		GD.Print($"Checkpoint {ID} anomaly fixed");

		SetRandomWaitTime();
		anomalyTimer.Start();
	}

	private void PlaySound(AudioStream stream)
	{
		if (audioPlayer == null || stream == null)
			return;

		audioPlayer.Stream = stream;
		audioPlayer.Play();
	}

}
