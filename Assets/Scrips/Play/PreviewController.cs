using UnityEngine;
using System.Collections;

public class PreviewController : MonoBehaviour {

    public BrickController[] bricks;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Create(int type, Color color)
    {
        Clean();

        switch (type)
        {
            case 0:
                bricks[1].setShow(true, color);
                bricks[2].setShow(true, color);
                bricks[5].setShow(true, color);
                bricks[6].setShow(true, color);

                break;

            case 1:
                bricks[0].setShow(true, color);
                bricks[1].setShow(true, color);
                bricks[2].setShow(true, color);
                bricks[3].setShow(true, color);

                break;

            case 2:
                bricks[1].setShow(true, color);
                bricks[4].setShow(true, color);
                bricks[5].setShow(true, color);
                bricks[6].setShow(true, color);

                break;

            case 3:
                bricks[0].setShow(true, color);
                bricks[1].setShow(true, color);
                bricks[2].setShow(true, color);
                bricks[6].setShow(true, color);

                break;

            case 4:
                bricks[2].setShow(true, color);
                bricks[4].setShow(true, color);
                bricks[5].setShow(true, color);
                bricks[6].setShow(true, color);

                break;

            case 5:
                bricks[0].setShow(true, color);
                bricks[1].setShow(true, color);
                bricks[5].setShow(true, color);
                bricks[6].setShow(true, color);

                break;

            case 6:
                bricks[4].setShow(true, color);
                bricks[5].setShow(true, color);
                bricks[1].setShow(true, color);
                bricks[2].setShow(true, color);

                break;
        }
    }

    public void Create(int type, Sprite sprite)
    {
        Clean();

        switch (type)
        {
            case 0:
                bricks[1].setShow(true, sprite);
                bricks[2].setShow(true, sprite);
                bricks[5].setShow(true, sprite);
                bricks[6].setShow(true, sprite);

                break;

            case 1:
                bricks[0].setShow(true, sprite);
                bricks[1].setShow(true, sprite);
                bricks[2].setShow(true, sprite);
                bricks[3].setShow(true, sprite);

                break;

            case 2:
                bricks[1].setShow(true, sprite);
                bricks[4].setShow(true, sprite);
                bricks[5].setShow(true, sprite);
                bricks[6].setShow(true, sprite);

                break;

            case 3:
                bricks[0].setShow(true, sprite);
                bricks[1].setShow(true, sprite);
                bricks[2].setShow(true, sprite);
                bricks[6].setShow(true, sprite);

                break;

            case 4:
                bricks[2].setShow(true, sprite);
                bricks[4].setShow(true, sprite);
                bricks[5].setShow(true, sprite);
                bricks[6].setShow(true, sprite);

                break;

            case 5:
                bricks[0].setShow(true, sprite);
                bricks[1].setShow(true, sprite);
                bricks[5].setShow(true, sprite);
                bricks[6].setShow(true, sprite);

                break;

            case 6:
                bricks[4].setShow(true, sprite);
                bricks[5].setShow(true, sprite);
                bricks[1].setShow(true, sprite);
                bricks[2].setShow(true, sprite);

                break;
        }
    }

    void Clean()
    {
        for(int i = bricks.Length - 1; i >= 0; i--)
        {
            bricks[i].setShow(false);
        }
    }
}
