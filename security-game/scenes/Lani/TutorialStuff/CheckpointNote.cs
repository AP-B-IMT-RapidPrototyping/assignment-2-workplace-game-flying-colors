using Godot;
using System;

public partial class CheckpointNote : CollisionNote
{
	[Export] private Checkpoint doorCheckpoint;
	[Export] private Checkpoint windowCheckpoint;
	[Export] private Checkpoint ventCheckpoint;

	public override void CloseNote()
	{
		base.CloseNote();
		doorCheckpoint.MakeAnomaly();
		windowCheckpoint.MakeAnomaly();
		ventCheckpoint.MakeAnomaly();
	}
}
