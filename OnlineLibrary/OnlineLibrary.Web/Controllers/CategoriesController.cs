using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Repository;
using OnlineLibrary.Service.Interface;

namespace OnlineLibrary.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategorySevice categorySevice;

        public CategoriesController(ICategorySevice categorySevice)
        {
            this.categorySevice = categorySevice;
        }
        

        // GET: Categories
        public IActionResult Index()
        {
            return View(categorySevice.GetAllCategories());
        }

        // GET: Categories/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = categorySevice.GetDetailsForCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Id")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                
                categorySevice.CreateNewCategory(category);

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = categorySevice.GetDetailsForCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Id")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    categorySevice.UpdeteExistingCategory(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = categorySevice.GetDetailsForCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (CategoryExists(id))
            {
                categorySevice.DeleteCategory(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return categorySevice.CategoryExists(id);
        }
    }
}
