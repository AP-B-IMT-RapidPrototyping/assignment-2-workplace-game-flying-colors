using Godot;
using System;

//Handles player movement and input

public partial class PlayerController : CharacterBody3D
{
	[Export] private float _mouseSensitivity = 0.003f;
	[Export] private Camera3D _camera;
	[Export] private PlayerStats _stats;
	[Export] private AudioStreamPlayer3D _footstepPlayer;

	[Export] private float _speed = 2.0f;
	[Export] private float _speedSprint = 3.0f;
	[Export] private float _sprintEnergyConsumption = 0.01f;
	[Export] private float _speedSlow = 1.0f;

	private float _footstepCooldown = 0.4f;
	private float _footstepSprintCooldown = 0.267f; // 0.4 / 1.5 for 1.5x faster
	private float _footstepTimer = 0f;

	// Jump disabled
	//public const float JumpVelocity = 4.5f;

	public PlayerStats Stats => _stats;
	public float Energy => _stats?.Energy ?? 0f;


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

	public void SetEnergy(float energy)
	{
		if (_stats == null)
		{
			GD.PushWarning("PlayerController: Cannot set energy because Stats is missing.");
			return;
		}

		_stats.SetEnergy(energy);
	}


	public void ChangeEnergy(float amount)
	{
		if (_stats == null)
		{
			GD.PushWarning("PlayerController: Cannot add energy because Stats is missing.");
			return;
		}

		_stats.ChangeEnergy(amount);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// // Handle Jump(disabled).
		// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		// {
		// 	velocity.Y = JumpVelocity;
		// }

		float speed;
		bool isSprinting = false;
		if (Input.IsActionPressed("sprint") && _stats.Energy > 0f)
		{
			speed = _speedSprint;
			isSprinting = true;

		}
		else if (_stats.Energy == 0f)
		{
			speed = _speedSlow;
		}
		else
		{
			speed = _speed;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;

			// Handle footstep sounds
			_footstepTimer -= (float)delta;
			if (_footstepTimer <= 0f && IsOnFloor())
			{
				PlayRandomFootstep();
				float cooldown = isSprinting ? _footstepSprintCooldown : _footstepCooldown;
				_footstepTimer = cooldown;
			}

			if (isSprinting)
			{
				ChangeEnergy(-_sprintEnergyConsumption * (float)delta);

			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
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

	private void PlayRandomFootstep()
	{
		if (_footstepPlayer == null)
		{
			return;
		}

		int randomIndex = (int)(GD.Randi() % 10); // Random number 0-9
		string footstepPath = $"res://scenes/Shengyan/footstep{randomIndex:00}.ogg";

		var audioStream = GD.Load<AudioStream>(footstepPath);
		if (audioStream != null)
		{
			_footstepPlayer.Stream = audioStream;
			_footstepPlayer.Play();
		}
		else
		{
			GD.PushWarning($"PlayerController: Failed to load footstep sound at {footstepPath}");
		}
	}

}
