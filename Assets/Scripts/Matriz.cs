using UnityEngine;
using System.Collections;

public class Matriz : MonoBehaviour {

    public static int w = 10;
    public static int h = 20;
    public static Transform[,] grid = new Transform[w, h]; //Tipo transform para no hacerlo cada vez (podría ser GameObject)

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Redondear coordenadas (Rotando puede que se desconfiguren las coordenadas)
    public static Vector2 roundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x),
                           Mathf.Round(v.y));
    }

    //Está dentro de los bordes o fuera de ellos 
    public static bool insideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < w &&
                (int)pos.y >= 0);
    }

    //Eliminar fila cuando se haya llenado
    public static void deleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    //Una fila baja cuando se elimina otra completa
    public static void decreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] != null)
            {
                // Move one towards bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // Update Block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    //Todas las filas bajan
    public static void decreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
            decreaseRow(i);
    }
    
    //Ver si una fila está llena de bloques
    public static bool isRowFull(int y)
    {
        for (int x = 0; x < w; x++)
            if (grid[x, y] == null)
                return false;
        return true;
    }

    //Si la fila está llena se elimina y bajan las filas restantes
    public static void deleteFullRows()
    {
        for (int y = 0; y < h; ++y)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                decreaseRowsAbove(y + 1);
                --y;
            }
        }
    }


}
