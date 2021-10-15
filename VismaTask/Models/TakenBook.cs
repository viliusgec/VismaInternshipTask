using System;

namespace VismaTask.Models
{
    class TakenBook
    {
        public string UserId { get; set; }
        public int TakenForDays { get; set; }
        public DateTime TakenWhen { get; set; }
        public Book Book { get; set; }

        public TakenBook()
        {
        }

        public TakenBook(string userId, int takenForDays, DateTime takenWhen, Book book)
        {
            UserId = userId;
            TakenForDays = takenForDays;
            TakenWhen = takenWhen;
            Book = book;
        }
    }
}
