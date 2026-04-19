using Domain.Cinema_Booking;
using Domain.ValueObject;
using System;
using System.Linq;
using static System.Collections.Specialized.BitVector32;

class Program
{
    static void Main() 
    {
        // Данные
        var user = new User(Guid.NewGuid(), new Username("Алексей"), new Email("alex@mail.ru"), DateTime.UtcNow);
        var movie = new Movie(Guid.NewGuid(), new MovieTitle("Аватар"), "Фантастика", 162, DateTime.UtcNow);
        var hall = new Hall(Guid.NewGuid(), "Зал 1", 30);
        var session = new Session(Guid.NewGuid(), movie, hall, DateTime.Now.AddHours(2), DateTime.Now.AddHours(4).AddMinutes(42));

        // Пользователь
        Console.WriteLine("=== ПОЛЬЗОВАТЕЛЬ ===");
        Console.WriteLine("1. Просмотреть фильмы: Аватар");
        Console.WriteLine("2. Просмотреть расписание: Аватар - " + DateTime.Now.AddHours(2).ToString("HH:mm"));
        Console.WriteLine("3. Выбрать сеанс: Аватар в " + DateTime.Now.AddHours(2).ToString("HH:mm"));
        Console.WriteLine("4. Просмотреть места: 1-1, 1-2, 1-3, 2-1, 2-2, 2-3");

        var seat = hall.Seats.First(s => s.Row == 2 && s.Number == 5);
        Console.WriteLine($"5. Выбрать место: {seat.SeatNumber.Value}");

        var booking = session.BookSeat(user, seat, DateTime.UtcNow.AddMinutes(10));
        Console.WriteLine($"6. Забронировать билет: ID {booking.Id.ToString().Substring(0, 8)}");

        Console.WriteLine("7. Отменить бронирование? (да/нет)");
        var answer = Console.ReadLine();
        if (answer == "да")
        {
            booking.Cancel();
            Console.WriteLine("Бронь отменена");
        }
        else
        {
            booking.Confirm();
            Console.WriteLine("Бронь подтверждена");
        }

        // Администратор
        Console.WriteLine("\n=== АДМИНИСТРАТОР ===");
        Console.WriteLine("1. Добавить фильм: Дюна 2");
        Console.WriteLine("2. Добавить сеанс: Дюна 2 на завтра 19:00");
        Console.WriteLine("3. Обновить сеанс: Дюна 2 перенесён на 20:00");
        Console.WriteLine("4. Удалить сеанс: Дюна 2");
        Console.WriteLine($"5. Просмотреть бронирования: {booking.User.Username.Value} - {booking.Session.Movie.Title.Value} - место {booking.Seat.SeatNumber.Value}");

        Console.ReadKey();
    }
}