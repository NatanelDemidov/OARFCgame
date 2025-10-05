using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Slider fovOptionValue;
    [SerializeField] Slider imageGenValue;
    [SerializeField] Slider mouseSensValue;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Camera cam;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] Player player;
    void Update()
    {
        
    }
    public void PauseMenuControl()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
            else
            {
                pauseMenu.SetActive(true);
                optionsMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
            
        }
    }
    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }
    public void CloseWindows()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void CloseGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
    public void Options()
    {
        cam.fieldOfView = fovOptionValue.value;
        cam.farClipPlane = imageGenValue.value;
        player.rotSpeed = mouseSensValue.value;
    }
}
