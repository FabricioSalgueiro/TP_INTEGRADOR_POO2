namespace DungeonSurvivor.Domain.Interfaces
{
    public interface IAtaqueStrategy
    {
        int CalcularDano();
        string NombreAtaque { get; }
    }
}