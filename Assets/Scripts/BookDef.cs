using UnityEngine;

namespace ARDiabetes
{
    /// <summary>Contenido de un libro (Fisiológico/Nutricional/Clínico): temas, modelo 3D y marcadores.</summary>
    public class BookDef
    {
        public string Title;
        public Color Accent;
        public Sprite HeroIcon;
        public GameObject Model;
        public Color ModelTint;
        public string[] TopicTitle;
        public string[] TopicSub;
        public Sprite[] TopicIcon;
        public Texture2D[] Markers;
        public AudioClip[] Narration;

        // Misma info en 3 niveles de lenguaje. La app usa por ahora un solo nivel (DescPre, 10-12 años);
        // DescKids y DescTeens se conservan por si se retoman.
        public string[] DescKids;
        public string[] DescPre;
        public string[] DescTeens;

        public string Desc(int topic) => DescPre[topic];

        // Juegos y Retos del libro.
        public GameItem[] Quiz;
    }

    /// <summary>Una pregunta de "Juegos y Retos". El tipo se deduce del texto: "Relaciona" muestra
    /// parejas ("A — B"), y "______" es una frase por completar.</summary>
    public class GameItem
    {
        public string Q;
        public string[] Options;
        public int Correct;
        public bool EsRelacion => Q.Contains("Relaciona");
        public bool EsFrase => Q.Contains("______");
    }
}
