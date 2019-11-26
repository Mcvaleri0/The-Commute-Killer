using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recticle : MonoBehaviour {

    private RectTransform recticle;
    private Transform circle;
    private Transform point;
    private Transform grab;

    int state = 0;
    Transform _current;

    [Range(10f, 250f)]
    public float size = 20f;

    private SelectionManager selectionManager;
    private Transform selected;

    private void Start() {

        recticle = GetComponent<RectTransform>();
        circle = this.transform.Find("Circle");
        point  = this.transform.Find("Point");
        grab   = this.transform.Find("Grab");
        _current = circle;

        selectionManager = GameObject.Find("Selection Manager").GetComponent<SelectionManager>();
    }

    private void switchRecticle(Transform newRect) {
        _current.gameObject.SetActive(false);
        newRect.gameObject.SetActive(true);
        _current = newRect;
    }

    private void Update() {

        _current.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
        selected = selectionManager.getSelection();

        switch (state)
        {
            case 0://Normal
                if(selected != null) {
                    switchRecticle(point);
                    state = 1;
                }
            break;

            case 1://Point
                if (selected == null)
                {
                    switchRecticle(circle);
                    state = 0;
                }
                if (Input.GetMouseButtonDown(0)) //if mb1 is clicked
                {
                    switchRecticle(grab);
                    state = 2;
                }
            break;

            case 2://Grab
                if (Input.GetMouseButtonUp(0)) //if mb1 is vo longer down
                {
                    switchRecticle(circle);
                    state = 0;
                }
            break;
        }
    }
}

