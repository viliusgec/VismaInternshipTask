using System;
using System.Collections.Generic;
using System.Linq;
using VismaTask.Models;
using VismaTask.Repository;

namespace VismaTask.Services
{
    class BookService
    {
        private readonly BookRepository bookRepository = new BookRepository();
        private readonly string[] filters = { "name", "author", "category", "language", "isbn", "taken", "available" };
        public void AddBook(string name, string author, string category, string language, DateTime publicationdate, string ISBN)
        {
            if(name == string.Empty || author == string.Empty || category == string.Empty || language == string.Empty || ISBN == string.Empty)
            {
                Console.WriteLine("Fields can not be empty");
                return;
            }    
            var bookRepository = new BookRepository();
            Book book = new(name, author, category, language, publicationdate, ISBN);
            bookRepository.AddBook(book);
            Console.WriteLine("Book added");
        }

        public Book TakeBook(string userId, string ISBN, string takeForDays)
        {
            if (userId == string.Empty || ISBN == string.Empty || takeForDays == string.Empty)
            {
                Console.WriteLine("Fields can not be empty");
                return null;
            }
            Book book = bookRepository.GetBook(ISBN);
            if (book == null)
            {
                Console.WriteLine("Book does not exist.");
                return null;
            }
            if (int.Parse(takeForDays) > 60)
            {
                Console.WriteLine("You can only take book for less than 2months (60days).");
                return null;
            }
            if (bookRepository.GetUserTakenBookAmmount(userId) >= 3)
            {
                Console.WriteLine("You can only take 3 books.");
                return null;
            }
            bookRepository.DeleteBookFromBooksDataFile(book);
            bookRepository.AddTakenBook(new TakenBook(userId, int.Parse(takeForDays), DateTime.UtcNow, book));
            Console.WriteLine("Book received:");
            return book;
        }

        public Book ReturnBook(string userId, string ISBN)
        {
            if (userId == string.Empty || ISBN == string.Empty)
            {
                Console.WriteLine("Fields can not be empty");
                return null;
            }
            var takenBook = bookRepository.GetTakenBook(userId, ISBN);
            if (takenBook == null)
            {
                Console.WriteLine("Wrong ISBN.");
                return null;
            }
            if (takenBook.TakenWhen.AddDays(Convert.ToDouble(takenBook.TakenForDays)) < DateTime.UtcNow)
            {
                Console.WriteLine("Next time return book on time.");
            }
            bookRepository.DeleteBookFromTakenBooksDataFile(takenBook);
            bookRepository.AddBook(takenBook.Book);
            Console.WriteLine("Book returned successfully:");
            return takenBook.Book;
        }

        public List<Book> ReturnFilteredData(string filter, string value)
        {
            if (!filters.Contains(filter))
            {
                Console.WriteLine("Filter invalid.");
                return null;
            }
            var books = bookRepository.GetAllBooksData();
            books = MapTakenBooksToExistingBooks(books, bookRepository.GetAllTakenBooksData());
            books = GetFilteredBookList(books, filter, value);
            return books;
        }

        public List<Book> GetAllBooks()
        {
            var books = bookRepository.GetAllBooksData();
            books = MapTakenBooksToExistingBooks(books, bookRepository.GetAllTakenBooksData());
            return books;
        }

        public Book DeleteBook(string ISBN)
        {
            if (ISBN == string.Empty)
            {
                Console.WriteLine("Fields can not be empty");
                return null;
            }
            Book book = bookRepository.GetBook(ISBN);
            if (book == null)
            {
                Console.WriteLine("Book does not exist.");
                return null;
            }
            bookRepository.DeleteBookFromBooksDataFile(book);
            Console.WriteLine("Book deleted successfully:");
            return book;
        }

        public List<Book> GetFilteredBookList(List<Book> books, string filter, string value)
        {
            switch (filter)
            {
                case "name":
                    return books.FindAll(x => x.Name == value);
                case "author":
                    return books.FindAll(x => x.Author == value);   
                case "category":
                    return books.FindAll(x => x.Category == value);
                case "language":
                    return books.FindAll(x => x.Language == value);
                case "isbn":
                    return books.FindAll(x => x.ISBN == value);
                case "available":
                    return bookRepository.GetAllBooksData();
                case "taken":
                    return MapTakenBooksToBooks(bookRepository.GetAllTakenBooksData());
                default:
                    return null;
            }
        }

        public static List<Book> MapTakenBooksToBooks(List<TakenBook> takenBooks)
        {
            var books = new List<Book>();
            foreach (var takenBook in takenBooks)
                books.Add(takenBook.Book);
            return books;
        }

        public static List<Book> MapTakenBooksToExistingBooks(List<Book> books, List<TakenBook> takenBooks)
        {
            foreach (var takenBook in takenBooks)
                books.Add(takenBook.Book);
            return books;
        }
    }
}
