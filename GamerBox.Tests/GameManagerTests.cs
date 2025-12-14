using Xunit;
using Moq;
using GamerBox.BusinessLayer.Concrete;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System.Threading.Tasks;
using System;

namespace GamerBox.Tests
{
    public class GameManagerTests
    {
        // Mock nesneleri: Veritabaný yerine geçecek sahte nesneler
        private readonly Mock<IGameDal> _mockGameDal;
        private readonly Mock<IUserDal> _mockUserDal;

        // Test edilecek asýl sýnýf
        private readonly GameManager _gameManager;

        public GameManagerTests()
        {
            // Her testten önce bu kýsým çalýþýr ve ortamý hazýrlar
            _mockGameDal = new Mock<IGameDal>();
            _mockUserDal = new Mock<IUserDal>();

            // GameManager'a gerçek DAL yerine bizim sahte (Mock) DAL'larý veriyoruz
            _gameManager = new GameManager(_mockGameDal.Object, _mockUserDal.Object);
        }

        [Fact]
        public async Task AddGame_ValidGame_ShouldCallAddAsync()
        {
            // 1. Arrange (Hazýrlýk)
            var validGame = new Game
            {
                Title = "Test Oyunu",
                Price = 100,
                Genre = "RPG"
            };

            // 2. Act (Eylem)
            await _gameManager.AddGameAsyncB(validGame);

            // 3. Assert (Doðrulama)
            // IGameDal içindeki AddAsync metodunun tam olarak 1 kere çaðrýlýp çaðrýlmadýðýný kontrol et.
            _mockGameDal.Verify(x => x.AddAsync(validGame), Times.Once);
        }

        [Fact]
        public async Task AddGame_NegativePrice_ShouldThrowException()
        {
            // 1. Arrange (Hazýrlýk)
            var invalidGame = new Game
            {
                Title = "Hatalý Oyun",
                Price = -50, // HATA! Fiyat eksi olamaz.
                Genre = "FPS"
            };

            // 2. Act & Assert (Eylem ve Doðrulama)
            // Bu kodun hata fýrlatmasýný BEKLÝYORUZ.
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _gameManager.AddGameAsyncB(invalidGame));

            // Hata mesajýnýn doðru olup olmadýðýný da kontrol edebiliriz
            Assert.Equal("Game price cannot be negative.", exception.Message);
        }

        [Fact]
        public async Task AddGame_EmptyTitle_ShouldThrowException()
        {
            // 1. Arrange
            var invalidGame = new Game
            {
                Title = "", // HATA! Ýsim boþ.
                Price = 100
            };

            // 2. Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _gameManager.AddGameAsyncB(invalidGame));
        }
    }
}