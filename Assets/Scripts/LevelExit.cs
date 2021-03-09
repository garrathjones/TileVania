using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 3f;
    [SerializeField] float exitSloMo = 0.3f;
    [SerializeField] AudioClip levelExitSFX;

    void OnTriggerEnter2D(Collider2D other)
    {
        AudioSource.PlayClipAtPoint(levelExitSFX, Camera.main.transform.position);
        StartCoroutine(LoadNextLevel());
    }


    IEnumerator LoadNextLevel()
    {
        
        Time.timeScale = exitSloMo;
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        Time.timeScale = 1f;
        LoadNextScene();
    }


    public void LoadNextScene()
    {
        //var scenePersist = FindObjectOfType<ScenePersist>();
        //Destroy(scenePersist);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;    
        SceneManager.LoadScene(currentSceneIndex + 1);            
        
        
            
    }




    }
