using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassWork08._12._2023MVCPatter.Models;
using Microsoft.Extensions.Hosting;

namespace ClassWork08._12._2023MVCPatter.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;
        IWebHostEnvironment _appEnvironment;
        public MoviesController(MovieContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckPoster(string poster)
        {
            var fullPath = _appEnvironment.WebRootPath + "/Posters/" + poster.Split('\\').Last();
            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    return Json(false);
                }
            }
            catch (Exception) { }
            return Json(true);
        }


        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Eror404");
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return View("Eror404");
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Director,Country,Ganre,Year,Cast,Poster")] Movie movie, IFormFile uploadedFile)
        {

            if (ModelState.IsValid)
            {

                string path = "/Posters/" + uploadedFile.FileName; // имя файла

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream); // копируем файл в поток
                    }
                    movie.Poster = path;

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Eror404");
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return View("Eror404");
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Director,Country,Ganre,Year,Cast,Poster")] Movie movie, IFormFile? uploadedFile)
        {
            if (id != movie.Id)
            {
                return View("Eror404");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadedFile != null)
                    {
                        var fullPath = _appEnvironment.WebRootPath + movie.Poster;
                        try
                        {
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                        catch (Exception) { }

                        string path = "/Posters/" + uploadedFile.FileName; // имя файла

                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream); // копируем файл в поток
                        }
                        movie.Poster = path;
                    }
                    else
                    { 
                        IQueryable<string> oldPath = from _movie in _context.Movies
                                                     where _movie.Id == movie.Id
                                                     select _movie.Poster;
                        movie.Poster = oldPath.FirstOrDefault();
                    }

                    //Movie? tmp = await _context.Movies.FindAsync(movie.Id);

                    //if (movie.CompareTo(tmp) == -1)
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return View("Eror404");
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("Details", movie);
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                var fullPath = _appEnvironment.WebRootPath + movie.Poster;
                try
                {
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }catch (Exception) {  }
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
