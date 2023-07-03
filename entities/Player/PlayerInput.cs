using Godot;

public partial class PlayerInput : Node
{
	[Export] public Vector3 Direction = Vector3.Zero;

	private const int cameraAngle = 45;
	private const string up = "up";
	private const string down = "down";
	private const string left = "left";
	private const string right = "right";
	private const string jump = "jump";

	public bool isJumping;

	public override void _PhysicsProcess(double delta)
	{
		ResetInnerState();

		if (Input.IsActionJustPressed(jump))
		{
			isJumping = true;
		}
	}

	public Vector3 GetDirection(Basis playerDirectionBasis)
	{
		Vector3 controllerDir = new(
			GetControllerDirection().X,
			0,
			GetControllerDirection().Y
		);

		return (playerDirectionBasis * controllerDir)
			.Rotated(Vector3.Up, Mathf.DegToRad(cameraAngle))
			.Normalized();
	}

	private void ResetInnerState()
	{
		isJumping = false;
	}

	private Vector2 GetControllerDirection()
	{
		return Input.GetVector(
			left,
			right,
			up,
			down
		);
	}
}
