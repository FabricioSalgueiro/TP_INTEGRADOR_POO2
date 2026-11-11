using System;

namespace DungeonSurvivor.Domain.Exceptions
{
    public class SinVidaException : Exception
    {
        public SinVidaException(string mensaje) : base(mensaje) { }
    }
}