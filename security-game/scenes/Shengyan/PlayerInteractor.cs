using Godot;
using System;

//Handles interaction

public partial class PlayerInteractor : Node3D
{
	[Signal]
	public delegate void InteractionRequestedEventHandler(Node3D target, Vector3 hitPosition);

	[Signal]
	public delegate void InteractionMissedEventHandler();

	[Signal]
	public delegate void LookedAtInteractableChangedEventHandler(Node3D interactable, bool isLookingAtInteractable);

	[Export] private Camera3D _camera;
	[Export] private CharacterBody3D _player;
	[Export] private string _interactAction = "interact";
	[Export] private float _interactionReach = 3.0f;
	[Export(PropertyHint.Layers3DPhysics)] private uint _interactionCollisionMask = uint.MaxValue;
	[Export] private string _interactableGroupName = "interactable";
	[Export] private bool _debugInteractionFocus = true;

	public Node3D CurrentLookedAtInteractable { get; private set; }
	public Vector3 CurrentLookedAtHitPosition { get; private set; } = Vector3.Zero;
	public bool IsLookingAtInteractable => CurrentLookedAtInteractable != null;

	public override void _Ready()
	{
		if (_camera == null)
		{
			GD.PushWarning("PlayerInteractor: Camera is not assigned. Interaction raycasts are disabled.");
		}

		if (_player == null)
		{
			GD.PushWarning("PlayerInteractor: Player is not assigned. Self-hit filtering may fail.");
		}

		if (!InputMap.HasAction(_interactAction))
		{
			GD.PushWarning($"PlayerInteractor: Input action '{_interactAction}' does not exist.");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdateLookedAtInteractable();

		if (!InputMap.HasAction(_interactAction) || !Input.IsActionJustPressed(_interactAction))
		{
			return;
		}

		if (TryGetLookedAtInteractable(out Node3D interactable, out Vector3 hitPosition))
		{
			EmitSignal(SignalName.InteractionRequested, interactable, hitPosition);

			if (interactable is IInteractible typedInteractible)
			{
				typedInteractible.Interact(_player, hitPosition);
			}
			else if (_debugInteractionFocus)
			{
				GD.PushWarning($"[Interaction] Node '{interactable.Name}' was detected but does not implement IInteractible.");
			}
		}
		else
		{
			EmitSignal(SignalName.InteractionMissed);
		}
	}

	public bool TryGetLookedAtInteractable(out Node3D interactable, out Vector3 hitPosition)
	{
		interactable = null;
		hitPosition = Vector3.Zero;

		if (!TryRaycastFromCamera(out Node3D hitNode, out hitPosition))
		{
			return false;
		}

		interactable = FindInteractibleNode(hitNode);
		return interactable != null;
	}

	private void UpdateLookedAtInteractable()
	{
		Node3D previousInteractable = CurrentLookedAtInteractable;

		if (TryGetLookedAtInteractable(out Node3D interactable, out Vector3 hitPosition))
		{
			CurrentLookedAtInteractable = interactable;
			CurrentLookedAtHitPosition = hitPosition;
		}
		else
		{
			CurrentLookedAtInteractable = null;
			CurrentLookedAtHitPosition = Vector3.Zero;
		}

		if (previousInteractable != CurrentLookedAtInteractable)
		{
			if (_debugInteractionFocus)
			{
				if (IsLookingAtInteractable)
				{
					GD.Print($"[Interaction] Looking at interactable: {CurrentLookedAtInteractable.Name}");
				}
				else
				{
					GD.Print("[Interaction] Not looking at an interactable.");
				}
			}

			EmitSignal(SignalName.LookedAtInteractableChanged, CurrentLookedAtInteractable, IsLookingAtInteractable);
		}
	}

	private bool TryRaycastFromCamera(out Node3D hitNode, out Vector3 hitPosition)
	{
		hitNode = null;
		hitPosition = Vector3.Zero;

		if (_camera == null)
		{
			return false;
		}

		Vector2 viewportCenter = GetViewport().GetVisibleRect().Size * 0.5f;
		Vector3 rayOrigin = _camera.ProjectRayOrigin(viewportCenter);
		Vector3 rayDirection = _camera.ProjectRayNormal(viewportCenter);

		PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(
			rayOrigin,
			rayOrigin + (rayDirection * _interactionReach),
			_interactionCollisionMask
		);

		if (_player != null)
		{
			query.Exclude = new Godot.Collections.Array<Rid> { _player.GetRid() };
		}

		Godot.Collections.Dictionary result = GetWorld3D().DirectSpaceState.IntersectRay(query);
		if (result.Count == 0)
		{
			return false;
		}

		hitPosition = ((Variant)result["position"]).AsVector3();
		GodotObject colliderObject = ((Variant)result["collider"]).AsGodotObject();
		if (colliderObject is Node3D node)
		{
			hitNode = node;
			return true;
		}

		return false;
	}

	private Node3D FindInteractibleNode(Node startNode)
	{
		Node current = startNode;

		while (current != null)
		{
			if (current is Node3D node3D && (node3D is IInteractible || node3D.IsInGroup(_interactableGroupName)))
			{
				return node3D;
			}

			current = current.GetParent();
		}

		return null;
	}
}
