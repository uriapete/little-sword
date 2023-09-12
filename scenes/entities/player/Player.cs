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
        // resets velocity to zero every frame
        Vector2 velocity =Vector2.Zero;

        // seperated out value to add/sub from velocity values for easy changing
        // might consider making this an export var to be able to tune straight from godot instead
        int velPreMult = 1;

        // gets animated sprite
        // hey web devs! you know query selector?
        AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // most of these should be pretty self explanatory
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



        // normalizes speed when velocity>0 so player will move same distance when moving diagonal as opposed to straight cardinals
        if (velocity.Length() > 0)
        {
            velocity=velocity.Normalized()*Speed;
        }
        // resetting anim when velocity <=0
        else
        {
            animatedSprite2D.Stop();
        }

        Position += velocity*(float)delta;

    }
}
