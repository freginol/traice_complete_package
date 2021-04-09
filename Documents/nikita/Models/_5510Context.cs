using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace PizzaKnight.Models
{
    public partial class _5510Context : DbContext
    {
        public _5510Context()
        {
        }

        public _5510Context(DbContextOptions<_5510Context> options)
            : base(options)
        {
        }

        public virtual DbSet<CustFeedback> CustFeedback { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<InvoiceInfo> InvoiceInfo { get; set; }
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<MenuControl> MenuControl { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<OrderList> OrderList { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<PaymentInfo> PaymentInfo { get; set; }
        public virtual DbSet<PizzaCust> PizzaCust { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:inclassdemo.database.windows.net,1433;Initial Catalog=5510;Persist Security Info=False;User ID=fabianreginold;Password=Welcome@12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustFeedback>(entity =>
            {
                entity.HasKey(e => e.FirstName)
                    .HasName("PK_custfeedback");

     
                entity.Property(e => e.Feedback).HasMaxLength(1000);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.Property(e => e.Addrses1).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.Emailaddress).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Postal).HasMaxLength(50);

                entity.Property(e => e.Province).HasMaxLength(50);
            });

            modelBuilder.Entity<InvoiceInfo>(entity =>
            {
                entity.HasKey(e => e.Invoiceid)
                    .HasName("PK_invoice");

                entity.ToTable("Invoice_Info");

                entity.Property(e => e.Invoiceid)
                    .HasColumnName("invoiceid")
                    .HasMaxLength(50);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(500);

                entity.Property(e => e.Items)
                    .IsRequired()
                    .HasColumnName("items")
                    .HasMaxLength(25);

                entity.Property(e => e.Paymentinfo)
                    .IsRequired()
                    .HasColumnName("paymentinfo")
                    .HasMaxLength(50);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.ItemsNavigation)
                    .WithMany(p => p.InvoiceInfo)
                    .HasForeignKey(d => d.Items)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Invoice_I__items__01142BA1");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Items);

                entity.Property(e => e.Items)
                    .HasColumnName("items")
                    .HasMaxLength(25);

                entity.Property(e => e.Itemdescription)
                    .IsRequired()
                    .HasColumnName("itemdescription")
                    .HasMaxLength(100);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<MenuControl>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Finalamount)
                    .HasColumnName("finalamount")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.Items)
                    .IsRequired()
                    .HasColumnName("items")
                    .HasMaxLength(25);

                entity.Property(e => e.Picture)
                    .IsRequired()
                    .HasColumnName("picture")
                    .HasMaxLength(500);

                entity.HasOne(d => d.ItemsNavigation)
                    .WithMany(p => p.MenuControl)
                    .HasForeignKey(d => d.Items)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MenuContr__items__07C12930");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.OrderDetailId);

                entity.Property(e => e.PizzaCustId).HasColumnName("pizzaCustId");

                entity.Property(e => e.Price).HasColumnType("numeric(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(d => d.OrderLines)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__Order__4A8310C6");

                entity.HasOne(d => d.PizzaCust)
                    .WithMany(d => d.OrderDetail)
                    .HasForeignKey(d => d.PizzaCustId)
                    .HasConstraintName("FK__OrderDeta__pizza__498EEC8D");
            });

            modelBuilder.Entity<OrderList>(entity =>
            {
                entity.HasKey(e => e.Ordid)
                    .HasName("PK__OrderLis__2158CCA08D9AE731");

                entity.Property(e => e.Ordid)
                    .HasColumnName("ordid")
                    .HasMaxLength(50);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(500);

                entity.Property(e => e.Custid).HasColumnName("custid");

                entity.Property(e => e.Custname)
                    .IsRequired()
                    .HasColumnName("custname")
                    .HasMaxLength(50);

                entity.Property(e => e.Discountcode)
                    .IsRequired()
                    .HasColumnName("discountcode")
                    .HasMaxLength(50);

                entity.Property(e => e.Ordertype)
                    .IsRequired()
                    .HasColumnName("ordertype")
                    .HasMaxLength(5);

                entity.Property(e => e.Totalamount)
                    .HasColumnName("totalamount")
                    .HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.OrderList)
                    .HasForeignKey(d => d.Custid)
                    .HasConstraintName("FK__OrderList__custi__72C60C4A");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => new { e.Custid, e.Ordid })
                    .HasName("PK_orderstatus");

                entity.Property(e => e.Custid).HasColumnName("custid");

                entity.Property(e => e.Ordid)
                    .HasColumnName("ordid")
                    .HasMaxLength(50);

                entity.Property(e => e.Invoiceid)
                    .IsRequired()
                    .HasColumnName("invoiceid")
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.Custid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderStat__custi__0B91BA14");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.Invoiceid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderStat__invoi__0C85DE4D");

                entity.HasOne(d => d.Ord)
                    .WithMany(p => p.OrderStatus)
                    .HasForeignKey(d => d.Ordid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderStat__ordid__0A9D95DB");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK_Order");

                entity.Property(e => e.OrderId);

                entity.Property(e => e.AddressLine1)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.AddressLine2).HasMaxLength(25);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.OrderPlaced)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.OrderTotal).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<PaymentInfo>(entity =>
            {
                entity.HasKey(e => e.CustomerName)
                    .HasName("PK_payment");

                entity.Property(e => e.CustomerName)
                .HasMaxLength(50);

                entity.Property(e => e.CardType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CardValue).HasMaxLength(50); 

                entity.Property(e => e.CVV)
                    .HasColumnName("CVV")
                    .HasColumnType("numeric(3, 0)");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expirydate")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<PizzaCust>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CategoriesId)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ImageUrl)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("numeric(18, 0)");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.Property(e => e.ShoppingCartId).HasMaxLength(1000);
            });

            modelBuilder.Entity<ShoppingCartItem>(entity =>
            {
                entity.Property(e => e.ShoppingCartItemId);

                entity.Property(e => e.PizzaCustId).HasColumnName("pizzaCustId");

                entity.Property(e => e.ShoppingCartId)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.HasOne(d => d.pizzaCust)
                    .WithMany(p => p.ShoppingCartItem)
                    .HasForeignKey(d => d.PizzaCustId)
                    .HasConstraintName("FK__ShoppingC__pizza__46B27FE2");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PK__UserInfo__C9F28457FE549EC6");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Role).HasMaxLength(5);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
