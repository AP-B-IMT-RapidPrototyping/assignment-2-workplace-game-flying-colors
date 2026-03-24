using Godot;

public interface IInteractible
{
	void Interact(Node3D interactor, Vector3 hitPosition);
}
