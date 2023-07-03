using Godot;

public partial class Player : CharacterBody3D
{
	[Export] public PlayerInput input { get; set; }

	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private Vector3 updVelocity = Vector3.Zero;

	public override void _PhysicsProcess(double delta)
	{
		updVelocity = Vector3.Zero;

		UpdateVelocityByGravity((float)delta);
		UpdateVelocityByInput();
		UpdateJumpByInput();

		Velocity = updVelocity;
		MoveAndSlide();
	}

	private void UpdateVelocityByGravity(float delta)
	{
		if (!IsOnFloor())
		{
			updVelocity.Y -= gravity * delta;
		}
	}

	private void UpdateJumpByInput()
	{
		if (input.isJumping)
		{
			updVelocity.Y = JumpVelocity;
		}
	}

	private void UpdateVelocityByInput()
	{
		Vector3 direction = input.GetDirection(Transform.Basis);

		if (direction != Vector3.Zero)
		{
			updVelocity.X = direction.X * Speed;
			updVelocity.Z = direction.Z * Speed;
		}
		else
		{
			updVelocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			updVelocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
	}
}
