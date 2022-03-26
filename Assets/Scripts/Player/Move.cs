using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Move : Photon.Pun.MonoBehaviourPun
{
    public float speed = 15f;
    Vector2 velocity;
    Rigidbody2D rb;
    [HideInInspector]
    // can't move while chat
    public UIControl _uiControl;
    public Animator _animator;
    private Vector3 sourceXScale;
    private Sound walkSound;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        velocity = Vector2.zero;
        sourceXScale = transform.localScale;
        walkSound = Array.Find<Sound>(AudioManager.instance.sounds, sound => sound.name == "walk");
        walkSound.source.Stop();

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) { return; }
        if (_uiControl.IsChatWindowActive) { return; }
        velocity.x = Input.GetAxisRaw("Horizontal");
        velocity.y = Input.GetAxisRaw("Vertical");
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (!walkSound.source.isPlaying)
            {
                AudioManager.instance.Play("walk");
            }

            _animator.SetBool("Run", true);
            Vector3 vec3Buffer = transform.localScale;
            vec3Buffer.x = velocity.x >= 0 ? sourceXScale.x : sourceXScale.x * -1;
            transform.localScale = vec3Buffer;
        }
        else
        {
            walkSound.source.Stop();
            _animator.SetBool("Run", false);

        }



    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            rb.MovePosition(rb.position + velocity.normalized * speed * Time.fixedDeltaTime);
        }
    }
}
