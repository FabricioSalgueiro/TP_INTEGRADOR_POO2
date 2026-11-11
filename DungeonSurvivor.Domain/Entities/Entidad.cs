using System;
using System.Numerics;
using DungeonSurvivor.Domain.Interfaces;
using DungeonSurvivor.Domain.Exceptions;

namespace DungeonSurvivor.Domain.Entities
{
    public abstract class Entidad : IDanable, IMovible
    {
        public string Id { get; private set; }
        public Vector2 Posicion { get; set; }
        public float Velocidad { get; set; }
        public int VidaActual { get; protected set; }
        public int VidaMaxima { get; protected set; }
        public bool EstaVivo => VidaActual > 0;

    
        public event Action<int>? OnVidaCambiada;

        protected Entidad(string id, int vidaMaxima, float velocidad, Vector2 posicionInicial)
        {
            Id = id;
            VidaMaxima = vidaMaxima;
            VidaActual = vidaMaxima;
            Velocidad = velocidad;
            Posicion = posicionInicial;
        }

        public virtual void RecibirDano(int cantidad)
        {
            if (!EstaVivo)
                throw new SinVidaException($"La entidad {Id} ya no tiene vida.");

            VidaActual = Math.Max(0, VidaActual - cantidad);
            OnVidaCambiada?.Invoke(VidaActual);
        }

        public virtual void Curar(int cantidad)
        {
            if (!EstaVivo) return;

            VidaActual = Math.Min(VidaMaxima, VidaActual + cantidad);
            OnVidaCambiada?.Invoke(VidaActual);
        }

        public virtual void Mover(Vector2 direccion, float deltaTime)
        {
            if (!EstaVivo) return;

            if (direccion != Vector2.Zero)
            {
                direccion = Vector2.Normalize(direccion);
                Posicion += direccion * Velocidad * deltaTime;
            }
        }
    }
}