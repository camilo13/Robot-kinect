using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;

namespace WpfApplication1
{
    /// <summary>
    /// Lógica de interacción para Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        KinectSensor _sensor;
        SpeechRecognitionEngine speechengine;

        public Window2()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this._sensor != null)
            {
                this._sensor.AudioSource.Stop();
                this._sensor.Stop();
                this._sensor = null;
            }
        }
        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case (KinectStatus.Connected):
                    conectaActiva();
                    break;

                case (KinectStatus.Disconnected):
                    if (this._sensor == e.Sensor)
                    {
                        this._sensor = null;
                        this._sensor = KinectSensor.KinectSensors.FirstOrDefault(x => x.Status == KinectStatus.Connected);
                        if (this._sensor == null)
                        {   
                            //No hay kinect conectado
                            aviso.Text = "No escucho";
                        }
                    }
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            conectaActiva();
        }

        void conectaActiva()
        {
            if (KinectSensor.KinectSensors.Count > 0)
            {
                if (this._sensor == null)
                {
                    this._sensor = KinectSensor.KinectSensors[0];
                    if (this._sensor != null)
                    {
                        try
                        {
                            this._sensor.Start();
                            _sensor.ElevationAngle = 4;
                            //Se conectó el kinect
                            aviso.Text = "Ya escucho";

                        }
                        catch (Exception ex)
                        {
                            aviso.Text = ex.Message.ToString();

                        }

                        RecognizerInfo ri = obtenerLP();

                        if (ri != null)
                        {
                            aviso.Text = "Se ha encontrado el lenguaje_pack";
                            this.speechengine = new SpeechRecognitionEngine(ri.Id);
                            var opciones = new Choices();
                            opciones.Add("ataca", "ATACA");
                            opciones.Add("ata", "ATACA");
                            opciones.Add("atac", "ATACA");
                            opciones.Add("atak", "ATACA");
                            opciones.Add("atacar", "ATACA");
                            opciones.Add("camina", "CAMINA");
                            opciones.Add("caminar", "CAMINA");
                            opciones.Add("camino", "CAMINA");
                            opciones.Add("camilo", "CAMINA");
                            opciones.Add("cama", "CAMINA");
                            /*opciones.Add(new SemanticResultValue("Windows ocho", "TRES"));
                            opciones.Add(new SemanticResultValue("nuevo windows", "TRES"));
                            */
                            var grammarb = new GrammarBuilder { Culture = ri.Culture };
                            grammarb.Append(opciones);
                            var grammar = new Grammar(grammarb);
                            this.speechengine.LoadGrammar(grammar);
                            speechengine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(speechengine_SpeechRecognized);
                            speechengine.SetInputToAudioStream(_sensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                            speechengine.RecognizeAsync(RecognizeMode.Multiple);
                        }
                    }
                }
            }
        }
        private RecognizerInfo obtenerLP()
        {
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "es-MX".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }

            }
            return null;
        }
        void speechengine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            const double igualdad = 0.7;

            if (e.Result.Confidence < igualdad)
            {
                
                switch (e.Result.Words[0].Text)
                {
                    case "ATACA":
                        mensaje.Text = "ataca";
                        mensaje.Background = new SolidColorBrush(Color.FromRgb(247, 126, 5));
                        
                        break;

                    case "CAMINA":
                        mensaje.Text = "camina";
                        mensaje.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        break;


                }
            }
        }
    }
}
