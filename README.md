# ECG Visualizer v0.96

## Description

This project presents a tool for visualization, filtering, signal peaks detection, features extraction and reports generation using the XML files provided by the General Electric CardioSoft 12SL.

This tool includes a full customization process: it allows either manual or automatic filtering, patient target selection (to modify ECG segments intervals), features selection and report generation in an open CSV file.

Future versions will include more ECG devices.

It is very important to point out that this tool has been developed expressly for the automation of the process of generating reports on ECGs taken on high-performance athletes; however, the manual configuration of all the parameters allows using this tool with all types of patients. This aspect is important since, in this type of people, the intervals on which the detection of peaks is based are considerably different from those of ordinary people. These aspects and others about the operation of the tool are indicated in the "help" menu of the tool.

In order to test the tool functionaloty, a couple of anonymized XMLs are provided.

## Table of Content

1. [General Description](#general-description)
2. [Automatic Processing](#automatic-processing)
3. [Manual Processing](#manual-processing)
4. [Modify ECG](#modify-ECG)
5. [Credits](#credits)
6. [License](#license)
7. [Cite this work](#cite-this-work)


## General description

Double-click on the ECGVisualizer.exe file to run it. The main sections of the tool can be observed next:

![MainFrame](https://github.com/mjdominguez/ECGVisualizer/blob/37612b7b5ce1bab836067e480c39e818fcd8bb93/ECGVisualizer/images/example1-described.png)

1) Section 1 (red): Toolsbar. This sections contains several options that allows the user loading ECG files, filtering ECGT signal, selecting the features extracted and configuring the report generated. All the options are described in the [User Manual](https://github.com/mjdominguez/ECGVisualizer/blob/3fb37baf7c4afc1af5753ee94a93dc224443a4e9/ECGVisualizer/documentation/User%20Manual.pdf).
2) Section 2 (blue): Basic patient information. Here, the baseline information obtained from the ECG file is shown. This information may be selected to be included in the final report.
3) Section 3 (orange): Visualization tools. These tools allows the user to increase or decrease zoom. In the case the user want to return to the default zoom, he/she can clic on "Default".
4) Section 4 (green): Signal information. This is an informative section, where the main charactreristics of the loaded ECG signal are shown: Hertzs, number of seconds and resolution of the signal. Hertzs may vary when applying fixed-window average filters.
5) Section 5 (purple): ECG signal visualization. The 12-lead ECG signal is represented here. User may select which lead to visualize and move though it using the scrolls. The results of the filtering and peaks detection process are represented here too.


## Automatic processing

The automatic processing includes a custom filtering chain based on a pre-selected combination of filters. It is described in "Help" menu.

The complete flow diagram when using the automatic filtering, the pre-congigured patient target and the most common features are detailed next:

![ProcessingChain](https://github.com/mjdominguez/ECGVisualizer/assets/26136706/49bc6bd7-2ddf-41d2-ba6b-0f4bfec43cae)

## Manual processing

The processing chain performed for the manual processing follows the same steps.

The differences are that the user can select what filters to apply (and their order), the target patient and the features selected to extract and include in the final report.

Here you can check some captures that may help to understand these steps:

1) Manual Filtering: when selecting "Manual Filtering" in the "Tools" menu, the program visualization changes.

![ManualFiltering](https://github.com/mjdominguez/ECGVisualizer/blob/035bdedad2666ad5619bd3cf20a3e359ad29523b/ECGVisualizer/images/manualFiltering.png)

The four types of filters the user can apply are: fixed-average filter, moving-average filter, moving-median filter and band-stop filter. Moreover, the tool includes the posibility of substracting signals, providing their sources (A to D, as output of the previously applied filters). The explanation of these filters are included in the [User Manual](https://github.com/mjdominguez/ECGVisualizer/blob/3fb37baf7c4afc1af5753ee94a93dc224443a4e9/ECGVisualizer/documentation/User%20Manual.pdf).

2) Selecting Patient Target: this tool allows to select the type of patient whose ECG signal is analyzed. This information is essential as, if the patient has a particular physical condition, the times between the segments of the different peaks of the ECG signal may change substantially. This is especially true for professional athletes.

Therefore, this programme allows a choice between an athlete patient and an ordinary patient. However, it also allows the inclusion of a personalised patient type (providing the times of the most recognisable segments of an ECG signal).

In the following screenshot you can see the menu to select the patient type:

![PatientTarget1](https://github.com/mjdominguez/ECGVisualizer/blob/035bdedad2666ad5619bd3cf20a3e359ad29523b/ECGVisualizer/images/TargetPatient1.png)

And the following screenshot shows the possibility of including a personalised patient:

![PatientTarget2](https://github.com/mjdominguez/ECGVisualizer/blob/035bdedad2666ad5619bd3cf20a3e359ad29523b/ECGVisualizer/images/TargetPatient2.png)

3) Selecting Features: Finally, this tool allows you to select which features you want to extract from the ECG signal and therefore include in the final report.

The following screenshot shows the configuration window for this task:

![FeaturesSelection](https://github.com/mjdominguez/ECGVisualizer/blob/035bdedad2666ad5619bd3cf20a3e359ad29523b/ECGVisualizer/images/FeaturesSelected.png)


All the tools and options that this programme contains are deeply detailed in the [User Manual](https://github.com/mjdominguez/ECGVisualizer/blob/3fb37baf7c4afc1af5753ee94a93dc224443a4e9/ECGVisualizer/documentation/User%20Manual.pdf).

## Modify ECG

ECGVisualizer also includes some tools to modify the ECG signal (after v0.98).

These tools can be accesed in "Tools" --> "Modify ECG". They are:

1) Amplitude Modification: this tool allows the user to apply a multiplier factor to the amplitudes of the whole ECG signal.

2) Timing Modification: this tool allows the user to apply a multiplier factor to the time between samples of the whole ECG signal.

3) Peaks Modification: this tool allows the user to apply a multiplier factor (from negative to positive) to specific peaks (P, Q, R, S or T) and a time window whithin these peaks are. This tool also allow to select which leads to be applied. It is important to mention that this tool will be only available (visible) after the peaks detection process.

![ModificationMenu](https://github.com/mjdominguez/ECGVisualizer/blob/7a7679c540a2e0cd22193f9cd7c99790a574ed6c/ECGVisualizer/images/modify.png)

## Credits

The authors of the ECGVisualizer's original idea are: Manuel Domínguez-Morales, Adolfo Muñoz-Macho and José Luis Sevillano.

The authors would like to thank and give credit to RCD Mallorca SAD, its football players and medical staff for allowing us to work with anonymized data from electrocardiograms taken from its football players.

## License

This project is licensed under the GPL License - see the LICENSE.md file for details.

Copyright © 2024 Manuel Domínguez-Morales
mjdominguez@us.es 

![License: GPL v3](https://img.shields.io/badge/License-GPL%20v3-blue)

## Cite this work

Domínguez-Morales, M., Muñoz-Macho, A., & Sevillano, J. L. (2023). _ECG visualizer, pre-processing, feature extraction and data augmentation tool for intelligent simulation systems_ [Manuscript submitted for publication to SIMULATION (ISSN: 0037-5497)]. Architecture and Technology of Computers dept., University of Seville (Spain).
