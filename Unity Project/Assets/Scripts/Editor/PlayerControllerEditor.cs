using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : GluEditor
{
    PlayerController baseScript;
    private void OnEnable()
    {
        baseScript = target as PlayerController;
    }
    public override void OnInspectorGUI()
    {
        BeginFold("Basic Fields", true);
        baseScript.movementsSpeed = EditorGUILayout.FloatField("Movement Speed", baseScript.movementsSpeed);
        EditorGUILayout.LabelField("Touch Sensitivity");
        baseScript.touchMovementSensitivity = EditorGUILayout.Slider(baseScript.touchMovementSensitivity,0,10);
        baseScript.fighterBody =(GameObject) EditorGUILayout.ObjectField("Fighter Body To Tilt", baseScript.fighterBody, typeof(GameObject), allowSceneObjects: true);
        baseScript.tiltAngle = EditorGUILayout.FloatField("Maximum Tilt angle", baseScript.tiltAngle);
        baseScript.smooth = EditorGUILayout.FloatField("Tilting Smooth Value", baseScript.smooth);

        BeginFold("Health Fields", true);
        baseScript.healthPoint = EditorGUILayout.FloatField("Health Point", baseScript.healthPoint);
        EndFold();

        BeginFold("Boundaries ", true);
        baseScript.boundaryX = EditorGUILayout.Vector2Field("Boundary X", baseScript.boundaryX);
        baseScript.boundaryZ = EditorGUILayout.Vector2Field("Boundary Z", baseScript.boundaryZ);
        EndFold();


        EndFold();
        // base.OnInspectorGUI();
        BeginFold("Hit Flash Effect Fields", true);
        baseScript.flash = (Material)EditorGUILayout.ObjectField("Flash Effect", baseScript.flash, typeof(Material), allowSceneObjects: true);
        if (baseScript.renderer.Count <= 0)
        {
            if (GUILayout.Button("Add Renderer Point"))
            {
                baseScript.renderer.Add(new Renderer());
            }
        }
        for (int i = 0; i < baseScript.renderer.Count; i++)
        {
            GUILayout.BeginHorizontal();
            baseScript.renderer[i] = (Renderer)EditorGUILayout.ObjectField("Fire Point " + (i + 1), baseScript.renderer[i], typeof(Renderer), allowSceneObjects: true);
            if (GUILayout.Button("-"))
            {
                baseScript.renderer.RemoveAt(i);
                i--;
            }
            if (i == baseScript.renderer.Count - 1)
            {

                if (GUILayout.Button("+"))
                {
                    baseScript.renderer.Add(new Renderer());
                }
            }
            GUILayout.EndHorizontal();
        }

        EndFold();
      


        BeginFold("Attack Fields", true);        
        baseScript.fireSpeed = EditorGUILayout.FloatField("Bullet Fire Speed", baseScript.fireSpeed);
        baseScript.delayBetweenFire = EditorGUILayout.FloatField("Delay Between Fires", baseScript.delayBetweenFire);                
        GUILayout.Space(10);
        baseScript.bulletPrefab = (PlayerFire)EditorGUILayout.ObjectField("Bullet Prefab", baseScript.bulletPrefab, typeof(PlayerFire), allowSceneObjects: true);
        baseScript.bullet2XPrefab = (PlayerFire)EditorGUILayout.ObjectField("Bullet 2X Prefab", baseScript.bullet2XPrefab, typeof(PlayerFire), allowSceneObjects: true);
        baseScript.particleFirePrefab = (ParticleSystem)EditorGUILayout.ObjectField("Bullet Flash Effect", baseScript.particleFirePrefab, typeof(ParticleSystem), allowSceneObjects: true);
        baseScript.particleScale = EditorGUILayout.Vector3Field("Particle Flash Scale", baseScript.particleScale);
        GUILayout.Space(10);
        BeginFold("Fire Points to Spawn Bullet", true);
        baseScript.firePointOffset = EditorGUILayout.Vector3Field("Fire Point Offset for Bullet", baseScript.firePointOffset);
        if (baseScript.firePoint.Count<=0)
        {
            if(GUILayout.Button("Add Fire Point"))
            {
                baseScript.firePoint.Add(new GameObject().transform);
            }
        }
        for(int i=0;i<baseScript.firePoint.Count;i++)
        {
            GUILayout.BeginHorizontal();
            baseScript.firePoint[i] = (Transform)EditorGUILayout.ObjectField("Fire Point " + (i + 1), baseScript.firePoint[i], typeof(Transform), allowSceneObjects: true);
            if (GUILayout.Button("-"))
            {
                baseScript.firePoint.RemoveAt(i);
                i--;
            }
            if (i==baseScript.firePoint.Count-1)
            {

                if(GUILayout.Button("+"))
                {
                    baseScript.firePoint.Add(null);
                }
            }
            GUILayout.EndHorizontal();
        }
        EndFold();



        EndFold();
    

        BeginFold("Other Fields", true);
        baseScript.fan = (Transform)EditorGUILayout.ObjectField("Fan", baseScript.fan, typeof(Transform), allowSceneObjects: true);
        if (baseScript.fan)
            baseScript.fanRotationSpeed = EditorGUILayout.FloatField("Fan Rotation Speed", baseScript.fanRotationSpeed);
        BeginFold("Drones Fields", true);
        for (int i =0;i<baseScript.playerDrones.Count;i++)
        {
            GUILayout.BeginHorizontal();
            baseScript.playerDrones[i] = (GameObject)EditorGUILayout.ObjectField("Drone " + (i + 1), baseScript.playerDrones[i], typeof(GameObject), allowSceneObjects: true);
          
            if (GUILayout.Button("-"))
            {
                baseScript.playerDrones.RemoveAt(i);
                i--;
                continue;
            }
            if (i==baseScript.playerDrones.Count-1)
            {
                if(GUILayout.Button("+"))
                {
                    baseScript.playerDrones.Add(null);
                }
            }
            GUILayout.EndHorizontal();
        }
        if (baseScript.playerDrones.Count<=0)
        {
            
            if(GUILayout.Button("Add Drones"))
            {
                baseScript.playerDrones.Add(null);
            }

        }
        EndFold();
        EndFold();

    }
}
