# MRS
 Mixed Reality Speech Recognition and Translation using Azure Speech to Text API. 
 
This application accepts live audio from the user's environment, providing real-time captioning for the user. Captions are provided at the lower portion of the user's view space. The captions are entirely movable and resiazable via user interaction. A handheld menu is displayed when the user shows their palm to the headset which allows the user to start, stop and exit the application. The application is optimized for lecture environments and intended to benefit individuals with, but not limited to, hearing impairment.

![Hand Menu](https://user-images.githubusercontent.com/105393865/236265727-acc92147-0fad-44f4-beaa-13be2dfc1204.png)

![Lecture Demo](https://user-images.githubusercontent.com/105393865/236265964-67938660-ba15-437e-acc0-a05fbe9d3e21.png)

## Translation Feature

The application now allows for translation as well. A user can select the language they wish to translate via an interactable drop down and when the recognition is run, it will translate to english. A toggle box will need to be checked by the user to run the transalator instead of the speech recognizer and unchecked to run the speech recognizer again.

[Translation Demo](https://mslivesfasu-my.sharepoint.com/:v:/g/personal/kelseymd_jacks_sfasu_edu/EQLvFsklEvFBgacfupREclABarPltUOp5cidyKHGyKOF5Q?e=tQFDlb)

The application uses Microsoft Azure's Speech-To-Text Translation API and was developed in Unity Game Engine (Version - 2021.3.3f1). The app was built with Microsoft's Mixed Reality Tool Kit (MRTK) and is deployable on mixed reality headsets. The application is currently modified for deployment on Augmented Reality headsets, specifically the Hololens 2.

## Future Features
- Addition of ASL images instead of, or accompanying, the captions. Some research shows that individuals with hearing impairment have an easier time understanding ASL compared to written text.
- Light/Dark Mode. There is currently an option for light/dark mode, but it does not work because of opacity issues with MRTK.
- Record captions or translation subtitles to a document for later reference. 
   - Time stamp feature for the document, incase user can't keep up with captions and would like to reference later.

## Notes:
- When deploying the application via Visual Studio 2022, I used the following settings:
> - Solution Configurations: Release (Master would work as well)
> - Solution Platforms: ARM64
- In Unity, you will be prompted to update the version of the project. Do not do this unless you are in a new branch trying to fix the issue, it will not update correctly otherwise. Skip the update and just run the old version.

### Research Poster and Explanation (Speech Recognition Only): <!-- this needs to be a better title -->
[SFA S.U.R.E.](https://www.sfasu.edu/academics/colleges/sciences-math/student-resources/undergraduate-research/sure)

### Undergrad Research Presentation (Speech Recognition and Translation):
[URC Presentation](https://scholarworks.sfasu.edu/urc/2023/Videos/6/)
