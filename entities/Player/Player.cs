using Godot;
using System;

interface IPlayerStatus
{
    int WeaponId { get; set; }
    float Speed { get; set; }
    int Health { get; set; }
    int Mana { get; set; }
    float JumpVelocity { get; }
}
struct PlayerStatus : IPlayerStatus
{
    public PlayerStatus(int id, float speed, int health, int mana)
    {
        WeaponId = id;
        Speed = speed;
        Health = health;
        Mana = mana;
        JumpVelocity = 4.5f;
    }

    public int WeaponId { get; set; }
    public float Speed { get; set; }
    public int Health { get; set; }
    public int Mana { get; set; }
    public float JumpVelocity { get; }
}

public partial class Player : CharacterBody3D
{
    // Player basic status defidned here
    IPlayerStatus player = new PlayerStatus(0, 5.0f, 100, 100);

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.Y = player.JumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized().Rotated(Vector3.Up, Mathf.DegToRad(45));
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * player.Speed;
            velocity.Z = direction.Z * player.Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, player.Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, player.Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
