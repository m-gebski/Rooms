using Moq;
using Rooms;
using Rooms.Commands;
using Rooms.Model;
using System.Globalization;
using System.Linq;

namespace RoomsTests.CommandTests
{
    public class SearchTests
    {
        private Search _search;
        public SearchTests()
        {
            _search = new Search();
        }

        [Theory]
        [InlineData(2, 7, 12, 17, 1, 2)]
        [InlineData(2, 7, 12, 17, 4, 2, 1)]
        [InlineData(2, 7, 12, 17, 7, 2, 1)]
        [InlineData(2, 7, 12, 17, 9, 2, 1, 2)]
        [InlineData(2, 7, 12, 17, 12, 2, 1, 2)]
        [InlineData(-1, 4, 9, 14, 2, 1)]
        [InlineData(-1, 4, 9, 14, 6, 1, 2)]
        [InlineData(-1, 4, 9, 14, 11, 1, 2, 1)]
        [InlineData(-1, 4, 9, 14, 16, 1, 2, 1, 2)]
        public void Execute_Should_Return_Correct_Search(int arrivalOffset1, int departureOffset1, int arrivalOffset2, int departureOffset2, int searchRange, params int[] expectedCounts)
        {
            // Setup
            var inputParams = $"H1, {searchRange}, SGL";
            var sampleHotel = new Hotel()
            {
                Id = "H1",
                Rooms = [
                    new Room()
                    {
                        RoomId="1",
                        RoomType="SGL"
                    },
                    new Room()
                    {
                        RoomId="2",
                        RoomType="SGL"
                    }],
                RoomTypes = [
                    new RoomType()
                    {
                        Code = "SGL"
                    }]
            };
            var today = DateTime.Today;
            var bookings = new List<Booking> {
                new()
                {
                    HotelId = "H1",
                    RoomType = "SGL",
                    Arrival = today.AddDays(arrivalOffset1),
                    Departure = today.AddDays(departureOffset1)
                },
                new Booking()
                {
                    HotelId = "H1",
                    RoomType = "SGL",
                    Arrival = today.AddDays(arrivalOffset2),
                    Departure = today.AddDays(departureOffset2)
                }
            };

            // Act
            _search.Execute(inputParams, [sampleHotel], bookings);

            // Assert
            Assert.NotEmpty(_search.ResultSeries);
            Assert.Equal(expectedCounts.Length, _search.ResultSeries.Count);

            var searchValues = _search.ResultSeries.Values.Select(v => v.free);
            Assert.Equal(expectedCounts, searchValues);
        }

        [Theory]
        [InlineData("20240101","20240101", 1, "(20240101, 1)")]
        [InlineData("20240101", "20240103", 10, "(20240101-20240103, 10)")]
        public void GetFormattedOutput_SingleDate_Should_Return_String(string arrivalDate, string departureDate, int count, string expectedResult)
        {
            // Setup
            _search.ResultSeries =
                new Dictionary<DateTime, (DateTime date, int free)>()
                {
                    { DateTime.ParseExact(arrivalDate, Consts.DateFormat, CultureInfo.InvariantCulture),
                        (DateTime.ParseExact(departureDate, Consts.DateFormat, CultureInfo.InvariantCulture), count) }
                };

            // Act
            var result = _search.GetFormattedOutput();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true, ignoreAllWhiteSpace: true);
        }

        [Theory]
        [InlineData("20240101", "20240101", "20240103", "20240103", 1, "(20240101, 1), (20240103, 1)")]
        [InlineData("20240101", "20240103", "20240102", "20240102", 1, "(20240101-20240103, 1), (20240102, 1)")]
        [InlineData("20240101", "20240103", "20240104", "20240105", 10, "(20240101-20240103, 10), (20240104-20240105, 10)")]
        [InlineData("20240101", "20240101", "20240104", "20240105", 10, "(20240101, 10), (20240104-20240105, 10)")]
        public void GetFormattedOutput_MultipleDates_Should_Return_String(string arrivalDate1, string departureDate1, string arrivalDate2, string departureDate2, int count, string expectedResult)
        {
            // Setup
            _search.ResultSeries =
                new Dictionary<DateTime, (DateTime date, int free)>()
                {
                    { DateTime.ParseExact(arrivalDate1, Consts.DateFormat, CultureInfo.InvariantCulture),
                        (DateTime.ParseExact(departureDate1, Consts.DateFormat, CultureInfo.InvariantCulture), count) },
                    { DateTime.ParseExact(arrivalDate2, Consts.DateFormat, CultureInfo.InvariantCulture),
                        (DateTime.ParseExact(departureDate2, Consts.DateFormat, CultureInfo.InvariantCulture), count) }
                };

            // Act
            var result = _search.GetFormattedOutput();

            // Assert
            Assert.Equal(expectedResult, result, ignoreLineEndingDifferences: true, ignoreAllWhiteSpace: true);
        }
    }
}
