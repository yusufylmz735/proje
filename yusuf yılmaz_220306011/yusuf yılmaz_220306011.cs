using System;
using System.Collections.Generic;
using System.Linq;

// Entity Layer
namespace SaatMagazasi.Entity
{
    public class Saat
    {
        public string Marka { get; set; }
        public string Model { get; set; }
        public decimal Fiyat { get; set; }
        public int StokMiktari { get; set; }
    }

    public class Musteri
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TelefonNumarasi { get; set; }
        public string Email { get; set; }
    }

    public class Siparis
    {
        public int SiparisNo { get; set; }
        public Musteri Musteri { get; set; }
        public List<Saat> UrunListesi { get; set; } = new List<Saat>();
        public decimal ToplamTutar { get; set; }
    }
}

// Business Layer
namespace SaatMagazasi.Business
{
    using SaatMagazasi.Entity;

    public interface ISaatYonetimi
    {
        void SaatEkle(Saat saat);
        List<Saat> SaatleriListele();
    }

    public interface ISiparisYonetimi
    {
        void SiparisOlustur(Siparis siparis);
        List<Siparis> SiparisleriListele();
    }

    public interface IMusteriYonetimi
    {
        void MusteriEkle(Musteri musteri);
        List<Musteri> MusterileriListele();
    }

    public class SaatYonetimi : ISaatYonetimi
    {
        private readonly List<Saat> saatListesi = new List<Saat>();

        public void SaatEkle(Saat saat)
        {
            saatListesi.Add(saat);
        }

        public List<Saat> SaatleriListele()
        {
            return saatListesi;
        }
    }

    public class SiparisYonetimi : ISiparisYonetimi
    {
        private readonly List<Siparis> siparisListesi = new List<Siparis>();

        public void SiparisOlustur(Siparis siparis)
        {
            siparis.ToplamTutar = siparis.UrunListesi.Sum(s => s.Fiyat);
            siparisListesi.Add(siparis);
        }

        public List<Siparis> SiparisleriListele()
        {
            return siparisListesi;
        }
    }

    public class MusteriYonetimi : IMusteriYonetimi
    {
        private readonly List<Musteri> musteriListesi = new List<Musteri>();

        public void MusteriEkle(Musteri musteri)
        {
            musteriListesi.Add(musteri);
        }

        public List<Musteri> MusterileriListele()
        {
            return musteriListesi;
        }
    }
}

// Console Application
class Program
{
    static void Main(string[] args)
    {
        SaatMagazasi.Business.ISaatYonetimi saatYonetimi = new SaatMagazasi.Business.SaatYonetimi();
        SaatMagazasi.Business.IMusteriYonetimi musteriYonetimi = new SaatMagazasi.Business.MusteriYonetimi();
        SaatMagazasi.Business.ISiparisYonetimi siparisYonetimi = new SaatMagazasi.Business.SiparisYonetimi();

        // Saat ekleme
        saatYonetimi.SaatEkle(new SaatMagazasi.Entity.Saat { Marka = "Rolex", Model = "Submariner", Fiyat = 5000, StokMiktari = 10 });
        saatYonetimi.SaatEkle(new SaatMagazasi.Entity.Saat { Marka = "Casio", Model = "G-Shock", Fiyat = 200, StokMiktari = 20 });
        saatYonetimi.SaatEkle(new SaatMagazasi.Entity.Saat { Marka = "Seiko", Model = "Prospex", Fiyat = 750, StokMiktari = 15 });
        saatYonetimi.SaatEkle(new SaatMagazasi.Entity.Saat { Marka = "Omega", Model = "Speedmaster", Fiyat = 3000, StokMiktari = 8 });
        saatYonetimi.SaatEkle(new SaatMagazasi.Entity.Saat { Marka = "Tag Heuer", Model = "Carrera", Fiyat = 2500, StokMiktari = 5 });
        saatYonetimi.SaatEkle(new SaatMagazasi.Entity.Saat { Marka = "Tissot", Model = "Le Locle", Fiyat = 500, StokMiktari = 12 });

        // Müşteri ekleme
        var musteri = new SaatMagazasi.Entity.Musteri { Ad = "Ahmet", Soyad = "Yılmaz", TelefonNumarasi = "5551234567", Email = "ahmet@mail.com" };
        musteriYonetimi.MusteriEkle(musteri);

        // Sipariş oluşturma
        var siparis = new SaatMagazasi.Entity.Siparis
        {
            SiparisNo = 1,
            Musteri = musteri,
            UrunListesi = new List<SaatMagazasi.Entity.Saat> { saatYonetimi.SaatleriListele()[0] }
        };
        siparisYonetimi.SiparisOlustur(siparis);

        // Listeleme
        Console.WriteLine("Saatler:");
        foreach (var saat in saatYonetimi.SaatleriListele())
        {
            Console.WriteLine($"Marka: {saat.Marka}, Model: {saat.Model}, Fiyat: {saat.Fiyat}, Stok: {saat.StokMiktari}");
        }

        Console.WriteLine("\nMüşteriler:");
        foreach (var m in musteriYonetimi.MusterileriListele())
        {
            Console.WriteLine($"Ad: {m.Ad}, Soyad: {m.Soyad}, Telefon: {m.TelefonNumarasi}, Email: {m.Email}");
        }

        Console.WriteLine("\nSiparişler:");
        foreach (var s in siparisYonetimi.SiparisleriListele())
        {
            Console.WriteLine($"Sipariş No: {s.SiparisNo}, Müşteri: {s.Musteri.Ad} {s.Musteri.Soyad}, Toplam Tutar: {s.ToplamTutar}");
        }
    }
}
