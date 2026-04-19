using System;
using System.Collections.Generic;
using Domain.ValueObject;
using Domain.Cinema_Booking.Exceptions;

namespace Domain.Cinema_Booking
{
    public class User
    {
        public Guid Id { get; private set; }
        public Username Username { get; private set; }
        public Email Email { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private readonly List<Booking> _bookings = new List<Booking>();
        public IReadOnlyCollection<Booking> Bookings => _bookings;

        protected User() { }

        public User(Guid id, Username username, Email email, DateTime createdAt)
        {
            Id = id;
            Username = username ?? throw new ArgumentNullValueException(nameof(username));
            Email = email ?? throw new ArgumentNullValueException(nameof(email));
            CreatedAt = createdAt;
        }

        public Booking CreateBooking(Session session, Seat seat, DateTime expiresAt)
        {
            var booking = new Booking(Guid.NewGuid(), this, session, seat, expiresAt);
            _bookings.Add(booking);
            return booking;
        }
    }
}