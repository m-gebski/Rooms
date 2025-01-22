using System.Globalization;

namespace Rooms.Commands
{
    public abstract class BasicCommand
    {
        internal bool TryGetParameters(string inputParameters, out (string hotelId, DateTime startDate, DateTime endDate, string roomType) parsedParameters)
        {
            var commandParams = inputParameters.Split(',', StringSplitOptions.TrimEntries);
            if (commandParams == null || commandParams.Length != 3)
            {
                parsedParameters = default;
                return false;
            }

            var dates = ParseDateParameters(commandParams[1]);

            parsedParameters.hotelId = commandParams[0];
            parsedParameters.startDate = dates.start;
            parsedParameters.endDate = dates.end;
            parsedParameters.roomType = commandParams[2];

            return true;
        }

        internal abstract (DateTime start, DateTime end) ParseDateParameters(string rawValue);
    }
}
