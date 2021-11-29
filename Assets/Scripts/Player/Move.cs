using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Photon.Pun.MonoBehaviourPun
{
    public float speed = 15f;
    Vector2 velocity;
    Rigidbody2D rb;
    [HideInInspector]
    // can't move while chat
    public UIControl _uiControl;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {

        if (photonView.IsMine && !_uiControl.IsChatWindowActive)
        {
            velocity.x = Input.GetAxisRaw("Horizontal");
            velocity.y = Input.GetAxisRaw("Vertical");
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
