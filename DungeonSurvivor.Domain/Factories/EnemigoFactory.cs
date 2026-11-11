using System;
using System.Numerics;
using DungeonSurvivor.Domain.Entities;

namespace DungeonSurvivor.Domain.Factories
{
    public static class EnemigoFactory
    {
        public static Enemigo CrearEnemigo(string tipo, Vector2 posicion)
        {
            string id = Guid.NewGuid().ToString().Substring(0, 8);

            return tipo.ToLower() switch
            {
                "esqueleto" => new Enemigo(id, "Esqueleto", 50, 100f, 10, posicion),
                "ogro" => new Enemigo(id, "Ogro", 150, 50f, 25, posicion),
                _ => throw new ArgumentException($"Tipo de enemigo desconocido: {tipo}")
            };
        }
    }
}