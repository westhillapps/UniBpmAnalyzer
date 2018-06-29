UniBpmAnalyzer
======

[![GitHub License](https://img.shields.io/badge/license-MIT-blue.svg)](http://opensource.org/licenses/mit-license.php)
[![GitHub Stars](https://img.shields.io/github/stars/WestHillApps/UniBpmAnalyzer.svg)](https://github.com/WestHillApps/UniBpmAnalyzer/stargazers)
[![GitHub Forks](https://img.shields.io/github/forks/WestHillApps/UniBpmAnalyzer.svg)](https://github.com/WestHillApps/UniBpmAnalyzer/network)

BPM analyzer for Unity.  

Get a bpm by audio clip in runtime.  

It is made with Unity5.3.4f1 (Mac, Win, Android, iOS), work probably also in other versions.

How To Use
-------
```csharp
public AudioClip targetClip;

private void Start()
{
    int bpm = BpmAnalyzer.AnalyzeBpm(targetClip);
    Debug.Log("BPM is " + bpm);
}
```

For more information, Please check the example scene and example components.  
* example scene - (Assets/BpmAnalyzer/Example/BpmAnalyzerExample.unity)  
* example component - (Assets/BpmAnalyzer/Example/BpmAnalyzerExample.cs)

Developed By
-------
WestHillApps (Hironari Nishioka) - <nishioka-h@westhillapps.com>

<a href="https://twitter.com/westhillapps">
<img alt="Follow me on Twitter"
src="https://raw.githubusercontent.com/WestHillApps/westhillapps.github.io/master/res/twitter.png" width="75"/>
</a>

License
-------
MIT License

Copyright (c) 2016 WestHillApps (Hironari Nishioka)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.