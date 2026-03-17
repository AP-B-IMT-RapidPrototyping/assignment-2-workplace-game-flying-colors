using Godot;
using System;
using System.Collections.ObjectModel;

public partial class ReportButton : Node3D, IInteractible
{
	[Signal]
	public delegate void ReportEventHandler(Node3D interactor, Vector3 hitPosition);

	private Godot.Collections.Array<Godot.Node> checkpoints;

	[Export] private ReportMenu reportMenu;
	[Export] private Label AnomalyFixedLabel;
	[Export] private Label AnomalyNotFoundLabel;
	[Export] private Timer labelTimer;

    public override void _Ready()
    {
        checkpoints = GetTree().GetNodesInGroup("checkpoints");
		AnomalyFixedLabel.Visible = false;
		AnomalyNotFoundLabel.Visible = false;
		labelTimer.Timeout += RemoveLabels;
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
			//GD.Print("Checking print");
			if (checkpoint.ID == id)
			{
				currentCheckpoint = checkpoint;
				if (currentCheckpoint.hasAnomaly)
				{
					currentCheckpoint.FixAnomaly();
					GD.Print($"Checkpoint ID:{id} is fixed.");
					AnomalyFixedLabel.Visible = true;
					labelTimer.Start();
					return;
				} else
				{
					GD.Print($"Checkpoint ID:{id} doesn't have an anomaly.");
					AnomalyNotFoundLabel.Visible = true;
					labelTimer.Start();
				}
			}
		}
		//GD.Print($"Checkpoint ID:{id} doesn't exist");
		return;
	}

	public void RemoveLabels()
	{
		AnomalyFixedLabel.Visible = false;
		AnomalyNotFoundLabel.Visible = false;
	}
}
