using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using DungeonSurvivor.Domain.Entities;
using DungeonSurvivor.Domain.Exceptions;
using DungeonSurvivor.Domain.Factories;
using DungeonSurvivor.Domain.Interfaces;
using DungeonSurvivor.Domain.Managers;
using DungeonSurvivor.Domain.Persistence;
using DungeonSurvivor.Domain.Strategies;

namespace DungeonSurvivor.Tests
{
    [TestFixture]
    public class PruebasDominio
    {
        [SetUp]
        public void Setup()
        {
            GameManager.Instancia.Reiniciar();
        }

        // --- PRUEBAS DE DAÑO Y VIDA ---
        [Test]
        public void RecibirDano_RestaVidaCorrectamente()
        {
            // Arrange
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueCuerpoACuerpo());

            // Act
            jugador.RecibirDano(30);

            // Assert
            Assert.That(jugador.VidaActual, Is.EqualTo(70));
        }

        [Test]
        public void Curar_AumentaVidaSinSuperarMaximo()
        {
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueCuerpoACuerpo());
            jugador.RecibirDano(50);
            jugador.Curar(30);

            Assert.That(jugador.VidaActual, Is.EqualTo(80));

            jugador.Curar(50);
            Assert.That(jugador.VidaActual, Is.EqualTo(100)); // Vida máxima
        }

        [Test]
        public void RecibirDano_HastaCero_EstaVivoEsFalse()
        {
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueCuerpoACuerpo());
            jugador.RecibirDano(100);

            Assert.Multiple(() =>
            {
                Assert.That(jugador.VidaActual, Is.EqualTo(0));
                Assert.That(jugador.EstaVivo, Is.False);
            });
        }

        [Test]
        public void RecibirDano_EstandoMuerto_LanzaSinVidaException()
        {
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueCuerpoACuerpo());
            jugador.RecibirDano(100);

            Assert.Throws<SinVidaException>(() => jugador.RecibirDano(10));
        }

        // --- PRUEBAS PARAMETRIZADAS DE MOVIMIENTO ---
        [TestCase(1, 0, 150, 0)]
        [TestCase(0, 1, 0, 150)]
        [TestCase(0, 0, 0, 0)]
        public void Mover_CalculaNuevaPosicionCorrectamente(float dirX, float dirY, float resX, float resY)
        {
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueCuerpoACuerpo());
            jugador.Mover(new Vector2(dirX, dirY), 1.0f); // 1 segundo transcurrido

            Assert.Multiple(() =>
            {
                Assert.That(jugador.Posicion.X, Is.EqualTo(resX).Within(0.01f));
                Assert.That(jugador.Posicion.Y, Is.EqualTo(resY).Within(0.01f));
            });
        }

        // --- PRUEBAS DE INVENTARIO Y EXCEPCIONES ---
        [Test]
        public void AgregarItem_AgregaItemsAlInventario()
        {
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueCuerpoACuerpo(), 2);
            jugador.AgregarItem("Pocion");

            Assert.That(jugador.Inventario.Count, Is.EqualTo(1));
            Assert.That(jugador.Inventario[0], Is.EqualTo("Pocion"));
        }

        [Test]
        public void AgregarItem_SuperaCapacidad_LanzaInventarioLlenoException()
        {
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueCuerpoACuerpo(), 1);
            jugador.AgregarItem("Pocion");

            Assert.Throws<InventarioLlenoException>(() => jugador.AgregarItem("Espada"));
        }

        // --- PRUEBAS DE PATRÓN STRATEGY ---
        [Test]
        public void Atacar_ConEstrategiaCuerpoACuerpo_DevuelveDanoBase()
        {
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueCuerpoACuerpo(20));
            Assert.That(jugador.Atacar(), Is.EqualTo(20));
        }

        [Test]
        public void Atacar_ConEstrategiaDistancia_DevuelveDanoConBonificacion()
        {
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueDistancia(10));
            Assert.That(jugador.Atacar(), Is.EqualTo(15));
        }

        [Test]
        public void CambiarEstrategiaAtaque_EnTiempoDeEjecucion()
        {
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueCuerpoACuerpo(20));
            Assert.That(jugador.Atacar(), Is.EqualTo(20));

            jugador.EstrategiaAtaque = new AtaqueDistancia(20);
            Assert.That(jugador.Atacar(), Is.EqualTo(25));
        }

        // --- PRUEBAS DE PATRÓN FACTORY ---
        [Test]
        public void EnemigoFactory_CreaEsqueletoCorrectamente()
        {
            var enemigo = EnemigoFactory.CrearEnemigo("esqueleto", Vector2.Zero);

            Assert.Multiple(() =>
            {
                Assert.That(enemigo.Tipo, Is.EqualTo("Esqueleto"));
                Assert.That(enemigo.VidaMaxima, Is.EqualTo(50));
            });
        }

        [Test]
        public void EnemigoFactory_CreaOgroCorrectamente()
        {
            var enemigo = EnemigoFactory.CrearEnemigo("ogro", Vector2.Zero);

            Assert.Multiple(() =>
            {
                Assert.That(enemigo.Tipo, Is.EqualTo("Ogro"));
                Assert.That(enemigo.VidaMaxima, Is.EqualTo(150));
            });
        }

        [Test]
        public void EnemigoFactory_TipoDesconocido_LanzaArgumentException()
        {
            Assert.Throws<ArgumentException>(() => EnemigoFactory.CrearEnemigo("dragon", Vector2.Zero));
        }

        // --- PRUEBAS DE SINGLETON Y LINQ ---
        [Test]
        public void GameManager_InstanciaEsUnica()
        {
            var g1 = GameManager.Instancia;
            var g2 = GameManager.Instancia;

            Assert.That(g1, Is.SameAs(g2));
        }

        [Test]
        public void LINQ_ObtenerEntidadesVivas_FiltraCorrectamente()
        {
            var gm = GameManager.Instancia;
            var j1 = new Jugador("1", "Vivo", Vector2.Zero, new AtaqueCuerpoACuerpo());
            var j2 = new Jugador("2", "Muerto", Vector2.Zero, new AtaqueCuerpoACuerpo());
            j2.RecibirDano(100);

            gm.AgregarEntidad(j1);
            gm.AgregarEntidad(j2);

            var vivas = gm.ObtenerEntidadesVivas().ToList();

            Assert.Multiple(() =>
            {
                Assert.That(vivas.Count, Is.EqualTo(1));
                Assert.That(vivas[0].Id, Is.EqualTo("1"));
            });
        }

        [Test]
        public void LINQ_ObtenerEntidadesClasificadasPorVida_OrdenaDescendente()
        {
            var gm = GameManager.Instancia;
            var j1 = new Jugador("1", "PocaVida", Vector2.Zero, new AtaqueCuerpoACuerpo());
            var j2 = new Jugador("2", "MuchaVida", Vector2.Zero, new AtaqueCuerpoACuerpo());
            j1.RecibirDano(40); // Queda con 60

            gm.AgregarEntidad(j1);
            gm.AgregarEntidad(j2); // Tiene 100

            var ordenadas = gm.ObtenerEntidadesClasificadasPorVida().ToList();

            Assert.Multiple(() =>
            {
                Assert.That(ordenadas[0].Id, Is.EqualTo("2"));
                Assert.That(ordenadas[1].Id, Is.EqualTo("1"));
            });
        }

        // --- PRUEBAS DE EVENTOS (OBSERVER) ---
        [Test]
        public void Evento_OnVidaCambiada_SeDisparaAlRecibirDano()
        {
            var jugador = new Jugador("1", "Heroe", Vector2.Zero, new AtaqueCuerpoACuerpo());
            int vidaNotificada = 0;
            bool eventoDisparado = false;

            jugador.OnVidaCambiada += (nuevaVida) =>
            {
                vidaNotificada = nuevaVida;
                eventoDisparado = true;
            };

            jugador.RecibirDano(25);

            Assert.Multiple(() =>
            {
                Assert.That(eventoDisparado, Is.True);
                Assert.That(vidaNotificada, Is.EqualTo(75));
            });
        }

        // --- PRUEBAS CON MOQ (MOCKING) ---
        [Test]
        public void Mocking_EstrategiaAtaque_SimulaComportamiento()
        {
            var mockAtaque = new Mock<IAtaqueStrategy>();
            mockAtaque.Setup(a => a.CalcularDano()).Returns(999);

            var jugador = new Jugador("1", "SuperHeroe", Vector2.Zero, mockAtaque.Object);

            Assert.That(jugador.Atacar(), Is.EqualTo(999));
            mockAtaque.Verify(a => a.CalcularDano(), Times.Once);
        }

        // --- PRUEBAS DE PERSISTENCIA (JSON) ---
        [Test]
        public async Task SaveSystem_GuardarYCargar_ConservaDatos()
        {
            string pathTemp = Path.GetTempFileName();
            var dataOriginal = new SaveData
            {
                PlayerName = "Coder",
                Score = 500,
                Level = 3,
                Health = 80,
                Inventory = new System.Collections.Generic.List<string> { "Llave", "Pocion" }
            };

            await SaveSystem.GuardarEnArchivoAsync(pathTemp, dataOriginal);
            var dataCargada = await SaveSystem.CargarDesdeArchivoAsync(pathTemp);

            File.Delete(pathTemp); // Limpiar archivo temporal

            Assert.That(dataCargada, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(dataCargada!.PlayerName, Is.EqualTo("Coder"));
                Assert.That(dataCargada.Score, Is.EqualTo(500));
                Assert.That(dataCargada.Inventory.Count, Is.EqualTo(2));
            });
        }
    }
}