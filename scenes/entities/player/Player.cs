using Godot;
using System;

public partial class Player : CharacterBody2D
{
    //basic player speed
    [Export] public int Speed { get; set; } = 100;

    // seperated out value to add/sub from velocity values for easy changing
    [Export] public int VelPreMult { get; set; } = 1;

    // knockback timer value
    private double KnockbackTimeLeft
    {
        get
        {
            Timer timer = GetNode<Timer>("KnockbackTimer");
            return timer.TimeLeft;
        }
    }

    private float KnockbackAngle { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // resets velocity to zero every frame
        Vector2 velocity = Vector2.Zero;

        if (KnockbackTimeLeft > 0)
        {
            velocity = Vector2.FromAngle(KnockbackAngle);
        }
        else
        {
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
        }

        // normalize speed so player will move same distance when moving diagonal compared to straight cardinals
        velocity = velocity.Normalized() * Speed;

        Position += velocity * (float)delta;

        // gets animated sprite
        // hey web devs! you know query selector?
        AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        // if player is moving:
        if (velocity.Length() > 0)
        {

            // calc what direction player is moving
            // and set animation mode to walk in that direction
            switch ((double)(Mathf.RadToDeg(velocity.Angle())))
            {
                case > -67.5 and < 67.5:
                    // 0 deg - right
                    if (animatedSprite2D.Animation != "walk_right" && animatedSprite2D.FlipH != false)
                    {
                        animatedSprite2D.Stop();
                    }
                    animatedSprite2D.Animation = "walk_right";
                    animatedSprite2D.FlipH = false;
                    break;
                case (< -112.5) or (> 112.5):
                    // 180 deg - left
                    if (animatedSprite2D.Animation != "walk_right" && animatedSprite2D.FlipH != true)
                    {
                        animatedSprite2D.Stop();
                    }
                    animatedSprite2D.Animation = "walk_right";
                    animatedSprite2D.FlipH = true;
                    break;
                case > 67.5 and < 112.5:
                    // 90 - down
                    if (animatedSprite2D.Animation != "walk_down")
                    {
                        animatedSprite2D.Stop();
                    }
                    animatedSprite2D.Animation = "walk_down";
                    animatedSprite2D.FlipH = false;
                    break;
                case < -67.5 and > -112.5:
                    // 270 - up
                    if (animatedSprite2D.Animation != "walk_up")
                    {
                        animatedSprite2D.Stop();
                    }
                    animatedSprite2D.Animation = "walk_up";
                    animatedSprite2D.FlipH = false;
                    break;
            }

            // then play animation (player is moving after all!)
            animatedSprite2D.Play();
        }

        // resetting anim when velocity <=0
        else
        {
            animatedSprite2D.Stop();
        }
    }

    private void OnPlayerHitArea2dBodyEntered(Node2D body)
    {
        if (body.IsInGroup("enemy"))
        {
            Timer knockbackTimer = GetNode<Timer>("KnockbackTimer");
            Vector2 vectorToHitEnemy=new Vector2(body.Position.X-Position.X, body.Position.Y-Position.Y);
            KnockbackAngle = (-vectorToHitEnemy).Angle();
            knockbackTimer.Start();
        }
    }
}
