using System.Collections.Generic;
using VismaTask.Models;

namespace VismaTask.Repositories
{
    public interface IBookRepository
    {
        public Book AddBook(Book book);
        public TakenBook AddTakenBook(TakenBook takenBook);
        public List<Book> GetAllBooksData();
        public List<TakenBook> GetAllTakenBooksData();
        public void DeleteBookFromBooksDataFile(Book book);
        public void DeleteBookFromTakenBooksDataFile(TakenBook takenBook);
        public int GetUserTakenBookAmmount(string userId);
        public Book GetBook(string ISBN);
        public TakenBook GetTakenBook(string userId, string ISBN);
    }
}
