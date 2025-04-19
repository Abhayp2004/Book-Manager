using Microsoft.EntityFrameworkCore;
using SimpleBookManager.Models;
using System;

namespace SimpleBookManager.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for testing
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin123", // In a real app, this would be hashed
                    FullName = "Administrator",
                    Email = "admin@example.com",
                    RegistrationDate = DateTime.Now
                }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Genre = "Classic",
                    PublicationYear = 1925,
                    ISBN = "9780743273565",
                    IsRead = true,
                    DateAdded = DateTime.Now,
                    DateRead = DateTime.Now.AddDays(-30),
                    Notes = "A classic American novel depicting the Roaring Twenties."
                },
                new Book
                {
                    Id = 2,
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Genre = "Classic",
                    PublicationYear = 1960,
                    ISBN = "9780061120084",
                    IsRead = true,
                    DateAdded = DateTime.Now,
                    DateRead = DateTime.Now.AddDays(-60),
                    Notes = "A powerful story about racial inequality and moral growth."
                },
                new Book
                {
                    Id = 3,
                    Title = "1984",
                    Author = "George Orwell",
                    Genre = "Dystopian",
                    PublicationYear = 1949,
                    ISBN = "9780451524935",
                    IsRead = false,
                    DateAdded = DateTime.Now,
                    Notes = "A dystopian novel about totalitarianism and surveillance."
                }
            );
        }
    }
}