using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverDoor : MonoBehaviour {
    public float restartDelay = 5;
    public GameObject gameOverPanel;
    bool restart = false;
    float countdown = 0;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (restart)
        {
            if (countdown <= 0)
                 SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            countdown -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (restart)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            countdown = restartDelay;
            restart = true;
            gameOverPanel.SetActive(true);
        }
    }
}
