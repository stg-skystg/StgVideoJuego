using DataAcces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.Dto;
using Moq;
using StgVideoJuego.Helper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StgVideoJuego.Test.Helper
{
    public class FuncionesHelpersTest
    {
        private FuncionesHelpers ObjFuncionesHelpers;
        public void Inicializar()
        {
            var ObjConfiguration = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=stgpruebas.database.windows.net; Database=PtcVideojuego; User ID=StgPrueba; Password=Clave123@;")
            .Options;
            var context = new ApplicationDbContext(ObjConfiguration);
            ObjFuncionesHelpers = new FuncionesHelpers(context);
        }
        [Theory]
        [InlineData("Super Mario Maker 2", "Nintendo", true)]
        [InlineData("Nioh", "Koei Tecmo", true)]
        [InlineData("Nioh   2", "Koei Tecmo", false)]
        [InlineData("Nioh   ", "Koei Tecmo2", false)]
        public async void BlVideoGaneExisteTest(string PrmStrTitulo, string PrmStrCompania, bool PrmBlEsperado)
        {
            Inicializar();
            Assert.True(await ObjFuncionesHelpers.BlVideoGameExiste(PrmStrTitulo, PrmStrCompania) == PrmBlEsperado);
        }
        [Theory]
        [InlineData("prueba@yopmail.com", "Stg", true)]
        [InlineData("prueba   ", "sfer12", false)]
        public async void BlUsuarioExisteTest(string PrmStrEmail, string PrmStrNickName, bool PrmBlEsperado)
        {
            Inicializar();
            Assert.True(await ObjFuncionesHelpers.BlUsuarioExiste(PrmStrEmail, PrmStrNickName) == PrmBlEsperado);
        }
        [Theory]
        [InlineData("prueba@yopmail.com", "Clave123@", true)]
        [InlineData("prueba   ", "sfer12", false)]
        public async void BlUsuarioExisteLoginTest(string PrmStrEmail, string PrmStrpassword, bool PrmBlEsperado)
        {
            Inicializar();
            Assert.True(await ObjFuncionesHelpers.BlUsuarioExisteLogin(PrmStrEmail, PrmStrpassword) == PrmBlEsperado);
        }
    }
}
