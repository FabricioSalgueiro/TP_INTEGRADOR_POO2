namespace DungeonSurvivor.Domain.Interfaces
{
    public interface IDanable
    {
        int VidaActual { get; }
        int VidaMaxima { get; }
        bool EstaVivo { get; }
        void RecibirDano(int cantidad);
        void Curar(int cantidad);
    }
}