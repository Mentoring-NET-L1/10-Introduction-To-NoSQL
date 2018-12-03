using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbUsage
{
    [TestClass]
    public class MongoDbTest
    {
        private static IMongoCollection<Book> _books;

        [ClassInitialize]
        public static void InitDb(TestContext context)
        {
            var client = new MongoClient();
            var db = client.GetDatabase("library");
            _books = db.GetCollection<Book>("books");

            CleanupDb();

            _books.InsertMany(new[]
            {
                new Book()
                {
                    Name = "Hobbit",
                    Author = "Tolkien",
                    Count = 5,
                    Geners = new List<string> {"fantasy"},
                    Year = 2014,
                },
                new Book()
                {
                    Name = "Lord of the rings",
                    Author = "Tolkien",
                    Count = 3,
                    Geners = new List<string> {"fantasy"},
                    Year = 2015,
                },
                new Book()
                {
                    Name = "Kolobok",
                    Count = 10,
                    Geners = new List<string> {"kids"},
                    Year = 2000,
                },
                new Book()
                {
                    Name = "Repka",
                    Count = 11,
                    Geners = new List<string> {"kids"},
                    Year = 2000,
                },
                new Book()
                {
                    Name = "Dyadya Stiopa",
                    Author = "Mihalkov",
                    Count = 1,
                    Geners = new List<string> {"kids"},
                    Year = 2001,
                },
            });
        }

        [ClassCleanup]
        public static void CleanupDb()
        {
            _books.DeleteMany(book => true);
        }

        [TestMethod]
        public void Task2()
        {
            var filteredBooks = _books
                .Find(book => book.Count > 1)
                .Project<Book>("{name: 1, _id: 0}")
                .SortBy(book => book.Name)
                .Limit(3);

            PrintBooks(filteredBooks.ToEnumerable());
        }

        [TestMethod]
        public void Task3()
        {
            var min =_books
                .Find(book => true)
                .SortBy(book => book.Count)
                .Limit(1)
                .FirstOrDefault();
            Console.WriteLine("Book with min count:");
            PrintBook(min);

            var max = _books
                .Find(book => true)
                .SortByDescending(book => book.Count)
                .Limit(1)
                .FirstOrDefault();
            Console.WriteLine("Book with max count:");
            PrintBook(max);
        }

        [TestMethod]
        public void Task4()
        {
            var filter = new BsonDocument {{"_id", "$author" }};

            var groupedBooks = _books.Aggregate().Group(filter).ToList();
            foreach (var book in groupedBooks)
            {
                PrintProperty("Author", book[0].IsBsonNull ? null : book[0].AsString);
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void Task5()
        {
            var authorlessBooks = _books.Find(book => book.Author == null);
            PrintBooks(authorlessBooks.ToEnumerable());
        }

        [TestMethod]
        public void Task6()
        {
            _books.UpdateMany(book => true, "{$inc: {count: 1}}");
            PrintBooks(_books.Find(book => true).ToEnumerable());
        }

        [TestMethod]
        public void Task7()
        {
            _books.UpdateMany("{ gener: { $elemMatch: { $eq: \"fantasy\" } } }", "{ $addToSet: {gener: \"favority\" } }");
            PrintBooks(_books.Find(book => true).ToEnumerable());
        }

        [TestMethod]
        public void Task8()
        {
            _books.DeleteMany(book => book.Count < 3);
            PrintBooks(_books.Find(book => true).ToEnumerable());
        }

        private static void PrintBooks(IEnumerable<Book> books)
        {
            foreach (var book in books)
            {
                PrintBook(book);
            }
        }

        private static void PrintBook(Book book)
        {
            PrintProperty("Name", book.Name);
            PrintProperty("Author", book.Author);
            PrintProperty("Count", book.Count.ToString());
            PrintProperty("Geners", book.Geners);
            PrintProperty("Year", book.Year.ToString());
            Console.WriteLine();
        }

        private static void PrintProperty(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
                Console.Write($"{name}: {value}; ");
        }

        private static void PrintProperty(string name, ICollection<string> values)
        {
            if (values != null && values.Count > 0)
                Console.Write($"{name}: {string.Join(", ", values)}; ");
        }
    }
}
