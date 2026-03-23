using Godot;
using System;

public partial class Note : Node3D, IInteractible
{
	[Export] private Control text;
	private bool openedNote = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		text.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel") && openedNote)
		{
			CloseNote();
		}
		
	}

	public void Interact(Node3D interactor, Vector3 hitPosition)
	{
		if (!openedNote) {
			GD.Print("Note interacted");
			text.Visible = true;
			openedNote = true;
		} else
		{
			CloseNote();
		}
	}

	public virtual void CloseNote()
	{
		text.Visible = false;
		openedNote = false;
	}

	public void OnLookedAtInteractableChanged(Node3D CurrentLookedAtInteractable, bool IsLookingAtInteractable)
	{
		if (!IsLookingAtInteractable && openedNote)
		{
			CloseNote();

		}
	}
}
