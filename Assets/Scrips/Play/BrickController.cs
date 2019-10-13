using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class BrickController : MonoBehaviour {

    public GameController gameController;

    public int i, j;

    public bool isShow;
    public Color color;
    public Sprite sprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setShow(bool isShow, Color color)
    {
        setShow(isShow);

        if (isShow)
        {
            this.color = color;
            GetComponent<Image>().color = color;
        }
        //else GetComponent<Image>().color = Color.white;
    }

    public void setShow(bool isShow, Sprite sprite)
    {
        setShow(isShow);

        if (isShow)
        {
            this.sprite = sprite;
            GetComponent<Image>().sprite = sprite;
        }
    }

    public void setShow(bool isShow)
    {
        this.isShow = isShow;
        GetComponent<Image>().enabled = isShow;
    }
}
