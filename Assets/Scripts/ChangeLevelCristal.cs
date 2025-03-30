using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class ChangeLevelCristal : MonoBehaviour
{
    
    [SerializeField] private GameObject player;
    [SerializeField] private string level;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private IEnumerator ChangeLevel() 
    {
        player.GetComponent<PlayerInput>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        audioSource.Play();
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        SceneManager.LoadScene(level);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MainCharacter>())
        {
            StartCoroutine(ChangeLevel());
        }
    }

}
