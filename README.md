# Network Analyzer Plugin

## Overview

The Network Analyzer (NA) Plugin is an OpenTAP plugin designed for seamless control of your Keysight network analyzer, automating tests through test automation editor. This plugin streamlines the process of generating different measurements, saving results, utilizing Spectrum Analyzer (SA) mode, and performing calibration, all without the need for writing tedious and error-prone scripts.

## Features
* Create Test plan in PathWave Test Automation Editor

    The NA Plugin simplifies the creation process of complex test scenarios without the need for manual scripting. By using PathWave Test Automation Editor, you can design test steps, plans, set parameters, connect DUTs, set result output, define conditions with ease, accommodating users with varying levels of technical expertise.

* Test Parameterization

    Effortlessly parameterize test configurations within the PathWave Test Automation editor. This feature allows dynamic adjustment of test scenarios on the fly, enhancing flexibility and promoting comprehensive test coverage without manual intervention.

* Data Acquisition and Measurement Output

    The plugin offers robost data acquisition capabilities. You can capture, log, and save data from your network analyzer, enhancing your ability to monitor and optimize performance.

* Spectrum Analyzer (SA) Mode
  
    Utilize Spectrum Analyzer (SA) mode for advanced frequency-domain analysis. The NA Plugin seamlessly integrates with your network analyzer to provide comprehensive SA capabilities allowing users to perform detailed frequency measurements effortlessly.

* Calibration

    Perform calibration tasks through PathWave Test Automation Editor without the need for intricate scripting. The plugin supports calibration routines, ensuring the accuracy and reliability of your network analyzer measurements.

* Channel/Trace Setup

    Effortlessly configure channels and define traces/markers within PathWave Test Automation editor. Tailor your test scenarios by specifying the desired channels and traces, allowing for precise and targeted measurements.

## Getting Started

### Installation:

Download the NA Plugin installer from [here](https://packages.opentap.io/)
Run the installer and follow the on-screen instructions to complete the installation process.

### Build from source (Windows with Visual Studio installed):
```
git clone https://github.com/opentap/Network-Analyzer.git
cd Network-Analyzer
start OpenTap.Plugins.PNAX.sln
```

## List of Supported Instruments
All the Keysight network analyzer with "NASCAR" firmware. The main testing environment for this plugin is N5247B.

## Support
For any questions, issues, or assistance, please submit an Issue in this repo.

## License
The Network Analyzer Plugin is released under the MIT License. Feel free to use, modify, and distribute it in accordance with the terms of the license.
