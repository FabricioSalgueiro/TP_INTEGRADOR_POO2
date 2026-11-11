using System;

namespace DungeonSurvivor.Domain.Exceptions
{
    public class InventarioLlenoException : Exception
    {
        public InventarioLlenoException(string mensaje) : base(mensaje) { }
    }
}