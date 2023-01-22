namespace OnionSozluk.WebApp.Infrastructure.Models
{
    // alt sınıfın üst sınıfa parametre yollamasıı sağlar. onda bir değişiklik olduğu zaman üst sınıf bunu bilir.
    public class FavClickedEventArgs : EventArgs 
    {
        public Guid? EntryId { get; set; }

        public bool IsFaved { get; set; }
    }
}
