using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
[CustomEditor(typeof(EnemyController))]
public class EnemyControllerEditor : GluEditor
{
    EnemyController baseScript;
    private void OnEnable()
    {
        baseScript = target as EnemyController;
    }
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        BeginFold("Hit Flash Effect Fields", true);
        baseScript.flash = (Material)EditorGUILayout.ObjectField("Flash Effect", baseScript.flash, typeof(Material), allowSceneObjects: true);
        if (baseScript.renderer.Count <= 0)
        {
            if (GUILayout.Button("Add Renderer Point"))
            {
                baseScript.renderer.Add(null);
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
        BeginFold("Health Fields", true);
        baseScript.healthPoint = EditorGUILayout.FloatField("Health Point", baseScript.healthPoint);
        baseScript.healthBg =(GameObject) EditorGUILayout.ObjectField("Health Bar GameObject",baseScript.healthBg,typeof(GameObject),allowSceneObjects:true);
        EndFold();

        BeginFold("Attack Fields", true);
        baseScript.fireToPlayer = EditorGUILayout.Toggle("Fire To Player", baseScript.fireToPlayer);
        baseScript.fireSpeed = EditorGUILayout.FloatField("Bullet Fire Speed", baseScript.fireSpeed);
        baseScript.delayBetweenFire = EditorGUILayout.FloatField("Delay Between Fires", baseScript.delayBetweenFire);
        GUILayout.Space(10);
        baseScript.nFireAtATime = EditorGUILayout.IntField("N Number of Fires", baseScript.nFireAtATime);
        baseScript.delayBetweenNFire = EditorGUILayout.FloatField("Relax Delay For Firing", baseScript.delayBetweenNFire);
        GUILayout.Space(10);
        baseScript.bulletPrefab = (PlayerFire)EditorGUILayout.ObjectField("Bullet Prefab", baseScript.bulletPrefab, typeof(PlayerFire), allowSceneObjects: true);
        baseScript.particleFirePrefab = (ParticleSystem)EditorGUILayout.ObjectField("Bullet Flash Effect", baseScript.particleFirePrefab, typeof(ParticleSystem), allowSceneObjects: true);
        baseScript.particleScale = EditorGUILayout.Vector3Field("Particle Flash Scale", baseScript.particleScale);
        GUILayout.Space(10);
        baseScript.firePointOffset = EditorGUILayout.Vector3Field("Fire Point Offset for Bullet", baseScript.firePointOffset);
        if (baseScript.firePoint.Count<=0)
        {
            if(GUILayout.Button("Add Fire Point"))
            {
                baseScript.firePoint.Add(null);
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
                continue;
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
        BeginFold("Cannon", true);
        baseScript.cannon = (GameObject)EditorGUILayout.ObjectField("Cannon", baseScript.cannon, typeof(GameObject), allowSceneObjects: true);
        if(baseScript.cannon)
        {
            baseScript.cannonRotationSpeed = EditorGUILayout.FloatField("Cannon Rotation Speed", baseScript.cannonRotationSpeed);
        }
        EndFold();

        BeginFold("Other Fields", true);
        baseScript.isReady = EditorGUILayout.Toggle("Is Ready", baseScript.isReady);
        baseScript.explosionEffectScale = EditorGUILayout.Vector3Field("Explosion Effect Scale", baseScript.explosionEffectScale);
        baseScript.powerUpToGive = (ePowerUpType)EditorGUILayout.EnumPopup("Power Up After Destroy", baseScript.powerUpToGive);//, typeof(ePowerUpType));, allowSceneObjects: true);
        //baseScript.explosionAS = (AudioSource)EditorGUILayout.ObjectField("Explosion Audio Source", baseScript.explosionAS, typeof(AudioSource), allowSceneObjects: true);
        EndFold();
        EditorUtility.SetDirty(baseScript);
    }
}
