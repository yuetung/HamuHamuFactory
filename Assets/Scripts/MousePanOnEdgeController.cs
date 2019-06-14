using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MousePanOnEdgeController : MonoBehaviour {

    [SerializeField]
    private bool isUseRatio = true;

    [SerializeField]
    private Vector2 boundries = new Vector2(0.1f, 0.1f);

    [SerializeField]
    private Vector2 speed = new Vector2(10f, 10f);

    private void Start()
    {
        RectTransform rT = this.GetComponent<RectTransform>();
        float scaleY = Screen.height / rT.sizeDelta.y;

        rT.sizeDelta *= scaleY; // new Vector2(rT.sizeDelta.x, scaleY * rT.sizeDelta.y);

        foreach (RectTransform child in this.GetComponentsInChildren<RectTransform>())
        {
            if (rT == child) continue;
            child.position  *= scaleY;
            child.sizeDelta *= scaleY;
        }
    }

    // Update is called once per frame
    void Update () {

        Vector2 boundry = this.boundries;

        if (isUseRatio) boundry = Vector2.Scale( boundry, new Vector2(Screen.width, Screen.height) );
        
        float dx = 0, dy = 0;

        if (Input.mousePosition.x > Screen.width - boundry.x) dx = -this.speed.x;
        if (Input.mousePosition.x < boundry.x) dx = this.speed.x;
        if (Input.mousePosition.y > Screen.height - boundry.y) dy = -this.speed.y;
        if (Input.mousePosition.y < boundry.y) dy = this.speed.y;

        RectTransform rect = this.GetComponent<RectTransform>();
        float minX = Screen.width - rect.sizeDelta.x / 2;
        float minY = Screen.height - rect.sizeDelta.y / 2;

        float x = Mathf.Clamp(this.transform.position.x + dx, minX, Screen.width - minX);
        float y = Mathf.Clamp(this.transform.position.y + dy, minY, Screen.height - minY);

        this.transform.position = new Vector3(x, y);
    }
}
