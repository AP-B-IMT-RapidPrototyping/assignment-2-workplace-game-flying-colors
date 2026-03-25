using Godot;
using System;
using System.Collections.ObjectModel;

public partial class ReportButton : Node3D, IInteractible
{
	[Signal]
	public delegate void ReportEventHandler(Node3D interactor, Vector3 hitPosition);

	[Signal]
	public delegate void AnomalyFixedEventHandler();

	private Godot.Collections.Array<Godot.Node> checkpoints;

	[Export] private ReportMenu reportMenu;
	[Export] private Label anomalyFixedLabel;
	[Export] private Label anomalyNotFoundLabel;
	[Export] private Label warningLabel;
	[Export] private Label firstStrikeLabel;
	[Export] private Label secondStrikeLabel;
	[Export] private Label noResponseLabel;
	[Export] private Timer labelTimer;
	[Export] private Timer bufferTimer;
	[Export] private PlayerInteractor playerInteractor;

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
	[Export] private Timer disableTimerLonger;
	private int consecutiveFalseCount = 0;
	private bool firstStrikeReached = false;
	private bool isEnabled = true;
	private int fixedCounter = 0;

	public override void _Ready()
	{
		checkpoints = GetTree().GetNodesInGroup("checkpoints");
		anomalyFixedLabel.Visible = false;
		anomalyNotFoundLabel.Visible = false;
		warningLabel.Visible = false;
		firstStrikeLabel.Visible = false;
		secondStrikeLabel.Visible = false;
		noResponseLabel.Visible = false;
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
		if (disableTimerLonger == null)
		{
			GD.PushWarning("ReportButton: disableTimerLonger is not assigned.");
		}
		else
		{
			disableTimerLonger.Timeout += Enable;
		}

		if (playerInteractor == null)
		{
			GD.PushWarning("ReportButton: playerInteractor is not assigned. LookedAtInteractableChanged will not be received.");
		}
		else
		{
			playerInteractor.LookedAtInteractableChanged += OnLookedAtInteractableChanged;
		}
	}

	public override void _ExitTree()
	{
		if (playerInteractor != null)
		{
			playerInteractor.LookedAtInteractableChanged -= OnLookedAtInteractableChanged;
		}
	}


	public void Interact(Node3D interactor, Vector3 hitPosition)
	{
		if (bufferTimer.TimeLeft > 0)
			return;
		if (reportMenu._isOpen)
			return;
		if (!isEnabled)
		{
			GD.Print("ReportButton: Ignored interaction while disabled.");
			ShowLabel(noResponseLabel);
			return;
		}

		GD.Print("Button pressed");
		reportMenu._isOpen = true;
		reportMenu.OpenMenu();
	}

	public void ReportCheckpoint(int id)
	{
		if (!isEnabled)
		{
			GD.Print("ReportButton: Ignored report while disabled.");
			ShowLabel(noResponseLabel);
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
					fixedCounter++;
					ShowLabel(anomalyFixedLabel);
					EmitSignal(SignalName.AnomalyFixed);
					return;
				}
				else
				{
					consecutiveFalseCount++;
					if (consecutiveFalseCount == 2)
					{
						ShowLabel(warningLabel);
					}
					else if (firstStrikeReached)
					{
						GD.Print("ReportButton: Second+ strike, disabled with longer timer");
						ShowLabel(secondStrikeLabel);
						Disable(false);

					}
					else if (consecutiveFalseCount >= tolerance)
					{
						GD.Print("ReportButton: First strike, disabled with timer");
						ShowLabel(firstStrikeLabel);
						firstStrikeReached = true;
						Disable(true);
					}
					else
					{
						ShowLabel(anomalyNotFoundLabel);
					}
					GD.Print($"Checkpoint ID:{id} doesn't have an anomaly.");

				}
			}
		}
		//GD.Print($"Checkpoint ID:{id} doesn't exist");
		return;
	}

	public void RemoveLabels()
	{
		anomalyFixedLabel.Visible = false;
		anomalyNotFoundLabel.Visible = false;
		warningLabel.Visible = false;
		firstStrikeLabel.Visible = false;
		secondStrikeLabel.Visible = false;
		noResponseLabel.Visible = false;

	}
	private void Disable(bool firstStrike = false)
	{
		isEnabled = false;

		light.Visible = false;

		reportMenu.CloseMenu();

		if (firstStrike)
		{
			disableTimer.Start();
		}
		else
		{
			disableTimerLonger.Start();
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
	private void ShowLabel(Label label)
	{
		RemoveLabels();
		label.Visible = true;
		labelTimer.Start();
	}

	public void OnLookedAtInteractableChanged(Node3D CurrentLookedAtInteractable, bool IsLookingAtInteractable)
	{
		if (!reportMenu._isOpen)
		{
			return;
		}

		bool lookingAtThisButton = IsLookingAtInteractable && CurrentLookedAtInteractable == this;
		if (!lookingAtThisButton)
		{
			reportMenu.CloseMenu();

		}
	}

	private void setFixedCounter()
	{
		GameStats.breakInsStopped = fixedCounter;
	}
}
