using DungeonSurvivor.Domain.Interfaces;

namespace DungeonSurvivor.Domain.Strategies
{
    public class AtaqueCuerpoACuerpo : IAtaqueStrategy
    {
        private readonly int _danoBase;
        public string NombreAtaque => "Cuerpo a Cuerpo";

        public AtaqueCuerpoACuerpo(int danoBase = 15)
        {
            _danoBase = danoBase;
        }

        public int CalcularDano() => _danoBase;
    }
}