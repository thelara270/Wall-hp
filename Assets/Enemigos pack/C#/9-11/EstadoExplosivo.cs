public abstract class EstadoExplosivo
{
    protected ControladorExplosivo enemigo;

    public EstadoExplosivo(ControladorExplosivo enemigo)
    {
        this.enemigo = enemigo;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
