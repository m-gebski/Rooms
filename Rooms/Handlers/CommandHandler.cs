using Rooms.Commands;
using Rooms.Model;
using System.Text.RegularExpressions;

namespace Rooms.Handlers
{
    public static class CommandHandler
    {
        public static string ParseCommandInput(string input, List<Hotel> hotels, List<Booking> bookings)
        {
            var matches = Regex.Match(input, Consts.CommandInputRegex);
            if (matches.Success)
            {
                var command = GetCommand(matches.Groups[1].Value);

                if(command == null)
                {
                    return string.Empty;
                }

                command.Execute(matches.Groups[2].Value, hotels, bookings);
                return command.GetFormattedOutput();
            }
            else
            {
                return string.Empty;
            }
        }

        private static ICommand? GetCommand(string keyword)
        {
            switch (keyword)
            {
                case Keywords.Availability:
                    return new Availability();
                case Keywords.Search:
                    return new Search();
                default:
                    return null;
            }
        }
    }
}
