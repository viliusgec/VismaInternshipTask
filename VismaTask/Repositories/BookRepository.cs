using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using VismaTask.Models;

namespace VismaTask.Repository
{
    class BookRepository
    {
        private static readonly string BooksDataFile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\BookData.json";
        private static readonly string TakenBooksDataFile = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Data\TakenBooksData.json";

        public BookRepository()
        {

        }

        public void AddBook(Book book)
        {
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(BooksDataFile));
            books.Add(book);
            string newJsonString = JsonSerializer.Serialize(books);
            File.WriteAllText(BooksDataFile, newJsonString);
        }

        public void AddTakenBook(TakenBook takenBook)
        {
            List<TakenBook> books = JsonSerializer.Deserialize<List<TakenBook>>(File.ReadAllText(TakenBooksDataFile));
            books.Add(takenBook);
            string newJsonString = JsonSerializer.Serialize(books);
            File.WriteAllText(TakenBooksDataFile, newJsonString);
        }

        public List<Book> GetAllBooksData()
        {
            return JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(BooksDataFile));
        }

        public List<TakenBook> GetAllTakenBooksData()
        {
            return JsonSerializer.Deserialize<List<TakenBook>>(File.ReadAllText(TakenBooksDataFile));
        }

        public void DeleteBookFromBooksDataFile(Book book)
        {
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(BooksDataFile));
            books.RemoveAll(x => x.ISBN == book.ISBN);
            string newJsonString = JsonSerializer.Serialize(books);
            File.WriteAllText(BooksDataFile, newJsonString);
        }

        public void DeleteBookFromTakenBooksDataFile(TakenBook takenBook)
        {
            List<TakenBook> takenBooks = JsonSerializer.Deserialize<List<TakenBook>>(File.ReadAllText(TakenBooksDataFile));
            takenBooks.Remove(takenBook);
            string newJsonString = JsonSerializer.Serialize(takenBooks);
            File.WriteAllText(TakenBooksDataFile, newJsonString);
        }

        public int GetUserTakenBookAmmount(string userId)
        {
            List<TakenBook> takenBooks = JsonSerializer.Deserialize<List<TakenBook>>(File.ReadAllText(TakenBooksDataFile));
            return takenBooks.FindAll(x => x.UserId == userId).Count;
        }

        public Book GetBook(string ISBN)
        {
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(BooksDataFile));
            return books.Find(x => x.ISBN == ISBN);
        }

        public TakenBook GetTakenBook(string userId, string ISBN)
        {
            List<TakenBook> books = JsonSerializer.Deserialize<List<TakenBook>>(File.ReadAllText(TakenBooksDataFile));
            return books.Find(x => x.Book.ISBN == ISBN && x.UserId == userId);
        }
    }
}
