using System.Globalization;
using Moq;
using Rooms.Commands;
using Rooms.Model;

namespace RoomsTests.CommandTests
{
    public class AvailabilityTests
    {
        private readonly Availability _availability;

        public AvailabilityTests()
        {
            _availability = new Availability();
        }

        [Theory]
        [InlineData("20241005","20241008", 1)]
        [InlineData("20241005", "20241010", 1)]
        [InlineData("20241005", "20241012", 0)]
        [InlineData("20241010", "20241020", 0)]
        [InlineData("20241010", "20241012", 0)]
        [InlineData("20241012", "20241013", 0)]
        [InlineData("20241012", "20241015", 0)]
        [InlineData("20241010", "20241015", 0)]
        [InlineData("20241005", "20241020", 0)]
        [InlineData("20241005", "20241015", 0)]
        [InlineData("20241012", "20241020", 0)]
        [InlineData("20241015", "20241020", 1)]
        [InlineData("20241018", "20241020", 1)]
        public void Execute_Should_Return_Correct_Availability(string arrival, string departure, int expectedResult)
        {
            // Setup
            var inputParams = "H1, 20241010-20241015, SGL";
            var sampleHotel = new Hotel()
            {
                Id = "H1",
                Rooms = [
                    new Room()
                    {
                        RoomId="1",
                        RoomType="SGL"
                    }],
                RoomTypes = [
                    new RoomType()
                    {
                        Code = "SGL"
                    }]
            };
            var booking = new Booking()
            {
                HotelId = "H1",
                RoomType = "SGL",
                Arrival = DateTime.ParseExact(arrival, "yyyyMMdd", CultureInfo.InvariantCulture),
                Departure = DateTime.ParseExact(departure, "yyyyMMdd", CultureInfo.InvariantCulture)
            };

            // Act
            _availability.Execute(inputParams, [sampleHotel], [booking]);

            // Assert
            Assert.Equal(expectedResult, _availability.AvailabilityResult);
        }

        [Theory]
        [InlineData(int.MinValue, "")]
        [InlineData(-10, "-10")]
        [InlineData(0, "0")]
        [InlineData(10, "10")]
        [InlineData(int.MaxValue, "2147483647")]
        public void GetFormattedOutput_Should_Return_String(int count, string expectedResult)
        {
            // Setup
            _availability.AvailabilityResult = count;

            // Act
            var result = _availability.GetFormattedOutput();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true, ignoreAllWhiteSpace: true);
        }
    }
}
