using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

public class PlaneController : BaseController
{

    public GameController gameController;
    public int deltaHorizontal = 55;
    public int deltaVertical = 30;

    Vector2 oldPoint;
    bool isDrag;

    bool longDown;
    IEnumerator LongDown;

    bool isHorizontal;
    bool isLockTrend;
    IEnumerator OpenTrend;

	// Use this for initialization
	void Start () {
        LongDown = Delay(0f, null);
        OpenTrend = Delay(0f, null);

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void onPointerDown(BaseEventData baseEventData)
    {
        if (!gameController.running) return;

        PointerEventData eventData = baseEventData as PointerEventData;
        oldPoint = getLocalPointPosition(GetComponent<RectTransform>(), eventData);

        isDrag = false;
        isLockTrend = false;

        longDown = false;
        StopCoroutine(LongDown);
        LongDown = Delay(0.3f, () => { longDown = true; });
        StartCoroutine(LongDown);
    }

    public void onDrag(BaseEventData baseEventData)
    {
        if (!gameController.running) return;
        isDrag = true;

        //StopCoroutine(OpenTrend);
        //OpenTrend = Delay(1f, () => { isLockTrend = false; });
        //StartCoroutine(OpenTrend);

        PointerEventData eventData = baseEventData as PointerEventData;
        Vector2 newPoint = getLocalPointPosition(GetComponent<RectTransform>(), eventData);

        if (!isLockTrend)
        {
            isHorizontal = Mathf.Abs(newPoint.x - oldPoint.x) > Mathf.Abs(newPoint.y - oldPoint.y) ? true : false;
            isLockTrend = true;
        }

        if (/*isHorizontal && */Mathf.Abs(newPoint.x - oldPoint.x) > (isHorizontal ? deltaHorizontal : deltaHorizontal * 3))
        {
            isHorizontal = true;
            if (newPoint.x - oldPoint.x > 0) gameController.moveCurrentBricks(Move.right);
            else gameController.moveCurrentBricks(Move.left);
            oldPoint.x = newPoint.x;
        }
        if(/*!isHorizontal && */newPoint.y - oldPoint.y < (!isHorizontal ? -deltaVertical : -deltaVertical * 3))
        {
            isHorizontal = false;
            gameController.moveCurrentBricks(Move.down);
            oldPoint.y = newPoint.y;
        }

    }

    public void onPointerUp(BaseEventData baseEventData)
    {
        if (!gameController.running) return;
        if (!isDrag && !longDown) gameController.rotateCurrentBricks();
    }

    public Vector2 getLocalPointPosition(RectTransform rectTransform, PointerEventData eventData)
    {
        Vector2 localPointPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPointPosition);
        return localPointPosition;
    }

}
