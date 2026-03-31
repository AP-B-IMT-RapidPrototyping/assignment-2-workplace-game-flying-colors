using Godot;
using System;

public partial class LevelTime : Node3D
{
	[Signal]
	public delegate void UpdateStatsEventHandler();
	[Signal]
	public delegate void StopTimersEventHandler();
	[Export] private Timer endTimer;
	[Export] private Timer bufferTimer;
	[Export] private Label3D clock;
	[Export] private Node3D spotlights;
	[Export] private Label endLabel;
	private int minutes;
	private int seconds;
	private double totalSeconds;

	public override void _Ready()
	{
		clock.Text = "00:00";
		endTimer.Timeout += EndRound;
		bufferTimer.Timeout += LoadTitleScreen;
		spotlights.Visible = false;
		endLabel.Visible = false;
	}

	public override void _Process(double delta)
	{
		totalSeconds = 420 - endTimer.TimeLeft;
		totalSeconds = Math.Clamp(totalSeconds, 0, 420);

		minutes = Convert.ToInt32(Math.Floor(totalSeconds / 60));
		seconds = Convert.ToInt32(Math.Floor(totalSeconds % 60));

		clock.Text = $"{minutes:D2}:{seconds:D2}";
	}


	private void EndRound()
	{
		EmitSignal(SignalName.StopTimers);
		clock.Text = "07:00";
		GameStats.timeSurvived = clock.Text;
		spotlights.Visible = true;
		endLabel.Visible = true;
		bufferTimer.Start();
	}

	private void LoadTitleScreen()
	{
		EmitSignal(SignalName.UpdateStats);
		GameStats.gameJustPLayed = true;
		GetTree().ChangeSceneToFile("res://scenes/Yusuf/startscherm.tscn");
	}

	private void setTimeSurvived()
	{
		GameStats.timeSurvived = clock.Text;
	}
}
