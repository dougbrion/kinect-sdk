# kinect-sdk
Hello World for the Kinect SDK - see the Instructables tutorial http://www.instructables.com/id/Kinect-SDK-Hello-World/

### Intro

In this application we will be using Visual Studio (In my case 2015) to create a WPF Application using the Kinect and to get you up and running with the SDK. We will be initiating the Kinect, retrieving skeleton data to get joint coordinates, and then using these coordinates to determine if your left or right hand is raised. This is just a hello world, but you can use the same principles for more advanced projects with the SDK. (I will try and upload some of mine if the swine that is time is on my side...)

### Prerequisites

In this application I am using the Kinect for Windows, this Kinect has the model number 1517 this means it is the Windows version and thus can be used for commercial use and sadly doesn't work with other OS's. You may have the Xbox version models 1414, 1473 and these should also work.

First thing to do is to install the Kinect SDK and Development Kit. You can get them from these links:

https://www.microsoft.com/en-gb/download/details.aspx?id=40278

https://www.microsoft.com/en-gb/download/details.aspx?id=40276

You will also need Visual Studio for this project, head over and download the VS2015 Community edition, its free and packed with so many features it will make your mind boggle!

https://www.visualstudio.com/vs/

The downloads may take some time but all should be okay! Once they have installed plug the Kinect in to the power and your PC's USB and we can begin coding.

### Creating Project and Importing References

Lets get down to writing the application.

First Select **‘New Project’** and choose WPF Application lets name is KinectSDK18. Once you have created the project we want to add some references, so right click on **‘References’** in the Solution Explorer and Browse to choose the references we want.

For this project we want to add **‘Microsoft.Kinect.dll’** which is found inside the \v1.8\Assemblies. We also want to add **Microsoft.Kinect.Toolkit, Microsoft.Kinect.Toolkit.Controls,** and **Microsoft.Kinect.Toolkit.Interaction** which can all be found in \Developer Toolkit v1.8.0\Assemblies.

Your path for the references may vary to mine depending on where you installed the Kinect SDK and Development Kit but they hopefully shouldn't be too hard to find.

### XAML

We can now start coding! First we will look at the XAML side of things, so head over to your **MainWindow.xaml**

Above is the whole XAML code for you to look at and think about, I will then go through how its done and what each line does. You can either drag and drop items from the toolbar which will automatically generate the code or just type the code in yourself. I am having issues uploading XAML at the moment so download the file and go through my intructions whilst following it, or copy from the picture.

First we want to add
```
xmlns:k="http://schemas.microsoft.com/kinect/2013"
```
 this brings in the kinect package allowing us to have access to nice UI features such as the Kinect Status one which we use in this tutorial.

We now want to add the Kinect Status UI so insert these lines of code, here I have called it KinectSensorChooserUI.

I have also created a label where the content changes depending on the current status of the Kinect. e.g. When the Kinect is connected it will say "Connected"

The rest of the XAML is just Textboxes, these are being treated both as titles such "Left Wrist" but I also have blank textboxes which I have linked to the C# code. These will have the current coordinate values for both wrists and also their state, whether they are raised or lowered.

### The Brains

First make sure you have imported the Microsoft Kinect and Toolkit Libraries.

In **MainWindow()** we activate the Kinect:

```
C#
private KinectSensor sensor;
```

Then when the window loads we are going to start the Kinect Chooser to get the Status of the kinect.

```
C#
private void MainWindowLoaded(object sender, RoutedEventArgs e)
{

var sensorStatus = new KinectSensorChooser();

sensorStatus.KinectChanged += KinectSensorChooserKinectChanged;

kinectChooser.KinectSensorChooser = sensorStatus; sensorStatus.Start();

}
```

The the label is updated by **KinectSensorChooserKinectChanged**, due to the switch we create in this function. Here also the Skeleton is initialised.

```
C#
private void KinectSensorChooserKinectChanged(object sender, KinectChangedEventArgs e)
{

if (sensor != null) sensor.SkeletonFrameReady -= KinectSkeletonFrameReady;

sensor = e.NewSensor;

if (sensor == null) return;

switch (Convert.ToString(e.NewSensor.Status))

{

case "Connected": KinectStatus.Content = "Connected"; break;

case "Disconnected": KinectStatus.Content = "Disconnected"; break;

case "Error": KinectStatus.Content = "Error"; break;

case "NotReady": KinectStatus.Content = "Not Ready"; break;

case "NotPowered": KinectStatus.Content = "Not Powered"; break;

case "Initializing": KinectStatus.Content = "Initialising"; break;

default: KinectStatus.Content = "Undefined"; break;

}

sensor.SkeletonStream.Enable(); sensor.SkeletonFrameReady += KinectSkeletonFrameReady;

}
```

Once we a tracking the Skeleton we can look for the X,Y and Z coordinates for specific points on the skeleton. In this example we are after the coordinates of the hands of the user. We show the values as text in the window in our textblocks. We then compare the Z position of the users hip to the Z position of their hand. If it is over a certain threshold we are going to treat the users arm as being 'raised' and therefore print to the window which arm is up.

```
C#
private void KinectSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
{

var skeletons = new Skeleton[0];

using (var skeletonFrame = e.OpenSkeletonFrame())

{

if (skeletonFrame != null)

{

skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength]; skeletonFrame.CopySkeletonDataTo(skeletons);

}

}

if (skeletons.Length == 0) { return; }

var skel = skeletons.FirstOrDefault(x => x.TrackingState == SkeletonTrackingState.Tracked);

if (skel == null) { return; }

var rightHand = skel.Joints[JointType.WristRight]; XValueRight.Text = rightHand.Position.X.ToString(CultureInfo.InvariantCulture); YValueRight.Text = rightHand.Position.Y.ToString(CultureInfo.InvariantCulture); ZValueRight.Text = rightHand.Position.Z.ToString(CultureInfo.InvariantCulture);

var leftHand = skel.Joints[JointType.WristLeft]; XValueLeft.Text = leftHand.Position.X.ToString(CultureInfo.InvariantCulture); YValueLeft.Text = leftHand.Position.Y.ToString(CultureInfo.InvariantCulture); ZValueLeft.Text = leftHand.Position.Z.ToString(CultureInfo.InvariantCulture);

var centreHip = skel.Joints[JointType.HipCenter];

if (centreHip.Position.Z - rightHand.Position.Z > 0.35)

{

RightRaised.Text = "Raised";

}

else if (centreHip.Position.Z - leftHand.Position.Z > 0.35)

{

LeftRaised.Text = "Raised";

}

else

{

LeftRaised.Text = "Lowered"; RightRaised.Text = "Lowered";

}

}
```

You can change the 0.35 value depending on your arm length and how far you want to reach out before it thinks your arm is raised.

### Summary

You can see here the Kinect Sensor Chooser is blue showing the kinect is connected plus our label also agrees and says it is. We are also streaming position data from each of our hands and then comparing the Z distance of our hands and hip and if greater than 0.35 we are treating the hand has being raised. Here you can see the right hand is raised and left lowered.

In this basic Hello World you hopefully have learnt how to start a new project, install the Kinect libraries, write some XAML to change how your app looks and then a little bit of C# for the brains of the operation. Now you are tracking the skeleton you can obtain the coordinates for any joint on the body, and by comparing these joints who knows what wonderful ideas you could come up with. You could possibly make a dance along app where if certain distances between joints are hit you have completed a 'dance move'. Any way that's me over and out!
