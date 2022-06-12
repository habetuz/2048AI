using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : GameController
{
    public override void AnimationFinished()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Slide(Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slide(Direction.Down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Slide(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Slide(Direction.Right);
        }
    }
}
