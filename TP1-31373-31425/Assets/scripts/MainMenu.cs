using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    void Start()
    {
        Debug.Log("MainMenu Script Iniciado!");

        if (playButton != null) 
        {
            playButton.onClick.AddListener(() => PlayGame());
        }

        if (exitButton != null) 
        {
            exitButton.onClick.AddListener(() => QuitGame());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC pressionado - Redirecionando para o Menu");
            GoToMainMenu();
        }
    }

    public void PlayGame()
    {
        Debug.Log("Carregar a cena: Jogo");
        SceneManager.LoadScene("Jogo");
    }

    public void GoToMainMenu()
    {
        Debug.Log("Voltando ao menu principal");
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Sair do jogo...");
        Application.Quit();
    }
}
