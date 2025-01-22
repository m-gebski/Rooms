using CommandLine;
using Rooms.Handlers;
using Rooms.Model;

namespace Rooms
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var fileHandler = new FileHandler();
                Parser.Default.ParseArguments<Options>(args).WithParsed(opt =>
                {
                    var hotelInfo = fileHandler.ReadJsonFile<List<Hotel>>(opt.HotelFile);
                    var bookingInfo = fileHandler.ReadJsonFile<List<Booking>>(opt.BookingFile);

                    while (true)
                    {
                        var input = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(input))
                        {
                            break;
                        }

                        var response = CommandHandler.ParseCommandInput(input, hotelInfo, bookingInfo);

                        Console.WriteLine(response);
                    }
                });
            }
            catch
            {
                Console.WriteLine("Press ENTER to exit");
                Console.ReadLine();
            }
        }
    }
}
