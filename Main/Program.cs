using QuizApi.Recolector;
using System.Net.Http;
using System.Text;
using System.Text.Json;

HttpClient cliente = new HttpClient();
string urlBase = "http://localhost:3000/questions";

bool ejecutando = true;

while (ejecutando)
{
    Console.WriteLine("\n--- MENÚ QUIZ ---");
    Console.WriteLine("1. Ver preguntas (GET)");
    Console.WriteLine("2. Actualizar pregunta (PUT)");
    Console.WriteLine("3. Borrar pregunta (DELETE)");
    Console.WriteLine("4. Jugar el quiz");
    Console.WriteLine("5. Salir");

    Console.Write("Elige una opción: ");
    string opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            await VerPreguntas();
            break;

        case "2":
            await ActualizarPregunta();
            break;

        case "3":
            await BorrarPregunta();
            break;

        case "4":
            await JugarQuiz();
            break;

        case "5":
            ejecutando = false;
            break;
    }
}

async Task VerPreguntas()
{
    try
    {
        var respuesta = await cliente.GetAsync(urlBase);

        if (!respuesta.IsSuccessStatusCode)
        {
            Console.WriteLine("Error al conectar con el servidor.");
            return;
        }

        var json = await respuesta.Content.ReadAsStringAsync();
        var preguntas = JsonSerializer.Deserialize<List<InfoPreguntas>>(json);

        Console.WriteLine("\n--- LISTA DE PREGUNTAS ---\n");

        foreach (var p in preguntas)
        {
            Console.WriteLine($"ID: {p.Id}");
            Console.WriteLine($"Pregunta: {p.Pregunta}");
            Console.WriteLine("Opciones:");
            for (int i = 0; i < p.Opciones.Count; i++)
                Console.WriteLine($"  [{i}] {p.Opciones[i]}");

            Console.WriteLine($"Opción correcta: [{p.Correcta}] {p.Opciones[p.Correcta]}");
            Console.WriteLine();
        }
    }
    catch (HttpRequestException)
    {
        Console.WriteLine("No se pudo conectar con la API.");
    }
}

async Task ActualizarPregunta()
{
    try
    {
        Console.Write("ID de la pregunta a actualizar: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Nueva pregunta: ");
        string pregunta = Console.ReadLine();

        List<string> opciones = new();
        for (int i = 0; i < 4; i++)
        {
            Console.Write($"Opción {i}: ");
            opciones.Add(Console.ReadLine());
        }

        Console.Write("Respuesta correcta (0-3): ");
        int respuestaCorrecta = int.Parse(Console.ReadLine());

        var cuerpo = new
        {
            pregunta = pregunta,
            opciones = opciones,
            correcta = respuestaCorrecta
        };

        var json = JsonSerializer.Serialize(cuerpo);
        var contenido = new StringContent(json, Encoding.UTF8, "application/json");

        var respuesta = await cliente.PutAsync($"{urlBase}/{id}", contenido);
        Console.WriteLine(await respuesta.Content.ReadAsStringAsync());

        await cliente.PutAsync($"{urlBase}/{id}", contenido);
        Console.WriteLine("Pregunta actualizada");
    }

    catch (HttpRequestException)
    {
        Console.WriteLine("No se pudo conectar con la API.");
    }
}

async Task BorrarPregunta()
{
    Console.Write("ID de la pregunta a borrar: ");
    int id = int.Parse(Console.ReadLine());

    try
    {
        await cliente.DeleteAsync($"{urlBase}/{id}");
        Console.WriteLine("Pregunta eliminada");
    }
    catch (HttpRequestException)
    {
        Console.WriteLine("No se pudo conectar con la API.");
    }
}

async Task JugarQuiz()
{
    try
    {
        var respuesta = await cliente.GetAsync(urlBase);
        var json = await respuesta.Content.ReadAsStringAsync();
        var preguntas = JsonSerializer.Deserialize<List<InfoPreguntas>>(json);

        int aciertos = 0;

        Console.WriteLine("\n--- COMIENZA EL QUIZ ---\n");

        foreach (var p in preguntas)
        {
            Console.WriteLine($"Pregunta: {p.Pregunta}");
            for (int i = 0; i < p.Opciones.Count; i++)
            {
                Console.WriteLine($"  [{i}] {p.Opciones[i]}");
            }

            Console.Write("Tu respuesta (0-3): ");
            string entrada = Console.ReadLine();
            int respuestaUsuario;

            if (!int.TryParse(entrada, out respuestaUsuario) || respuestaUsuario < 0 || respuestaUsuario >= p.Opciones.Count)
            {
                Console.WriteLine("Respuesta inválida. Se cuenta como incorrecta.");
                Console.WriteLine($"La respuesta correcta era: [{p.Correcta}] {p.Opciones[p.Correcta]}\n");
                continue;
            }

            if (respuestaUsuario == p.Correcta)
            {
                Console.WriteLine("Correcto!\n");
                aciertos++;
            }
            else
            {
                Console.WriteLine("Incorrecto");
                Console.WriteLine($"La respuesta correcta era: [{p.Correcta}] {p.Opciones[p.Correcta]}\n");
            }
        }

        Console.WriteLine($"Has acertado {aciertos} de {preguntas.Count} preguntas.");
    }
    catch (HttpRequestException)
    {
        Console.WriteLine("No se pudo conectar con la API.");
    }
}