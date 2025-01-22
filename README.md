# Rooms
Small console application for checking room availability and reservations.

## Usage
Example run command:
```
Rooms --hotels hotels.json --bookings bookings.json
```

Both _hotels_ and _bookings_ parameters are required and should point to JSON files of predetermined structure, containing hotel information, and booking information respectively.

Application will wait for user input in the form of preconfigured commands: Availability, Search.

Application exits when provided with empty line.

### Availability
Command returns a number of rooms available for reservation for a provided range of dates.
```
Availability(HotelID, Date_yyyyMMdd, RoomTypeID)
```
```
Availability(HotelID, yyyyMMdd-yyyyMMdd, RoomTypeID)
```

### Search
Command returns ranges of dates, in a given time frame expressed in days, together with number of rooms available for reservation within each range.
```
Search(HotelID, Days, RoomTypeID)
```
