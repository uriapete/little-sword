using Godot;
using System;

public partial class PlayerArea : Area2D
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

        // gets animated sprite
        // hey web devs! you know query selector?
        AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        //moved velocity to top-level velocity - temp. commented lines out
        //will be replaced by signal fns

        //// if player is moving:
        //if (velocity.Length() > 0)
        //{

        //    // calc what direction player is moving
        //    // and set animation mode to walk in that direction
        //    switch ((double)(velocity.Angle() * 180 / MathF.PI))
        //    {
        //        case >-67.5 and <67.5:
        //            // 0 deg - right
        //            if (animatedSprite2D.Animation != "walk_right" && animatedSprite2D.FlipH != false)
        //            {
        //                animatedSprite2D.Stop();
        //            }
        //            animatedSprite2D.Animation = "walk_right";
        //            animatedSprite2D.FlipH = false;
        //            break;
        //        case (<-112.5)or(>112.5):
        //            // 180 deg - left
        //            if (animatedSprite2D.Animation != "walk_right" && animatedSprite2D.FlipH != true)
        //            {
        //                animatedSprite2D.Stop();
        //            }
        //            animatedSprite2D.Animation = "walk_right";
        //            animatedSprite2D.FlipH = true;
        //            break;
        //        case >67.5and<112.5:
        //            // 90 - down
        //            if (animatedSprite2D.Animation != "walk_down")
        //            {
        //                animatedSprite2D.Stop();
        //            }
        //            animatedSprite2D.Animation = "walk_down";
        //            animatedSprite2D.FlipH = false;
        //            break;
        //        case <-67.5and>-112.5:
        //            // 270 - up
        //            if (animatedSprite2D.Animation != "walk_up")
        //            {
        //                animatedSprite2D.Stop();
        //            }
        //            animatedSprite2D.Animation = "walk_up";
        //            animatedSprite2D.FlipH = false;
        //            break;
        //    }

        //    // then play animation (player is moving after all!)
        //    animatedSprite2D.Play();
        //}

        //// resetting anim when velocity <=0
        //else
        //{
        //    animatedSprite2D.Stop();
        //}

    }

    public void OnBodyEntered()
    {

    }
}
