using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class GameController : MonoBehaviour
{
    private readonly uint[] grid = new uint[16];

    private uint score;

    public uint Score
    {
        get { return score; }
    }
    
    public uint[] Grid
    {
        get { return grid; }
    }

    public Action<Direction> OnSlide;
    public Action<uint> OnScoreUpdate;

    public void StartGame()
    {
        GenerateTile();
        GenerateTile();
    }

    public abstract void AnimationFinished();

    private void GenerateTile()
    {
        uint value = (uint)(Random.Range(0f, 1f) < 0.9 ? 2 : 4);
        while (true)
        {
            byte index = (byte)Random.Range(0, 16);

            if (grid[index] == 0)
            {
                grid[index] = value;
                break;
            }
        }
    }

    protected bool Slide(Direction direction)
    {
        bool changed = false;
        switch (direction)
        {
            case Direction.Left:
                changed = SlideLeft();
                break;
            case Direction.Right:
                changed = SlideRight();
                break;
            case Direction.Up:
                changed = SlideUp();
                break;
            case Direction.Down:
                changed = SlideDown();
                break;
        }

        if (!changed)
        {
            return false;
        }

        GenerateTile();

        OnSlide?.Invoke(direction);
        OnScoreUpdate?.Invoke(Score);

        return true;
    }

    private bool SlideUp()
    {
        bool changed = false;
        for (sbyte i = 0; i <= 12; i++)
        {
            bool recheck;

            do
            {
                recheck = false;
                for (sbyte j = (sbyte)(i + 4); j < grid.Length; j += 4)
                {
                    if (grid[j] != 0)
                    {
                        if (grid[i] == 0 || grid[i] == grid[j])
                        {
                            recheck = grid[i] == 0;
                            score = grid[i] == grid[j] ? score + grid[i] * 2 : score;
                            grid[i] += grid[j];
                            grid[j] = 0;
                            changed = true;
                        }

                        break;
                    }
                }
            }
            while (recheck);
            
        }
        
        return changed;
    }

    private bool SlideDown()
    {
        bool changed = false;
        for (sbyte i = 15; i > 3; i--)
        {
            bool recheck;

            do
            {
                recheck = false;
                for (sbyte j = (sbyte)(i - 4); j >= 0; j -= 4)
                {
                    if (grid[j] != 0)
                    {
                        if (grid[i] == 0 || grid[i] == grid[j])
                        {
                            recheck = grid[i] == 0;
                            score = grid[i] == grid[j] ? score + grid[i] * 2 : score;
                            grid[i] += grid[j];
                            grid[j] = 0;
                            changed = true;
                        }

                        break;
                    }
                }
            }
            while (recheck);
        }

        return changed;
    }

    private bool SlideLeft()
    {
        bool changed = false;
        for (sbyte i = 0; i < 18; i += 4)
        {
            if (i > 15)
            {
                i = (sbyte)(i - 15);
            }

            bool recheck;

            do
            {
                recheck = false;
                for (sbyte j = (sbyte)(i + 1); j < i + (4 - (i % 4)); j++)
                {
                    if (grid[j] != 0)
                    {
                        if (grid[i] == 0 || grid[i] == grid[j])
                        {
                            recheck = grid[i] == 0;
                            score = grid[i] == grid[j] ? score + grid[i] * 2 : score;
                            grid[i] += grid[j];
                            grid[j] = 0;
                            changed = true;
                        }

                        break;
                    }
                }
            }
            while (recheck);
        }

        return changed;
    }

    private bool SlideRight()
    {
        bool changed = false;
        for (sbyte i = 15; i > -3; i -= 4)
        {
            if (i < 0)
            {
                i = (sbyte)(15 + i);
            }

            bool recheck;

            do
            {
                recheck = false;
                for (int j = i - 1; j >= i - (i % 4); j--)
                {
                    if (grid[j] != 0)
                    {
                        if (grid[i] == 0 || grid[i] == grid[j])
                        {
                            recheck = grid[i] == 0;
                            score = grid[i] == grid[j] ? score + grid[i] * 2 : score;
                            grid[i] += grid[j];
                            grid[j] = 0;
                            changed = true;
                        }

                        break;
                    }
                }
            }
            while (recheck);
        }

        return changed;
    }
}
