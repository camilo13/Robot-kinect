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
using System.IO;
using Microsoft.Kinect;
using System.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        String com = "COM4"; //Puerto COM

        //Variables requeridas por Kinect
        private const float RenderWidth = 640.0f;
        private const float RenderHeight = 480.0f;
        private const double JointThickness = 3;
        private const double BodyCenterThickness = 10;
        private const double ClipBoundsThickness = 10;
        private readonly Brush centerPointBrush = Brushes.Blue;
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        private readonly Brush inferredJointBrush = Brushes.Yellow;
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);
        private KinectSensor sensor;
        private DrawingGroup drawingGroup;
        private DrawingImage imageSource;

        //Variables para el Swipe o gesto de atacar
        const float SwipeMinimalLength = 0.4f;
        const float SwipeMaximalHeight = 0.2f;
        const int SwipeMininalDuration = 250;
        const int SwipeMaximalDuration = 1500;
        DateTime lastGestureDate = DateTime.Now;
        int MinimalPeriodBetweenGestures = 0;
        //

        List<Vector3> positionList = new List<Vector3>();
        List<Gesture> gestureAcceptedList = new List<Gesture>();
        
        public Window1()
        {
            InitializeComponent();
        }

        //Dibujando el Skeleton o esqueleto que reperesentará a la persona frente al kinect
        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.drawingGroup = new DrawingGroup();
            this.imageSource = new DrawingImage(this.drawingGroup);
            Image.Source = this.imageSource;
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {               
                this.sensor.SkeletonStream.Enable();
                this.sensor.ColorStream.Enable();
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;
                
                // Iniciando el sensor Kinect
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }

            if (null == this.sensor)
            {   
                //No se encuentra el kinect
                this.mensajemov.Text = "No encuentro mis ojos";
            }

        }   

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }


        [Serializable]
        public struct Vector3
        {
            public float X;
            public float Y;
            public float Z;
            public DateTime date;
        }

        public enum Gesture
        {
            None,
            SwipeRightToLeft,
        }

        //Metodo que permite reconocer el gesto y atacar
        void SwipeRightToLeft()
        {
            int start = 0;
            for (int index = 0; index < positionList.Count - 1; index++)
            {
                if ((Math.Abs(positionList[0].Y - positionList[index].Y) > SwipeMaximalHeight) || (positionList[index].X - positionList[index + 1].X < -0.01f))
                {
                    start = index;
                }

                if ((Math.Abs(positionList[index].X - positionList[start].X) > SwipeMinimalLength))
                {
                    double totalMilliseconds = (positionList[index].date - positionList[start].date).TotalMilliseconds;
                    if (totalMilliseconds >= SwipeMininalDuration && totalMilliseconds <= SwipeMaximalDuration)
                    {
                        if (DateTime.Now.Subtract(lastGestureDate).TotalMilliseconds > MinimalPeriodBetweenGestures)
                        {
                            mensajemov.Text = "SwipeRightToLeft " + gestureAcceptedList.Count;
                            gestureAcceptedList.Add(Gesture.SwipeRightToLeft);
                            lastGestureDate = DateTime.Now;
                            positionList.Clear();
                            Robot robot = new Robot(com);
                            robot.atacar();
                        }
                    }
                }

            }
        }

        //Algoritmo que detecta la postura girar derecha
        bool Derecha(Vector3 mano, Vector3 codo, Vector3 hombro, Vector3 cabeza)
        {
            if (Math.Abs((mano.Z - codo.Z) + (mano.Z - hombro.Z) + (codo.Z - hombro.Z)) <= 0.10f && Math.Abs((mano.Y - codo.Y) + (mano.Y - hombro.Y) + (codo.Y - hombro.Y)) < 0.10f && mano.X > cabeza.X)
            {
                return true;
            }
            else
                return false;
        }

        //Algoritmo que detecta la postura girar izquierda
        bool Izquierda(Vector3 mano, Vector3 codo, Vector3 hombro, Vector3 cabeza)
        {
            if (Math.Abs((mano.Z - codo.Z) + (mano.Z - hombro.Z) + (codo.Z - hombro.Z)) <= 0.10f && Math.Abs((mano.Y - codo.Y) + (mano.Y - hombro.Y) + (codo.Y - hombro.Y)) <= 0.10f && mano.X < cabeza.X)
            {
                return true;
            }
            else
                return false;
        }

        //Algoritmo que detecta postura caminar
        bool Adelante(Vector3 manod, Vector3 manoi, Vector3 codod, Vector3 codoi)
        {
            if (Math.Abs((manod.X - codod.X)) <= 0.1 && Math.Abs((manod.Y - codod.Y)) <= 0.1 && manod.Z < codod.Z && Math.Abs((manoi.X - codoi.X)) <= 0.1 && Math.Abs((manoi.Y - codoi.Y)) <= 0.1 && manoi.Z < codoi.Z)
            {
                return true;
            }
            else
                return false;
        }

        const int PostureDetectionNumber = 10;
        int accumulator = 0;
        Posture postureInDetection = Posture.None;
        Posture previousPosture = Posture.None;
        public enum Posture
        {
            None,
            Derecha,
            Izquierda,
            Adelante
        }

        //algoritmo que permite detectar postura
        bool PostureDetector(Posture posture)
        {
            if (postureInDetection != posture)
            {
                accumulator = 0;
                postureInDetection = posture;
                return false;
            }
            if (accumulator < PostureDetectionNumber)
            {
                accumulator++;
                return false;
            }
            if (posture != previousPosture)
            {
                previousPosture = posture;
                accumulator = 0;
                return true;
            }
            else
                accumulator = 0;
            return false;
        }
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Dibuja fondo oscuro
                dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {
                        RenderClippedEdges(skel, dc);

                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.DrawBonesAndJoints(skel, dc);
                            
                            Vector3 manoD = new Vector3();
                            manoD.X = skel.Joints[JointType.HandRight].Position.X;
                            manoD.Y = skel.Joints[JointType.HandRight].Position.Y;
                            manoD.Z = skel.Joints[JointType.HandRight].Position.Z;

                            Vector3 cabeza = new Vector3();
                            cabeza.X = skel.Joints[JointType.Head].Position.X;
                            cabeza.Y = skel.Joints[JointType.Head].Position.Y;
                            cabeza.Z = skel.Joints[JointType.Head].Position.Z;

                            Vector3 manoI = new Vector3();
                            manoI.X = skel.Joints[JointType.HandLeft].Position.X;
                            manoI.Y = skel.Joints[JointType.HandLeft].Position.Y;
                            manoI.Z = skel.Joints[JointType.HandLeft].Position.Z;

                            Vector3 hombroD = new Vector3();
                            hombroD.X = skel.Joints[JointType.ShoulderRight].Position.X;
                            hombroD.Y = skel.Joints[JointType.ShoulderRight].Position.Y;
                            hombroD.Z = skel.Joints[JointType.ShoulderRight].Position.Z;

                            Vector3 hombroI = new Vector3();
                            hombroI.X = skel.Joints[JointType.ShoulderLeft].Position.X;
                            hombroI.Y = skel.Joints[JointType.ShoulderLeft].Position.Y;
                            hombroI.Z = skel.Joints[JointType.ShoulderLeft].Position.Z;

                            Vector3 codoI = new Vector3();
                            codoI.X = skel.Joints[JointType.ElbowLeft].Position.X;
                            codoI.Y = skel.Joints[JointType.ElbowLeft].Position.Y;
                            codoI.Z = skel.Joints[JointType.ElbowLeft].Position.Z;

                            Vector3 codoD = new Vector3();
                            codoD.X = skel.Joints[JointType.ElbowRight].Position.X;
                            codoD.Y = skel.Joints[JointType.ElbowRight].Position.Y;
                            codoD.Z = skel.Joints[JointType.ElbowRight].Position.Z;
                            
                            positionList.Add(new Vector3()
                            {
                                X = manoD.X,
                                Y = manoD.Y,
                                Z = manoD.Z,
                                date = DateTime.Now
                            });
                            
                            SwipeRightToLeft();

                            if (positionList.Count() > 20)
                            {
                                positionList.RemoveAt(0);
                            }
                            
                            if (Adelante(manoD, manoI, codoD, codoI))
                            {
                                if (PostureDetector(Posture.Adelante))
                                {
                                    mensajemov.Text = previousPosture.ToString();

                                    Robot robot = new Robot(com);
                                    robot.caminar();
                                }
                            }

                            else if (Derecha( manoD, codoD, hombroD, cabeza))
                            {
                                if (PostureDetector(Posture.Derecha))
                                {
                                    mensajemov.Text = previousPosture.ToString();
                                    Robot robot = new Robot(com);
                                    robot.derecha();
                                }

                            }
                            else if(Izquierda(manoI, codoI, hombroI, cabeza))
                            {
                                if (PostureDetector(Posture.Izquierda))
                                {
                                    mensajemov.Text = previousPosture.ToString();
                                    Robot robot = new Robot(com);
                                    robot.izquierda();
                                }
                            }
                            else 
                                if (PostureDetector(Posture.None))
                                    mensajemov.Text = previousPosture.ToString();
                        }
                        else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                        {
                            dc.DrawEllipse(
                            this.centerPointBrush,
                            null,
                            this.SkeletonPointToScreen(skel.Position),
                            BodyCenterThickness,
                            BodyCenterThickness);
                        }


                    }
                }
                // Previene que el dibujo salgan del cuadro
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }

        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Dibuja el torso
            this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

            // dibuja brazo izquierdo
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // dibuja brazo derecho
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

            // dibuja pierna izquierda
            this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // dibuja pierna derecha
            this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

            // dibuja articulaciones o joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                }
            }
        }

        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Obtiene los joints desde el espectro de profundidad  
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }
            
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = this.trackedBonePen;
            }

            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
        }

    }
}
