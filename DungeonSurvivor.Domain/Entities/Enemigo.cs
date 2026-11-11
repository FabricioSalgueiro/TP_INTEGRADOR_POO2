using System.Numerics;

namespace DungeonSurvivor.Domain.Entities
{
    public class Enemigo : Entidad
    {
        public string Tipo { get; private set; }
        public int DanoAtaque { get; private set; }

        public Enemigo(string id, string tipo, int vidaMaxima, float velocidad, int danoAtaque, Vector2 posicionInicial)
            : base(id, vidaMaxima, velocidad, posicionInicial)
        {
            Tipo = tipo;
            DanoAtaque = danoAtaque;
        }
    }
}