using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 10;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 velocity=Vector2.Zero;
		int velPreMult = 1;

        AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y -= velPreMult;
        }

        if (Input.IsActionPressed("move_down"))
        {
            velocity.Y += velPreMult;
            animatedSprite2D.Animation = "walk_down";
            animatedSprite2D.Play();
        }

        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= velPreMult;
        }

        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += velPreMult;
        }

        if (velocity.Length() > 0)
        {
            velocity=velocity.Normalized()*Speed;
        }
        else
        {
            animatedSprite2D.Animation = "idle";
            animatedSprite2D.Stop();
        }

        Position += velocity*(float)delta;

    }
}
