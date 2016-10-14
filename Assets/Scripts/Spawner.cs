using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public InstanceCtrl myInstancer;
    //public GameObject[] figuras; //Arreglo que va a contener las figuras
    public GameObject[] figuras;
    Object seleccionada;
	// Use this for initialization
	void Start () {
		spawnNext (); //Cuando el juego inicie se va crear un objeto aleatorio
	}
	
	// Update is called once per frame
	void Update () {
	}

public void spawnNext() {

	int i = Random.Range(0, figuras.Length); //Escogemos indice del arreglo aleatoriamente
        Debug.Log("Estuve acá con i=" + i);
	// Spawn Group at current Position
	seleccionada=Instantiate(figuras[i],transform.position,Quaternion.identity);
	//myInstancer.myInstancedObject =  seleccionada;
        //Instantiate(figuras[i], new Vector3(4, 19), Quaternion.identity);
	}

    
}