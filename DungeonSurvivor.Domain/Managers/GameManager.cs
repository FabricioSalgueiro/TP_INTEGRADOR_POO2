using System;
using System.Collections.Generic;
using System.Linq;
using DungeonSurvivor.Domain.Entities;

namespace DungeonSurvivor.Domain.Managers
{
    public class GameManager
    {
        private static GameManager? _instancia;
        private static readonly object _lock = new object();

        public static GameManager Instancia
        {
            get
            {
                lock (_lock)
                {
                    _instancia ??= new GameManager();
                    return _instancia;
                }
            }
        }

        public int Puntuacion { get; private set; }
        public int NivelActual { get; private set; }
        private readonly List<Entidad> _entidadesActivas;

        public event Action<int>? OnPuntuacionCambiada;

        private GameManager()
        {
            Puntuacion = 0;
            NivelActual = 1;
            _entidadesActivas = new List<Entidad>();
        }

        public void AgregarEntidad(Entidad entidad) => _entidadesActivas.Add(entidad);

        public void SumarPuntos(int puntos)
        {
            Puntuacion += puntos;
            OnPuntuacionCambiada?.Invoke(Puntuacion);
        }

        public void SiguienteNivel() => NivelActual++;

        // Consultas LINQ
        public IEnumerable<Entidad> ObtenerEntidadesVivas()
        {
            return _entidadesActivas.Where(e => e.EstaVivo);
        }

        public IEnumerable<Entidad> ObtenerEntidadesClasificadasPorVida()
        {
            return _entidadesActivas.OrderByDescending(e => e.VidaActual);
        }

        public void Reiniciar()
        {
            Puntuacion = 0;
            NivelActual = 1;
            _entidadesActivas.Clear();
        }
    }
}