using Rooms.Handlers;
using Rooms.Model;

namespace RoomsTests.HandlerTests
{
    public class FileHandlerTests
    {
        private readonly FileHandler _fileHandler;

        public FileHandlerTests()
        {
            _fileHandler = new FileHandler();
        }

        [Fact]
        public void ReadJsonFile_NoFile_Should_Throw_Exception()
        {
            Assert.Throws<FileNotFoundException>(() =>
            {
                _fileHandler.ReadJsonFile<string>("IDontExist");
            });
        }

        [Fact]
        public void ReadJsonFile_EmptyFile_Should_Throw_Exception()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _fileHandler.ReadJsonFile<string>("TestResources/EmptyFile.txt");
            });
        }

        [Fact]
        public void ReadJsonFile_Should_Read_Hotels_Correctly()
        {
            var result = _fileHandler.ReadJsonFile<List<Hotel>>("TestResources/hotels.json");
            
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);

            Assert.DoesNotContain(result, h => string.IsNullOrEmpty(h.Id));
            Assert.DoesNotContain(result, h => string.IsNullOrEmpty(h.Name));
            Assert.DoesNotContain(result, h => h.RoomTypes.Count == 0);
            Assert.DoesNotContain(result, h => h.Rooms.Count == 0);
        }

        [Fact]
        public void ReadJsonFile_Should_Read_Bookings_Correctly()
        {
            var result = _fileHandler.ReadJsonFile<List<Booking>>("TestResources/bookings.json");

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);

            Assert.DoesNotContain(result, b => string.IsNullOrEmpty(b.HotelId));
            Assert.DoesNotContain(result, b => string.IsNullOrEmpty(b.HotelId));
            Assert.DoesNotContain(result, b => string.IsNullOrEmpty(b.HotelId));
            Assert.DoesNotContain(result, b => string.IsNullOrEmpty(b.HotelId));
            Assert.DoesNotContain(result, b => string.IsNullOrEmpty(b.HotelId));
        }
    }
}
