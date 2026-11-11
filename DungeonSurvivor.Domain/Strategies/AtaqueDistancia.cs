using DungeonSurvivor.Domain.Interfaces;

namespace DungeonSurvivor.Domain.Strategies
{
    public class AtaqueDistancia : IAtaqueStrategy
    {
        private readonly int _danoBase;
        public string NombreAtaque => "Ataque a Distancia";

        public AtaqueDistancia(int danoBase = 10)
        {
            _danoBase = danoBase;
        }

        public int CalcularDano() => _danoBase + 5;
    }
}