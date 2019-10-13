using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;

public enum Move
{
    left, right, down
}

public class GameController : BaseController {

    public static GameController current;

    public AdsController adsController;
    public GameObject Plane;
    public GameObject Brick;
    public PreviewController Preview;
    public GameObject SnareVertical, DividerVertical;
    public Text Score, HighScore, TimeLife;

    BrickController[,] bricks;
    List<BrickController> currentBricks = new List<BrickController>();

    public int row, column;

    float timeLife = 0, oldTimeDown = 0, oldTimeKeyLeft = 0, oldTimeKeyRight = 0, oldTimeKeyDown = 0;
    float oldTimeMoveUp = 0;
    public float deltaTimeMoveUp = 10;

    public float deltaTimeClear = 0.03f;
    public float deltaTimeDown = 0.5f;
    public float deltaTimeKeyHorizontal = 0.1f;
    public float deltaTimeKeyDown = 0.05f;
    public bool left, right, down;

    public bool eating = false;
    List<List<int>> listEatingRows = new List<List<int>>();
    public bool running = true;

    int type, newType;

    public Color[] colors;
    public Color color, newColor;

    public bool usingSprite;
    public Sprite[] sprites;
    public Sprite sprite, newSprite;

    int score, highScore;

    void Awake()
    {
        current = this;
    }

	// Use this for initialization
	void Start () {
        StartGame();
    }

    // Update is called once per frame
    void Update () {
        if (!running) return;

        timeLife += Time.deltaTime;
        TimeLife.text = ((int)timeLife).ToString();
        deltaTimeDown = 0.5f - timeLife / 1000;
        deltaTimeKeyHorizontal = 0.1f - timeLife / 9000;

        left = !right && (Input.GetKey("left") || Input.GetKey(KeyCode.A));
        right = !left && (Input.GetKey("right") || Input.GetKey(KeyCode.D));
        down = Input.GetKey("down") || Input.GetKey(KeyCode.S);

        if (Input.GetKeyDown(KeyCode.Space)) rotateCurrentBricks();

        if (left)
        {
            if (timeLife - oldTimeKeyLeft >= deltaTimeKeyHorizontal || (down && timeLife - oldTimeKeyLeft >= deltaTimeKeyDown))
            {
                oldTimeKeyLeft = timeLife;
                moveCurrentBricks(Move.left);
            }
        }

        if (right)
        {
            if (timeLife - oldTimeKeyRight >= deltaTimeKeyHorizontal || (down && timeLife - oldTimeKeyRight >= deltaTimeKeyDown))
            {
                oldTimeKeyRight = timeLife;
                moveCurrentBricks(Move.right);
            }
        }

        if (down)
        {
            if (timeLife - oldTimeKeyDown >= deltaTimeKeyDown)
            {
                oldTimeDown = timeLife;
                oldTimeKeyDown = timeLife;
                moveCurrentBricks(Move.down);
            }
        }
        else if(timeLife - oldTimeDown >= deltaTimeDown)
        {
            oldTimeDown = timeLife;
            moveCurrentBricks(Move.down);
        }

        if(timeLife - oldTimeMoveUp >= deltaTimeMoveUp)
        {
            oldTimeMoveUp = timeLife;
            moveUp();
        }
        


    }

    void addCurrentBricks()
    {
        currentBricks.Clear();
        type = newType;
        color = newColor;
        sprite = newSprite;
        switch (type)
        {
            case 0:
                currentBricks.Add(bricks[2, 4]);
                currentBricks.Add(bricks[2, 5]);
                currentBricks.Add(bricks[3, 4]);
                currentBricks.Add(bricks[3, 5]);

                break;

            case 1:
                currentBricks.Add(bricks[0, 4]);
                currentBricks.Add(bricks[1, 4]);
                currentBricks.Add(bricks[2, 4]);
                currentBricks.Add(bricks[3, 4]);

                break;

            case 2:
                currentBricks.Add(bricks[1, 5]);
                currentBricks.Add(bricks[2, 5]);
                currentBricks.Add(bricks[3, 5]);
                currentBricks.Add(bricks[2, 4]);

                break;

            case 3:
                currentBricks.Add(bricks[1, 4]);
                currentBricks.Add(bricks[2, 4]);
                currentBricks.Add(bricks[3, 4]);
                currentBricks.Add(bricks[3, 5]);

                break;

            case 4:
                currentBricks.Add(bricks[1, 5]);
                currentBricks.Add(bricks[2, 5]);
                currentBricks.Add(bricks[3, 5]);
                currentBricks.Add(bricks[3, 4]);

                break;

            case 5:
                currentBricks.Add(bricks[1, 4]);
                currentBricks.Add(bricks[2, 4]);
                currentBricks.Add(bricks[2, 5]);
                currentBricks.Add(bricks[3, 5]);

                break;

            case 6:
                currentBricks.Add(bricks[3, 4]);
                currentBricks.Add(bricks[2, 4]);
                currentBricks.Add(bricks[2, 5]);
                currentBricks.Add(bricks[1, 5]);

                break;
        }

        showCurrentBricks(true);
        createPreview();
    }

    void createPreview()
    {
        newType = Random.Range(0, 7);
        if (!usingSprite) {
            newColor = colors[newType];
            Preview.Create(newType, newColor);
        }
        else
        {
            newSprite = sprites[newType];
            Preview.Create(newType, newSprite);
        }
    }

    void createBricks()
    {
        RectTransform PlaneRect = Plane.GetComponent<RectTransform>();
        GridLayoutGroup gridLayoutGroup = Plane.GetComponent<GridLayoutGroup>();
        //RectTransform BrickRect = Brick.GetComponent<RectTransform>();

        row = (int) (PlaneRect.rect.height / (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y));
        column = (int)(PlaneRect.rect.width / (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x));

        bricks = new BrickController[row, column];

        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < column; j++)
            {
                GameObject NewBrick = createBrick(Plane);
                BrickController brickController = NewBrick.GetComponent<BrickController>();
                brickController.i = i;
                brickController.j = j;
                //brickController.setShow(false, Color.white);

                bricks[i, j] = brickController;
            }

            //createDividerVertical(SnareVertical);
        }

    }

    GameObject createBrick(GameObject parent)
    {
        GameObject clone = Instantiate(Brick) as GameObject;
        clone.transform.SetParent(parent.transform);
        clone.transform.localScale = Vector3.one;
        clone.GetComponent<BrickController>().gameController = this;

        return clone;
    }

    GameObject createDividerVertical(GameObject parent)
    {
        GameObject clone = Instantiate(DividerVertical) as GameObject;
        clone.transform.SetParent(parent.transform);
        clone.transform.localScale = Vector3.one;

        return clone;
    }

    public bool moveCurrentBricks(Move move)
    {
        List<BrickController> NewBricks = getNewBricks(move);
        if (NewBricks == null)
        {
            if(move == Move.down)
            {
                if(!checkEat() && !eating) if(checkEndGame())
                    {
                        running = false;
                        return false;
                    }
                                       
                addCurrentBricks();
            }
            return false;
        }
        showCurrentBricks(false);
        currentBricks.Clear();
        currentBricks.AddRange(NewBricks);
        showCurrentBricks(true);

        return true;
    }

    List<BrickController> getNewBricks(Move move)
    {
        List<BrickController> NewBricks = new List<BrickController>();

        switch (move)
        {
            case Move.left:
                foreach (BrickController brickController in currentBricks)
                {
                    if (brickController.j - 1 < 0) return null;

                    BrickController NewBrick = bricks[brickController.i, brickController.j - 1];
                    if (!checkNewBrick(NewBrick)) return null;

                    NewBricks.Add(NewBrick);
                }

                break;

            case Move.right:
                foreach (BrickController brickController in currentBricks)
                {
                    if (brickController.j + 1 >= column) return null;

                    BrickController NewBrick = bricks[brickController.i, brickController.j + 1];
                    if (!checkNewBrick(NewBrick)) return null;

                    NewBricks.Add(NewBrick);
                }

                break;

            case Move.down:
                foreach (BrickController brickController in currentBricks)
                {
                    if (brickController.i + 1 >= row) return null;

                    BrickController NewBrick = bricks[brickController.i + 1, brickController.j];
                    if (!checkNewBrick(NewBrick)) return null;

                    NewBricks.Add(NewBrick);
                }

                break;
        }

        return NewBricks;
    }

    public void rotateCurrentBricks()
    {
        List<BrickController> NewBricks = getNewBricks();
        if (NewBricks == null) return;
        
        showCurrentBricks(false);
        currentBricks.Clear();
        currentBricks.AddRange(NewBricks);
        showCurrentBricks(true);

    }

    List<BrickController> getNewBricks()
    {
        if (type == 0) return null;

        List<BrickController> NewBricks = new List<BrickController>();

        int i0 = currentBricks[1].i;
        int j0 = currentBricks[1].j;

        for(int i = 0; i < 4; i++)
        {
            int i1 = currentBricks[i].i;
            int j1 = currentBricks[i].j;

            int i2 = i0 + (j1 - j0);
            int j2 = j0 - (i1 - i0);

            if (i2 < 0 || i2 >= row) return null;
            if (j2 < 0)
            {
                if(moveCurrentBricks(Move.right)) rotateCurrentBricks();
                return null;
            }
            if (j2 >= column)
            {
                if(moveCurrentBricks(Move.left)) rotateCurrentBricks();
                return null;
            }

            if (!checkNewBrick(bricks[i2, j2])) return null;
            NewBricks.Add(bricks[i2, j2]);
        }

        return NewBricks;
    }

    float getPivotI()
    {
        int min, max;
        min = max = currentBricks[0].i;
        foreach (BrickController brick in currentBricks)
        {
            if (brick.i > max) max = brick.i;
            if (brick.i < min) min = brick.i;
        }

        return (min + max) / 2f;
    }

    float getPivotJ()
    {
        int min, max;
        min = max = currentBricks[0].j;
        foreach (BrickController brick in currentBricks)
        {
            if (brick.j > max) max = brick.j;
            if (brick.j < min) min = brick.j;
        }

        return (min + max) / 2f;
    }

    BrickController getBrickPivot()
    {
        List<int> columns = new List<int>();
        foreach (BrickController brick in currentBricks) if (!columns.Contains(brick.j)) columns.Add(brick.j);
        List<int> rows = new List<int>();
        foreach (BrickController brick in currentBricks) if (!rows.Contains(brick.i)) rows.Add(brick.i);
        rows.Sort();
        columns.Sort();

        return getBrickPivot(rows[(rows.Count - 1) / 2], columns[(columns.Count - 1) / 2]);
    }

    BrickController getBrickPivot(int row, int column)
    {
        foreach (BrickController brick in currentBricks) if (brick.i == row && brick.j == column) return brick;
        return currentBricks[1];
    }

    bool checkIJ(int i, int j)
    {
        return i >= 0 && i < row && j >= 0 && j < column;
    }

    bool checkNewBrick(BrickController NewBrick)
    {
        if (NewBrick.isShow && !currentBricks.Contains(NewBrick)) return false;
        return true;
    }

    bool checkEat()
    {
        List<int> rows = new List<int>();
        foreach (BrickController brick in currentBricks) if(!rows.Contains(brick.i)) rows.Add(brick.i);
        for (int i = rows.Count - 1; i >= 0; i--) if (!checkEat(rows[i])) rows.RemoveAt(i);

        if(rows.Count > 0)
        {
            eating = true;
            rows.Sort();
            rows.Reverse();
            listEatingRows.Add(rows);
            StartCoroutine(moveDown(rows, null, false));
            return true;
        }
        return false;
    }

    bool checkEat(int row)
    {
        for (int j = 0; j < column; j++) if (!bricks[row, j].isShow) return false;
        return true;
    }

    bool checkEndGame()
    {
        foreach (BrickController brick in currentBricks) if (brick.i < 4) return true;
        return false;
    }

    IEnumerator moveDown(List<int> rows, Action action, bool runWhenNotRunning)
    {
        yield return new WaitForSeconds(deltaTimeClear);

        // Clear Row
        for (int j = 0; j < column; j++)
        {
            if (!running && !runWhenNotRunning) yield break;
            BrickController brick = bricks[rows[0], j];
            if (brick.isShow)
            {
                brick.setShow(false);
                yield return new WaitForSeconds(deltaTimeClear);
            }
        }

        // Copy Brick
        for (int i = rows[0] - 1; i >= 0; i--)
            for(int j = 0; j < column; j++)
            {
                BrickController brick = bricks[i, j];

                if (brick.isShow && !currentBricks.Contains(brick))
                {
                    brick.setShow(false);
                    if(!usingSprite) bricks[i + 1, j].setShow(true, brick.color);
                    else bricks[i + 1, j].setShow(true, brick.sprite);
                }
            }    

        // Change row eat
        int rowEat = rows[0];
        rows.RemoveAt(0);
        foreach (List<int> eatingRows in listEatingRows)
            for (int i = eatingRows.Count - 1; i >= 0; i--)
                if (eatingRows[i] < rowEat) eatingRows[i]++;
                else break;

        // Score
        if (running) addScore(1);

        if (rows.Count > 0) StartCoroutine(moveDown(rows, action, runWhenNotRunning));
        else
        {
            listEatingRows.Remove(rows);
            if(listEatingRows.Count == 0) eating = false;
            if (action != null) action();
        }
    }

    public void moveUp()
    {
        if (getNewBricks(Move.down) == null) moveCurrentBricks(Move.down);

        // Change row eat
        if (listEatingRows.Count > 0)
            foreach(List<int> eatingRows in listEatingRows)
                for (int i = eatingRows.Count - 1; i >= 0; i--) eatingRows[i]--;

        // Copy Brick
        for (int i = 1; i < row; i++)
            for (int j = 0; j < column; j++)
            {
                BrickController brick = bricks[i, j];
                if (brick.isShow && !currentBricks.Contains(brick))
                {
                    brick.setShow(false);
                    if(!usingSprite) bricks[i - 1, j].setShow(true, brick.color);
                    else bricks[i - 1, j].setShow(true, brick.sprite);
                }
            }

        // Create Last Row
        for (int j = 0; j < column; j++)
        {
            bool isShow = Random.Range(0, 3) < 2 ? true : false;
            if (!usingSprite) bricks[row - 1, j].setShow(isShow, colors[Random.Range(0, 7)]);
            else bricks[row - 1, j].setShow(isShow, sprites[Random.Range(0, 7)]);
        }

        if (checkEat(row - 1)) bricks[row - 1, Random.Range(0, column)].setShow(false);

    }

    void showCurrentBricks(bool isShow)
    {
        foreach (BrickController brickController in currentBricks)
        {
            if (!usingSprite) brickController.setShow(isShow, color);
            else brickController.setShow(isShow, sprite);
        }
    }

    // Game Life Cycle
    public void StartGame()
    {
        createBricks();
        createPreview();
        addCurrentBricks();

        setHighScore(PlayerPrefs.GetInt("highScore"), false);
    }

    public void RestartGame()
    {
        timeLife = oldTimeDown = oldTimeKeyLeft = oldTimeKeyRight = oldTimeKeyDown = 0;
        oldTimeMoveUp = 0;

        running = false;
        currentBricks.Clear();

        List<int> rows = new List<int>();
        for (int i = row - 1; i >= 0; i--) rows.Add(i);
        listEatingRows.Clear();
        listEatingRows.Add(rows);
        StartCoroutine(moveDown(rows, () =>
        {
            addCurrentBricks();
            setScore(0);
            running = true;
            //adsController.ShowVideosAds();
        }, true));

        // Test
        //eating = true;
        //deltaTimeClear = 0.5f;

        //List<int> rows1 = new List<int>();
        ///*for (int i = row - 1; i >= 17; i--)*/ rows1.Add(row - 2); rows1.Add(row - 4);
        //listEatingRows.Add(rows1);
        //StartCoroutine(moveDown(rows1, null));

        //List<int> rows2 = new List<int>();
        ///*for (int i = 15; i >= 0; i--)*/ rows2.Add(row - 2);
        //listEatingRows.Add(rows2);
        //StartCoroutine(Delay(1f, () => { StartCoroutine(moveDown(rows2, null)); }));

        ////StartCoroutine(Delay(1f, moveUp));
    }

    public void PauseGame()
    {
        running = false;
    }

    public void ResumeGame()
    {
        running = true;
    }

    // Game Info
    void addScore(int add)
    {
        setScore(score + add);
    }

    void setScore(int score)
    {
        this.score = score;
        Score.text = score.ToString();

        if (score > highScore) setHighScore(score, true);
    }

    void setHighScore(int highScore, bool save)
    {
        this.highScore = highScore;
        HighScore.text = highScore.ToString();
        if (save) PlayerPrefs.SetInt("highScore", highScore);
    }

    // Game Control
    public void GoLeft(bool isLeft)
    {
        left = isLeft;
    }

    public void GoRight(bool isRight)
    {
        right = isRight;
    }

    public void GoDown(bool isDown)
    {
        down = isDown;
    }
}
