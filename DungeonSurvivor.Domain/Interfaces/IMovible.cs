using System.Numerics;

namespace DungeonSurvivor.Domain.Interfaces
{
    public interface IMovible
    {
        Vector2 Posicion { get; set; }
        float Velocidad { get; set; }
        void Mover(Vector2 direccion, float deltaTime);
    }
}