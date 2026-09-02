using AzureFunction.Services.ProductCategory.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunction.Services.ProductCategory
{
	public interface IProductCategoryCommandService
	{
		 Task<Guid> PostCategory(Category category, string queueNameForExceptions);
	}
}
