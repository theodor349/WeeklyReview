using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WeeklyReview.Database.Models;
using WeeklyReview.Server.Controllers.Internal;
using WeeklyReview.Server.Persitance;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Server.Controllers
{
    public class CategoryController : GenericAuthorizedApiController
    {
        private readonly IWeeklyReviewService _weeklyReviewService;
        private ICategoryService _categoryService => _weeklyReviewService.Category;

        public CategoryController(IWeeklyReviewService weeklyReviewService)
        {
            _weeklyReviewService = weeklyReviewService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<CategoryModel>> GetAll()
        {
            return Ok(_categoryService.GetAll(UserGuid));
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public ActionResult<CategoryModel> Get([FromRoute] int key)
        {
            return Ok(_categoryService.Get(key, UserGuid));
        }

        [HttpDelete("{key}")]
        public ActionResult<ActivityModel> Delete([FromRoute] int key)
        {
            return Ok(_categoryService.Delete(key, UserGuid));
        }

        [HttpPost("{key}/ChangeColor")]
        public ActionResult<ActivityChangeModel> ChangeColor([FromRoute] int key, [FromBody] Color color)
        {
            return Ok(_categoryService.ChangeColor(key, color, UserGuid));
        }
    }
}
