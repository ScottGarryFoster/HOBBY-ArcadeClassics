using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private float deltaCount;

    private KeyCode lastKey;
    private float movementSpeed;
    private Vector3 originalPosition;
    private Vector2Int headLocation;

    private void Start()
    {
        this.lastKey = KeyCode.None;
        this.movementSpeed = this.transform.localScale.x;

        this.originalPosition = this.transform.position;
        this.headLocation = new Vector2Int(0, 0);
    }
    
    private void Update()
    {
        UpdateKeyPress();
        
        this.deltaCount += Time.deltaTime;
        if (this.deltaCount > 0.2f)
        {
            this.deltaCount -= 0.2f;
            
            //Vector3 currentPosition = this.transform.position;
            switch (this.lastKey)
            {
                case KeyCode.W:
                    this.headLocation.y += 1;
                    break;
                case KeyCode.S:
                    this.headLocation.y -= 1;
                    break;
                case KeyCode.A:
                    this.headLocation.x -= 1;
                    break;
                case KeyCode.D:
                    this.headLocation.x += 1;
                    break;
            }

            Vector3 currentPosition = new Vector2(
                this.headLocation.x * this.movementSpeed,
                this.headLocation.y * this.movementSpeed);

            this.transform.position = currentPosition;
        }
    }

    private void UpdateKeyPress()
    {
        if (this.lastKey != KeyCode.S && Input.GetKey(KeyCode.W))
        {
            this.lastKey = KeyCode.W;
        }
        else if (this.lastKey != KeyCode.W && Input.GetKey(KeyCode.S))
        {
            this.lastKey = KeyCode.S;
        }
        else if (this.lastKey != KeyCode.A && Input.GetKey(KeyCode.D))
        {
            this.lastKey = KeyCode.D;
        }
        else if (this.lastKey != KeyCode.D && Input.GetKey(KeyCode.A))
        {
            this.lastKey = KeyCode.A;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag.Equals("Food"))
        {
            
        }
        else if (other.collider.tag.Equals("Border"))
        {
            this.transform.position = this.originalPosition;
            this.headLocation = new Vector2Int(0, 0);
        }
    }
}
