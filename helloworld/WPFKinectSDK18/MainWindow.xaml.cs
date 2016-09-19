using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.IO.Ports;

namespace WPFKinectSDK18
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var sensorStatus = new KinectSensorChooser();

            sensorStatus.KinectChanged += KinectSensorChooserKinectChanged;

            kinectChooser.KinectSensorChooser = sensorStatus;
            sensorStatus.Start();

        }

        private void KinectSensorChooserKinectChanged(object sender, KinectChangedEventArgs e)
        {
            
            if (sensor != null)
                sensor.SkeletonFrameReady -= KinectSkeletonFrameReady;

            sensor = e.NewSensor;

            if (sensor == null)
                return;

            switch (Convert.ToString(e.NewSensor.Status))
            {
                case "Connected": KinectStatus.Content = "Connected";
                    break;
                case "Disconnected": KinectStatus.Content = "Disconnected";
                    break;
                case "Error": KinectStatus.Content = "Error";
                    break;
                case "NotReady": KinectStatus.Content = "Not Ready";
                    break;
                case "NotPowered": KinectStatus.Content = "Not Powered";
                    break;
                case "Initializing": KinectStatus.Content = "Initialising";
                    break;
                default: KinectStatus.Content = "Undefined";
                    break;
            }

            sensor.SkeletonStream.Enable(); 
            sensor.SkeletonFrameReady += KinectSkeletonFrameReady;

        }


        private void SensorDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void KinectSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            var skeletons = new Skeleton[0];

            using (var skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            if (skeletons.Length == 0)
            {
                return;
            }

            var skel = skeletons.FirstOrDefault(x => x.TrackingState == SkeletonTrackingState.Tracked);
            if (skel == null)
            {
                return;
            }

            var rightHand = skel.Joints[JointType.WristRight];
            XValueRight.Text = rightHand.Position.X.ToString(CultureInfo.InvariantCulture);
            YValueRight.Text = rightHand.Position.Y.ToString(CultureInfo.InvariantCulture);
            ZValueRight.Text = rightHand.Position.Z.ToString(CultureInfo.InvariantCulture);

            var leftHand = skel.Joints[JointType.WristLeft];
            XValueLeft.Text = leftHand.Position.X.ToString(CultureInfo.InvariantCulture);
            YValueLeft.Text = leftHand.Position.Y.ToString(CultureInfo.InvariantCulture);
            ZValueLeft.Text = leftHand.Position.Z.ToString(CultureInfo.InvariantCulture);

            var centreHip = skel.Joints[JointType.HipCenter];

            if (centreHip.Position.Z - rightHand.Position.Z > 0.3)
            {
                RightRaised.Text = "Raised";
            }
            else if (centreHip.Position.Z - leftHand.Position.Z > 0.3)
            {
                LeftRaised.Text = "Raised";
            }
            else
            {
                LeftRaised.Text = "Lowered";
                RightRaised.Text = "Lowered";
            }
        }
    }
}