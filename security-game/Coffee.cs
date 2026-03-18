using Godot;
using System;

public partial class Coffee : Node3D, IInteractible
{
	[Signal]
	public delegate void DrinkEventHandler(Node3D interactor, Vector3 hitPosition);

	public void Interact(Node3D interactor, Vector3 hitPosition)
	{
		GD.Print("Drink signal emitted");
		EmitSignal(SignalName.Drink, 1f);
	}
}
