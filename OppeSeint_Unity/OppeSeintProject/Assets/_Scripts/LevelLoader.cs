using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Transform nextLevelSpawnPoint;
    public string nextLevelName;
    public PlayerMovement playerMovement;
    public ScreenFade screenFade;
    public GameObject levelTextUI;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        levelTextUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TransitionToNextLevel());
        }
    }

    private IEnumerator TransitionToNextLevel()
    {
        //freezing the game
        Time.timeScale = 0;

        //fade in
        yield return StartCoroutine(screenFade.FadeIn());

        levelTextUI.SetActive(true);
        playerMovement.transform.position = nextLevelSpawnPoint.position;
        yield return new WaitForSecondsRealtime(1f);

        //fade out
        yield return StartCoroutine(screenFade.FadeOut());

        levelTextUI.SetActive(false);

        //unfreeze
        Time.timeScale = 1;
    }
}
