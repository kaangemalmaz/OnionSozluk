using System.Text.Json.Serialization;

namespace OnionSozluk.Common.Infrastructure.Results
{
    public class ValidationResponseModel
    {
        public IEnumerable<string> Errors { get; set; } //

        public ValidationResponseModel()
        {

        }

        public ValidationResponseModel(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public ValidationResponseModel(string message) : this(new List<string>() { message })
        {
            // dışardna gelen parametreyi bir liste olarak alıyor ve sadece o mesajı dönüyor.
        }

        [JsonIgnore]
        public string FlattenErrors => Errors != null
            ? string.Join(Environment.NewLine, Errors) //Liste olan stringi tek bir string halinde döner. alt alta liste oluşturur.
            : string.Empty;
    }
}
