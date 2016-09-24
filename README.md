# kinect-sdk
Hello World for the Kinect SDK - see the Instructables tutorial http://www.instructables.com/id/Kinect-SDK-Hello-World/

![alt-text][hellopic]

### Intro

In this application we will be using Visual Studio (In my case 2015) to create a WPF Application using the Kinect and to get you up and running with the SDK. We will be initiating the Kinect, retrieving skeleton data to get joint coordinates, and then using these coordinates to determine if your left or right hand is raised. This is just a hello world, but you can use the same principles for more advanced projects with the SDK. (I will try and upload some of mine if the swine that is time is on my side...)

### Prerequisites

In this application I am using the Kinect for Windows, this Kinect has the model number 1517 this means it is the Windows version and thus can be used for commercial use and sadly doesn't work with other OS's. You may have the Xbox version models 1414, 1473 and these should also work.

First thing to do is to install the Kinect SDK and Development Kit. You can get them from these links:

https://www.microsoft.com/en-gb/download/details.a...

https://www.microsoft.com/en-gb/download/details.a...

You will also need Visual Studio for this project, head over and download the VS2015 Community edition, its free and packed with so many features it will make your mind boggle!

https://www.visualstudio.com/en-us/products/vs-201...

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

### C#

### Summary

You can see here the Kinect Sensor Chooser is blue showing the kinect is connected plus our label also agrees and says it is. We are also streaming position data from each of our hands and then comparing the Z distance of our hands and hip and if greater than 0.35 we are treating the hand has being raised. Here you can see the right hand is raised and left lowered.

In this basic Hello World you hopefully have learnt how to start a new project, install the Kinect libraries, write some XAML to change how your app looks and then a little bit of C# for the brains of the operation. Now you are tracking the skeleton you can obtain the coordinates for any joint on the body, and by comparing these joints who knows what wonderful ideas you could come up with. You could possibly make a dance along app where if certain distances between joints are hit you have completed a 'dance move'. Any way that's me over and out!

[hellopic]: http://www.instructables.com/id/Kinect-SDK-Hello-World/
