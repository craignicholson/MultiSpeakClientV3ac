# MultiSpeak Broker Setup for Clients and Servers

This document will provide the user with a walk through in setting up the
MultiSpeak Broker.  The goal is to provide a production like example anyone
can follow and configure the broker.

## Goals

Review the terminology of the application, how the application works and
how to setup Senders and Reviecers.

- Sender is the the CIS, AMI, or OMS or any other vendor who wants to send CIM or multispeak messages to the Broker.
- Broker is the main application which hosts the MultiSpeak, CIM endpoints
- Receivers can be AMI (Aclara, Sensus, Trilliant, etc..)

### Applications

- MultiSpeakBroker Web Application
- MultiSpeakBroker Windows Service

#### Types of Requests

- Synchronous

MutliSpeak supports synchronous requests where the Sender sends a request to the Broker
and waits for the response, holding the connection.

- Asynchronous

MultiSpeak also supports asynchronous requests where the Sender sends a request to the Broker
and then moves on to do more work.  Each asynchronous task will have a transactionId and each
asynchronous response is called a Notification.

#### Version 3

| Sender Request Method  |  Receiver Response Method | Supported Receivers  |
|------------------------|---------------------------|----------------------|
| InitiateCDStateRequest  | CDStateChangedNotification  | CIM - MSpeak3 - MSpeak4  |
| InitiateConnectDisconnect  | CDStatesChangeNotification  | CIM - MSpeak3 - MSpeak4 - Utiliwise  |
| InitiateDemandReset  | MeterEventNotification  | CIM - MSpeak3 - MSpeak4 - Utiliwise  |
| InitiateMeterReadByMeterNumber  | ReadingChangedNotification  | CIM - MSpeak3 - MSpeak4 |
| InitiateOutageDetectionEventRequest  | ODEventNotification  | CIM - MSpeak3 - MSpeak4  |
| InitiateUsageMonitoring  | UsageMonitoringNotification  | MSpeak3 - MSpeak4  |
| CancelDisconnectedStatus  |   | MSpeak3 - MSpeak4  |
| CancelUsageMonitoring  |   | MSpeak3 - MSpeak4  |
| GetAMRSupportedMeters  |   | MSpeak3 - MSpeak4.13 - MSpeak4  |
| GetCDMeterState  |   | MSpeak3 - MSpeak4  |
| GetCDSupportedMeters  |   | MSpeak3 - MSpeak4  |
| GetLatestReadingByMeterNo  |   | MSpeak3 - MSpeak4  |
| GetReadingsByDate  |   | MSpeak3 - MSpeak4  |
| InitiateDisconnectedStatus  |   | MSpeak3 - MSpeak4  |
| MeterAddNotification  |   | MSpeak3 - MSpeak4  |
| MeterChangedNotification  |   | MSpeak3 - MSpeak4  |
| MeterRemoveNotification  |   | MSpeak3 - MSpeak4  |
| MeterRetireNotification  |   | MSpeak3 - MSpeak4  |
| ServiceLocationChangedNotification  |   | MSpeak3 - MSpeak4  |
| IsAMRMeter  |   | MSpeak3 - MSpeak4  |
| GetMethods - Broker Supported Methods |   | MSpeak3 - MSpeak4  |

*See the wsdls for more information about each of the methods*

> Synchronous methods which do not have a meter or service location are handled differently than the methods which require a meter or service location.

##### Notification (Origin of Notification)

- CancelDisconnectedStatus (CIS)
- CancelUsageMonitoring (CIS)
- MeterAddNotification (CIS)
- MeterChangedNotification (CIS)
- MeterEventNotification (AMI)
- MeterRemoveNotification (CIS)
- ODEventNotification (AMI)
- ReadingChangedNotification  (AMI)
- ServiceLocationChangedNotification (CIS)
- UsageMonitoringNotification (AMI)

##### Meter Commands

- GetCDMeterState
- GetLatestReadingByMeterNo
- IntiaiteCDStateRequest
- InitiateConnectDisconnect
- InitiateDemandReset
- InitiateDisconnectedStatus
- InitiateOutageDetectionEventRequest
- InitiateUsageMonitoring
- IsAMRMeter

> Since the Broker can route requests to more than one AMI system, the Broker will ask the MDM which AMI system (MDM.dbo.ReadSource) the meter belongs to so we can route the method to the correct AMI system.

##### Methods which do not require meter or service location

- GetAMRSupportedMeters
- GetCDSupportedMeters

*Typically these methods are requested by the CIS*

Note, these methods will always be setup on the CIS Vendor and 
not on the AMI Vendor.  This is confusing and we will have a full
example listed below.

## Use Case One

We have one CIS Vendor, two AMI Vendors, and one OMS vendor.

- FakeCISMSpeak3  aka FCorp
- FakeAMIMSpeak3  aka Asweara
- FakeAMIMSpeak4  aka Sensei
- FakeOMSMSpeak3  aka HardMil

The FakeCISMSpeak3 will send requests to the broker and expect requests involving
meters to be routed to the correct AMI system, and if required transform the
MultiSpeak Version 3 to MultiSpeak Version 4.

*Hey, if you cringe at those names... we can call them anything you want.*

### Pre-Setup Work

You will need to pull the ReadSources the meters will have for this setup.

```sql

SELECT ReadSourceDescrition, COUNT(*)
FROM MDM.dbo.Meter m
INNER JOIN MDM.dbo.ReadSource rs 
        ON rs.ReadSourceId = m.ReadSourceId
GROUP BY ReadSourceDescription

```

For test systems, this repository has a few sql scripts to setup and add additional meters. When testing against another vendors system it would be very cool to use GetAMISupported Meters to build the INSERT statement for AddMeters.sql.

- AddReadSource.sql
- AddMeters.sql
- AddLocation.sql

### Setup Authorized Users

The main point of setting up auhtorized users is to give each vendor a user
and password to the Broker.  This will allow them to send requests and responses
back to the broker.

Before we setup any authorized users, you will need to ask or discover
what the Company value is in the MutliSpeak Header.

We use the UserID, Pwd, and Company values when authenticating the incoming requests from Senders.

#### Example MultiSpeakMsgHeader

```xml

<MultiSpeakMsgHeader
        Version="3.0"
        UserID=""
        Pwd=""
        AppName="CRCLink"
        Company="CRC"
        xmlns="http://www.multispeak.org/Version_3.0"/>

```

#### Step 1

- Click or navigate to [Authorized Users](http://63.164.96.175/MultiSpeakBroker/UserConfig.aspx)
- Click Add

Enter the following

|       Key     |                                Value                                   |
|---------------|------------------------------------------------------------------------|
| Company       | This value should match the value in the multispeak header for Company |
| User Name     | You will create a user name for the vendor                             |
| User Password | You will create a password for the vendor                              |

> Add FakeCISMSpeak3 to Authorized Users.  That's it move on.

|       Key     |    Value   |
|---------------|------------|
| Company       | FCorp      |
| User Name     | FCorpUser  |
| User Password | 1337Fcorp  |

*Screen Shots to Add*

An authorized users only allows a vendor to authenticate.  If we stop here and FCorp sends us
a GetCDSupportedMeters request, the request will be denied because we still need Vendor setup
for FCord and Subscribers (Receivers) for the method GetCDSupportedMeters.

```C#
errorObject[] >
        objectID :
        errorString : Fail to get subscriber for Meter 100. Reason: #100 might not be in the FakeCISMSpeak3 system or might not have Readsource assigned.
        nounType :
        eventTime : 9/27/2017 2:42:23 PM
        eventTimeSpecified : True
```

Need to re-test and validate this error message.

#### Step 1.1

Now since we setup FCorp as an Authorized Users and we know this vendor will send meter and service location requests, notifications,
and requests which do not involve meters and locations we have to setup additional steps.

##### Vendor Config for FakeCISMSpeak3 (FCorp)

- Click [Vendor Configuration](http://63.164.96.175/MultiSpeakBroker/VendorConfig.aspx)
- Click Add Vendor

> In general, there are 2 type of vendors - Client and ReadSource. Client is a vendor that makes requests to Broker. Those vendors can be CIS, OMS or other. ReadSource is MDM ReadSource. It is a collection of one or more systems that process meter commands or return information.

Re-write of the above statement

> We can have two types of Vendors. A Client vendor makes request to the broker. Typically CIS and OMS are clients. A ReadSource vendor is a Vendor which needs to have the Vendor Name match the MDM.dbo.ReadSourceDescription.  A ReadSource vendor is typically the AMI System. This allows us to route incoming requests from Senders (Clients) to the correct Reciever (Servers), which in this case is an AMI system.  When setting up a ReadSource Vendor (AMI) you will also need to setup the MDMWebAPI call to be pointed at the MDM instance where the meters exist.

Enter the following

|       Key     |                                Value                                   |
|---------------|------------------------------------------------------------------------|
| Vendor        | FakeCISMSpeak3 aka FCorp                                               |
| Info          | FakeCISMSpeak3, this is the client which will make requests...         |

> Issue, when adding new Vendor with 'Add Vendor' the Key is **Vendor**.  When editting the Key is **Name**.

Since we are setting up a CIS Vendor the MDMWebAPI configuration can be skipped.

#### Step 1.2

Add Subscribers by clicking [Set Subsribers](http://63.164.96.175/MultiSpeakBroker/SubscribersConfig.aspx?ReadSource=FakeCISMSpeak3)

The Subscribers we will setup up for FakeCISMSpeak3 will be the following:

- MSpeak3 GetAMRSupportedMeters
- MSpeak3 GetCDSupportedMeters

Both of these methods pass no parameters to the AMI System.  We need to set these subribers up under the FakeCISMSPeak3 (FCorp) Vendor.
Since FakeCISMSPeak3 (FCorp) uses MultiSpeak version 3 we need to use the MSpeak3 methods.

Click EDIT for the MSpeak3 GetAMRSupportedMeters Subscriber.

IMAGE- SCREENSHOT

Edit Subscriber - GetAMRSupportedMeters (MSpeak3)
Response Method(MSpeak3):

|       Key     |                                Value                                              |
|---------------|-----------------------------------------------------------------------------------|
| Host          | Host is really the Receiver's MultiSpeak Version.  Our AMI Provider is on MSPeak3 |
| Host Url      | Reciever's Url.  This will be the AMI Systems URL                                 |
| USer ID       | User ID the AMI System has given us                                               |
| User Password | User Password the AMI System has given us                                         |
| AMI Meter Type| The device id we need to send to the AMI System                                   |

The AMI Meter Type is the value or deviceId we will send to the AMI System. Ocassionaly the MeterIdentifer
is not the value the AMI System requires.  You will need to check with the AMI Vendor before setting this
up to determin what deviceId will be used {MeterIdentifier, AMISerialNo, AMIControlIdentifier}.

|       Key     |                                                       |
|---------------|-------------------------------------------------------|
| Host          | MSpeak3                                               |
| Host Url      | http://demo.turtletech.com/latest/webAPI/MR_CB.asmx   |
| USer ID       | UNITILTST                                             |
| User Password | Unitil1!                                              |
| AMI Meter Type| Meter Identifier                                      |

Another key important fact, you need to map the method to the AMI vendors correct server. GetAMRSupportedMeters is in the MR_CB server for this AMI vendor.  Other methods may be in differnt endpoints.

Ok, we are getting close.  Breath deep, take a walk, smoke, coke, granola break or something.  Or just push forward. Or maybe test?

#### Step 2

Add the first AMI Vendor (FakeAMIMSpeak3  aka Asweara)

- Click [Vendor Configuration](http://63.164.96.175/MultiSpeakBroker/VendorConfig.aspx)
- Click Add Vendor
- Configure MDM Web API

Enter the following

|       Key     |                                Value                                   |
|---------------|------------------------------------------------------------------------|
| Vendor        | Asweara                                                                |
| Info          | Asweara, slayer of kW, breader of Volts                                |

**This is our AMI Vendor. The Value for Vendor needs to match the MDM.dbo.ReadSourceDescription exactly**

Each request with a meter or service location a Sender like FCorp will send to the Broker will be routed to the correct AMI vendor based on the a query something like the following:

```sql
        SELECT TOP 1 MeterIdentifier FROM
        MDM.dbo.Meter
        WHERE MeterIdentifier = @MeterIdentifier;
```

OR

```sql
        SELECT TOP 1 AMISerialNo FROM
        MDM.dbo.Meter
        WHERE AMISerialNo = @AMISerialNo;
```

If an AMI Vendor's Name does not match the MDM.dbo.ReadSourceDescription you might see something like this:

```C#
errorObject[] >
        objectID :
        errorString : Fail to get subscriber for Meter 100. Reason: #100 might not be in the FakeCISMSpeak3 system or might not have Readsource assigned.
        nounType :
        eventTime : 9/27/2017 2:42:23 PM
        eventTimeSpecified : True
```

*Re-test this error*

#### Step 2.1

Add Subscribers by clicking [Set Subsribers](http://63.164.96.175/MultiSpeakBroker/SubscribersConfig.aspx?ReadSource=FakeCISMSpeak3)

The Subscribers we will setup up for FakeCISMSpeak3 will be the following:

- MSpeak3 CancelDisconnectedStatus
- MSpeak3 CancelUsageMonitoring
- MSpeak3 GetCDMeterState
- MSpeak3 GetLatestReadingByMeterNo
- MSpeak3 GetReadingsByDate
- MSpeak3 InitiateConnectDisconnect
- MSpeak3 InitiateDisconnectedStatus
- MSpeak3 InitiateMeterReadByMeterNumber
- MSpeak3 InitiateOutageDetectionEventRequest
- MSpeak3 InitiateUsageMonitoring
- MSpeak3 MeterAddNotification
- MSpeak3 MeterChangedNotification
- MSpeak3 MeterRemoveNotification
- MSpeak3 MeterRetireNotification
- MSpeak3 ServiceLocationChangedNotification
- MSpeak3 IsAMRMeter

Here is one example.  Rememner the Host Url can be different for each of
the methods.

|       Key     |                                                       |
|---------------|-------------------------------------------------------|
| Host          | MSpeak3                                               |
| Host Url      | http://demo.turtletech.com/latest/webAPI/MR_CB.asmx   |
| USer ID       | UNITILTST                                             |
| User Password | Unitil1!                                              |
| AMI Meter Type| Meter Identifier                                      |


------------------------------------------------------------------
------------------------------------------------------------------
------------------------------------------------------------------

Step 2:
Setup the AMI Vendor, and we can call the vendor... something special like FakeAMI

Landis's Web API
hint - "http://{server name}/MDMWebAPI/" 
URL:
User:
Password: 



Vendor Configuration > Add Vendor
Name: Landis & Gyr
Description: ReadSource to send and receive AMI Commands for MSpeak v3 (old version)

Since we setup an AMI Vendor, we also need to setup the MDMWebAPI Settings.

Normally the MDMWebAPI will be on our on Application Server and use the typical user name and passwords.
Check the MDMWebAPI.config for user and passwords... unless we store this somewhere else now.

Remember, we can support multiple AMI Vendors so when we receive a request that will be
sent to an AMI Vendor, we will make a request to the MDMWebAPI to see which AMI Vendor
owns this meter.   How we did this is by using the VendorName which needs to 
match (EXACTLY, cases and spelling) the name of the read source in MDM.dbo.ReadSource.

The Vendor Name is used to generate a query to the MDM to see if this
meter exists in MDM.dbo.Meter either by MeterIdentifier, (Serial) AMISerialNo
or AMIControlIdentifier.

In most cases use MeterIdentifier.  If you get back Meter does not exists or see no meter
number in the request... make sure the AMI Meter Type is correctly setup to the

If this vendor is making requests to Broker, please configure Client Web API where Broker can retreive meter information.


TODO: Go back and get the real query ...

Step 3:
Set Subscribers for all the methods above... that a CIS Vendor will send to the AMI system.

http://10.86.1.81/MultiSpeakBroker/SubscribersConfig.aspx?ReadSource=Landis & Gyr

http://10.86.1.81/MultiSpeakBroker/SubscribersConfig2.aspx?ReadSource=Landis

TODO: Advanced Editor and Normal Editor do not show the same list when looking at the setup endpoint methods.
http://10.86.1.81/MultiSpeakBroker/SubscribersConfig2.aspx?ReadSource=Landis
Is missing al lthe Meter*.* Notifications

http://10.86.1.81/MultiSpeakBroker/SubscribersConfig.aspx?ReadSource=Landis

URL is not handled properly when there are spaces in the Vendor Name.
Landis & Gyr has an Ampersand... this is mess up the data in the database and query?

TODO: Tab always shows 'Waiting for response from $URLOfWebSite
Back to the Topic at hand...

Click Set Subscribers for this Vendor (FakeAMI 0r Landis & Gyr)

Each individual method will need the following.

Example
Find 'MSpeak3 ServiceLocationChangedNotification' on this page http://10.86.1.81/MultiSpeakBroker/SubscribersConfig.aspx?ReadSource=Landis+&+Gyr
FakeCISMSpeak3 will send a a ServiceLocationChangedNotification to the Broker in, what version? MultiSpeak Version 3!! Yes.
So we choose 'MSpeak3 ServiceLocationChangedNotification'

Or AMI Vendor could be using MultiSpeak 3 or MultiSpeak 4... which one is it?  Ask the AMI vendor!

TODO: Rename Client Request to something like Incoming Client Request Method and Version or anything to help a dev out.

Edit Subscriber - ServiceLocationChangedNotification (MSpeak3)
Response Method(MSpeak3):

TODO:  The title in the popup is good but can be better.  The Response Method is really the Host Field parameter.  And If I 
change Host from MSpeak3 to MSpeak4, the ResponseMethod is still (MSpeak3)... Is this valid?? Why is it like this? 

TODO:  The Client Request and Receiver Host, and Task are the columns in this grid on page: SubscribersConfig.aspx
The label for the Edit popup has Host: instead of Receiver Host.

Host:  THIS SHOULD BE RECEIVER HOST, I WILL JUST NOTE THIS AND MOVE ON

When you click Edit you will see the following Urls
Host: is the MultiSpeak Version the AMI Vendor will be expecting to receive.  You see, we can accept a MSpeak3 message and transform this message
into a MSPeak4 Message.

Host URL: The endpoint for the AMI system. Something like :  http://demo.turtletech.com/latest/webAPI/MR_CB.asmx  

User ID:  The user the AMI system expects.  The AMI Vendor will need to give you a username. Ask Them for a Username.

User Password:  The password the AMI system expects. The AMI Vendor will need to give you a password. Ask them for a Password.

AMI Meter Type:
This is one is crucial. Very Very Special.

Each AMI system can accept generally one device id, and our MDM stores 3 device ids (MeterIdentifier, (Serial) AMISerialNo or AMIControlIdentifier).

Normal users our CSR Portal will typical just remember and use MeterIdentifier.  But sometimes the AMI systems require a different device id to issue
a command.  When the AMI requires an id different than MeterIdentifier, you can either choose Serial, or AMIControlIdenifier.  The AMI vendor
will tell you which id they will expect.  Ask them. Ask someone.  Test. Test Again.  And Test your Test.

Step Final: Test that thing...!





## Outage Management Systems

Typically we will receive InitiateOutageDetectionEventRequest from OMS.  This request is
routed to the correct AMI and the AMI responds with an ODEventNotification.

## Information Needed before any setup is completed

CIS Systems will be sending requests to the MultiSpeak Broker.
The CIS system will need an endpoint to send data to, which ElectSolve will setup
and give the CIS Vendor the following:

Company Name, they have in the MultiSpeak Header
User
Pwd

## Step 1 Authorized User

Add Server for the Clients User

Client - MDMPortal / CSR Portal
Server - MultiSpeakBroker

Add Authorized User for the MultiSpeak Broker Web App.

### Example 1

Example - CSR Portal Makes a call to the MultiSpeak Broker
Authorized Users - http://10.86.1.81/MultiSpeakBroker/UserConfig.aspx

Company : BackOffice
UserName : bo
User Password: bo

This company, user and password will be used to post data to the MultiSpeakBroker Url:

> https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/416/1/MDM_Server.asmx

db.User.find({Compnay:"BackOffice"}).pretty()
{
        "_id" : ObjectId("58ed494497c3742734702331"),
        "Company" : "BackOffice",
        "UserID" : "bo",
        "Password" : "bo"
}

#### Automate

```mongo

db.User.insert()
{
        "Company" : "BackOffice",
        "UserID" : "bo",
        "Password" : "bo"
}

```

### Example 2

Example - CIS Vendor MultiSpeak Broker
Authorized Users - http://10.86.1.81/MultiSpeakBroker/UserConfig.aspx

Company : NISC
UserName : NISC
User Password: 12345

This company, user and password will be used to post data to the MultiSpeakBroker Url:

> https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/416/1/MDM_Server.asmx

``` mongo

db.User.find({Compnay:"BackOffice"}).pretty()
{
        "_id" : ObjectId("58ed494497c3742734702331"),
        "Company" : "NISC",
        "UserID" : "NISC",
        "Password" : "12345sbo
}

```

#### Automate

```mongo

db.User.insert()
{
        "Company" : "BackOffice",
        "UserID" : "bo",
        "Password" : "bo"
}

```

This will allow the CSR portal to send requests to the MultiSpeakBroker Web App.
For example, in the MDMPortal.config you would setup the following:

J:\ElectSolve\Green\Config\MDMPortal.config

```xml

  <add key="Multispeak.415.Generic.User" value="bo" />
  <add key="Multispeak.415.Generic.Password" value="bo" />
  <add key="Multispeak.415.Generic.ApplicationName" value="MDM" />
  <add key="Multispeak.415.Generic.Company" value="BackOffice" />
  <add key="Multispeak.415.Generic.ApplicationVersion" value="2.0" />
  <add key="Multispeak.415.Generic.BaseURL" value="https://etss-apploadtest.etss.com/MultiSpeakBroker/MultiSpeak/416/1/" />
  <add key="Multispeak.415.Generic.CD_ServerPath" value="MDM_Server.asmx" />
  <add key="Multispeak.415.Generic.MR_ServerPath" value="MDM_Server.asmx" />
  <add key="Multispeak.415.Generic.Response.BaseURL" value="https://etss-apploadtest.etss.com/MDMServices/Multispeak/415/" />
  <add key="Multispeak.415.Generic.Response.NOT_ServerPath" value="NOT_Server.asmx" />
  <add key="Multispeak.415.Generic.Response.ConnectDisconnectList" value="N" />
  <add key="Multispeak.415.Generic.meterID.IsAttribute" value="N" />
  <add key="Multispeak.415.Generic.Response.ConvertWhToKwh" value="N" />
  <add key="Multispeak.415.Generic.Response.NotServerReadMethod" value="ReadingChangedNotification" />

```

So what we have is a person using the page InitiateMeterRead, InitiateConnect, InitiateDisconnect, InitiateDemandReset
will forward or post this command to the MultiSpeak Broker.

It will be the MultiSpeak Broker's job to forward the request to the actual vendor (AMI, CIS, OMS, etc..)

## Step 2 - Vendor Configuration - http://10.86.1.81/MultiSpeakBroker/VendorConfig.aspx

Step 2 has two parts.   The first part is adding the Authorized user we just setup which is going to receive
the requests from the CSR Portal to be able to choose the correct endpoint to send the requests.  We can
support many AMI systems, and each system has a ReadSource.  We use this read source to forward meter
commands to the correct AMI system.

As the broker receives requests, having the CSR Portal's Receiver (BackOffice) setup as a vendor we
can then setup this receiver to use the MDMWebAPI to query the MDM system about the meter in the request.
Each InitiateMeterRead, Connect, Disconnect, and Demand Reset will be for a meter, and a meter could be
in either Aclara or SRFN or Sensus, or SSN AMI system.

We map the meter's readsource when we inititally load the data.

### Step 2.A - Setup the CSR Portal's Authorized User as a Vendor.

https://etss-apploadtest.etss.com/MultiSpeakBroker/VendorConfig.aspx
Add
    Name : BackOffice
    Info : TYPE =  Client. Store credential to Backoffice

If this vendor is making requests to Broker, please configure Client Web API where Broker 
can retreive meter information.

**Configure the MDMWebAPi**

If Broker is required to return or notify result to this vendor, the vendor might expect user/pass. 
Configure Credential.

~This is more for synchronous responses which need their own example~

> db.Vendor.find({VendorName: "BackOffice"}).pretty()

```json

{
        "_id" : ObjectId("58e417c297c37811841113fe"),
        "VendorName" : "BackOffice",
        "MultiSpeakVersion" : "3",
        "MRCB_UserID" : "member0",
        "MRCB_Password" : "member0",
        "CDCB_UserID" : "member0",
        "CDCB_Password" : "member0",
        "Info" : "TYPE =  Client. Store credential to Backoffice",
        "APIUrl" : "https://etss-apploadtest.etss.com/MDMWebAPI/",
        "APIUserID" : "electsolve",
        "APIPassword" : "3lects01ve!"
}

### Step 2.B - Setup the Vendor's endpoint who will recieve the request and return a response.

This step adds the Vendor who will receive the request.  The vendor could be an AMI, CIS, or OMS system.

Typically for Asynchronous request and responses, where we send a request, and move on, and then server then
responds with a Notification, we will ndver have to setup the MR_BC and CD_CB user and passwords.

The main area to pay attention to here is the VendorName which needs to match the 
MDM.dbo.ReadSource.ReadSourceDescription.

> db.Vendor.find({VendorName: "SSN"}).pretty()
{
        "_id" : ObjectId("5880ead597c37b1bc0905f2b"),
        "VendorName" : "SSN",
        "MultiSpeakVersion" : "4",
        "MRCB_UserID" : "",
        "MRCB_Password" : "",
        "CDCB_UserID" : "",
        "CDCB_Password" : "",
        "Info" : "TYPE = RS. Shared ReadSource (for all utilies)"
}

```

### Step 2.C

Click 'Set Subscribers'
> https://etss-apploadtest.etss.com/MultiSpeakBroker/SubscribersConfig.aspx?ReadSource=SSN

Once you have the Vendor setup (AMI, CIS, OMS) we have to configure or setup the requests we
will allow the MultiSpeakBroker to forward on to the AMI, CIS, or OMS System.

Each method will need to be setup individually.

Edit Subscriber - InitiateCDStateRequest (MSpeak4)
Response Method(MSpeak4): MSpeak4CDStateChangedNotification

Host: **SSN**  (Available from Dropdown) - This should match your vendor
Host URL:  http://10.86.1.32:8081/soapserver
User ID:
User Password:
AMI Meter Type:

AMI Meter Type is **VERY** important.  AMI systems typically can accept one value
which is used to look up the meter and forward the command to the meter.

Remember we have two types of messages we have to setup:

- Commands which go to the AMI
- Commands which are coming back from AMI as Notifications.

We will send a multispeak message version 4 message (InitiateCDStateRequest) to
the SSN AMI system.

Add/Edit Subscriber
Channel: ChannelMSpeak4InitiateCDStateRequest
Receiver Host Type: SSN 
Host URL:  http://10.86.1.32:8081/soapserver
User ID  ******
User Password  ****
AMI Meter Type  MeterIdntifier

The user and password you enter here will be used to authenticate with the AMI provider URLs
and API methods.

Examples

> db.Subscriber.find({MeterReadSource: "SSN"}).pretty()

```json

{
        "_id" : ObjectId("58e4166a97c37811841113f5"),
        "MeterReadSource" : "SSN",
        "Type" : "SSN",
        "Channel" : "ChannelMSpeak4InitiateCDStateRequest",
        "Url" : "http://10.86.1.32:8081/soapserver",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4166a97c37811841113f6"),
        "MeterReadSource" : "SSN",
        "Type" : "MSpeak4",
        "Channel" : "ChannelMSpeak4CDStateChangedNotification",
        "Url" : "",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4166a97c37811841113f7"),
        "MeterReadSource" : "SSN",
        "Type" : "MSpeak4",
        "Channel" : "ChannelMSpeak4CDStatesChangedNotification",
        "Url" : "",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4167897c37811841113f8"),
        "MeterReadSource" : "SSN",
        "Type" : "SSN",
        "Channel" : "ChannelMSpeak4InitiateConnectDisconnect",
        "Url" : "http://10.86.1.32:8081/soapserver",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4168397c37811841113f9"),
        "MeterReadSource" : "SSN",
        "Type" : "SSN",
        "Channel" : "ChannelMSpeak4InitiateMeterReadingsByMeterID",
        "Url" : "http://10.86.1.32:8081/soapserver",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "AMIControlIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4168397c37811841113fa"),
        "MeterReadSource" : "SSN",
        "Type" : "MSpeak4",
        "Channel" : "ChannelMSpeak4ReadingChangedNotification",
        "Url" : "",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4169097c37811841113fb"),
        "MeterReadSource" : "SSN",
        "Type" : "SSN",
        "Channel" : "ChannelMSpeak4InitiateDemandReset",
        "Url" : "http://10.86.1.32:8081/soapserver",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}
{
        "_id" : ObjectId("58e4169097c37811841113fc"),
        "MeterReadSource" : "SSN",
        "Type" : "MSpeak4",
        "Channel" : "ChannelMSpeak4MeterEventNotification",
        "Url" : "",
        "UserID" : "",
        "Password" : "",
        "AMIMeterType" : "MeterIdentifier",
        "SettingAppendSerialNo" : false,
        "SettingSourceSerialNo" : null
}

```

## Done

Are we done?

## InfoSec

Gathering knowledge about what is deployed - regarding the broker at a specific location.

Steps
- Get Location of mongo
- Get user and pwd of mongo
- If you don't have any of these in the wiki... you will have to view the MultiSpeakBroker Web App web.config or MultiSpeak Broker 
Service MultiSpeakBrokerService.exe.config.

