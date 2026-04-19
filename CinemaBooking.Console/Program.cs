using Domain.Cinema_Booking;
using Domain.ValueObject;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        IClock clock = new SystemClock();

        var user = new User(Guid.NewGuid(),
            new Username("Алексеев"),
            new Email("alex@mail.ru"),
            DateTime.UtcNow);

        var movie = new Movie(Guid.NewGuid(),
            new MovieTitle("Аватар"),
            "Фантастика",
            162,
            DateTime.UtcNow);

        var hall = new Hall(Guid.NewGuid(), "Зал 1", 30);

        //сеанс в будущем
        var session = new Session(
            Guid.NewGuid(),
            movie,
            hall,
            DateTime.Now.AddHours(1),
            DateTime.Now.AddHours(3),
            clock
        );

        Console.WriteLine("=== ПОЛЬЗОВАТЕЛЬ ===");

        //безопасный выбор места
        var seat = hall.Seats.FirstOrDefault(s => s.Row == 2 && s.Number == 2);

        if (seat == null)
        {
            Console.WriteLine("Место не найдено");
            return;
        }

        Console.WriteLine($"Выбрано место: {seat.SeatNumber.Value}");

        try
        {
            var booking = session.BookSeat(
                user,
                seat,
                DateTime.UtcNow.AddMinutes(10)
            );

            Console.WriteLine($"Бронь создана: {booking.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(" Ошибка: " + ex.Message);
        }

        Console.WriteLine("7. Отменить бронирование? (да/нет)");
        var answer = Console.ReadLine();

        if (answer == "да")
        {
            session.Bookings.First().Cancel();
            Console.WriteLine("Бронь отменена");
        }
        else
        {
            session.Bookings.First().Confirm();
            Console.WriteLine("Бронь подтверждена");
        }

        Console.WriteLine("\n=== АДМИНИСТРАТОР ===");

        Console.WriteLine(
            $"Бронирование: {bookingInfo(session)}"
        );

        Console.ReadKey();
    }

    static string bookingInfo(Session session)
    {
        var b = session.Bookings.FirstOrDefault();
        if (b == null) return "нет бронирований";

        return $"{b.User.Username.Value} - {b.Session.Movie.Title.Value} - {b.Seat.SeatNumber.Value}";
    }
}