# Network Analyzer Plugin

<img src="https://img.shields.io/badge/packages.opentap.io-Network%20Analyzer>
<object data="https://shields.io/github/v/tag/opentap/Network-Analyzer?label=packages.opentap.io%3ANetwork%20Analyzer&link=https%3A%2F%2Fpackages.opentap.io%2F%23name%3D%252FPackages%252FNetwork%2BAnalyzer" type="image/svg" width="400" height="300">
    <img src="https://shields.io/github/v/tag/opentap/Network-Analyzer?label=packages.opentap.io%3ANetwork%20Analyzer&link=https%3A%2F%2Fpackages.opentap.io%2F%23name%3D%252FPackages%252FNetwork%2BAnalyzer" alt="fallback image">
</object>

## Overview

The Network Analyzer (NA) Plugin is an OpenTAP plugin designed for seamless control of your Keysight network analyzer, automating tests through test automation editor. This plugin streamlines the process of generating different measurements, saving results, utilizing Spectrum Analyzer (SA) mode, and performing calibration, all without the need for writing tedious and error-prone scripts.

## Features
* Create Test plan in PathWave Test Automation Editor

    The NA Plugin simplifies the creation process of complex test scenarios without the need for manual scripting. By using PathWave Test Automation Editor, you can design test steps, plans, set parameters, connect DUTs, set result output, define conditions with ease, accommodating users with varying levels of technical expertise.

* Test Parameterization

    Effortlessly parameterize test configurations within the PathWave Test Automation editor. This feature allows dynamic adjustment of test scenarios on the fly, enhancing flexibility and promoting comprehensive test coverage without manual intervention.

* Data Acquisition and Measurement Output

    The plugin offers robust data acquisition capabilities. You can capture, log, and save data from your network analyzer, enhancing your ability to monitor and optimize performance.

* Spectrum Analyzer (SA) Mode
  
    Utilize Spectrum Analyzer (SA) mode for advanced frequency-domain analysis. The NA Plugin seamlessly integrates with your network analyzer to provide comprehensive SA capabilities allowing users to perform detailed frequency measurements effortlessly.

* Calibration

    Perform calibration tasks through PathWave Test Automation Editor without the need for intricate scripting. The plugin supports calibration routines, ensuring the accuracy and reliability of your network analyzer measurements.

* Channel/Trace Setup

    Effortlessly configure channels and define traces/markers within PathWave Test Automation editor. Tailor your test scenarios by specifying the desired channels and traces, allowing for precise and targeted measurements.

## Getting Started

### Installation:

Download the NA Plugin installer from [here](https://packages.opentap.io/#name=%2FPackages%2FNetwork+Analyzer)
Run the installer and follow the on-screen instructions to complete the installation process.

### Build from source (Windows with Visual Studio installed):
```
git clone https://github.com/opentap/Network-Analyzer.git
cd Network-Analyzer
start OpenTap.Plugins.PNAX.sln
```

## List of Supported Instruments
All active Keysight Network Analyzers with recent firmware. You can check the compatibility in below table. The main testing environment for this plugin is the N5245B.

| VNA Family | Supported Instruments |
| :---:      | :----                 |
| PNA        | N5221B, N5222B, N5224B, N5225B, N5227B |
| PNA        | N5231B, N5232B, N5234B, N5235B, N5239B |
| PNA        | N5241B, N5242B, N5244B, N5245B, N5247B, N5249B |
| ENA        | E5080A                |
| ENA        | E5080B                |
| ENA        | E5081A                |
| PXI        | M9370A, M9371A, M9372A, M9373A, M9374A, M9375A |
| PXI        | M9800A, M9801A, M9802A, M9803A, M9804A, M9805A, M9806A, M9807A, M9808A |
| PXI        | M9370B, M9371B, M9372B, M9373B, M9374B, M9375B |
| Streamline | P5000A, P5001A, P5002A, P5003A, P5004A, P5005A, P5006A, P5007A, P5008A, P5020A, P5021A, P5022A, P5023A, P5024A, P5025A, P5026A, P5027A, P5028A, P5000B, P5001B, P5002B, P5003B, P5004B, P5005B, P5006B, P5007B, P5008B, P5020B, P5021B, P5022B, P5023B, P5024B, P5025B, P5026B, P5027B, P5028B |
| Streamline | P9370A, P9371A, P9372A, P9373A, P9374A, P9375A, P9377B, P9370B, P9371B, P9372B, P9373B, P9374B, P9375B |
| Streamline | P9382B, P9384B |

## Support
For any questions, issues, or assistance, please submit an Issue in this repo.

## License
The Network Analyzer Plugin is released under the MIT License. Feel free to use, modify, and distribute it in accordance with the terms of the license.
