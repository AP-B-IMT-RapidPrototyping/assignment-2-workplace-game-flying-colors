using Godot;
using System;

public partial class EnergyTimer : Timer
{
	[Signal]
	public delegate void LoseEnergyEventHandler();
	public override void _Ready()
	{
		this.Timeout += UseEnergy;
		//StartTimer(); //Moet later pas aangeroepen worden wanneer een level start
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StartTimer()
	{
		this.Start();
	}

	public void StopTimer()
	{
		this.Stop();
	}

	public void UseEnergy()
	{
		EmitSignal(SignalName.LoseEnergy, -0.05f);
	}
}
