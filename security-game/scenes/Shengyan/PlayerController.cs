using Godot;
using System;

//Handles player movement and input

public partial class PlayerController : CharacterBody3D
{
	[Export] private float _mouseSensitivity = 0.003f;
    [Export] private Camera3D _camera;
	[Export] private PlayerStats _stats;

	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	public PlayerStats Stats => _stats;
	public bool HasEnergy => _stats != null && _stats.HasEnergy;


    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;

		if (_camera == null)
		{
			GD.PushWarning("PlayerController: Camera is not assigned.");
		}

		if (_stats == null)
		{
			_stats = GetNodeOrNull<PlayerStats>("Stats");
			if (_stats == null)
			{
				GD.PushWarning("PlayerController: Stats is not assigned.");
			}
		}
    }

	public void SetEnergy(bool hasEnergy)
	{
		if (_stats == null)
		{
			GD.PushWarning("PlayerController: Cannot set energy because Stats is missing.");
			return;
		}

		_stats.SetEnergy(hasEnergy);
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
