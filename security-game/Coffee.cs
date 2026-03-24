using Godot;
using System;

public partial class Coffee : Node3D, IInteractible
{
	[Signal]
	public delegate void DrinkEventHandler(Node3D interactor, Vector3 hitPosition);
	[Export] private Label coffeeText;
	[Export] private Timer visibilityTimer;
	[Export] private AudioStreamPlayer3D audioPlayer;
	[Export] private AudioStream drinkSound;

	public override void _Ready()
	{
		if (audioPlayer == null)
			audioPlayer = GetNodeOrNull<AudioStreamPlayer3D>("AudioStreamPlayer3D");

		visibilityTimer.Timeout += eraseText;
	}

	public void Interact(Node3D interactor, Vector3 hitPosition)
	{
		GD.Print("Drink signal emitted");
		EmitSignal(SignalName.Drink, 1f);
		PlaySound();
		coffeeText.Visible = true;
		visibilityTimer.Start();
	}

	private void eraseText()
	{
		coffeeText.Visible = false;
	}

	private void PlaySound()
	{
		if (audioPlayer == null || drinkSound == null)
			return;

		audioPlayer.Stream = drinkSound;
		audioPlayer.Play();
	}
}
