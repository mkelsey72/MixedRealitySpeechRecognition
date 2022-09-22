# MRS
 Mixed Reality Speech Recognition using Azure Speech to Text API. 
 
This application accepts live audio from the user's environment, providing real-time captioning for the user. Subtitles are provided at the lower portion of the user's view space. The subtitles are entirely movable and resiazable via user interaction. A handheld menu is displayed when the user shows their palm to the headset which allows the user to start, stop and exit the application. The application is optimized for lecture environments and intended to benefit individuals with, but not limited to, hearing impairment.

The application uses Microsoft Azure's Speech-To-Text Translation API and was developed in Unity Game Engine (Version - 2021.3.3f1). The app was built with Microsoft's Mixed Reality Tool Kit (MRTK) and is deployable on mixed reality headsets. The application is currently modified for deployment on Augmented Reality headsets, specifically the Hololens 2.

## Future Features
- An option for translation of live audio input into the user’s preferred language. This would expand the range of users that could benefit from this software beyond strictly individuals with hearing impairment.
- Addition of ASL images instead of, or accompanying, the subtitles. Some research shows that individuals with hearing impairment have an easier time understanding ASL compared to written text.
- Light/Dark Mode. There is currently an option for light/dark mode, but it does not work because of opacity issues with MRTK.


## Notes:
- When deploying the application via Visual Studio, I used the following settings:
> - Solution Configurations: Release (Master would work as well)
> - Solution Platforms: ARM64

### Research Poster and Explanation: <!-- this needs to be a better title -->
[SFA S.U.R.E.](https://www.sfasu.edu/academics/colleges/sciences-math/student-resources/undergraduate-research/sure)
