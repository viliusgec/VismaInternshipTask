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
        private Book _mockedBook;
        private TakenBook _mockedTakenBook;
        private List<Book> _mockedBookList;
        private List<TakenBook> _mockedTakenBookList;

        [SetUp]
        public void Setup()
        {
            _mockedBook = new Book("testName", "testAuthor", "testCategory", "testLanguage", DateTime.Parse("2000-02-02"), "123-123");
            _mockedTakenBook = new TakenBook("123", 20, DateTime.Parse("2021-02-02"), _mockedBook);
            _mockedBookList = new List<Book> { _mockedBook, _mockedBook, _mockedBook };
            _mockedTakenBookList = new List<TakenBook> { _mockedTakenBook, _mockedTakenBook };
        }

        [Test]
        public void GetAllBooksCount()
        {
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetAllBooksData()).Returns(_mockedBookList);
            repositoryMock.Setup(x => x.GetAllTakenBooksData()).Returns(_mockedTakenBookList);
            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(5, service.GetAllBooks().Count);
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
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetUserTakenBookAmmount("123")).Returns(2);
            repositoryMock.Setup(x => x.GetBook("123-123")).Returns(_mockedBook);
            repositoryMock.Setup(x => x.DeleteBookFromBooksDataFile(_mockedBook));
            repositoryMock.Setup(x => x.AddTakenBook(new TakenBook("123", 50, DateTime.UtcNow, _mockedBook)));
            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(_mockedBook, service.TakeBook("123", "123-123", "50"));
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
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetTakenBook("user", "123-123")).Returns(_mockedTakenBook);
            repositoryMock.Setup(x => x.DeleteBookFromTakenBooksDataFile(_mockedTakenBook));
            repositoryMock.Setup(x => x.AddBook(_mockedTakenBook.Book));

            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(_mockedBook, service.ReturnBook("user", "123-123"));
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
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetBook("123-123")).Returns(_mockedBook);
            repositoryMock.Setup(x => x.DeleteBookFromBooksDataFile(_mockedBook));

            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(_mockedBook, service.DeleteBook("123-123"));
        }
        
        [Test]
        public void ReturnFilteredDataGivenInvalidFilterReturnNull()
        {
            var service = new BookService();
            Assert.AreEqual(null, service.ReturnFilteredData("wrongFilter", ""));
        }

        [Test]
        public void FilterBookListByTakenAndAvailable()
        {
            var repositoryMock = new Mock<IBookRepository>();
            repositoryMock.Setup(x => x.GetAllBooksData()).Returns(_mockedBookList);
            repositoryMock.Setup(x => x.GetAllTakenBooksData()).Returns(_mockedTakenBookList);
            var service = new BookService(repositoryMock.Object);
            Assert.AreEqual(2, service.ReturnFilteredData("taken", "").Count);
        }
    }
}