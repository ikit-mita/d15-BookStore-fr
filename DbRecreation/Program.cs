﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using BookStore.DataAccess.EF;
using BookStore.DataAccess.EF.Models;
using JetBrains.Annotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Configuration = BookStore.DataAccess.EF.Migrations.Configuration;

namespace DbRecreation
{
    class Program
    {
        static void Main([NotNull] string[] args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var contextName = typeof(BookStoreDbContext).Name;

            if (Database.Exists(contextName))
            {
                var connectionString = ConfigurationManager.ConnectionStrings[contextName].ConnectionString;
                using (var connection = new SqlConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"ALTER DATABASE [{connection.Database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                    command.ExecuteNonQuery();
                }

                Database.Delete(contextName);
            }
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BookStoreDbContext, Configuration>());

            using (var db = new BookStoreDbContext())
            {
                CreateBranches(db);
                CreateEmployees(db);
                CreateItems<Client>(db, "JsonData/Сlients.json");
                CreateItems<Author>(db, "JsonData/Authors.json");
                CreateBooks(db);

                db.SaveChanges();
                CreateApplicationUsers(db);
            }
        }

        private static void CreateApplicationUsers(BookStoreDbContext db)
        {
            var jsonData = File.ReadAllText("jsonData/users.json");
            var userModels = JsonConvert.DeserializeObject<List<UserModel>>(jsonData);
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            foreach (UserModel userModel in userModels)
            {
                var applicationUser = new ApplicationUser
                {
                    UserName = userModel.Login
                };

                userManager.Create(applicationUser, userModel.Password);
                var employee = db.Employees.Find(userModel.EmployeeId);
                employee.UserId = applicationUser.Id;
                db.SaveChanges();
            }
        }

        private static void CreateBooks(BookStoreDbContext db)
        {
            List<Book> books = CreateItems<Book>(db, "JsonData/Book.json");

            foreach (var book in books)
            {
                //23-12345-123
                book.Isbn = $"{_rand.Next(10, 100)}-{_rand.Next(10000,100000)}-{_rand.Next(100, 1000)}";
                book.Price = _rand.Next(100, 5000);
                book.PublishYear = _rand.Next(1990, 2016);

                var authorsCount = _rand.Next(1, 4);
                book.Authors = new List<Author>();
                for (int i = 0; i < authorsCount; i++)
                {
                    book.Authors.Add(GetRandomItem(db.Authors.Local));
                }

                foreach (Branch branch in db.Branches.Local)
                {
                    if (_rand.Next()%10 == 0)
                    {
                        continue;
                    }
                    db.BookAmounts.Add(new BookAmount
                    {
                        Book = book,
                        Branch = branch,
                        Amount = _rand.Next(5, 50)
                    });
                }
            }
        }

        static void CreateBranches(BookStoreDbContext db)
        {
            db.Branches.Add(new Branch { Title = "Книжный на Мира", Address = "Мира 10" });
            db.Branches.Add(new Branch { Title = "Книжный на правом", Address = "КрасРаб 105" });
            db.Branches.Add(new Branch { Title = "Книжный на Взлетке", Address = "Взлека Плаза, Весны 10" });
        }

        static void CreateEmployees(BookStoreDbContext db)
        {
            var employeesJson = File.ReadAllText("jsondata/Employees.json");
            var employees = JsonConvert.DeserializeObject<List<Employee>>(employeesJson);

            foreach (Employee employee in employees)
            {
                employee.Branch = GetRandomItem(db.Branches.Local);
                db.Employees.Add(employee);
            }
        }        

        private static readonly Random _rand = new Random();
        private static T GetRandomItem<T>(ICollection<T> collection)
        {
            var index = _rand.Next(0, collection.Count);
            var item = collection.ElementAt(index);
            return item;
        }

        private static List<T> CreateItems<T>(BookStoreDbContext db, string filePath) where T : class
        {
            var jsonData = File.ReadAllText(filePath);
            var items = JsonConvert.DeserializeObject<List<T>>(jsonData);
            db.Set<T>().AddRange(items);
            return items;
        }
    }
}
