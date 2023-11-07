using DentaEquip.BL.ViewModels.Comment;
using DentaEquip.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.IRepositories
{
    public interface IServiceComments
    {
        Task<List<CommentViewModel>> GetAllCommentsOfProdect(int productId);
        Task<string> DeleteComment(int Id);
        Task<string> AddComment(CommentViewModel comment);
    }
}
