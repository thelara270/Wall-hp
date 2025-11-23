using System.Collections.Generic;
using UnityEngine;

public class PoolProyectiles : MonoBehaviour
{
    public static PoolProyectiles Instance;

    public GameObject prefabBala;
    public int maxBalas = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < maxBalas; i++)
        {
            GameObject bala = Instantiate(prefabBala);
            bala.SetActive(false);
            pool.Enqueue(bala);
        }
    }

    public GameObject ObtenerBala()
    {
        if (pool.Count > 0)
        {
            GameObject bala = pool.Dequeue();
            bala.SetActive(true);
            return bala;
        }
        return null;
    }

    public void RegresarBala(GameObject bala)
    {
        bala.SetActive(false);
        pool.Enqueue(bala);
    }
}
