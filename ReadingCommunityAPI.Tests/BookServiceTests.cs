using Xunit;
using Moq;
using ReadingCommunityApi.Application.Services;
using ReadingCommunityApi.Core.Interfaces;
using ReadingCommunityApi.Application.Interfaces.mappers;
using ReadingCommunityApi.Application.Dtos;
using ReadingCommunityApi.Core.Models;
using ReadingCommunityApi.Application.Interfaces;

namespace ReadingCommunityAPI.Tests;
public class BookServiceTests
{
    private readonly Mock<IBookRepository> _mockBookRepo;
    private readonly Mock<IAuthorRepository> _mockAuthorRepo;
    private readonly Mock<IBookMapper> _mockMapper;
    private readonly Mock<ICacheService> _mockCache;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _mockBookRepo = new Mock<IBookRepository>();
        _mockAuthorRepo = new Mock<IAuthorRepository>();
        _mockMapper = new Mock<IBookMapper>();
        _mockCache = new Mock<ICacheService>();
        
        _bookService = new BookService(
            _mockBookRepo.Object, 
            _mockAuthorRepo.Object, 
            _mockMapper.Object,
            _mockCache.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnSuccess_WhenAuthorExists()
    {
        // Arrange
        var bookDto = new BookCreateDTO { AuthorId = 1, Title = "The Test Book", Description= "Book test", CoverImageUrl= "", Depth = 1, Height = 1, NumberOfPages = 10, PublicationDate=new DateTime(), Width=10 };
        var author = new Author("Test Author", "...", new DateTime(1950, 1, 1), "Fictional", "test.jpg");
        // Use reflection to set the private set Id
        typeof(Author).GetProperty("Id").SetValue(author, 1);

        var bookEntity = new Book("The Test Book", "Test", new DateTime(), 1, 10, 1, 1, 10, "");
        // Use reflection to set the private set Id
        typeof(Book).GetProperty("Id").SetValue(bookEntity, 5);
        
        var detailDto = new BookDetailDTO { Id = 5, Title = "The Test Book Detail" };

        // 1. Setup AuthorRepo to return an Author
        _mockAuthorRepo
            .Setup(r => r.GetByIdAsync(bookDto.AuthorId))
            .ReturnsAsync(author);
        
        // 2. Setup Mapper to convert DTO -> Entity
        _mockMapper
            .Setup(m => m.MapToEntity(bookDto))
            .Returns(bookEntity);

        // 3. Setup BookRepo.AddAsync to return the created entity (usually witQh a generated ID)
        _mockBookRepo
            .Setup(r => r.AddAsync(bookEntity))
            .ReturnsAsync(bookEntity);

        // 4. Setup BookRepo.GetByIdAsync to return the created entity again
        _mockBookRepo
            .Setup(r => r.GetByIdAsync(bookEntity.Id))
            .ReturnsAsync(bookEntity);

        // 5. Setup Mapper to convert Entity -> Detail DTO
        _mockMapper
            .Setup(m => m.MapToDetailDto(bookEntity))
            .Returns(detailDto);

        // Act
        var result = await _bookService.AddAsync(bookDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(detailDto.Title, result.Data.Title);

        _mockAuthorRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
        _mockBookRepo.Verify(r => r.AddAsync(bookEntity), Times.Once);
        _mockBookRepo.Verify(r => r.GetByIdAsync(5), Times.Once);
    }
}
