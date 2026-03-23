using Godot;
using System;

public partial class ReportMenu : Control
{
	[Export] private ReportButton reportButton;
	[Export] private Label label1;
	[Export] private Label label2;
	[Export] private Label label3;
	[Export] private Label label4;
	[Export] private Timer bufferTimer;
	public bool _isOpen = false;
	private int _selectedCheckpoint = 0;
	private Color gray = new Color(0.668f, 0.668f, 0.668f);
	private Color white = new Color(1.0f, 1.0f, 1.0f);

	public override void _Ready()
	{
		Visible = false;
		ResetColors();
	}
	public override void _Input(InputEvent @event)
	{
		if (!_isOpen)
			return;

		if (Input.IsActionJustPressed("1"))
		{
			_selectedCheckpoint = 1;
			ResetColors();
			GD.Print($"Selected checkpoint: {_selectedCheckpoint}");
		}

		if (Input.IsActionJustPressed("2"))
		{
			_selectedCheckpoint = 2;
			ResetColors();
			GD.Print($"Selected checkpoint: {_selectedCheckpoint}");
		}

		if (Input.IsActionJustPressed("3"))
		{
			_selectedCheckpoint = 3;
			ResetColors();
			GD.Print($"Selected checkpoint: {_selectedCheckpoint}");
		}

		if (Input.IsActionJustPressed("4"))
		{
			_selectedCheckpoint = 4;
			ResetColors();
			GD.Print($"Selected checkpoint: {_selectedCheckpoint}");
		}

		if (Input.IsActionJustPressed("enter") || Input.IsActionJustPressed("interact"))
		{
			GD.Print("Interacted to report");
			reportButton.ReportCheckpoint(_selectedCheckpoint);
			_selectedCheckpoint = 0;
			ResetColors();
			CloseMenu();
		} else if (Input.IsActionJustPressed("ui_cancel"))
		{
			_selectedCheckpoint = 0;
			ResetColors();
			CloseMenu();
		}
	}

	private void ResetColors()
	{
		label1.LabelSettings.FontColor = gray;
		label2.LabelSettings.FontColor = gray;
		label3.LabelSettings.FontColor = gray;
		label4.LabelSettings.FontColor = gray;
		if (_selectedCheckpoint == 1)
		{
			label1.LabelSettings.FontColor = white;
		} else if (_selectedCheckpoint == 2)
		{
			label2.LabelSettings.FontColor = white;
		} else if (_selectedCheckpoint == 3)
		{
			label3.LabelSettings.FontColor = white;
		} else if (_selectedCheckpoint == 4)
		{
			label4.LabelSettings.FontColor = white;
		}
	}

	public void OpenMenu()
	{
		_isOpen = true;
		Visible = true;
		_selectedCheckpoint = 1;
		GD.Print("Report menu opened");
	}

	public void CloseMenu()
	{
		_isOpen = false;
		Visible = false;
		GD.Print("Report menu closed");
		bufferTimer.Start();
	}
	
}
