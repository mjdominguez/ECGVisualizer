# ECG Visualizer v0.96

## Description

This project presents a tool for visualization, filtering, signal peaks detection, features extraction and reports generation using the XML files provided by the General Electric CardioSoft 12SL.

This tool includes a full customization process: it allows either manual or automatic filtering, patient target selection (to modify ECG segments intervals), features selection and report generation in an open CSV file.

Future versions will include more ECG devices.

It is very important to point out that this tool has been developed expressly for the automation of the process of generating reports on ECGs taken on high-performance athletes; however, the manual configuration of all the parameters allows using this tool with all types of patients. This aspect is important since, in this type of people, the intervals on which the detection of peaks is based are considerably different from those of ordinary people. These aspects and others about the operation of the tool are indicated in the "help" menu of the tool.

In order to test the tool functionaloty, a couple of anonymized XMLs are provided.

## Table of Content
1. [Getting started](#getting-started)
2. [Usage](#usage)
3. [Credits](#credits)
4. [License](#license)
5. [Cite this work](#cite-this-work)


## Getting started


## Usage

Double-click on the ECGVisualizer.exe file to run it. The main sections of the tool can be observed next:



Here you can check the flow diagram when using the automatic filtering, the pre-congigured patient target and the most common features:

![ProcessingChain](https://github.com/mjdominguez/ECGVisualizer/assets/26136706/49bc6bd7-2ddf-41d2-ba6b-0f4bfec43cae)

## Credits

The authors of the ECGVisualizer's original idea are: Manuel Domínguez-Morales, Adolfo Muñoz-Macho and José Luis Sevillano.

The authors would like to thank and give credit to RCD Mallorca SAD, its football players and medical staff for allowing us to work with anonymized data from electrocardiograms taken from its football players.

## License

This project is licensed under the GPL License - see the LICENSE.md file for details.

Copyright © 2023 Manuel Domínguez-Morales
mjdominguez@us.es 

![License: GPL v3](https://img.shields.io/badge/License-GPL%20v3-blue)

## Cite this work

Domínguez-Morales, M., Muñoz-Macho, A., & Sevillano, J. L. (2023). _ECGVisualizer: A 12-lead ECG pre-processing, feature extraction and report generation tool_ [Manuscript submitted for publication]. Architecture and Technology of Computers dept., University of Seville (Spain).
