using NUnit.Framework;
using Moq;
using VismaTask.Repository;
using VismaTask.Models;
using VismaTask.Services;
using System.Collections.Generic;
using VismaTask.Repositories;
using System;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetAllBooksCount()
        {
            var mockedBookList = new List<Book> { new Book("testName", "testAuthor", "testCategory", "testLanguage", DateTime.Parse("2000-02-02"), "123-123") };
            var mockedTakenBookList = new List<TakenBook> { new TakenBook("123", 20, DateTime.Parse("2021-02-02"),
                            new Book("testName2", "testAuthor2", "testCategory2", "testLanguage2", DateTime.Parse("2000-02-02"), "123-123")) };
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetAllBooksData()).Returns(mockedBookList);
            repositoryMock.Setup(x => x.GetAllTakenBooksData()).Returns(mockedTakenBookList);
            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(2, service.GetAllBooks().Count);
        }

        [Test]
        public void AddBookGivenBadParametersReturnNull()
        {
            var service = new BookService();
            Assert.AreEqual(null, service.AddBook("", "author", "category", "language", DateTime.Parse("2000-01-20"), "123-123"));
            Assert.AreEqual(null, service.AddBook("name", "", "category", "language", DateTime.Parse("2000-01-20"), "123-123"));
            Assert.AreEqual(null, service.AddBook("name", "author", "", "language", DateTime.Parse("2000-01-20"), "123-123"));
            Assert.AreEqual(null, service.AddBook("name", "author", "category", "", DateTime.Parse("2000-01-20"), "123-123"));
            Assert.AreEqual(null, service.AddBook("name", "author", "category", "language", DateTime.Parse("2000-01-20"), ""));
        }

        [Test]
        public void TakeBookGivenEmptyOrWrongParametersReturnNull()
        {
            var service = new BookService();
            Assert.AreEqual(null, service.TakeBook("", "123", "12"));
            Assert.AreEqual(null, service.TakeBook("123", "", "12"));
            Assert.AreEqual(null, service.TakeBook("123", "123", ""));
            Assert.AreEqual(null, service.TakeBook("123", "123", "70"));
        }

        [Test]
        public void TakeBookGivenInvalidISBNParameterReturnNull()
        {
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetBook("123")).Returns((Book) null );

            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(null, service.TakeBook("123", "123", "50"));
        }

        [Test]
        public void TakeBookGivenUserThatHas3BooksTakenReturnNull()
        {
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetUserTakenBookAmmount("123")).Returns(3);

            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(null, service.TakeBook("123", "123", "50"));
        }

        [Test]
        public void TakeBookGivenGoodParametersReturnBook()
        {
            var mockedBook = new Book("testName", "testAuthor", "testCategory", "testLanguage", DateTime.Parse("2000-02-02"), "123-123");
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetUserTakenBookAmmount("123")).Returns(2);
            repositoryMock.Setup(x => x.GetBook("123-123")).Returns(mockedBook);
            repositoryMock.Setup(x => x.DeleteBookFromBooksDataFile(mockedBook));
            repositoryMock.Setup(x => x.AddTakenBook(new TakenBook("123", 50, DateTime.UtcNow, mockedBook)));
            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(mockedBook, service.TakeBook("123", "123-123", "50"));
        }

        [Test]
        public void ReturnBookGivenEmptyOrWrongParametersReturnNull()
        {
            var service = new BookService();
            Assert.AreEqual(null, service.ReturnBook("", "123"));
            Assert.AreEqual(null, service.ReturnBook("123", ""));
            Assert.AreEqual(null, service.ReturnBook("", ""));
        }

        [Test]
        public void ReturnBookGivenInvalidISBNParameterReturnNull()
        {
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetTakenBook("user", "isbn")).Returns((TakenBook)null);

            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(null, service.ReturnBook("user", "isbn"));
        }

        [Test]
        public void ReturnBookGivenGoodParametersReturnBook()
        {
            var mockedBook = new Book("testName", "testAuthor", "testCategory", "testLanguage", DateTime.Parse("2000-02-02"), "123-123");
            var mockedTakenBook = new TakenBook("user", 50, DateTime.UtcNow, mockedBook);
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetTakenBook("user", "123-123")).Returns(mockedTakenBook);
            repositoryMock.Setup(x => x.DeleteBookFromTakenBooksDataFile(mockedTakenBook));
            repositoryMock.Setup(x => x.AddBook(mockedTakenBook.Book));

            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(mockedBook, service.ReturnBook("user", "123-123"));
        }

        [Test]
        public void DeleteBookGivenEmptyParametersReturnNull()
        {
            var service = new BookService();
            Assert.AreEqual(null, service.DeleteBook(""));
        }

        [Test]
        public void DeleteBookGivenInvalidISBNParameterReturnNull()
        {
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetBook("isbn")).Returns((Book)null);

            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(null, service.DeleteBook("isbn"));
        }

        [Test]
        public void DeleteBookGivenGoodParametersReturnBook()
        {
            var mockedBook = new Book("testName", "testAuthor", "testCategory", "testLanguage", DateTime.Parse("2000-02-02"), "123-123");
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetBook("123-123")).Returns(mockedBook);
            repositoryMock.Setup(x => x.DeleteBookFromBooksDataFile(mockedBook));

            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(mockedBook, service.DeleteBook("123-123"));
        }
    }
}