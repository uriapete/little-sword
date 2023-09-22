using Godot;
using System;

public partial class Player : CharacterBody2D
{
    //basic player speed
    [Export] public int Speed { get; set; } = 100;

    // seperated out value to add/sub from velocity values for easy changing
    [Export] public int VelPreMult = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // resets velocity to zero every frame
        Vector2 velocity = Vector2.Zero;

        // most of these should be pretty self explanatory
        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y -= VelPreMult;
        }

        if (Input.IsActionPressed("move_down"))
        {
            velocity.Y += VelPreMult;
        }

        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= VelPreMult;
        }

        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += VelPreMult;
        }

        // normalize speed so player will move same distance when moving diagonal compared to straight cardinals
        velocity = velocity.Normalized() * Speed;

        Position += velocity * (float)delta;
    }
}
