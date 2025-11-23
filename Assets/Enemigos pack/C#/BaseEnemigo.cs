public abstract class EstadoEnemigo
{
    // Referencia al enemigo que posee este estado
    protected ControladorEnemigo enemigo;

    // Constructor: recibe al enemigo dueño de este estado
    public EstadoEnemigo(ControladorEnemigo enemigo)
    {
        this.enemigo = enemigo;
    }

    // Método que se llama una vez al entrar al estado
    public abstract void Enter();

    // Método que se llama cada frame mientras el estado esté activo
    public abstract void Update();

    // Método que se llama una vez al salir del estado
    public abstract void Exit();
}
