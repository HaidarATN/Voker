using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayerControl : MonoBehaviour
{
    public GameObject StartGameMenu;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGameMenu.SetActive(!StartGameMenu.activeSelf);

            Cursor.visible = !Cursor.visible;

            switch(Cursor.visible){
                case true:
                    Cursor.lockState = CursorLockMode.None;
                    break;

                case false:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
            }
        }
    }
}
