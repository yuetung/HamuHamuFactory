using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProduceController : MonoBehaviour
{
    public List<float> workstationPositions; // start & end of each workstation, in ascending order
    public Sprite[] produceSprites; // (n+1) sprites for n workstations
    public float startPos_x = 0f;
    public float endPos_x = 100f;
    public float productionTime = 10; // in seconds
    public int goldGained = 0;
    public bool move = false;
    private Animator _animator;
    private int pointer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            transform.Translate(Vector3.right * Time.deltaTime * (endPos_x-startPos_x)/productionTime);
        }
        if (transform.position.x >= endPos_x)
        {
            StopMoving();
        }
        else if (pointer<workstationPositions.Count && transform.position.x >= workstationPositions[pointer])
        {
            _animator.SetTrigger("next");
            if (pointer % 2 == 1)
            {
                gameObject.GetComponent<Image>().overrideSprite = produceSprites[(pointer + 1) / 2];
            }
            else
            {
                gameObject.GetComponent<Image>().overrideSprite = null;
            }
            pointer++;
        }
    }

    public void Initiate(List<float> workstationPositions, Sprite[] produceSprites, float endPos_x, float productionTime, int goldGained)
    {
        startPos_x = transform.position.x;
        this.endPos_x = endPos_x;
        this.workstationPositions = workstationPositions;
        this.produceSprites = produceSprites;
        this.productionTime = productionTime;
        this.goldGained = goldGained;
        _animator = gameObject.GetComponent<Animator>();
        gameObject.GetComponent<Image>().overrideSprite = produceSprites[0];
    }

    public void StartMoving()
    {
        move = true;
    }

    public void StopMoving()
    {
        move = false;
        _animator.SetTrigger("disappear");
    }

    public void Done()
    {
        GameManager.instance.GainMoney(goldGained);
        GameManager.instance.GainExp(goldGained/2);
        // TODO: do some fancy teleport thingy here
        Destroy(gameObject);
    }
}
