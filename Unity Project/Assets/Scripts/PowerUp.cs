using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public float downSpeed;

    public string powerUpName = "Shield";
    // Update is called once per frame
    void Update()
    {
        //Moving down
      
        if(collideWithPlayer)
        {
            float dis = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
            transform.position = Vector3.Lerp(transform.position, GameManager.instance.player.transform.position, Time.deltaTime * downSpeed );
            if (dis<=0.5f)
            {
                GameManager.instance.PowerUpPickUp(powerUpName);
                gameObject.SetActive(false);
                
            }
        }
        else
        {
            transform.Translate(Vector3.back * Time.deltaTime * downSpeed);
        }

    }
    bool collideWithPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            collideWithPlayer = true;
        }
    }
}
