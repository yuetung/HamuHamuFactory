using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Animator _animator;
    private RectTransform _rect;
    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _rect = this.GetComponent<RectTransform>();
        _audio = this.GetComponent<AudioSource>();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            _animator.SetBool("isMoving", true);
            if (moveHorizontal > 0)
            {
                _rect.localScale = new Vector3(-1, 1, 1);
            }
            else if (moveHorizontal < 0)
            {
                _rect.localScale = new Vector3(1, 1, 1);
            }
            _rect.position = _rect.position + new Vector3(moveHorizontal, moveVertical) * moveSpeed;
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    public void PlaySound()
    {
        _audio.Play();
    }
}
