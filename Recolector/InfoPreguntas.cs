using System.Text.Json.Serialization;

namespace QuizApi.Recolector
{
    public class InfoPreguntas
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("pregunta")]
        public string Pregunta { get; set; }

        [JsonPropertyName("opciones")]
        public List<string> Opciones { get; set; }

        [JsonPropertyName("correcta")]
        public int Correcta { get; set; }
    }

}
