using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using VismaTask.Models;
using VismaTask.Repositories;

namespace VismaTask.Repository
{
    public class BookRepository : IBookRepository
    {
        private static readonly string BooksDataFile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\BookData.json";
        private static readonly string TakenBooksDataFile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\TakenBooksData.json";

        public BookRepository()
        {

        }

        public Book AddBook(Book book)
        {
            List<Book> books = DeserializeBooks();
            books.Add(book);
            string newJsonString = JsonSerializer.Serialize(books);
            File.WriteAllText(BooksDataFile, newJsonString);
            return book;
        }

        public TakenBook AddTakenBook(TakenBook takenBook)
        {
            List<TakenBook> books = JsonSerializer.Deserialize<List<TakenBook>>(File.ReadAllText(TakenBooksDataFile));
            books.Add(takenBook);
            string newJsonString = JsonSerializer.Serialize(books);
            File.WriteAllText(TakenBooksDataFile, newJsonString);
            return takenBook;
        }

        public List<Book> GetAllBooksData()
        {
            return DeserializeBooks();
        }

        public List<TakenBook> GetAllTakenBooksData()
        {
            return DeserializeTakenBooks();
        }

        public void DeleteBookFromBooksDataFile(Book book)
        {
            List<Book> books = DeserializeBooks();
            books.RemoveAll(x => x.ISBN == book.ISBN);
            string newJsonString = JsonSerializer.Serialize(books);
            File.WriteAllText(BooksDataFile, newJsonString);
        }

        public void DeleteBookFromTakenBooksDataFile(TakenBook takenBook)
        {
            List<TakenBook> takenBooks = DeserializeTakenBooks();
            takenBooks.Remove(takenBook);
            string newJsonString = JsonSerializer.Serialize(takenBooks);
            File.WriteAllText(TakenBooksDataFile, newJsonString);
        }

        public int GetUserTakenBookAmmount(string userId)
        {
            List<TakenBook> takenBooks = DeserializeTakenBooks();
            return takenBooks.FindAll(x => x.UserId == userId).Count;
        }

        public Book GetBook(string ISBN)
        {
            List<Book> books = DeserializeBooks();
            return books.Find(x => x.ISBN == ISBN);
        }

        public TakenBook GetTakenBook(string userId, string ISBN)
        {
            List<TakenBook> books = DeserializeTakenBooks();
            return books.Find(x => x.Book.ISBN == ISBN && x.UserId == userId);
        }

        private List<Book> DeserializeBooks()
        {
            return File.Exists(BooksDataFile) ? JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(BooksDataFile)) : new List<Book>();
        }

        private List<TakenBook> DeserializeTakenBooks()
        {
            return File.Exists(TakenBooksDataFile) ? JsonSerializer.Deserialize<List<TakenBook>>(File.ReadAllText(TakenBooksDataFile)) : new List<TakenBook>();
        }
    }
}
