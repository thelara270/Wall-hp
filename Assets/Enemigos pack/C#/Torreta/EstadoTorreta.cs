public abstract class EstadoTorreta
{
    protected ControladorTorreta torreta;

    public EstadoTorreta(ControladorTorreta torreta)
    {
        this.torreta = torreta;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
