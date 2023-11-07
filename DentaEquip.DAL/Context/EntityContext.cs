using DentaEquip.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Context
{
    public class EntityContext : IdentityDbContext<ApplicationUser>
    {

        public EntityContext(DbContextOptions<EntityContext> options) : base(options)
        {

        }

        public virtual DbSet<MainCategory> MainCategories { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Requests> Requests { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<FinishedOrders> FinishedOrders { get; set; }
        public virtual DbSet<Products> Product { get; set; }
        public virtual DbSet<WishList> WishList { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<OrdersRequest> OrdersRequests { get; set; }
        public virtual DbSet<OrdersCompelete> OrdersCompeletes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SubCategory>().HasOne(p => p.MainCategory).WithMany(p => p.SubCategories).HasForeignKey(p => p.MainCategoryId).HasConstraintName("MainCategorySubCategory");
            builder.Entity<Products>().HasOne(p => p.SubCategories).WithMany(p => p.Products).HasForeignKey(p => p.SubCategoryId).HasConstraintName("SubCategoryProducts");
            builder.Entity<Products>().HasOne(p => p.Brand).WithMany(p => p.Products).HasForeignKey(p => p.BrandId).HasConstraintName("BrandProducts");
            builder.Entity<Requests>().HasOne(p => p.User).WithMany(p => p.Requests).HasForeignKey(p => p.UserId).HasConstraintName("UserRequests");
            builder.Entity<OrdersRequest>().HasOne(p => p.Requests).WithMany(p => p.OrdersRequests).HasForeignKey(p => p.RequestsId).HasConstraintName("RequestsOrder");
            builder.Entity<Cart>().HasOne(p => p.User).WithMany(p => p.Carts).HasForeignKey(p => p.UserId).HasConstraintName("UserCart");
            builder.Entity<WishList>().HasOne(p => p.User).WithMany(p => p.WishLists).HasForeignKey(p => p.UserId).HasConstraintName("UserWishList");
            builder.Entity<Comment>().HasOne(p => p.User).WithMany(p => p.Comment).HasForeignKey(p => p.UserId).HasConstraintName("UserComments");
            builder.Entity<Comment>().HasOne(p => p.Products).WithMany(p => p.Comment).HasForeignKey(p => p.ProductId).HasConstraintName("ProductComments");
            builder.Entity<FinishedOrders>().HasOne(p => p.User).WithMany(p => p.FinishedOrders).HasForeignKey(p => p.UserId).HasConstraintName("UserFinishedOrders");
            builder.Entity<OrdersCompelete>().HasOne(p => p.FinishedOrders).WithMany(p => p.OrdersCompeletes).HasForeignKey(p => p.FinishedOdersId).HasConstraintName("OrdersFinishedOrders");

            base.OnModelCreating(builder);
        }

    }
}
