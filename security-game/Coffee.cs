using Godot;
using System;

public partial class Coffee : Node3D, IInteractible
{
	[Signal]
	public delegate void DrinkEventHandler(Node3D interactor, Vector3 hitPosition);
	[Export] private Label coffeeText;
	[Export] private Timer visibilityTimer;

    public override void _Ready()
    {
        visibilityTimer.Timeout += eraseText;
    }

	public void Interact(Node3D interactor, Vector3 hitPosition)
	{
		GD.Print("Drink signal emitted");
		EmitSignal(SignalName.Drink, 1f);
		coffeeText.Visible = true;
		visibilityTimer.Start();
	}

	private void eraseText()
	{
		coffeeText.Visible = false;
	}
}
