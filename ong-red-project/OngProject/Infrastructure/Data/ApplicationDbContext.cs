﻿using System;
using Microsoft.EntityFrameworkCore;
using OngProject.Common;
using OngProject.Core.Entities;

namespace OngProject.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //de esta manera la propiedad email de los usuarios sera unica y no se podra repetir
            builder.Entity<User>(entity => {
                entity.HasIndex(e => e.Email).IsUnique();
            });
            SeedRoles(builder);
            SeedContacts(builder);
            SeedOrganizations(builder);
            SeedUsers(builder);

            SeedTestimonios(builder);

            SeedMiembros(builder);

            builder.Seed();
        }

        public DbSet<Activities> Activities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Member> Members { get; set; }  
        public DbSet<News> News { get; set; }   
        public DbSet<Organizations> Organizations { get; set; }
        public DbSet<Role> Roles { get; set; }  
        public DbSet<Slides> Slides { get; set; }
        public DbSet<Testimonials> Testimonials { get; set; }
        public DbSet<User> Users { get; set; }

       
        private void SeedOrganizations(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Organizations>().HasData(
                    new Organizations
                    {
                        Id = i,
                        Name = "Organization " + i,
                        Image = "ImageOrganizations" + i + ".jpg",
                        Address = "Address for Organization " + i,
                        Phone = 381 + i,
                        Email = "Email for Organization " + i,
                        WelcomeText = "WelcomeText for Organization " + i,
                        AboutUsText = "AboutUsText for Organization " + i,
                        CreatedAt = DateTime.Now,
                        FacebookUrl = "https://www.facebook.com/" + i,
                        InstagramUrl = "https://www.instagram.com/" + i,
                        LinkedinUrl = "https://www.linkedin.com/in/" + i
                    }
                );
            }
        }

        private void SeedContacts(ModelBuilder modelBuilder)
        {
            for (int i = 1; i < 11; i++)
            {
                modelBuilder.Entity<Contacts>().HasData(
                    new Contacts
                    {
                        Id = i,
                        Name = "Contact " + i,
                        Phone = 381 + i,
                        Email = "Email for contact " + i,
                        Message = "Message from contact " + i,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }

        private void SeedUsers(ModelBuilder modelBuilder)
        {
            for (int i = 1; i <= 2; i++)
            {
                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = i,
                        FirstName = "User" + i,
                        LastName = "LastName for user " + i,
                        Email = "Email for user " + i,
                        Password = Encrypt.GetSHA256("123456"),
                        Photo = "Photo for user " + i,
                        RoleId = 1,
                        CreatedAt = DateTime.Now
                    }
                );
            }
        }
        
        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                    new Role
                    {
                        Id = 1,
                        Name = "Administrator",
                        Description = "Description User Admin",
                        CreatedAt = DateTime.Now
                    },
                    new Role
                    {
                        Id = 2,
                        Name = "Standard",
                        Description = "Description User Standard"
                    },
                    new Role
                    {
                        Id = 3,
                        Name = "Regular",
                        Description = "Description User Regular"
                    }                    
                );
        }



        private void SeedTestimonios(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Testimonials>().HasData(
                new Testimonials
                {
                    Id = 1,
                    Name = "Ayuda a los niños sin hogar en santa fe",
                    Image = "",
                    Content = "El ONG ayudo a muchos niños en una ciudad humilde de santa fe"
                },
                 new Testimonials
                 {
                     Id = 2,
                     Name = "Ayuda a las personas con VIH Mar del plata",
                     Image = "",
                     Content = "El ONG ayudo a muchas personas con VIH sin exception"
                 },
                  new Testimonials
                  {
                      Id = 3,
                      Name = "Ayuda a las mujeres embarazadas",
                      Image = "",
                      Content = "El ONG ayudo a muchos mujeres "
                  },
                   new Testimonials
                   {
                       Id = 4,
                       Name = "Ayuda a muvha gente con discapacidad en Once",
                       Image = "",
                       Content = "Durante la cuarentena,El ONG ayudo a mucha gente con discapacidad "
                   });



        }



        private void SeedMiembros(ModelBuilder modelBuilder)
        {
           

                modelBuilder.Entity<Member>().HasData(
                    new Member
                    {
                        Id = 1,
                        Name = "Rodrigo Fuente",
                        Image = "1"
                        
                    },
                      new Member
                      {
                          Id = 2,
                          Name = "Miriam Rodriguez",
                          Image = "2"
                      },
                        new Member
                        {
                            Id = 3,
                            Name = "Maria Irola",
                            Image = "3"
                        },
                          new Member
                          {
                              Id = 4,
                              Name = "Marita Gomez",
                              Image = "4"

                          },
                          new Member
                          {
                              Id = 5,
                              Name = "Maria Garcia",
                              Image = "5"

                          },
                           new Member
                          {
                              Id = 6,
                              Name = "Marco Fernandez",
                              Image = "6"
                          },

                            new Member
                            {
                                Id = 7,
                                Name = "Cecilia Mendez",
                                Image = "7"
           
                            });
        }


    }
}
