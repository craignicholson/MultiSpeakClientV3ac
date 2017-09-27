# MultiSpeak Client for 3.0AC

 Client used to make soap requests to OA_Server, MDM_Server, and CB_Server, Scada_Server

## Quick Start

 Fire up this app
 Build and compile this app.

 > cd C:\CSharp\source\MultiSpeakClientV30ac\MultiSpeakClientV30ac\bin\Debug

### Sample Calls

> MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetActiveOutages

Results 112

> MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetAllActiveOutageEvents

Results 112 

> MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetAllCircuitElements
 BUSTED XML ERROR - DURING serialization

 MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2453
 Success

 .\MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 
 Left off parameter, app worked, displayed flags

cd C:\CSharp\source\MultiSpeakClientV30ac\MultiSpeakClientV30ac\bin\Debug

MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2655
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2426
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2425
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2012
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2732
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2678
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2696
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2675
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2680
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2456
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1319
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2508
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2528
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2109
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2430
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2087
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2266
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-0745
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2716
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1464
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-0763
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2409
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2743
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2453
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2195
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2558
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2616
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2625
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-0917
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2416
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2428
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-1872
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1214
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1534
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2685
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2515
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2683
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2738
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2706
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1173
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2464
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2139
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2730
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2581
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2560
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2601
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2447
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2739
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-0803
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2511
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2727
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2557
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2420
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1543
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2371
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-1955
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2729
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2301
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-0826
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2697
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2674
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-1927
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-1952
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2379
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2045
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2692
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2690
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1597
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2475
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2468
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-1883
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2623
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2726
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2711
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2537
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2733
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2410
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2552
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2427
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2736
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2693
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2386
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2077
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2191
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2734
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2737
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-0666
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-0873
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1442
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2563
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2740
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2002
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2302
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1189
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2618
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2022
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2121
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2682
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2689
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2704
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2569
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2437
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2385
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-0767
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1003
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2304
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-26-1559
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-27-2286
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2670
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2661
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-29-2707
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetOutageDurationEvents -o 2017-08-28-2677
- Success on 112 GetOutageDurationEvents

MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  GetOutageEventStatus -o 2017-08-28-2682
 Success

MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  GetOutageStatusByLocation -l 65788
 Success
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  GetCustomerOutageHistory -a 48638001 -l 65788
 Success

MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  GetCustomersAffectedByOutage -o 2017-04-05-0001
Success

MultiSpeakClientV30ac.exe -e http://63.164.96.175/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u milsoft -p milsoft -c Milsoft -s MDM_Server -m  InitiateOutageDetectionEventRequest -d 0111500 -r http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/OA_Server.asmx

MultiSpeakClientV30ac.exe -e http://63.164.96.175/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u milsoft -p milsoft -c Milsoft -s MDM_Server -m  InitiateOutageDetectionEventRequest -d 15524652 -r http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/OA_Server.asmx

MultiSpeakClientV30ac.exe -e http://63.164.96.175/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u milsoft -p milsoft -c Milsoft -s MDM_Server -m  InitiateOutageDetectionEventRequest -d 0132786 -r http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/OA_Server.asmx

MultiSpeakClientV30ac.exe -e http://63.164.96.175/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u FakeCISMSpeak3 -p FakeCISMSpeak3 -c FakeCISMSpeak3 -s MR_Server -m  ServiceLocationChangeNotification -d 100

MultiSpeakClientV30ac.exe -e http://63.164.96.175/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u milsoft -p milsoft -c Milsoft -s MDM_Server -m  InitiateOutageDetectionEventRequest -d 0132786 -r http://10.86.1.81/SimulatorFakeOMS/MultiSpeak/30ac/OA_Server.asmx

MultiSpeakClientV30ac.exe -e http://63.164.96.175/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u milsoft -p milsoft -c Milsoft -s MDM_Server -m  InitiateOutageDetectionEventRequest -d 15524652 -r http://10.86.1.81/SimulatorFakeOMS/MultiSpeak/30ac/OA_Server.asmx



Sent 610 InitiateOutageDetectionEventRequests using the GVEC meters

See list.


Works for me...
Sent for MeterId {15524652, 0111500}
ODEventNotifications
Set 633 x2 = 1266 Messages
Success

GetCustomerOutageHistory
Sent 637 x2 = 1274 Messages
Success

Mean Response Time; 2.5 seconds

END OF MILSOFT
	

 MultiSpeakClientV30ac.exe -e http://localhost:55273/MDM_Server.asmx -u ElectSolve -p Password123 -c ElectSolve -s MDM_Server -m  PingUrl
 cd C:\CSharp\source\MultiSpeakClientV30ac\MultiSpeakClientV30ac\bin\Debug
 MultiSpeakClientV30ac.exe -e http://demo.turtletech.com/latest/webAPI/MR_CB.asmx -u UNITILTST -p Unitil1! -c ElectSolve -s MDM_Server -m  PingUrl
 

 MultiSpeakClientV30ac.exe -e http://localhost:55273/MDM_Server.asmx -u ElectSolve -p Password123 -c ElectSolve -s OA_Server -m  ODEventNotification -d meterNo
 
 MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  ODEventNotification -d 5373213 -v Restoration
 MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  ODEventNotification -d 32159562 -v Restoration
 MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  ODEventNotification -d 4158277 -v Restoration
 MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  ODEventNotification -d 16273402 -v Restoration
 MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m  ODEventNotification -d 5373213 -v Restoration
 

 ./MultiSpeakClientV30ac.exe -e https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u bo -p bo -c BackOffice -s MDM_Server -m  InitiateCDStateRequest -d 5373213 -r https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/30ac/1/CB_Server.asmx



  ./MultiSpeakClientV30ac.exe -e https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u bo -p bo -c BackOffice -s MDM_Server -m  InitiateCDStateRequest -d 5373213 -r http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/MDM_Server.asmx
 
  ./MultiSpeakClientV30ac.exe -e https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u bo -p bo -c BackOffice -s MDM_Server -m  InitiateCDStateRequest -d 5373213 -r http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/MDM_Server.asmx
 
 ./MultiSpeakClientV30ac.exe -e http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/MDM_Server.asmx -u bo -p bo -c BackOffice -s MDM_Server -m  InitiateCDStateRequest -d 5373213 -r http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/MDM_Server.asmx
 
 ./MultiSpeakClientV30ac.exe -e https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u bo -p bo -c BackOffice -s MDM_Server -m  InitiateCDStateRequest -d 5373213 -r http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/MDM_Server.asmx
 
 
 ERROR Reported > "ResponseURL validation failed for http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/MDM_Server.asmx. "
 Actual Error > The request failed with HTTP status 401: Unauthorized.
 Resolution > IIS Enable Anonomous Authentication

MultiSpeakClientV30ac.exe -e http://10.86.1.81/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u milsoft -p milsoft -c Milsoft -s MDM_Server -m  InitiateOutageDetectionEventRequest -d 5373213 -r http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/OA_Server.asmx
 

 2017-09-19 10:31:29.3770 | Error | ETSS.Broker.Channel.Channel.PostMessage | 
 . [Channel:ChannelMSpeak4InitiateCDStateRequest| Receiver:SSN] cannot process this message. Error Detail - Sequence contains no elements 

 ERROR : 
 "ResponseURL validation failed for https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/30ac/1/CB_Server.asmx. "
 Cause ... Using 416 and passed in wrong responseUrl.
 Correct url is "https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/416/1/NOT_Server.asmx"


 

 MultiSpeakClientV30ac.exe -e http://localhost:55273/MDM_Server.asmx -u ElectSolve -p Password123 -c ElectSolve -s MDM_Server -m  InitiateOutageDetectionEventRequest -d meterNo

 MultiSpeakClientV30ac.exe -e http://10.86.1.81/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u milsoft -p milsoft -c Milsoft -s MDM_Server -m  InitiateOutageDetectionEventRequest -d 0111500  -r http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/OA_Server.asmx
 
 MultiSpeakClientV30ac.exe -e http://10.86.1.81/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u milsoft -p milsoft -c Milsoft -s MDM_Server -m  InitiateOutageDetectionEventRequest -d 5373213 -r http://10.86.1.81/SimulatorCraig/MultiSpeak/30ac/OA_Server.asmx
IntitiateODEventRequest


 GETALLCONNECTIVITY - DONT DO THIS ANY MORE
MultiSpeakClientV30ac.exe -e http://12.218.155.140:85/soap/OA_Server -u ElectSolve -p ElectSolve2017-Testing -c ElectSolve -s OA_Server -m GetAllConnectivity



 ## Dependencies 

 ## OA_Server Supported Methods
 - GetActiveOutages
 - GetAllActiveOutageEvents
 - GetAllConnectivity - chunking example
 - GetAllCircuitElements  
 - GetOutageDurationEvents- Requires options options.OutageEventId -o YourEventID
 - GetOutageEventStatus
 - GetOutageStatusByLocation
 - GetCustomersAffectedByOutage
 - GetCustomerOutageHistory
 - ODEventNotification - Requires options (-d meterNo)

 ### Use Cases - Send and ODEventNotification.
 Outage Management Systems and Meter Data Management systems are the typical receivers of outage and restore events.
 This client can play the role of an AMI system, by emitting an outage or restore.

 
 ## MDM_Server Supported Methods
 - PingURL
 - InitiateOutageDetectionEventRequest - Requires Device and ResponseUrl options.
     
## EA_Server
TOD0

## MR_Server

## L+G Testing

cd C:\CSharp\source\MultiSpeakClientV30ac\MultiSpeakClientV30ac\bin\Debug
 
MultiSpeakClientV30ac.exe -e http://demo.turtletech.com/latest/webAPI/MR_CB.asmx -u UNITILTST -p Unitil1! -c ElectSolve -s MDM_Server -m  PingUrl
 
MultiSpeakClientV30ac.exe -e http://demo.turtletech.com/latest/webAPI/MR_CB.asmx -u UNITILTST -p Unitil1! -c ElectSolve -s MR_Server -m  GetAMRSupportedMeters
 
MultiSpeakClientV30ac.exe -e http://demo.turtletech.com/latest/webAPI/MR_CB.asmx -u UNITILTST -p Unitil1! -c ElectSolve -s MR_Server -m  GetHistoryLogByMeterNo -d 100
 
MultiSpeakClientV30ac.exe -e http://demo.turtletech.com/latest/webAPI/MR_CB.asmx -u UNITILTST -p Unitil1! -c ElectSolve -s MR_Server -m   GetLatestReadingByMeterNo -d 100

MultiSpeakClientV30ac.exe -e http://demo.turtletech.com/latest/webAPI/MR_CB.asmx -u UNITILTST -p Unitil1! -c ElectSolve -s MR_Server -m   InitiateMeterReadByMeterNumber -d 100 -r http://10.86.1.81/MultiSpeakBroker/MultiSpeak/30ac/1/CB_Server.asmx

 MultiSpeakClientV30ac.exe -e http://demo.turtletech.com/latest/webAPI/MR_CB.asmx -u UNITILTST -p Unitil1! -c ElectSolve -s MR_Server -m   InitiateMeterReadByMeterNumber -d 100 
 MultiSpeakClientV30ac.exe -e http://10.86.1.81/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u simulator -p simulator -c ElectSolve -s MR_Server -m   InitiateMeterReadByMeterNumber -d 100 -r http://10.86.1.81/MultiSpeakBroker/MultiSpeak/30ac/1/CB_Server.asmx






 
MultiSpeakClientV30ac.exe -e http://63.164.96.175/MultiSpeakBroker/MultiSpeak/30ac/1/MDM_Server.asmx -u FakeCISMSpeak3 -p FakeCISMSpeak3 -c FakeCISMSpeak3 -s MR_Server -m  ServiceLocationChangeNotification -d 100

FakeCISMSpeak3
FakeCISMSpeak3, this is the client which will make requests to the AMI through the broker

Test 1:
MDMWebAPI is setup. 

errorObject[] >
        objectID :
        errorString : Fail to get subscriber for Location Number 100. Reason: #100 might not be in the FakeCISMSpeak3 system 
		or might not have Readsource assigned.
        nounType :
        eventTime : 9/27/2017 2:28:52 PM
        eventTimeSpecified : True

Test 2:
Remove WebAPI Credentials

errorObject[] >
        objectID :
        errorString : Fail to get subscriber for Location Number 100. Reason: Client - FakeCISMSpeak3 does not have API.
        nounType :
        eventTime : 9/27/2017 2:38:29 PM
        eventTimeSpecified : True
TASK FINISHED
Execution Time : 00:00:01.8139689

Test 3:
Add MSpeak3 ServiceLocationChangedNotification, no routing.
errorObject[] >
        objectID :
        errorString : Fail to get subscriber for Location Number 100. Reason: Client - FakeCISMSpeak3 does not have API.
        nounType :
        eventTime : 9/27/2017 2:41:27 PM
        eventTimeSpecified : True

Test 4:
Add MDM WebAPI back
errorObject[] >
        objectID :
        errorString : Fail to get subscriber for Location Number 100. Reason: #100 might not be in the FakeCISMSpeak3 system or might not have Readsource assigned.
        nounType :
        eventTime : 9/27/2017 2:42:23 PM
        eventTimeSpecified : True

SELECT
ReadSourceId, *
FROM
MDM.dbo.Meter
WHERE MeterIdentifier='100'

SELECT * FROM MDM.dbo.ReadSource
WHERE ReadSourceID = 17

INSERT INTO MDM.dbo.Location VALUES
(1,100,NULL, NULL, NULL, NULL, NULL, 1, GetDate(), NULL, NULL)

SELECT * FROM MDM.dbo.Location
WHERE LocationNumber = '100'

UPDATE MDM.dbo.Meter
SET LocationId = 42944
WHERE MeterIdentifier = '100'

errorObject[] >
        objectID :
        errorString : Fail to get subscriber for Location Number 100. Reason: Cannot find subcriber for method - ServiceLocationChangedNotification. 
		Either Landis & Gyr is not setup or does not have receiver for this method
        nounType :
        eventTime : 9/27/2017 2:49:24 PM
        eventTimeSpecified : True


UPDATE MDM.dbo.ReadSource
SET ReadSourceDescription = 'Landis'
WHERE ReadSourceDescription = 'Landis & Gyr'

Update VendorName in Vendor  Edit for Landis & Gyr to Landis

Re-Add the MSpeak3 ServiceLocationChangedNotification

SUCCESS

MultiSpeakMsgHeader Server >
        Version : 3.0AC
        UserID : FakeCISMSpeak3
        Pwd : FakeCISMSpeak3
        AppName : MultiSpeakClient30ac
        AppVersion : 1.0beta
        Company : FakeCISMSpeak3
        CSUnits : feet
        CoordinateSystem :
        Datum :
        SessionID :
        PreviousSessionID :
        ObjectsRemaining :
        LastSent :
        RegistrationID :
        AuditID :
        MessageID :
        TimeStamp : 1/1/0001 12:00:00 AM
        TimeStampSpecified : False
        BuildString :
        AnyAttr :
        EncodedMustUnderstand : 0
        EncodedMustUnderstand12 : 0
        MustUnderstand : False
        Actor :
        Role :
        DidUnderstand : True
        EncodedRelay : 0
        Relay : False
TASK FINISHED
Execution Time : 00:00:14.7539258



## https
Using and overide to ignore self signed certs. .Net will reject sites with self signed certs
and this is the only way I have resolved this issue to date.

## How to run a test with the sample endpoints

## Security

How to code or implmeent security in the MultiSpeak endpoints

## MultiSpeakBusArch30AC
 
 Setup and Run..
 Monitoring... with Fiddler
 Monitoring... with Wireshark
 
 Using Burp Suite...