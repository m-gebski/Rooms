using Rooms.Model;

namespace Rooms.Commands
{
    public interface ICommand
    {
        void Execute(string inputParams, IEnumerable<Hotel> hotelData, IEnumerable<Booking> bookingData);
        string GetFormattedOutput();
    }
}
