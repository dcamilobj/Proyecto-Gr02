using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Boton : MonoBehaviour
{

    public void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0))
        Debug.Log("Acá estoyyy");
        print("Estoy dando click");
        //SceneManager.LoadScene(1)
        SceneManager.LoadScene("scene_tetris");
		SceneManager.UnloadScene ("inicio");
    }
}
