# MultiSpeak Client for 3.0AC

 Client used to make soap requests to OA_Server, MDM_Server, and CB_Server, Scada_Server

## Quick Start

 Fire up this app
 Build and compile this app.

 > cd C:\CSharp\source\MultiSpeakClientV30ac\MultiSpeakClientV30ac\bin\Debug

### Sample Calls



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