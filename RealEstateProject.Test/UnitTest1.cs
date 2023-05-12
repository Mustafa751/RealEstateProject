using Moq;
using RealEstateProject.BLL.Services;
using RealEstateProject.DAL;
using RealEstateProject.DAL.Repositories;

namespace RealEstateProject.Test
{
    public class EstateServiceTests
    {
        private readonly Mock<IEstateRepository> _estateRepositoryMock;
        private readonly EstateService _estateService;

        public EstateServiceTests()
        {
            _estateRepositoryMock = new Mock<IEstateRepository>();
            _estateService = new EstateService(_estateRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateEstate_ReturnsEstate()
        {
            // Arrange
            var estate = new Estate { Price = 100000, Address = "123 Main St" };
            _estateRepositoryMock.Setup(x => x.AddEstate(estate)).ReturnsAsync(estate);

            // Act
            var result = await _estateService.CreateEstate(estate);

            // Assert
            Assert.Equal(estate, result);
        }

        [Fact]
        public async Task DeleteEstate_WithValidId_ReturnsTrue()
        {
            // Arrange
            var estate = new Estate { Id = 1, Price = 100000, Address = "123 Main St" };
            _estateRepositoryMock.Setup(x => x.GetEstateById(1)).ReturnsAsync(estate);
            _estateRepositoryMock.Setup(x => x.DeleteEstate(estate)).ReturnsAsync(true);

            // Act
            var result = await _estateService.DeleteEstate(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteEstate_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            _estateRepositoryMock.Setup(x => x.GetEstateById(1)).ReturnsAsync((Estate)null);

            // Act
            var result = await _estateService.DeleteEstate(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllEstates_ReturnsAllEstates()
        {
            // Arrange
            var estates = new List<Estate> {
            new Estate { Id = 1, Price = 100000, Address = "123 Main St" },
            new Estate { Id = 2, Price = 200000, Address = "456 Elm St" }
        };
            _estateRepositoryMock.Setup(x => x.GetAllEstates()).ReturnsAsync(estates);

            // Act
            var result = await _estateService.GetAllEstates();

            // Assert
            Assert.Equal(estates, result);
        }

        [Fact]
        public async Task GetEstateById_WithValidId_ReturnsEstate()
        {
            // Arrange
            var estate = new Estate { Id = 1, Price = 100000, Address = "123 Main St" };
            _estateRepositoryMock.Setup(x => x.GetEstateById(1)).ReturnsAsync(estate);

            // Act
            var result = await _estateService.GetEstateById(1);

            // Assert
            Assert.Equal(estate, result);
        }

        [Fact]
        public async Task GetEstateById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            _estateRepositoryMock.Setup(x => x.GetEstateById(1)).ReturnsAsync((Estate)null);

            // Act
            var result = await _estateService.GetEstateById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetEstates_ReturnsFilteredEstates()
        {
            // Arrange
            var estates = new List<Estate> {
            new Estate { Id = 1, Price = 100000, Address = "123 Main St", City = "New York", EstateType = "Apartment" },
        new Estate { Id = 2, Price = 200000, Address = "456 Elm St", City = "Los Angeles", EstateType = "House" },
        new Estate { Id = 3, Price = 300000, Address = "789 Oak St", City = "New York", EstateType = "House" }
            };
            var filter = new Filter { MinPrice = 150000, MaxPrice = 300000, City = "New York", Type = "House" };
            _estateRepositoryMock.Setup(x => x.GetEstates(filter.MinPrice, filter.MaxPrice, filter.City, filter.Type)).ReturnsAsync(estates.Where(e => e.Price >= filter.MinPrice && e.Price <= filter.MaxPrice && e.City == filter.City && e.EstateType == filter.Type));

            // Act
            var result = await _estateService.GetEstates(filter);

            // Assert
            Assert.Equal(estates.Where(e => e.Price >= filter.MinPrice && e.Price <= filter.MaxPrice && e.City == filter.City && e.EstateType == filter.Type), result);
        }

        [Fact]
        public async Task UpdateEstate_WithValidId_ReturnsTrue()
        {
            // Arrange
            var estate = new Estate { Id = 1, Price = 100000, Address = "123 Main St" };
            var newEstate = new Estate { Id = 1, Price = 150000, Address = "456 Elm St" };
            _estateRepositoryMock.Setup(x => x.GetEstateById(1)).ReturnsAsync(estate);

            // Act
            var result = await _estateService.UpdateEstate(1, newEstate);

            // Assert
            Assert.True(result);
            Assert.Equal(newEstate.Price, estate.Price);
            Assert.Equal(newEstate.Address, estate.Address);
            _estateRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateEstate_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            _estateRepositoryMock.Setup(x => x.GetEstateById(1)).ReturnsAsync((Estate)null);

            // Act
            var result = await _estateService.UpdateEstate(1, new Estate());

            // Assert
            Assert.False(result);
            _estateRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

    }
}