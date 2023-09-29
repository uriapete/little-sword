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

    private enum Directions
    {
        down = 1,
        up = -1,
        right = 2,
        left = -2
    }

    private Directions PlayerDirection { get; set; } = Directions.down;

    private enum PlayerStates
    {
        walk, knockback_to
    }

    private PlayerStates PlayerState
    {
        get
        {
            if (KnockbackTimeLeft > 0)
            {
                return PlayerStates.knockback_to;
            }
            else
            {
                return PlayerStates.walk;
            }
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    private void SetSpriteAnimation(Directions direction=(Directions)4)
    {
        if (direction != (Directions)4)
        {
            PlayerDirection = direction;
        }
        AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animatedSprite2D.Stop();
        bool facingLeft = PlayerDirection == Directions.left;
        animatedSprite2D.Animation = $"{PlayerState}_{(facingLeft ? Directions.right : PlayerDirection)}";
        animatedSprite2D.FlipH = facingLeft;
    }

    private bool SpriteAnimationEqualTo(PlayerStates state, Directions direction)
    {
        AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (direction == Directions.left)
        {
            if (!animatedSprite2D.FlipH || animatedSprite2D.Animation != $"{state}_{Directions.right}")
            {
                return false;
            }
            else return true;
        }
        if (animatedSprite2D.Animation != $"{state}_{direction}"||animatedSprite2D.FlipH)
        {
            return false;
        }
        return true;
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
                    if (!SpriteAnimationEqualTo(PlayerState, Directions.right))
                    {
                        SetSpriteAnimation(Directions.right);
                    }
                    break;
                case (< -112.5) or (> 112.5):
                    // 180 deg - left
                    if (!SpriteAnimationEqualTo(PlayerState, Directions.left))
                    {
                        SetSpriteAnimation(Directions.left);
                    }
                    break;
                case > 67.5 and < 112.5:
                    // 90 - down
                    if (!SpriteAnimationEqualTo(PlayerState, Directions.down))
                    {
                        SetSpriteAnimation(Directions.down);
                    }
                    break;
                case < -67.5 and > -112.5:
                    // 270 - up
                    if (!SpriteAnimationEqualTo(PlayerState, Directions.up))
                    {
                        SetSpriteAnimation(Directions.up);
                    }
                    break;
            }

            // then play animation (player is moving after all!)
            animatedSprite2D.Play();
        }

        // resetting anim when velocity <=0
        else
        {
            if (!SpriteAnimationEqualTo(PlayerState,PlayerDirection))
            {
                SetSpriteAnimation();
            }
            else
            {
                animatedSprite2D.Stop();
            }
        }
    }

    private void OnPlayerHitArea2dBodyEntered(Node2D body)
    {
        if (body.IsInGroup("enemy"))
        {
            Timer knockbackTimer = GetNode<Timer>("KnockbackTimer");
            Vector2 vectorToHitEnemy=new Vector2(body.Position.X-Position.X, body.Position.Y-Position.Y);
            float rawKnockbackAngle = (-vectorToHitEnemy).Angle();
            if (rawKnockbackAngle<0)
            {
                for (float i = 0, j = i - MathF.PI / 4; i > (-2 * MathF.PI); i = j, j -= MathF.PI / 4)
                {
                    if (rawKnockbackAngle < j)
                    {
                        continue;
                    }
                    double halfPt = (i + j) / 2;
                    if (rawKnockbackAngle > halfPt)
                    {
                        KnockbackAngle = i;
                    }
                    else
                    {
                        KnockbackAngle = j;
                    }
                    break;
                }
            }
            else
            {
                for (float i = 0, j = i + MathF.PI / 4; i < (2 * MathF.PI); i = j, j += MathF.PI / 4)
                {
                    if (rawKnockbackAngle > j)
                    {
                        continue;
                    }
                    double halfPt = (i + j) / 2;
                    if (rawKnockbackAngle < halfPt)
                    {
                        KnockbackAngle = i;
                    }
                    else
                    {
                        KnockbackAngle = j;
                    }
                    break;
                }
            }
            knockbackTimer.Start();
        }
    }
    private void OnKnockbackTimerTimeout()
    {
        PlayerDirection = (Directions)((int)PlayerDirection * -1);
    }
}
