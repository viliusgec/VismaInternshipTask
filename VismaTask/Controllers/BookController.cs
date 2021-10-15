using System;
using System.Collections.Generic;
using VismaTask.Models;
using VismaTask.Services;

namespace VismaTask.Controllers
{
    class BookController
    {
        private readonly BookService bookService = new BookService();
        public BookController()
        {

        }

        public void AddBook()
        {
            Console.WriteLine("Type book name:");
            var bookName = Console.ReadLine();
            Console.WriteLine("Type book author name:");
            var bookAuthor = Console.ReadLine();
            Console.WriteLine("Type book category:");
            var bookCategory = Console.ReadLine();
            Console.WriteLine("Type book language:");
            var bookLanguage = Console.ReadLine();
            Console.WriteLine("Type book publication date (YYYY-MM-DD):");
            var bookPublicationDate = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Type book ISBN:");
            var bookISBN = Console.ReadLine();
            bookService.AddBook(bookName, bookAuthor, bookCategory, bookLanguage, bookPublicationDate, bookISBN);
        }

        public void TakeBook()
        {
            Console.WriteLine("Type your user ID:");
            var userId = Console.ReadLine();
            Console.WriteLine("Type book ISBN:");
            var bookISBN = Console.ReadLine();
            Console.WriteLine("For how many days you want to take this book?:");
            var takeForDays = Console.ReadLine();
            PrintBook(bookService.TakeBook(userId, bookISBN, takeForDays));
        }

        public void ReturnBook()
        {
            Console.WriteLine("Type your user ID:");
            var userId = Console.ReadLine();
            Console.WriteLine("Type book ISBN:");
            var bookISBN = Console.ReadLine();
            PrintBook(bookService.ReturnBook(userId, bookISBN));
        }

        public void GetBookList()
        {
            Console.WriteLine("Type filter (if none - leave empty):");
            Console.WriteLine("Available filters: name, author, category, language, ISBN, available, taken");
            var filter = Console.ReadLine();
            if (filter == string.Empty)
            {
                PrintBooks(bookService.GetAllBooks());
                return;
            }
            Console.WriteLine("Enter value");
            var value = Console.ReadLine();
            PrintBooks(bookService.ReturnFilteredData(filter.ToLower(), value.ToLower()));
        }

        public void DeleteBook()
        {
            Console.WriteLine("Type book ISBN:");
            var bookISBN = Console.ReadLine();
            PrintBook(bookService.DeleteBook(bookISBN));
        }

        public static void PrintBooks(List<Book> books)
        {
            if(books != null)
            {
                Console.WriteLine("| Name               | Author              | Category            | Language             |Publication date| ISBN ");
                foreach (var book in books)
                {
                    Console.WriteLine($"| {book.Name, -18} | {book.Author, -18}  | {book.Category, -18}  | {book.Language, -18}   | {book.PublicationDate.ToString("yyyy-MM-dd"), -14} | {book.ISBN, -10}");
                }
            }
        }

        public static void PrintBook(Book book)
        {
            if (book != null)
            {
                Console.WriteLine("| Name               | Author              | Category            | Language             |Publication date| ISBN ");
                Console.WriteLine($"| {book.Name,-18} | {book.Author,-18}  | {book.Category,-18}  | {book.Language,-18}   | {book.PublicationDate.ToString("yyyy-MM-dd"),-14} | {book.ISBN,-10}");
            }
        }
    }
}