
### ¿Qué hace cada archivo?

- **Quiz/data.json**  
  Contiene todas las preguntas, opciones y respuestas correctas del quiz.

- **Script/Node.js**  
  Servidor API que:
  - Lee y escribe en `data.json`
  - Permite ver, actualizar y borrar preguntas mediante HTTP

- **Main/Program.cs**  
  Cliente de consola que:
  - Consume la API
  - Muestra preguntas
  - Permite actualizar y borrar
  - Incluye un modo para **jugar el quiz completo**

---

## Instalación

### Requisitos
- Node.js instalado  
- .NET SDK instalado  

---

## Ejecución

### 1. Encender el servidor Node.js
Desde la carpeta `Script/`:

```bash
node Node.js
```
Deberia salirle este mensaje:
```bash
API corriendo en http://localhost:3000/questions
```

### 2. Ejecutar el C#
Desde la carpeta raiz abra una nueva terminal dejando la anterior activa y
escriba el siguiente comando:

```bash
dotnet run
```
