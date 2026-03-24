using Godot;
using System;

public partial class Note : Node3D, IInteractible
{
	// De text control note waar de 2D note stuff achter zit
	[Export] private Control text;
	// Variable om te controlleren of de note open is of niet
	private bool openedNote = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Zorg dat bij het maken van het object de 2D stuff nog niet zichtbaar is
		text.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Als er op de cancel knop gedrukt wordt en de note is open
		if (Input.IsActionJustPressed("ui_cancel") && openedNote)
		{
			CloseNote();
		}
		
	}

	// wordt aangeroepen wanneer de speler interact met het object
	public void Interact(Node3D interactor, Vector3 hitPosition)
	{
		// Als de note nog niet open is
		if (!openedNote) {
			GD.Print("Note interacted");
			//Laat de 2D note stuff zien
			text.Visible = true;
			openedNote = true;
		} else
		{
			//Als die al wel open is, doe dan terug dicht
			CloseNote();
		}
	}
 
	public virtual void CloseNote()
	{
		text.Visible = false;
		openedNote = false;
	}

	//Zorgt ervoor dat als de speler weg kijkt van de note dat de 2D stuff ook dicht gaat
	public void OnLookedAtInteractableChanged(Node3D CurrentLookedAtInteractable, bool IsLookingAtInteractable)
	{
		if (!IsLookingAtInteractable && openedNote)
		{
			CloseNote();

		}
	}
}
