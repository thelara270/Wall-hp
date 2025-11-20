using UnityEngine;

public class Puertas : MonoBehaviour
{
    private Animator animator;       //Referencia al componente Animator
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void AbrirPuerta()
    {
        animator.SetBool("Abrir", true);
    }
}
