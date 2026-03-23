using Godot;
using System;

public partial class TitelCheckpoint : Checkpoint
{
	[Export] private Timer fixAnomalyTimer;

	public override void _Ready()
	{
		base._Ready();
		SetRandomWaitTime();
		anomalyTimer.Start();
		anomalyTimer.Timeout += MakeAnomaly;
		fixAnomalyTimer.Timeout += base.FixAnomaly;
	}
	public override void SetRandomWaitTime()
	{
		base.anomalyTimer.WaitTime = GD.RandRange(10, 60);
	}

	public override void MakeAnomaly()
	{
		base.MakeAnomaly();
		fixAnomalyTimer.Start();
	}
}
