using System.Collections.Generic;
using UnityEngine;

namespace ARDiabetes
{
    public partial class AppBootstrap
    {
        // ============================================================
        // Contenido de los 3 libros
        // ============================================================
        // Junta varias misiones en un solo quiz, anotando a cuál pertenece cada pregunta.
        static GameItem[] Misiones(params (string mision, string tema, GameItem[] preguntas)[] misiones)
        {
            var todas = new List<GameItem>();
            foreach (var (mision, tema, preguntas) in misiones)
                for (int i = 0; i < preguntas.Length; i++)
                {
                    preguntas[i].Mision = mision; preguntas[i].MisionTema = tema;
                    preguntas[i].NumEnMision = i + 1; preguntas[i].TotalMision = preguntas.Length;
                    todas.Add(preguntas[i]);
                }
            return todas.ToArray();
        }

        void BuildBooks()
        {
            AudioClip N(int i) => (narracion != null && i < narracion.Length) ? narracion[i] : null;
            AudioClip ClinN(int i, AudioClip porDefecto) => clinNarracion != null && i < clinNarracion.Length && clinNarracion[i] != null ? clinNarracion[i] : porDefecto;

            books = new BookDef[3];
            books[0] = new BookDef
            {
                Title = "Libro Fisiológico", Accent = UIKit.Coral, HeroIcon = spPancreas,
                // Modelos animados del cliente: sistema inmunitario (tema 1), insulina y la célula (tema 3)
                // y glucagón (tema 4); el páncreas (tema 2) sigue con su modelo.
                Model = pancreasModel, TopicModel = fisioModelos, ModelTint = UIKit.Hex("DD5450"),
                TopicTitle = new[] { "¿Qué es la diabetes tipo 1?", "El páncreas", "Insulina y glucosa", "El mensaje del glucagón" },
                TopicSub = new[] { "Conoce la enfermedad", "El órgano de la insulina", "Cómo obtienes energía", "El hígado y su reserva" },
                DescKids = new[]
                {
                    "Tu cuerpo tiene un ayudante llamado insulina. En la diabetes tipo 1, ese ayudante casi no aparece. ¡Vamos a conocerlo juntos!",
                    "El páncreas es un amigo escondido en tu barriga. Fabrica la insulina. ¡Míralo en 3D!",
                    "La insulina es como una llave mágica. Abre la puerta de tus células para que entre la energía (glucosa).",
                    "Cuando pasas un rato sin comer, el glucagón le manda un mensaje al hígado para que saque la energía que tiene guardada."
                },
                DescPre = new[]
                {
                    "La diabetes tipo 1 aparece cuando el cuerpo casi no produce insulina, la hormona que nos da energía. ¡Aprende a conocerla y cuidarte!",
                    "El páncreas es el órgano que fabrica la insulina en tu cuerpo. Explóralo en 3D y descubre cómo es por dentro.",
                    "La insulina es como una llave que deja entrar la glucosa (energía) a tus células para que funcionen.",
                    "Cuando pasas un tiempo sin comer, el glucagón le avisa al hígado que libere la glucosa que tiene guardada. Así tu cuerpo mantiene su energía."
                },
                DescTeens = new[]
                {
                    "La diabetes tipo 1 es una enfermedad autoinmune: el sistema de defensas del cuerpo ataca por error las células del páncreas que producen insulina, la hormona clave para usar la glucosa como energía.",
                    "El páncreas es una glándula mixta: sus células beta, en los islotes de Langerhans, producen insulina, la hormona que regula tu glucosa en sangre.",
                    "La insulina actúa como una llave molecular: se une a receptores en la membrana celular y permite el transporte de glucosa al interior para producir energía.",
                    "En ayuno, las células alfa del páncreas liberan glucagón, que estimula al hígado a convertir glucógeno en glucosa (glucogenólisis) y liberarla a la sangre, regulando la glucemia junto con la insulina."
                },
                // Ilustraciones y textos del cliente (Recursos_LibroFisiologico, Información_temas.pdf).
                TopicIlustracion = fisioIlustraciones,
                TopicDescubre = new[]
                {
                    "Dentro del páncreas viven unas pequeñas células llamadas células beta. Su trabajo es producir insulina, que aquí vemos representada como una llave.\n\nNuestro cuerpo también tiene un equipo de defensa llamado sistema inmunitario. Su trabajo es ayudarnos a protegernos. Pero, en la diabetes tipo 1, este equipo se confunde y ataca por error a las células beta.\n\nCon el paso del tiempo, algunas células beta dejan de funcionar y se van perdiendo. Por eso, el páncreas produce menos insulina. Esto no ocurrió por algo que hiciste. La diabetes tipo 1 no es tu culpa.",
                    null,
                    "La glucosa viaja por la sangre hasta los músculos. La insulina también viaja por la sangre y ayuda a que la glucosa entre en las células musculares.\n\nCuando la insulina llega a una célula muscular, se une a su receptor, como si tocara un timbre. El receptor envía una señal al interior de la célula. Esa señal ayuda a preparar las pequeñas puertas GLUT4, por donde puede entrar la glucosa.\n\n¡Ahora los cubitos de glucosa entran uno por uno! Con ayuda de la mitocondria, la célula aprovecha la glucosa para producir ATP, una molécula que le permite usar energía para realizar sus tareas.",
                    "Cuando pasa un tiempo sin comer, la cantidad de glucosa en la sangre comienza a disminuir. Entonces, las células alfa del páncreas liberan una hormona llamada glucagón. El glucagón viaja por la sangre hasta llegar al hígado y le envía un mensaje.\n\nEl hígado guarda una reserva llamada glucógeno. Al recibir el mensaje, transforma parte de esa reserva en glucosa. Después, libera la glucosa a la sangre para que pueda llegar a otras partes del cuerpo. Así, el glucagón y el hígado ayudan a regular la cantidad de glucosa en la sangre."
                },
                TopicIcon = new[] { figTemas[0], figTemas[1], figTemas[2], figTemas[3] },
                Markers = markersFisio, MarkersExtra = markersFisioExtra,
                Narration = new[] { N(0), N(1), N(2), N(3) },
                Quiz = new[]
                {
                    new GameItem { Q = "Relaciona cada célula con lo que le corresponde:",
                        Options = new[] { "Célula beta — insulina", "Célula alfa — glucosa", "Glóbulo rojo — insulina", "Neurona — glucógeno" }, Correct = 0 },
                    new GameItem { Q = "Cuando la glucosa entra a la célula, se transforma en ______ que es la energía que la célula realmente puede usar para hacer su trabajo.",
                        Options = new[] { "ATP", "glucógeno", "insulina", "agua" }, Correct = 0 },
                    new GameItem { Q = "¿Qué le permite a la insulina ayudar a que la glucosa entre a la célula?",
                        Options = new[] { "Un receptor en la célula", "El agua", "Los glóbulos rojos", "El calcio de los huesos" }, Correct = 0 }
                },
            };
            books[1] = new BookDef
            {
                Title = "Libro Nutricional", Accent = UIKit.Nutri, HeroIcon = spIconNutri,
                Model = platoModel, TopicModel = nutriModelos, TopicPaleta = nutriPaletas, ModelTint = UIKit.Hex("7CB86F"),
                TopicTitle = new[] { "Plato saludable", "Carbohidratos", "Alimentos recomendados", "Hábitos saludables" },
                TopicSub = new[] { "Equilibra tu comida", "La energía más rápida", "Elige mejor", "Cuídate cada día" },
                DescKids = new[]
                {
                    "Un plato feliz tiene la mitad de verduras, un poco de proteína y un poco de carbohidratos. ¡Así tu cuerpo está contento!",
                    "Los carbohidratos son la comida que sube tu energía más rápido, como el pan, el arroz y las frutas.",
                    "Las verduras y las proteínas te ayudan a mantenerte fuerte y con energía estable todo el día.",
                    "Comer a la misma hora, tomar agua y no comer tantos dulces hace que cuidarte sea más fácil."
                },
                DescPre = new[]
                {
                    "El plato saludable te ayuda a equilibrar tu comida: mitad de vegetales, un cuarto de proteína y un cuarto de carbohidratos. Así tu glucosa sube de forma más estable.",
                    "Los carbohidratos son la energía que más rápido sube tu glucosa. Aprende a reconocerlos en el pan, el arroz, la pasta y las frutas.",
                    "Las verduras, las proteínas magras y las grasas saludables te ayudan a mantener tu glucosa estable durante más tiempo.",
                    "Comer a las mismas horas, tomar agua y evitar azúcares en exceso hacen que tu diabetes sea mucho más fácil de controlar cada día."
                },
                DescTeens = new[]
                {
                    "El método del plato ayuda a controlar la carga glucémica de tus comidas: 50% vegetales, 25% proteína y 25% carbohidratos, favoreciendo una curva de glucosa más estable.",
                    "Los carbohidratos tienen el mayor impacto en la glucemia postprandial. Conocer su índice glucémico te ayuda a anticipar y ajustar tu dosis de insulina.",
                    "Priorizar fibra, proteínas magras y grasas saludables enlentece la absorción de glucosa, reduciendo picos glucémicos bruscos.",
                    "Mantener horarios regulares de comida, buena hidratación y control de azúcares simples mejora la variabilidad glucémica a largo plazo."
                },
                TopicIcon = new[] { figTemas[4], figTemas[5], figTemas[6], figTemas[7] },
                Markers = markersNutri, MarkersExtra = markersNutriExtra,
                Narration = new[] { N(4), N(5), N(6), N(7) },
                Quiz = new[]
                {
                    new GameItem { Q = "¿Qué combinación forma un plato saludable?",
                        Options = new[] { "Verduras, proteína y carbohidratos", "Solo dulces", "Solo carne", "Bebidas azucaradas" }, Correct = 0 },
                    new GameItem { Q = "¿Qué nutriente se convierte principalmente en glucosa?",
                        Options = new[] { "Carbohidratos", "Agua", "Vitaminas", "Minerales" }, Correct = 0 },
                    new GameItem { Q = "¿Cuál es una buena opción para una colación?",
                        Options = new[] { "Fruta con yogur natural", "Refresco", "Caramelos", "Papas fritas" }, Correct = 0 }
                },
            };
            books[2] = new BookDef
            {
                Title = "Libro Clínico", Accent = UIKit.Clin, HeroIcon = spIconClinico,
                // Modelos animados del cliente (Recursos Libro Clínico): aplicación de insulina (tema 1),
                // glucómetro (tema 3) y autocuidado interactivo (tema 4); Síntomas sigue con Gluco.fbx.
                Model = glucoModel, TopicModel = clinModelos, ModelTint = UIKit.Hex("4E8FD1"),
                TopicTitle = new[] { "Cómo aplicar la insulina", "Síntomas", "Monitoreo de glucosa", "Organizo mi autocuidado" },
                TopicSub = new[] { "Paso a paso con la pluma", "Reconócelos a tiempo", "Mide y controla", "Mi rutina diaria" },
                DescKids = new[]
                {
                    "La insulina se aplica con una plumita, siempre con un adulto y como dice tu equipo de salud. ¡Primero, manos limpias!",
                    "Tener mucha sed, ir mucho al baño y sentirte cansado puede avisarte que algo anda diferente.",
                    "Medir tu glucosa con el glucómetro te ayuda a saber cómo está tu cuerpo, como revisar la energía de un videojuego.",
                    "Comer rico y sano, medir tu glucosa, seguir tu tratamiento, jugar y dormir bien: ¡esa es tu rutina de superhéroe!"
                },
                DescPre = new[]
                {
                    "La insulina se aplica con una pluma o una bomba, generalmente antes de comer. Lavarse las manos, preparar la pluma, revisar la dosis y elegir la zona son pasos que se hacen siempre siguiendo a tu equipo de salud.",
                    "Mucha sed, ganas frecuentes de ir al baño y cansancio pueden indicar que tu glucosa está muy alta o muy baja.",
                    "Medir tu glucosa con el glucómetro varias veces al día te ayuda a saber cómo reacciona tu cuerpo a la comida y al ejercicio.",
                    "El autocuidado son las acciones que haces cada día para estar sano: comer bien, medir tu glucosa, seguir tu tratamiento, moverte y descansar. ¡Arma tu rutina en el calendario!"
                },
                DescTeens = new[]
                {
                    "La insulina se administra por vía subcutánea (pluma, jeringa o bomba): higiene de manos, purgado de la pluma, verificación de la dosis y rotación de la zona de aplicación, siempre según la indicación del equipo de salud.",
                    "Poliuria, polidipsia y fatiga son signos clásicos de hiperglucemia; temblor, sudoración y confusión pueden indicar hipoglucemia — ambos requieren atención inmediata.",
                    "El automonitoreo (glucómetro o sensor continuo) permite identificar patrones glucémicos y ajustar decisiones de insulina, alimentación y actividad física.",
                    "Una rutina diaria de autocuidado (alimentación, automonitoreo, tratamiento, actividad física y descanso) facilita el control glucémico y crea hábitos sostenibles."
                },
                // Contenido del botón Información en la pantalla AR (PDF del cliente).
                TopicInfoAR = new[]
                {
                    "<b>¿Qué es la insulina?</b>\nLa insulina es una hormona que produce el páncreas y ayuda a que la glucosa entre en muchas células del cuerpo para utilizarse como energía.\n\n<b>¿Cómo funciona?</b>\n1. La insulina está representada por una llave azul.\n2. La llave se une a un receptor de la célula.\n3. Se activa una señal que permite la entrada de glucosa.\n4. La glucosa entra en la célula.\n5. La célula puede utilizar la glucosa para obtener energía.\n\n<b>¿Por qué es importante?</b>\nEn la diabetes tipo 1, el cuerpo produce muy poca o ninguna insulina, por lo que necesitamos administrarla como parte del tratamiento.\n\n<i>Esta animación es una demostración educativa. La administración real debe seguir las indicaciones del equipo de salud.</i>",
                    null,
                    "<b>¿Qué es el glucómetro?</b>\nEl glucómetro es un pequeño dispositivo que mide la cantidad de glucosa (azúcar) que tenemos en la sangre.\n\n<b>¿Cómo funciona?</b>\n1. Se coloca una tira reactiva en el glucómetro.\n2. Se aplica una pequeña muestra de sangre en la tira.\n3. El dispositivo analiza la muestra.\n4. La pantalla muestra el resultado de glucosa.\n\n<b>¿Por qué es importante?</b>\nNos ayuda a conocer nuestros niveles de glucosa y a cuidar nuestra salud siguiendo las indicaciones del equipo médico.",
                    "<b>¿Qué es el autocuidado?</b>\nEl autocuidado es el conjunto de acciones que realizamos cada día para mantenernos saludables y controlar nuestra diabetes.\n\n<b>¿Cómo puedo cuidarme?</b>\n1. Alimentación: comer alimentos saludables y variados.\n2. Medición: revisar mi glucosa según mi plan de cuidado.\n3. Tratamiento: seguir las indicaciones de mi equipo de salud para administrar la insulina.\n4. Actividad física: realizar actividades y movimientos que disfrute.\n5. Descanso: dormir lo suficiente para recuperar energía.\n\n<b>¿Por qué es importante?</b>\nTener una rutina nos ayuda a recordar nuestras actividades y desarrollar hábitos para cuidar nuestra salud.\n\n<i>Toca las cinco actividades para ponerlas en tu calendario.</i>"
                },
                TopicIcon = new[] { figTemas[8], figTemas[9], figTemas[10], figTemas[11] },
                Markers = markersClinico, MarkersExtra = markersClinicoExtra,
                // Narraciones del cliente («Texto para el botón Audio») en los temas con modelo nuevo.
                Narration = new[] { ClinN(0, N(8)), N(9), ClinN(2, N(10)), ClinN(3, N(11)) },
                // Minijuegos del cliente: 3 misiones de 3 preguntas.
                Quiz = Misiones(
                    ("Misión 1", "Monitoreo de glucosa", new[]
                    {
                        new GameItem { Q = "¿Para qué sirve el glucómetro?",
                            Options = new[] { "Medir la temperatura del cuerpo.", "Medir la cantidad de glucosa en la sangre.", "Medir el peso corporal." }, Correct = 1 },
                        new GameItem { Q = "¿Qué colocamos en el glucómetro para realizar una medición?",
                            Options = new[] { "Una llave.", "Una aguja de insulina.", "Una tira reactiva." }, Correct = 2 },
                        new GameItem { Q = "¿Dónde podemos observar el resultado de nuestra glucosa?",
                            Options = new[] { "En la pantalla del glucómetro.", "En la tira reactiva.", "En el botón del glucómetro." }, Correct = 0 },
                    }),
                    ("Misión 2", "Administración de insulina", new[]
                    {
                        new GameItem { Q = "¿A qué se parece la insulina en nuestro libro?",
                            Options = new[] { "Una llave que ayuda a abrir la puerta de las células.", "Un reloj que mide el tiempo.", "Un termómetro que mide la temperatura." }, Correct = 0 },
                        new GameItem { Q = "¿Qué ayuda a hacer la insulina en nuestro cuerpo?",
                            Options = new[] { "Cambiar el color de la sangre.", "Ayudar a que la glucosa entre en muchas células.", "Medir la cantidad de glucosa." }, Correct = 1 },
                        new GameItem { Q = "¿Qué órgano produce la insulina?",
                            Options = new[] { "El corazón.", "Los pulmones.", "El páncreas." }, Correct = 2 },
                    }),
                    ("Misión 3", "Autocuidado", new[]
                    {
                        new GameItem { Q = "¿Cuál de estas acciones forma parte de mi autocuidado diario?",
                            Options = new[] { "Olvidarme de mi tratamiento.", "Revisar mi glucosa según mi plan de cuidado.", "No descansar durante el día." }, Correct = 1 },
                        new GameItem { Q = "¿Qué debo hacer si me siento mal o noto señales de alerta?",
                            Options = new[] { "Ignorar lo que siento.", "Esperar sin decirle a nadie.", "Avisar a un adulto de confianza y seguir mi plan de cuidado." }, Correct = 2 },
                        new GameItem { Q = "¿Qué hábitos me ayudan a cuidar mi salud?",
                            Options = new[] { "Alimentarme bien, realizar actividad física segura y descansar.", "Comer solamente dulces.", "Dormir poco y no realizar actividades." }, Correct = 0 },
                    })),
            };
            for (int i = 0; i < books.Length; i++) AppState.QuizTotal[i] = books[i].Quiz.Length;
        }
    }
}
