using System.ComponentModel.DataAnnotations;
using efcoreApp.Data;

namespace efcoreApp.Models
{
    public class KursVM
    {
        [Key]
        public int KursId { get; set; }
        [Required(ErrorMessage ="Kurs Adı Zorunlu bir alandır!")]
        [StringLength(50, ErrorMessage = "Kurs Adı 50 Karakterden Uzun Olamaz!")]
        public string? Baslik { get; set; }
        [Required(ErrorMessage ="Öğretmen Seçmek Zorunludur!")]
        public int OgretmenId { get; set; }
        public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();
    }
}