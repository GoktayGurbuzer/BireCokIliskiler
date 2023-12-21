using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly DataContext _context;
        public OgrenciController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ogrenciler = await _context.Ogrenciler.ToListAsync();
            return View(ogrenciler);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci ogrenci)
        {
            _context.Ogrenciler.Add(ogrenci);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) { return NotFound(); }

            var ogrenci = await _context.Ogrenciler.Include(o => o.KursKayitlari).ThenInclude(o => o.Kurs).FirstOrDefaultAsync(o => o.OgrenciId == id);

            if(ogrenci == null) { return NotFound(); }

            return View(ogrenci);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Ogrenci ogrenci)
        {
            if(id == null) { return NotFound(); }
            if(ogrenci == null) { return NotFound(); }
            if(id != ogrenci.OgrenciId) {return NotFound(); }
            if(!ModelState.IsValid) {return View(ogrenci); }
            
            try
            {
                _context.Update(ogrenci);
                await _context.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException)
            {
                if(!_context.Ogrenciler.Any( x => x.OgrenciId == ogrenci.OgrenciId))
                {
                    return NotFound();
                }else
                {
                    throw;
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null) { return NotFound(); }
            var ogrenci = await _context.Ogrenciler.FindAsync(id);

            if(ogrenci == null) { return NotFound(); }
            _context.Ogrenciler.Remove(ogrenci);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}