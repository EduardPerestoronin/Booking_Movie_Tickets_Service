using System;
using System.Collections.Generic;
using System.Linq;
using Domain.ValueObject;
using Domain.Cinema_Booking.Exceptions;

namespace Domain.Cinema_Booking
{
    public interface IClock
    {
        DateTime Now { get; }
    }

    public class SystemClock : IClock
    {
        public DateTime Now => DateTime.Now;
    }

    public class Session
    {
        public Guid Id { get; private set; }
        public Movie Movie { get; private set; }
        public Hall Hall { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        private readonly IClock _clock;

        private readonly List<Booking> _bookings = new();
        public IReadOnlyCollection<Booking> Bookings => _bookings.AsReadOnly();

        protected Session() { }

        public Session(
            Guid id,
            Movie movie,
            Hall hall,
            DateTime startTime,
            DateTime endTime,
            IClock clock)
        {
            Id = id;
            Movie = movie ?? throw new ArgumentNullException(nameof(movie));
            Hall = hall ?? throw new ArgumentNullException(nameof(hall));

            if (endTime <= startTime)
                throw new ArgumentException("EndTime must be after StartTime");

            StartTime = startTime;
            EndTime = endTime;

            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public bool IsSeatAvailable(Seat seat)
        {
            return !_bookings.Any(b =>
                b.Seat.Id == seat.Id &&
                b.IsActive());
        }

        public Booking BookSeat(User user, Seat seat, DateTime expiresAt)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (seat == null) throw new ArgumentNullException(nameof(seat));

            if (_clock.Now >= StartTime)
                throw new SessionAlreadyStartedException(Id, StartTime);

            if (!IsSeatAvailable(seat))
                throw new SeatAlreadyBookedException(Id, seat.SeatNumber.Value);

            var booking = new Booking(Guid.NewGuid(), user, this, seat, expiresAt);
            _bookings.Add(booking);

            return booking;
        }
    }
}