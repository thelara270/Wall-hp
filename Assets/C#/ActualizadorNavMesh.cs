using UnityEngine;
using Unity.AI.Navigation;

public class ActualizadorNavMesh : MonoBehaviour
{
    public NavMeshSurface superficie;

    public void ReconstruirNavMesh()
    {
        if (superficie != null)
            superficie.BuildNavMesh();
    }
}
