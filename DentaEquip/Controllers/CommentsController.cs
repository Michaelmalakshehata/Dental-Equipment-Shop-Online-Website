using AspNetCoreHero.ToastNotification.Abstractions;
using DentaEquip.BL.IRepositories;
using DentaEquip.BL.ViewModels.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentaEquip.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly IServiceComments serviceComments;
        private readonly INotyfService notyf;
        public CommentsController(INotyfService notyf, IServiceComments serviceComments)
        {
            this.serviceComments = serviceComments;
            this.notyf = notyf;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CommentViewModel comment)
        {
            if (ModelState.IsValid)
            {
                var name = User.Identity.Name;
                comment.UserName = name;
              string result=  await serviceComments.AddComment(comment);
                if(string.IsNullOrWhiteSpace(result) == false)
                {
                    notyf.Success("Add Comment Done", 10);
                }
            }
            return RedirectToAction("ShowDetailes", "ShowProduct", new { Id = comment.ProductId ,pg=comment.PageNumber});
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int commentId, int productID)
        {
            if (commentId > 0)
            {
               string result= await serviceComments.DeleteComment(commentId);
                if (string.IsNullOrWhiteSpace(result) == false)
                {
                    notyf.Success("Delete Comment Done", 10);
                }
            }
            return RedirectToAction("ShowDetailes", "ShowProduct", new { Id = productID });
        }
    }
}
