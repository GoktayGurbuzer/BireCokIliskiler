using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class KursController : Controller
    {
        private readonly DataContext _context;
        public KursController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kurslar = await _context.Kurslar.Include(k => k.Ogretmen).ToListAsync();
            return View(kurslar);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursVM kurs)
        {
            if(ModelState.IsValid)
            {
            _context.Kurslar.Add(new Kurs() {KursId = kurs.KursId, Baslik = kurs.Baslik, OgretmenId = kurs.OgretmenId});
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            }
            
            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");
            return View(kurs);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) { return NotFound(); }

            var kurs = await _context.Kurslar.Include(k => k.KursKayitlari).ThenInclude(k => k.Ogrenci).Select(k => new KursVM{
            KursId = k.KursId,
            Baslik = k.Baslik,
            OgretmenId = k.OgretmenId,
            KursKayitlari = k.KursKayitlari
            }).FirstOrDefaultAsync(k => k.KursId == id);

            if(kurs == null) { return NotFound(); }

            ViewBag.Ogretmenler = new SelectList(await _context.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");
            return View(kurs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, KursVM kurs)
        {
            if(id != kurs.KursId) { return NotFound(); }
            if(!ModelState.IsValid) { return NotFound(kurs); }

            _context.Kurslar.Update(new Kurs() {KursId = kurs.KursId, Baslik = kurs.Baslik, OgretmenId = kurs.OgretmenId});
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");            
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null) { return NotFound(); }
            var kurs = await _context.Kurslar.FindAsync(id);

            if(kurs == null) { return NotFound(); }
            _context.Kurslar.Remove(kurs);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}