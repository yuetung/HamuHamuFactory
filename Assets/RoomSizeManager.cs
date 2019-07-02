using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSizeManager : MonoBehaviour
{
    public MainCharacterController mainCharacterController;
    public Sprite start;
    public Sprite repeatable;
    public Sprite end;
    public int starting_roomSize = 8;
    public int roomSize = 8; // in units
    public int roomHeight = 1756;
    public int roomWidth = 1000;
    public int y_pos = 0;
    public float playerBoundDelta = 200; // preventing player from reaching the edge of room
    public List<GameObject> existingRooms = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        existingRooms = new List<GameObject>();
        UpdatePlayerMinMaxPositions();
        if (PlayerPrefs.HasKey("roomSize"))
        {
            roomSize = PlayerPrefs.GetInt("roomSize");
        }
        else
        {
            roomSize = starting_roomSize;
            PlayerPrefs.SetInt("roomSize", roomSize);
        }
        ConstructRooms();
        UpdatePlayerMinMaxPositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void ConstructRooms()
    {
        float currentXPos = 0;
        existingRooms.Add(CreateGrid(start, currentXPos));
        currentXPos += roomWidth;
        for (int i=0; i<roomSize-2; i++)
        {
            existingRooms.Add(CreateGrid(repeatable, currentXPos));
            currentXPos += roomWidth;
        }
        existingRooms.Add(CreateGrid(end, currentXPos));
    }

    public GameObject CreateGrid(Sprite sprite, float x_position)
    {
        GameObject go = new GameObject("BackgroundGrid");
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(roomWidth, roomHeight);
        rect.localPosition = new Vector2(x_position,y_pos);
        Image image = go.AddComponent<Image>();
        image.sprite = sprite;
        go.transform.parent = gameObject.transform;
        //go.AddComponent<CanvasRenderer>();
        return go;
    }

    public void AddRoom(int amount = 1)
    {
        roomSize += amount;
        foreach (GameObject o in existingRooms)
        {
            Destroy(o);
        }
        existingRooms = new List<GameObject>();
        ConstructRooms();
        UpdatePlayerMinMaxPositions();
        PlayerPrefs.SetInt("roomSize", roomSize);
    }

    public void UpdatePlayerMinMaxPositions()
    {
        mainCharacterController.minPos = playerBoundDelta - roomWidth/2;
        mainCharacterController.maxPos = GetTotalRoomWidth() - playerBoundDelta - roomWidth / 2;
    }

    public int GetTotalRoomWidth()
    {
        return roomSize*roomWidth;
    }
}
