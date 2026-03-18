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

	// --- Penalty for mass false reporting ---
	// After x amount of consecutive false report, 
	// all reports will be ignored for y amount of time.
	// After the reenabling, if it receives another false report 
	// the button is permanently disabled.

	// Light on when enabled, off when disabled.
	[Export] private OmniLight3D light;
	// Amount of consecutive false report before disabled.
	[Export] private int tolerance = 3;
	[Export] private Timer disableTimer;
	private int consecutiveFalseCount = 0;
	private bool firstStrikeReached = false;
	private bool isEnabled = true;

	public override void _Ready()
	{
		checkpoints = GetTree().GetNodesInGroup("checkpoints");
		AnomalyFixedLabel.Visible = false;
		AnomalyNotFoundLabel.Visible = false;
		labelTimer.Timeout += RemoveLabels;
		if (light == null)
		{
			GD.PushWarning("ReportButton: light is not assigned.");
		}
		if (disableTimer == null)
		{
			GD.PushWarning("ReportButton: disableTimer is not assigned.");
		}
		else
		{
			disableTimer.Timeout += Enable;
		}
	}


	public void Interact(Node3D interactor, Vector3 hitPosition)
	{
		if (!isEnabled)
		{
			GD.Print("ReportButton: Ignored interaction while disabled.");
			return;
		}

		GD.Print("Button pressed");
		reportMenu.OpenMenu();
	}

	public void ReportCheckpoint(int id)
	{
		if (!isEnabled)
		{
			GD.Print("ReportButton: Ignored report while disabled.");
			return;
		}

		Checkpoint currentCheckpoint;

		foreach (Checkpoint checkpoint in checkpoints)
		{
			//GD.Print("Checking print");
			if (checkpoint.ID == id)
			{
				currentCheckpoint = checkpoint;
				if (currentCheckpoint.hasAnomaly)
				{
					consecutiveFalseCount = 0;
					currentCheckpoint.FixAnomaly();
					GD.Print($"Checkpoint ID:{id} is fixed.");
					AnomalyFixedLabel.Visible = true;
					labelTimer.Start();
					return;
				}
				else
				{
					consecutiveFalseCount++;
					if (firstStrikeReached)
					{
						GD.Print("ReportButton: Second strike, permanently disabled");
						Disable(false);

					}
					else if (consecutiveFalseCount >= tolerance)
					{
						GD.Print("ReportButton: First strike, disabled with timer");
						firstStrikeReached = true;
						Disable(true);

					}
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
	private void Disable(bool withTimer = false)
	{
		isEnabled = false;

		light.Visible = false;

		reportMenu.CloseMenu();

		if (withTimer)
		{
			disableTimer.Start();
		}
	}

	private void Enable()
	{
		if (!isEnabled)
		{
			isEnabled = true;
			consecutiveFalseCount = 0;
			GD.Print("ReportButton: Re-enabled");
		}
		light.Visible = true;
	}
}
