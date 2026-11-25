# Co-working Space Manager

A small WinForms (C#) desktop app with a SQLite database, built with the MVC pattern.

## Run it
1. Install the .NET 8 SDK (or newer) and Visual Studio 2022 with the ".NET desktop development" workload.
2. Open `CoworkingApp.csproj` (or the folder) in Visual Studio and press F5.
   Or from a terminal: `dotnet run`
   (NuGet restores the SQLite package automatically.)
3. The database file `coworking.db` is created next to the .exe on first launch.

If you use a newer .NET, change `net8.0-windows` in the .csproj to match (e.g. `net10.0-windows`).

## Structure (MVC)
- `Models/`      Member, Desk, Booking (plain data classes) + Database.cs (opens SQLite, creates tables)
- `Views/`       MainForm, MembersForm, DesksForm, BookingsForm (the GUI)
- `Controllers/` MemberController, DeskController, BookingController (all SQL + validation)

Views never touch SQL; they call a controller and show the result.

## Features
- Members: add / edit / delete
- Desks and rooms: add / edit / delete (type + price per day)
- Bookings: choose member, desk and date; a desk cannot be booked twice on the same date
- Deleting a member or desk also deletes its bookings
