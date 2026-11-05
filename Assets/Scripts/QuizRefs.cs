using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ARDiabetes
{
    // Referencias de una opción de respuesta; la posición se decide en AcomodarQuiz.
    class OpcionQuiz
    {
        public Button Btn, Audio;
        public Image Borde, Interior, Par, Pill, PillIco;
        public RectTransform Sombra, AudioSombra;
        public TMP_Text Texto, ParA, ParB, PillTxt;
        public RectTransform ParLinea;
    }

    class QuizRefs : MonoBehaviour
    {
        public RectTransform Content, Pregunta, Resultado, CoachBurbuja, CoachAudio, Coach, Tarjeta, TarjetaSombra, Reproductor, Feedback, FeedbackSombra, FeedbackAcciones;
        public RectTransform BurbujaSombra;
        public TMP_Text Progress, ProgressType, CoachTxt, Tipo, Question, FeedbackTitle, FeedbackText, ContinuarTxt, AudioStatus;
        public TMP_Text ResultTexto, ResultEstrellas;
        public Image ProgressFill, ResultFill, FeedbackBorde, FeedbackInterior, FeedbackIcon, AudioToggleIcon, AudioPulse;
        public OpcionQuiz[] Opciones;
        public int[] Orden;
        public int Elegida = -1;
        public bool Pausado, Sonando;
    }
}
