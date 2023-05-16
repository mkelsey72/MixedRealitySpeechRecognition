# MRS
 Mixed Reality Speech Recognition and Translation using Azure Speech to Text API. 
 
 I began this project for the Summer Undergraduate Research Experience (S.U.R.E.) event at SFA in the summer of 2022 and closed the project with an initial prototype. The intention behind the initial prototype was to aid individuals that have some degree of hearing impairment in the classroom and is optimized for a lecture environment. In the Spring of 2023, I reopened the project and broadened the intent of this project to benefit a much wider audience in addition to individuals with hearing impairment. This project was chosen as the Top Scholar project of the College of Sciences And Mathematics and I got to present it amongst the other colleges' Top Scholars. A video of the presentation is attached under 'Top Scholar Presentation' below.
 
 ## The Initial Prototype (Speech Recognition Only): Enhancing the Classroom Experience for Hearing Impaired Students Utilizing Mixed Reality Real-Time Caption System
This application accepts live audio from the user's environment using speech recognition, providing real-time captioning for the user. Captions are provided at the lower portion of the user's view space and update as audio is detected. The captions are entirely movable and resiazable via user interaction. A handheld menu is displayed when the user shows their palm to the headset which allows the user to start, stop and exit the application. The application is optimized for lecture environments and intended to benefit individuals with, but not limited to, hearing impairment in the classroom.

#### Project Poster:
[Project Poster](https://www.sfasu.edu/docs/college-sciences-mathematics/sure/sure-22-madison-kelsey-poster.pdf)

#### Video Description:
[Project Video Description](https://www.youtube.com/watch?v=iV1j6CtQxII)

## The Current Prototype (Speech Recognition and Translation): Addition Of The Translation Feature

The application now allows for translation in the form of subtitles in addition to captions provided by the original speech recognition feature. A user can select the language they wish to translate via an interactable drop down. The user will then need to toggle a checkbox to run the translator instead of the speech recognizer. When the user selects 'Start', translation will run and translate the user selected language to english. Uncheck the toggle box to run the speech recognizer again.

#### Demo Of Translation
[App and Translation Demo](https://mslivesfasu-my.sharepoint.com/:v:/g/personal/kelseymd_jacks_sfasu_edu/EQLvFsklEvFBgacfupREclABarPltUOp5cidyKHGyKOF5Q?e=tQFDlb)

#### Top Scholar Presentation
[Top Scholar Presentation](https://scholarworks.sfasu.edu/urc/2023/Videos/6/)


## Tools
The application uses Microsoft Azure's Speech-To-Text Translation API and was developed in Unity Game Engine (Version - 2021.3.3f1). The app was built with Microsoft's Mixed Reality Tool Kit (MRTK) and is deployable on mixed reality headsets. The application is currently modified for deployment on Augmented Reality headsets and tested on the Hololens 2.

## Future Features
- Addition of ASL images instead of, or accompanying, the captions. Some research shows that individuals with hearing impairment have an easier time understanding ASL compared to written text.
- Record captions or translation subtitles to a document for later reference. 
   - Time stamp feature for the document, incase user can't keep up with captions and would like to reference later.

## Notes:
- When deploying the application via Visual Studio 2022, I used the following settings:
> - Solution Configurations: Release (Master would work as well)
> - Solution Platforms: ARM64
- In Unity, you will be prompted to update the version of the project. Do not do this, it will not update correctly. Skip the update and just run the old version.
