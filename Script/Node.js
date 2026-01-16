const fs = require("fs");
const path = require("path");
const http = require("http");

const ruta = path.join(__dirname, "..", "Quiz", "data.json");

function cargarPreguntas() {
  try {
    return JSON.parse(fs.readFileSync(ruta, "utf8"));
  } catch (error) {
    console.error("Error al cargar preguntas:", error.message);
    return [];
  }
}

function guardarPreguntas(preguntas) {
  fs.writeFileSync(ruta, JSON.stringify(preguntas, null, 2));
}

const servidor = http.createServer((req, res) => {
  res.setHeader("Content-Type", "application/json");

  let preguntas = cargarPreguntas();

  //Verificar si se han cargado las preguntas
  console.log("Preguntas cargadas:", preguntas);

  // GET
  if (req.url === "/questions" && req.method === "GET") {
    res.end(JSON.stringify(preguntas));
  }

  // DELETE
  else if (req.url.startsWith("/questions/") && req.method === "DELETE") {

    const id = parseInt(req.url.split("/")[2]);

    preguntas = preguntas.filter(p => p.id !== id);

    guardarPreguntas(preguntas);

    res.end("Pregunta eliminada" );
  }

  // PUT
  else if (req.url.startsWith("/questions/") && req.method === "PUT") {
    let cuerpo = "";

    req.on("data", chunk => cuerpo += chunk);

    req.on("end", () => {

      const id = parseInt(req.url.split("/")[2]);
      const actualizada = JSON.parse(cuerpo);

      const indice = preguntas.findIndex(p => p.id === id);

      if (indice === -1) {

        res.end(JSON.stringify({ error: "Pregunta no encontrada" }));
        return;
      }

      preguntas[indice] = { id, 
        pregunta: actualizada.pregunta,
        opciones: actualizada.opciones,
        correcta: actualizada.correcta
      };
      
      guardarPreguntas(preguntas);

      res.end(JSON.stringify(preguntas[indice]));
    });
  }

  else {
    res.end(JSON.stringify({ error: "Endpoint no encontrado" }));
  }
});

servidor.listen(3000, () => {
  console.log("API corriendo en http://localhost:3000/questions");
});
