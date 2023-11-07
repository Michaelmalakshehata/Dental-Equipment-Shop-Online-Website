using DentaEquip.BL.IRepositories;
using DentaEquip.BL.ViewModels.Comment;
using DentaEquip.DAL.Context;
using DentaEquip.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.Repositories
{
    public class ServiceComments : IServiceComments
    {
        private readonly EntityContext context;
        public ServiceComments(EntityContext context)
        {
            this.context = context;

        }

        public async Task<string> AddComment(CommentViewModel comment)
        {
            try
            {
                if (comment is  null)
                {
                    return string.Empty;
                }
                var userId = await context.Users.Where(o => o.UserName.Equals(comment.UserName)).AsNoTracking().Select(o => o.Id).FirstOrDefaultAsync();
                Comment comments = new Comment()
                {
                    ProductId = comment.ProductId,
                    UserName = comment.UserName,
                    Comments = comment.Comments,
                    Rating = comment.Rating,
                    UserId = userId
                };

                var result = await context.Comment.AddAsync(comments);
                await context.SaveChangesAsync();
                if (result is not null)
                {
                    return "Added";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<string> DeleteComment(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return string.Empty;
                }
                Comment comment = await context.Comment.Where(o => o.Id == Id).FirstOrDefaultAsync();
                if (comment is not null)
                {
                    context.Comment.Remove(comment);
                    await context.SaveChangesAsync();
                    return "deleted";
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<List<CommentViewModel>> GetAllCommentsOfProdect(int productId)
        {
            try
            {
                if (productId == 0)
                {
                    return new List<CommentViewModel>();
                }
                List<CommentViewModel> comments = new List<CommentViewModel>();
                List<Comment> commentList = await context.Comment.Where(o => o.ProductId == productId).AsNoTracking().ToListAsync();
                foreach (var cmnt in commentList)
                {
                    comments.Add(new CommentViewModel
                    {
                        ProductId = cmnt.ProductId,
                        UserName = cmnt.UserName,
                        Comments = cmnt.Comments,
                        Date = cmnt.Date,
                        Rating = cmnt.Rating,
                        Id = cmnt.Id
                    });
                }
                return comments;
            }
            catch (Exception)
            {
                return new List<CommentViewModel>();
            }
        }
    }
}
