using Godot;
using System;
using System.Collections.ObjectModel;

public partial class ReportButton : Node3D, IInteractible
{
	[Signal]
	public delegate void ReportEventHandler(Node3D interactor, Vector3 hitPosition);

	private Godot.Collections.Array<Godot.Node> checkpoints;

	[Export] private ReportMenu reportMenu;

    public override void _Ready()
    {
        checkpoints = GetTree().GetNodesInGroup("checkpoints");
    }

	public void Interact(Node3D interactor, Vector3 hitPosition)
	{
		GD.Print("Button pressed");
		reportMenu.OpenMenu();
	}

	public void ReportCheckpoint(int id)
	{
		Checkpoint currentCheckpoint;
		
		foreach	(Checkpoint checkpoint in checkpoints)
		{
			GD.Print("Checking print");
			if (checkpoint.ID == id)
			{
				currentCheckpoint = checkpoint;
				if (currentCheckpoint.hasAnomaly)
				{
					currentCheckpoint.FixAnomaly();
					GD.Print($"Checkpoint ID:{id} is fixed.");
					return;
				} else
				{
					GD.Print($"Checkpoint ID:{id} doesn't have an anomaly.");
				}
			}
		}
		GD.Print($"Checkpoint ID:{id} doesn't exist");
		return;
	}
}
