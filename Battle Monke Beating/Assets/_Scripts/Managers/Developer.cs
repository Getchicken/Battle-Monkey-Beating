using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


public class Developer 
{
    [MenuItem("Developer/NextScene")]
    public static void NextScene()
    {
        Debug.Log("Scene has been changed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    [MenuItem("Developer/Teleport")]
    public static void Teleport()
    {
        Debug.Log("No destination available");
        // Teleport Code - Player Transform and Destination
    }

    [MenuItem("Developer/MenuScene")]
    public static void MenuScene()
    {
        Debug.Log("Menu Scene Loaded");
        if (SceneManager.GetActiveScene().buildIndex - 1 < 0 || SceneManager.GetActiveScene().buildIndex - 1 >= SceneManager.sceneCountInBuildSettings) 
            return;
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    [MenuItem("Developer/Die")]
    public static void DevDie() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
