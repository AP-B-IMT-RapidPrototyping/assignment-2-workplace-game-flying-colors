using Godot;
using System;

public partial class ReportMenu : Control
{
	[Export] private ReportButton reportButton;
	[Export] private Label label1;
	[Export] private Label label2;
	[Export] private Label label3;
	[Export] private Label label4;
	private bool _isOpen = false;
    private int _selectedCheckpoint = 1;
	private Color selectedColor = Colors.LimeGreen;

    public override void _Ready()
    {
        Visible = false;
    }
	public override void _Process(double delta)
    {
        if (!_isOpen)
            return;

        if (Input.IsActionJustPressed("1"))
        {
            _selectedCheckpoint = 1;
			ResetColors();
			label1.LabelSettings.OutlineSize = 3;
            GD.Print($"Selected checkpoint: {_selectedCheckpoint}");
        }

        if (Input.IsActionJustPressed("2"))
        {
            _selectedCheckpoint = 2;
			ResetColors();
			label2.LabelSettings.OutlineSize = 3;
            GD.Print($"Selected checkpoint: {_selectedCheckpoint}");
        }

		if (Input.IsActionJustPressed("3"))
        {
            _selectedCheckpoint = 3;
			ResetColors();
			label3.LabelSettings.OutlineSize = 3;
            GD.Print($"Selected checkpoint: {_selectedCheckpoint}");
        }

		if (Input.IsActionJustPressed("4"))
        {
            _selectedCheckpoint = 4;
			ResetColors();
			label4.LabelSettings.OutlineSize = 3;
            GD.Print($"Selected checkpoint: {_selectedCheckpoint}");
        }

        if (Input.IsActionJustPressed("enter"))
        {
            reportButton.ReportCheckpoint(_selectedCheckpoint);
            CloseMenu();
        }

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            CloseMenu();
        }
    }

	private void ResetColors()
	{
		label1.LabelSettings.OutlineSize = 0;
		label2.LabelSettings.OutlineSize = 0;
		label3.LabelSettings.OutlineSize = 0;
		label4.LabelSettings.OutlineSize = 0;
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
    }
	
}
