using SearchEngine.Indexing;
using Moq;

namespace SearchEngine.Indexing.Tests
{
    public class IndexServiceTests
    {
        [Fact]
        public void Constructor_Should_Initialize_With_EventHandler()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();

            // Act
            var indexService = new IndexService(mockEventHandler.Object);

            // Assert
            Assert.NotNull(indexService);
        }

        [Fact]
        public void Constructor_Should_Accept_Null_EventHandler()
        {
            // Act - The current implementation doesn't validate null, so it should not throw
            var indexService = new IndexService(null!);

            // Assert
            Assert.NotNull(indexService);
        }

        [Fact]
        public async Task IndexDocumentAsync_Should_Call_EventHandler()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();
            var indexService = new IndexService(mockEventHandler.Object);
            var documentId = Guid.NewGuid();

            // Act
            await indexService.IndexDocumentAsync(documentId);

            // Assert
            mockEventHandler.Verify(
                handler => handler.HandleAsync(documentId),
                Times.Once
            );
        }

        [Fact]
        public async Task IndexDocumentAsync_Should_Pass_Correct_DocumentId()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();
            var indexService = new IndexService(mockEventHandler.Object);
            var documentId = Guid.NewGuid();

            // Act
            await indexService.IndexDocumentAsync(documentId);

            // Assert
            mockEventHandler.Verify(
                handler => handler.HandleAsync(It.Is<Guid>(id => id == documentId)),
                Times.Once
            );
        }

        [Fact]
        public async Task IndexDocumentAsync_Should_Handle_Multiple_Calls()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();
            var indexService = new IndexService(mockEventHandler.Object);
            var documentId1 = Guid.NewGuid();
            var documentId2 = Guid.NewGuid();

            // Act
            await indexService.IndexDocumentAsync(documentId1);
            await indexService.IndexDocumentAsync(documentId2);

            // Assert
            mockEventHandler.Verify(
                handler => handler.HandleAsync(documentId1),
                Times.Once
            );
            mockEventHandler.Verify(
                handler => handler.HandleAsync(documentId2),
                Times.Once
            );
        }

        [Fact]
        public async Task IndexDocumentAsync_Should_Handle_Empty_Guid()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();
            var indexService = new IndexService(mockEventHandler.Object);
            var documentId = Guid.Empty;

            // Act
            await indexService.IndexDocumentAsync(documentId);

            // Assert
            mockEventHandler.Verify(
                handler => handler.HandleAsync(documentId),
                Times.Once
            );
        }

        [Fact]
        public async Task IndexDocumentAsync_Should_Propagate_EventHandler_Exceptions()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();
            var indexService = new IndexService(mockEventHandler.Object);
            var documentId = Guid.NewGuid();
            var expectedException = new InvalidOperationException("Test exception");

            mockEventHandler
                .Setup(handler => handler.HandleAsync(documentId))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => indexService.IndexDocumentAsync(documentId)
            );
            Assert.Equal("Test exception", exception.Message);
        }

        [Fact]
        public async Task IndexDocumentAsync_Should_Handle_Concurrent_Calls()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();
            var indexService = new IndexService(mockEventHandler.Object);
            var documentIds = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid()).ToArray();

            // Act
            var tasks = documentIds.Select(id => indexService.IndexDocumentAsync(id));
            await Task.WhenAll(tasks);

            // Assert
            foreach (var documentId in documentIds)
            {
                mockEventHandler.Verify(
                    handler => handler.HandleAsync(documentId),
                    Times.Once
                );
            }
        }

        [Fact]
        public async Task IndexDocumentAsync_Should_Complete_Successfully()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();
            var indexService = new IndexService(mockEventHandler.Object);
            var documentId = Guid.NewGuid();

            mockEventHandler
                .Setup(handler => handler.HandleAsync(documentId))
                .Returns(Task.CompletedTask);

            // Act
            var task = indexService.IndexDocumentAsync(documentId);

            // Assert
            Assert.NotNull(task);
            await task; // Should complete without exception
            Assert.True(task.IsCompletedSuccessfully);
        }

        [Fact]
        public async Task IndexDocumentAsync_Should_Handle_EventHandler_Delay()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();
            var indexService = new IndexService(mockEventHandler.Object);
            var documentId = Guid.NewGuid();

            mockEventHandler
                .Setup(handler => handler.HandleAsync(documentId))
                .Returns(async () =>
                {
                    await Task.Delay(100); // Simulate some processing time
                });

            // Act
            var startTime = DateTime.UtcNow;
            await indexService.IndexDocumentAsync(documentId);
            var endTime = DateTime.UtcNow;

            // Assert
            Assert.True(endTime - startTime >= TimeSpan.FromMilliseconds(100));
            mockEventHandler.Verify(
                handler => handler.HandleAsync(documentId),
                Times.Once
            );
        }

        [Fact]
        public async Task IndexDocumentAsync_Should_Handle_Same_DocumentId_Multiple_Times()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();
            var indexService = new IndexService(mockEventHandler.Object);
            var documentId = Guid.NewGuid();

            // Act
            await indexService.IndexDocumentAsync(documentId);
            await indexService.IndexDocumentAsync(documentId);
            await indexService.IndexDocumentAsync(documentId);

            // Assert
            mockEventHandler.Verify(
                handler => handler.HandleAsync(documentId),
                Times.Exactly(3)
            );
        }

        [Fact]
        public void IndexService_Should_Implement_IIndexService()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();

            // Act
            var indexService = new IndexService(mockEventHandler.Object);

            // Assert
            Assert.IsAssignableFrom<IIndexService>(indexService);
        }

        [Fact]
        public async Task IndexDocumentAsync_Should_Return_Task()
        {
            // Arrange
            var mockEventHandler = new Mock<IDocumentIndexedEventHandler>();
            var indexService = new IndexService(mockEventHandler.Object);
            var documentId = Guid.NewGuid();

            // Act
            var result = indexService.IndexDocumentAsync(documentId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Task>(result); // Task<T> is assignable from Task
            await result; // Ensure it completes
        }
    }
}
