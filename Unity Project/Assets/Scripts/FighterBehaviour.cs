using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script will handle the Behaviour of AI
/// v0.2 
/// Straight -- Fighter AI will go Straight
/// GoLeft   ---  Fighter AI will go left (Going Left will depend on Z value)
/// GoRight   ---- Fighter AI will go right (Going Left will depend on Z value)
/// ZigZag   ---- Fighter AI will move zigzag (Left right)
/// ComeAndShoot --- Fighter Ai will stop at a defined Z value and shoot 
/// </summary>
public class FighterBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 startPosition;
    bool move = true;
    void Start()
    {
        startPosition = transform.position;
    }
    [Header("Forward Speed")]
    public float forwardSpeed = 5;
    [Header("Properties for Speed left, right and zigzag")]
    public float speed;
    [Header("Properties for Behaviour Go Left And Right")]
    public Vector3 pointToMove;    
    public float behaviourWhenZ = 8;
    public  eFighterBehaviour fighterBehaviour;
    [Header("Properties for Behaviour Zig Zag")]
    public Vector2 zigZagRange;
    public bool isLeft;
    
    [Header("Properties for Behaviour Come And Shoot")]
    public float stopAtZ;
    // Update is called once per frame
    void Update()
    {
       

        move = true;

        switch (fighterBehaviour)
        {
            case eFighterBehaviour.Straight:
                
                break;
            case eFighterBehaviour.GoLeft:    
            case eFighterBehaviour.GoRight:
                if (transform.position.z <= behaviourWhenZ)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(pointToMove - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);
                }
                break;
            case eFighterBehaviour.ZigZag:
                if (transform.position.x > startPosition.x + zigZagRange.y)
                {
                    isLeft = false;
                }
                if (transform.position.x < startPosition.x + zigZagRange.x)
                {
                    isLeft = true;
                    
                }
                if(isLeft)
                 transform.Translate(Vector3.left * Time.deltaTime * speed);
                else
                    transform.Translate(Vector3.right * Time.deltaTime * speed);


                break;
            case eFighterBehaviour.comeAndShoot:
                if(transform.position.z<stopAtZ)
                {
                    move = false;
                }
                break;
        }
        if (move)
        {
            //We will use simple Translate function with forward direction and multiple with delta for smooth movement
            transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);
        }
    }
}

public enum eFighterBehaviour
{
    Straight,GoLeft,GoRight,ZigZag,comeAndShoot
}