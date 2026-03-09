using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
	[Signal]
	public delegate void InteractionRequestedEventHandler(Node3D target, Vector3 hitPosition);

	[Signal]
	public delegate void InteractionMissedEventHandler();

	[Signal]
	public delegate void LookedAtInteractableChangedEventHandler(Node3D interactable, bool isLookingAtInteractable);

	[Export] private float _mouseSensitivity = 0.003f;
    [Export] private Camera3D _camera;

	[Export] private string _interactAction = "interact";
	[Export] private float _interactionReach = 3.0f;
	[Export(PropertyHint.Layers3DPhysics)] private uint _interactionCollisionMask = uint.MaxValue;
	[Export] private string _interactableGroupName = "interactable";
	[Export] private bool _debugInteractionFocus = true;

	public Node3D CurrentLookedAtInteractable { get; private set; }
	public Vector3 CurrentLookedAtHitPosition { get; private set; } = Vector3.Zero;
	public bool IsLookingAtInteractable => CurrentLookedAtInteractable != null;

	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;


    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;

		if (_camera == null)
		{
			GD.PushWarning("PlayerController: Camera is not assigned. Interaction raycasts are disabled.");
		}

		if (!InputMap.HasAction(_interactAction))
		{
			GD.PushWarning($"PlayerController: Input action '{_interactAction}' does not exist.");
		}
    }

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		UpdateLookedAtInteractable();

		if (InputMap.HasAction(_interactAction) && Input.IsActionJustPressed(_interactAction))
		{
			if (IsLookingAtInteractable)
			{
				EmitSignal(SignalName.InteractionRequested, CurrentLookedAtInteractable, CurrentLookedAtHitPosition);
			}
			else
			{
				EmitSignal(SignalName.InteractionMissed);
			}
		}
	}

	public bool TryGetInteractionTarget(out Node3D target, out Vector3 hitPosition)
	{
		return TryGetLookedAtInteractable(out target, out hitPosition);
	}

	public bool TryGetLookedAtInteractable(out Node3D interactable, out Vector3 hitPosition)
	{
		interactable = null;
		hitPosition = Vector3.Zero;

		if (!TryRaycastFromCamera(out Node3D hitNode, out hitPosition))
		{
			return false;
		}

		interactable = FindInteractableNode(hitNode);
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
		query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };

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

	private Node3D FindInteractableNode(Node startNode)
	{
		Node current = startNode;

		while (current != null)
		{
			if (current is Node3D node3D && node3D.IsInGroup(_interactableGroupName))
			{
				return node3D;
			}

			current = current.GetParent();
		}

		return null;
	}

	public override void _Input(InputEvent @event)
    {
		if (_camera == null)
		{
			return;
		}

		if (@event is InputEventMouseMotion mouseMotion)
        {
			// Horizontal rotation: rotate the whole player (Y-axis).
            RotateY(-mouseMotion.Relative.X * _mouseSensitivity);

			// Vertical rotation: rotate only the camera (X-axis).
            _camera.RotateX(-mouseMotion.Relative.Y * _mouseSensitivity);

			// Keep camera pitch in a safe range so it does not flip.
            Vector3 cameraRotation = _camera.Rotation;
            cameraRotation.X = Mathf.Clamp(cameraRotation.X, -1.5f, 1.5f);
            _camera.Rotation = cameraRotation;
        }
    }
    
}
