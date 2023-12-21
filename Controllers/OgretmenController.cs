using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class OgretmenController : Controller
    {
        private readonly DataContext _context;
        public OgretmenController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ogretmenler = await _context.Ogretmenler.ToListAsync();
            return View(ogretmenler);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogretmen ogretmen)
        {
            var ogretmenEkle = _context.Ogretmenler.Add(ogretmen);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) { return NotFound(); }
            var ogretmen = await _context.Ogretmenler.FirstOrDefaultAsync(o => o.OgretmenId == id);

            if(ogretmen == null) { return NotFound(); }

            return View(ogretmen);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Ogretmen ogretmen)
        {
            if(id != ogretmen.OgretmenId) { return NotFound(); }

            if(ModelState.IsValid)
            {
                try{
                    _context.Update(ogretmen);
                    await _context.SaveChangesAsync();
                }catch(DbUpdateConcurrencyException){
                    if(!_context.Ogretmenler.Any(o => o.OgretmenId == ogretmen.OgretmenId)) { return NotFound(); }
                    else { throw; }
                }

                return RedirectToAction("Index");
            }

            return View(ogretmen);
        }

    }
}