using System;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] Text score;

    public GameController Controller;

    public bool AnimationsFinished = true;

    // Start is called before the first frame update
    private void Start()
    {
        Controller.StartGame();
        Controller.OnSlide += OnSlide;
        Controller.OnScoreUpdate += OnScoreUpdate;
        Draw();
    }

    private void Update()
    {
        if (AnimationsFinished) Controller.AnimationFinished();
    }

    private void LateUpdate()
    {
        if (!AnimationsFinished)
        {
            AnimationsFinished = true;
            return;
        }

        Draw();
    }

    private void Draw()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var cell = transform.GetChild(i);
            for (int j = 0; j < cell.childCount; j++)
            {
                Destroy(cell.GetChild(j).gameObject);
            }
        }

        for (int i = 0; i < 16; i++)
        {
            if (Controller.Grid[i] != 0)
            {
                var tile = Instantiate(tilePrefab, gameObject.transform.GetChild(i));
                tile.GetComponent<Tile>().UpdateValue(Controller.Grid[i]);
            }
        }
    }

    private void OnSlide(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                SlideLeft();
                break;
            case Direction.Right:
                SlideRight();
                break;
            case Direction.Up:
                SlideUp();
                break;
            case Direction.Down:
                SlideDown();
                break;
        }
    }

    private void OnScoreUpdate(uint newScore)
    {
        score.text = newScore.ToString();
    }

    private void SlideUp()
    {
        for (sbyte i = 0; i <= 12; i++)
        {
            bool recheck;

            do
            {
                recheck = false;
                for (sbyte j = (sbyte)(i + 4); j < transform.childCount; j += 4)
                {
                    if (transform.GetChild(j).childCount != 0)
                    {
                        if (transform.GetChild(i).childCount == 0 || transform.GetChild(i).GetChild(0).GetComponent<Tile>().Value == transform.GetChild(j).GetChild(0).GetComponent<Tile>().Value)
                        {
                            recheck = transform.GetChild(i).childCount == 0;
                            transform.GetChild(j).GetChild(0).SetParent(transform.GetChild(i), true);
                        }

                        break;
                    }
                }
            }
            while (recheck);

        }
    }

    private void SlideDown()
    {
        for (sbyte i = 15; i > 3; i--)
        {
            bool recheck;

            do
            {
                recheck = false;
                for (sbyte j = (sbyte)(i - 4); j >= 0; j -= 4)
                {
                    if (transform.GetChild(j).childCount != 0)
                    {
                        if (transform.GetChild(i).childCount == 0 || transform.GetChild(i).GetChild(0).GetComponent<Tile>().Value == transform.GetChild(j).GetChild(0).GetComponent<Tile>().Value)
                        {
                            recheck = transform.GetChild(i).childCount == 0;
                            transform.GetChild(j).GetChild(0).SetParent(transform.GetChild(i), true);
                        }

                        break;
                    }
                }
            }
            while (recheck);
        }
    }

    private void SlideLeft()
    {
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
                    if (transform.GetChild(j).childCount != 0)
                    {
                        if (transform.GetChild(i).childCount == 0 || transform.GetChild(i).GetChild(0).GetComponent<Tile>().Value == transform.GetChild(j).GetChild(0).GetComponent<Tile>().Value)
                        {
                            recheck = transform.GetChild(i).childCount == 0;
                            transform.GetChild(j).GetChild(0).SetParent(transform.GetChild(i), true);
                        }

                        break;
                    }
                }
            }
            while (recheck);
        }
    }

    private void SlideRight()
    {
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
                    if (transform.GetChild(j).childCount != 0)
                    {
                        if (transform.GetChild(i).childCount == 0 || transform.GetChild(i).GetChild(0).GetComponent<Tile>().Value == transform.GetChild(j).GetChild(0).GetComponent<Tile>().Value)
                        {
                            recheck = transform.GetChild(i).childCount == 0;
                            transform.GetChild(j).GetChild(0).SetParent(transform.GetChild(i), true);
                        }

                        break;
                    }
                }
            }
            while (recheck);
        }
    }
}
