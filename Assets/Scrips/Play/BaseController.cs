using UnityEngine;
using System.Collections;

public class BaseController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator Delay(float t, Action action)
    {
        yield return new WaitForSeconds(t);
        action();
    }

    public delegate void Action();
}
