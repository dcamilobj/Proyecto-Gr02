using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Boton : MonoBehaviour
{

    public void OnMouseDown()
    {

        print("Estoy dando click");
        //SceneManager.LoadScene(1)
        SceneManager.LoadScene("scene_tetris");
		SceneManager.UnloadScene ("inicio");
    }
}
