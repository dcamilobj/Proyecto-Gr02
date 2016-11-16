using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;
using UnityEditor;

public class Group : MonoBehaviour {

    float lastFall = 0;
    public  GUIText tPuntaje;
    int puntaje = 0;
    bool der = false;
    bool izq = false;
    public bool mov;
    GameObject current;
    public GameObject myo=null;
    private Pose _lastPose = Pose.Unknown;
    public  Text scoreT;
    private int score;
    private int scoreB;

    //-----------------------------------------

    // Use this for initialization
    void Start()
    {
        score = 0;
        if (mov==true && name != "BarraZ" && name != "CuadroA" && name != "LderechaM" && name != "LizquierdaC" && name != "SR" && name != "TV" && name != "ZN" && name != "Hub - 1 Myo")
        {
            // Default position not valid? Then it's game over
           /* if (!isValidMatrizPos() && name!="BarraZ"
                 && name != "CuadroA" && name != "LderechaM"
                 && name != "LizquierdaC" && name != "SR"
                 && name != "TV" && name != "ZN")*/
          if (!isValidMatrizPos())
        {
           Debug.Log("GAME OVER");
                EditorUtility.DisplayDialog("GAME OVER", "¿Desea volver a jugar?","Reiniciar");
                if (name != "BarraZ" && name != "CuadroA" && name != "LderechaM" && name != "LizquierdaC" && name != "SR" && name != "TV" && name != "ZN" && name != "Hub - 1 Myo")
           Destroy(gameObject);
        }
        }
    }

    bool isValidMatrizPos()
    {
        if (mov == true && name != "BarraZ" && name != "CuadroA" && name != "LderechaM" && name != "LizquierdaC" && name != "SR" && name != "TV" && name != "ZN" && name != "Hub - 1 Myo")
        {
            foreach (Transform child in transform)
            {
                Vector2 v = Matriz.roundVec2(child.position);

                // Not inside Border?
                if (!Matriz.insideBorder(v))
                    return false;

                // Block in Matriz cell (and not part of same group)?
                if (Matriz.grid[(int)v.x, (int)v.y] != null &&
                    Matriz.grid[(int)v.x, (int)v.y].parent != transform)
                    return false;
            }
            return true;
       }
       return false;//Quitar
    }


    //--------------------------------
    void updateMatriz()
    {
       if (mov == true && name != "BarraZ" && name != "CuadroA" && name != "LderechaM" && name != "LizquierdaC" && name != "SR" && name != "TV" && name != "ZN" && name != "Hub - 1 Myo")
        {
            // Remove old children from Matriz
            for (int y = 0; y < Matriz.h; ++y)
                for (int x = 0; x < Matriz.w; ++x)
                    if (Matriz.grid[x, y] != null)
                        if (Matriz.grid[x, y].parent == transform)
                            Matriz.grid[x, y] = null;

            // Add new children to Matriz
            foreach (Transform child in transform)
            {
                Vector2 v = Matriz.roundVec2(child.position);
                Matriz.grid[(int)v.x, (int)v.y] = child;
            }
       }
    }




    // Update is called once per frame
   void Update()
    {
      ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
        bool fist = false;
        bool wavein = false;
        bool waveout = false;
        bool doubletap = false;
        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.Fist)
            {
                fist = true;
            }
            else if (thalmicMyo.pose == Pose.WaveIn)
            {
                wavein = true;
            }
            else if (thalmicMyo.pose == Pose.WaveOut)
            {
                waveout = true;
            }
            else if (thalmicMyo.pose == Pose.DoubleTap)
            {
                doubletap = true;
            }
        }
        if (mov == true && name != "BarraZ" && name != "CuadroA" && name != "LderechaM" && name != "LizquierdaC" && name != "SR" && name != "TV" && name != "ZN" && name != "Hub - 1 Myo")
        {
            // Mover hacia la izquierda
            if (Input.GetKeyDown(KeyCode.LeftArrow)||wavein)
            {
                // Modificar posición
                transform.position += new Vector3(-1, 0, 0);

                // Ver si es válido
                if (isValidMatrizPos())
                    // Actualizar matriz
                    updateMatriz();
                else
                    // Si no es valido revertir movimiento
                    transform.position += new Vector3(1, 0, 0);
            }

            // Mover hacia la derecha
            else if (Input.GetKeyDown(KeyCode.RightArrow)||waveout)
            {
                // Modificar posición
                transform.position += new Vector3(1, 0, 0);

                // Ver si es válido
                if (isValidMatrizPos())
                    // Actualizar matriz
                    updateMatriz();
                else
                    // Si no es valido revertir movimiento
                    transform.position += new Vector3(-1, 0, 0);
            }
            // Rotar
            else if (Input.GetKeyDown(KeyCode.UpArrow) ||fist)
            //Input.GetMouseButtonDown(0) == true
            {
                transform.Rotate(0, 0, -90);

                // Ver si es válido
                if (isValidMatrizPos())
                    // Actualizar matriz
                    updateMatriz();
                else
                    // Si no es valido revertir movimiento
                    transform.Rotate(0, 0, 90);
            }
            // Fall
            else if (Input.GetKeyDown(KeyCode.DownArrow)|| doubletap)
            {
                // Modificar posición
                transform.position += new Vector3(0, -1, 0);

                // Ver si es válido
                if (isValidMatrizPos())
                {
                    // Actualizar matriz
                    updateMatriz();
                }
                else
                {
                    // Si no es valido revertir movimiento
                    transform.position += new Vector3(0, 1, 0);

                    // Clear filled horizontal lines

                    scoreB = Matriz.deleteFullRows();
                    for (int i = 0; i < scoreB; i++)
                    {
                        string text = scoreT.text;
                        score = int.Parse(text) + 10;
                        scoreT.text = score.ToString();
                    }

                    scoreB = 0;


                    // Spawn next Group
                    FindObjectOfType<Spawner>().spawnNext();


                    // Disable script
                    enabled = false;
                }
            }

            // Mover hacia abajo
            else if (Input.GetKeyDown(KeyCode.DownArrow)||
                    Time.time - lastFall >= 1|| doubletap)
            {
                // Modificar posisción
                transform.position += new Vector3(0, -1, 0);

                // Ver si es válido
                if (isValidMatrizPos())
                {
                    // Actualizar matriz
                    updateMatriz();
                }
                else
                {
                    // Si no es valido revertir movimiento
                    transform.position += new Vector3(0, 1, 0);

                    // Clear filled horizontal lines
                    scoreB = Matriz.deleteFullRows();
                    for(int i=0; i<scoreB; i++)
                    { 
                        string text =scoreT.text;
                        score = int.Parse(text) + 10;
                        scoreT.text = score.ToString();
                    }

                    scoreB = 0;


                    // Spawn next Group
                    FindObjectOfType<Spawner>().spawnNext();

                    // Disable script
                    enabled = false;
                }

                lastFall = Time.time;

            }
        }
    }


    public void VerificarD(GameObject o)
    {
        //ouch h = Input.touches.getTou;
        //if (mov == true)
        //{
            der = true;
            print("Estoy presionando");

            // Modificar posición
            o.transform.position += new Vector3(1, 0, 0);
            // Ver si es válido
            if (isValidMatrizPos())
                // Actualizar matriz
                updateMatriz();
            else
                // Si no es valido revertir movimiento
                o.transform.position += new Vector3(-1, 0, 0);
      //  }
       // return false;
    }




}
