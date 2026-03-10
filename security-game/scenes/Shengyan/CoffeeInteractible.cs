using Godot;

public partial class CoffeeInteractible : Node3D, IInteractible
{
	[Signal]
	public delegate void DrinkEventHandler(Node3D interactor, Vector3 hitPosition);

	public void Interact(Node3D interactor, Vector3 hitPosition)
	{
		EmitSignal(SignalName.Drink, interactor, hitPosition);
	}
}
