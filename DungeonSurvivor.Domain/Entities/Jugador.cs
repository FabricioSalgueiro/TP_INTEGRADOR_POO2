using System.Collections.Generic;
using System.Numerics;
using DungeonSurvivor.Domain.Interfaces;
using DungeonSurvivor.Domain.Exceptions;

namespace DungeonSurvivor.Domain.Entities
{
    public class Jugador : Entidad
    {
        public string Nombre { get; private set; }
        public IAtaqueStrategy EstrategiaAtaque { get; set; }
        
        private readonly List<string> _inventario;
        public IReadOnlyList<string> Inventario => _inventario.AsReadOnly();
        public int CapacidadInventario { get; private set; }

        public Jugador(string id, string nombre, Vector2 posicionInicial, IAtaqueStrategy estrategiaInicial, int capacidadInventario = 5)
            : base(id, 100, 150f, posicionInicial)
        {
            Nombre = nombre;
            EstrategiaAtaque = estrategiaInicial;
            CapacidadInventario = capacidadInventario;
            _inventario = new List<string>();
        }

        public void AgregarItem(string item)
        {
            if (_inventario.Count >= CapacidadInventario)
                throw new InventarioLlenoException("El inventario está lleno.");

            _inventario.Add(item);
        }

        public int Atacar() => EstrategiaAtaque.CalcularDano();
    }
}