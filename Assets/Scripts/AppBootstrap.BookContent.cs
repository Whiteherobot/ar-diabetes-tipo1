using System.Collections.Generic;
using UnityEngine;

namespace ARDiabetes
{
    public partial class AppBootstrap
    {
        // ============================================================
        // Contenido de los 3 libros
        // ============================================================
        void BuildBooks()
        {
            AudioClip N(int i) => (narracion != null && i < narracion.Length) ? narracion[i] : null;

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
                Markers = markersNutri,
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
                Model = glucoModel, ModelTint = UIKit.Hex("4E8FD1"),
                TopicTitle = new[] { "Uso de insulina", "Síntomas", "Monitoreo de glucosa", "Cuidados diarios" },
                TopicSub = new[] { "Cómo aplicarla", "Reconócelos a tiempo", "Mide y controla", "Rutina de cuidado" },
                DescKids = new[]
                {
                    "La insulina se aplica con una inyectorcita o una bombita, antes de comer, para ayudarte a usar la energía de la comida.",
                    "Tener mucha sed, ir mucho al baño y sentirte cansado puede avisarte que algo anda diferente.",
                    "Medir tu glucosa con el glucómetro te ayuda a saber cómo está tu cuerpo, como revisar la energía de un videojuego.",
                    "Revisar tus piecitos, guardar bien tu insulina en frío y llevar algo dulce por si acaso son tus superpoderes de cuidado."
                },
                DescPre = new[]
                {
                    "La insulina se aplica con una inyección o una bomba, generalmente antes de comer, para ayudar a que la glucosa entre a tus células.",
                    "Mucha sed, ganas frecuentes de ir al baño y cansancio pueden indicar que tu glucosa está muy alta o muy baja.",
                    "Medir tu glucosa con el glucómetro varias veces al día te ayuda a saber cómo reacciona tu cuerpo a la comida y al ejercicio.",
                    "Revisar tus pies, guardar bien tu insulina en frío y llevar siempre algo dulce por si tu glucosa baja son hábitos que cuidan tu salud."
                },
                DescTeens = new[]
                {
                    "La insulina se administra por vía subcutánea (pluma, jeringa o bomba), habitualmente antes de las comidas, ajustando la dosis según los carbohidratos y la glucosa previa.",
                    "Poliuria, polidipsia y fatiga son signos clásicos de hiperglucemia; temblor, sudoración y confusión pueden indicar hipoglucemia — ambos requieren atención inmediata.",
                    "El automonitoreo (glucómetro o sensor continuo) permite identificar patrones glucémicos y ajustar decisiones de insulina, alimentación y actividad física.",
                    "La inspección diaria de los pies, la conservación adecuada de la insulina en frío y llevar una fuente rápida de glucosa son hábitos clave para prevenir complicaciones."
                },
                TopicIcon = new[] { figTemas[8], figTemas[9], figTemas[10], figTemas[11] },
                Markers = markersClinico,
                Narration = new[] { N(8), N(9), N(10), N(11) },
                Quiz = new[]
                {
                    new GameItem { Q = "¿Qué necesitas para medir tu glucosa?",
                        Options = new[] { "Glucómetro", "Termómetro", "Regla", "Reloj" }, Correct = 0 },
                    new GameItem { Q = "La insulina debe aplicarse siguiendo las indicaciones de:",
                        Options = new[] { "Tu equipo de salud", "Cualquier amigo", "Internet", "Nadie" }, Correct = 0 },
                    new GameItem { Q = "¿Qué debes hacer antes de medir tu glucosa?",
                        Options = new[] { "Lavar y secar tus manos", "Comer un dulce", "Correr", "Dormir" }, Correct = 0 }
                },
            };
        }
    }
}
