using Godot;
using System;

public partial class EndScreen : Control
{
	[Export] private Label breakInLabel;
	[Export] private Label timeLabel;
	[Export] private Label itemLabel;
    public void UpdateStats()
    {
		breakInLabel.Text = $"Break-ins fixed: {GameStats.breakInsStopped}";
		timeLabel.Text = $"Time survived: {GameStats.timeSurvived}";
		itemLabel.Text = $"Remaining items: {GameStats.remainingItems}";
    }

	private void CloseScreen()
	{
		this.Visible = false;
	}
}
