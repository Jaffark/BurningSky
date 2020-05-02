using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Common Script for AI and Player
/// This script is the parent script for Enemy Controller and PlayerController
/// 
/// </summary>

public class FighterPlane : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 defaultRotation;
    //Holds Max Health Point
    public float maxHealthPoint;
    //Holds Current Health Point
    public float healthPoint;

    //We will link the renderer in order to give flash effect if player bullet hit
    public List<Renderer> renderer;
    //we will store the Materials information in order to reset back to the default material once the flash is done
    List<Material> defaultMaterial=new List<Material>();
    //Flash material , We will set this material when it get hit by player 
    public Material flash;
    //Delay till the flash Should show after this delay default Materials will assign
    public float flashDelay = 0.1f;
    //Store rotation information as Every Fighter has different behaviour
   protected  void SetDefaultValues()
    {
        //Setting Max value from Health Point
        maxHealthPoint = healthPoint;
        //Setting rotation value
        defaultRotation = transform.eulerAngles;
        //This will Set the Default material using linked renderer
        SetDefaultMaterial();
    }
    //Added Material from Renderer to Default Material
    void SetDefaultMaterial()
    {
        for (int i = 0; i < renderer.Count; i++)
        {
            defaultMaterial.Add(renderer[i].material);
        }
    }
    //Reset Function to set material back to default after flash
    public void ResetFlash()
    {
        for (int i = 0; i < renderer.Count; i++)
        {
            renderer[i].material = defaultMaterial[i];
        }
    }


    public virtual void  OnHit(float damageAmount)
    {       
        for (int i = 0; i < renderer.Count; i++)
        {
            renderer[i].material = flash;
        }
        Invoke("ResetFlash", flashDelay);     
        healthPoint -= damageAmount;
    }
}
