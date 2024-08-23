using DataAcces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Dto;
using Moq;
using StgVideoJuego.Controllers;
using StgVideoJuego.Helper;

namespace StgVideoJuego.Test.Controllers
{

    public class VideoGameControllerTest
    {
        private ServiceProvider _serviceProvider;
        private FuncionesHelpers ObjFuncionesHelpers;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IMemoryCache> _memoryCacheMock;
        private VideoGameController _controller;
        public void Inicializar()
        {
            var ObjConfiguration = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=stgpruebas.database.windows.net; Database=PtcVideojuego; User ID=StgPrueba; Password=Clave123@;")
            .Options;
            var context = new ApplicationDbContext(ObjConfiguration);
            _configurationMock = new Mock<IConfiguration>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            ObjFuncionesHelpers = new FuncionesHelpers(context);
            _controller = new VideoGameController(context, _configurationMock.Object, _memoryCacheMock.Object);
        }
        [Fact]
        public async void GetVideoJuegoTest()
        {
            try
            {
                Inicializar();
                await _controller.GetVideoJuego(1);
                Assert.True(true);
            }
            catch(Exception ex)
            {
                Assert.True(false);
            }
        }
    }
}